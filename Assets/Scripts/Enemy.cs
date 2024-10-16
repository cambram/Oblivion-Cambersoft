using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private PlayerLightSources _lightSources;
    private float _speed = 4f, _distance;
    private float _slowSpeed = 2f, _fastSpeed = 4f;
    [SerializeField]
    private int _enemyID; //0 = umbra; 1 = lux

    private Animator _enemyAnim;
    
    [SerializeField]
    private AudioSource _enemyNoiseSource;
    [SerializeField]
    private AudioSource _enemyDeathSource;
    [SerializeField]
    private GameObject _enemyEyes;
    bool _isDead = false, _disableNoise = false, _isJumpActive = false;

    private UIManager _uiManager;
    private Vector3 _direction;


    // Start is called before the first frame update
    void Start() {
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _lightSources = GameObject.Find("Player").GetComponent<PlayerLightSources>();
        _enemyAnim = GetComponent<Animator>();
        _enemyEyes.GetComponent<Light2D>().intensity = 0.1f;
        _direction = _player.transform.position - this.transform.position;
        switch (_enemyID) {
            case 0:
                _slowSpeed = Random.Range(3, 5);
                _fastSpeed = Random.Range(5, 7);
                break;
            case 1:
                _slowSpeed = 0;
                _fastSpeed = Random.Range(5, 8);
                break;
        }
    }

    void Update() {
        if(_player != null && !_uiManager.GetIsPaused()) {
            _direction = _player.transform.position - this.transform.position;
            switch (_enemyID) {
                case 0:
                    Umbra();
                    break;
                case 1:
                    Lux();
                    break;
            }
            if(this.transform.position.y < -15) {
                KillEnemy();
            }
        }
    }

    private void Umbra() {
        _distance = Vector3.Distance(this.transform.position, _player.transform.position);
        if (!_disableNoise && _distance <= Random.Range(22, 31)) {
            _disableNoise = true;
            PlayEnemyNoise();
        }
        if (_distance < 22 && !_isDead) {
            _enemyAnim.SetTrigger("Walking");
            if (_lightSources.GetIsAnyLightActive()) { // if player flashlight is on, umbra runs away if...
                if (_lightSources.GetCurrentLightSource() == 0) {
                    CalculateCorrectEnemyMovementFlashlight(_direction);
                } else {
                    CalculateCorrectEnemyMovementLantern(_direction);
                }
            } else { // if player flashlight is off, enemy approaches
                _enemyEyes.GetComponent<Light2D>().intensity = 9f;
                _enemyAnim.ResetTrigger("Afraid");
                _speed = _fastSpeed;
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
                if (_direction.x < 0) {
                    transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
                } else if (_direction.x >= 0) {
                    transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
                }
            }
            if (_lightSources.GetIsFlashCameraActive()) {
                if (_lightSources.GetCurrentLightSource() == 0) {
                    if(_distance <= 13) {
                        if (_direction.x < 0 && _player.GetDirection()) {
                            KillEnemy();
                        } else if (_direction.x > 0 && !_player.GetDirection()) {
                            KillEnemy();
                        }
                    }
                } else {
                    if (_distance <= 6.5) {
                        KillEnemy();
                    }
                }
            }
        }
    }

    private void Lux() {
        _distance = Vector3.Distance(this.transform.position, _player.transform.position);
        if (!_disableNoise && _distance <= Random.Range(22, 31)) {
            _disableNoise = true;
            PlayEnemyNoise();
        }
        if (_distance < 30 && !_isDead) {
            if (_lightSources.GetIsAnyLightActive() || _player.GetAttraction()) { // if player flashlight is on, lux approches
                _enemyEyes.GetComponent<Light2D>().intensity = 0.1f;
                _enemyAnim.SetTrigger("Approach");
                _speed = _fastSpeed;
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
                if (_direction.x < 0) {
                    transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
                } else if (_direction.x >= 0) {
                    transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
                }
            } else { // if player flashlight is off, lux stands still
                _enemyEyes.GetComponent<Light2D>().intensity = 2f;
                _enemyAnim.ResetTrigger("Approach");
                _speed = _slowSpeed;
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
                if (_direction.x < 0) {
                    transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
                } else if (_direction.x >= 0) {
                    transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
                }
            }
            if (_lightSources.GetIsFlashCameraActive()) {
                if (_lightSources.GetCurrentLightSource() == 0) {
                    if (_distance <= 13) {
                        if (_direction.x < 0 && _player.GetDirection()) {
                            KillEnemy();
                        } else if (_direction.x > 0 && !_player.GetDirection()) {
                            KillEnemy();
                        }
                    }
                } else {
                    if (_distance <= 6.5) {
                        KillEnemy();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (!_isDead) {
                switch (_enemyID) {
                    case 0:
                        _enemyAnim.ResetTrigger("Walking");
                        _player.KillPlayer();
                        break;
                    case 1:
                        _player.KillPlayer();
                        break;
                }
            }
        } else if (collision.CompareTag("Death")) {
            if (!_isDead) {
                KillEnemy();
            }
        } else if (collision.CompareTag("Enemy")) {
            if (collision.gameObject.GetComponent<Enemy>().GetEnemyID() == 1 && this.GetEnemyID() != 1) {
                Physics2D.IgnoreCollision(collision, this.GetComponent<Collider2D>());
            }
        }
    }

    public int GetEnemyID() {
        return _enemyID;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.CompareTag("Jump")) {
            if (!_isDead && !_isJumpActive) {
                _isJumpActive = true;
                if (_direction.x <= 0) {
                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(25, 500)); //_rigidbody.velocity.x
                } else {
                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-25, 500)); //_rigidbody.velocity.x
                }
                StartCoroutine(EnableJump());
            }
        }
    }

    //Could be used primarily for gap jumps
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("JumpGap")) {
            if (!_isDead && !_isJumpActive) {
                _isJumpActive = true;
                if (_direction.x <= 0) {
                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(25, 500)); //_rigidbody.velocity.x
                } else {
                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-25, 500)); //_rigidbody.velocity.x
                }
                StartCoroutine(EnableJump());
            }
        }
    }

    private void CalculateCorrectEnemyMovementFlashlight(Vector3 dir) {
        if (dir.x < 0) { // if enemy is to the right of the player
            if (_player.GetDirection()) { // ... only if the player is pointing the flashlight in the correct direction
                _enemyEyes.GetComponent<Light2D>().intensity = 0.3f;
                _enemyAnim.SetTrigger("Afraid");
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position 
                    + new Vector3(13, transform.position.y, 0), _speed * Time.deltaTime);
                CorrectUmbraSpriteDirection(dir, 12, 14);
            } else {
                _enemyEyes.GetComponent<Light2D>().intensity = 9f;
                _speed = _fastSpeed;
                _enemyAnim.ResetTrigger("Afraid");
                transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
            }
        } else if (dir.x >= 0) { // if enemy is to the left of the player
            if (!_player.GetDirection()) { // ... only if the player is pointing the flashlight in the correct direction
                _enemyEyes.GetComponent<Light2D>().intensity = 0.3f;
                _enemyAnim.SetTrigger("Afraid");
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position 
                    - new Vector3(13, transform.position.y, 0), _speed * Time.deltaTime);
                CorrectUmbraSpriteDirection(dir, 12, 14);
            } else {
                _enemyEyes.GetComponent<Light2D>().intensity = 9f;
                _speed = _fastSpeed;
                _enemyAnim.ResetTrigger("Afraid");
                transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
            }
        }
    }

    private void CalculateCorrectEnemyMovementLantern(Vector3 dir) {
        _enemyEyes.GetComponent<Light2D>().intensity = 0.3f;
        if (dir.x < 0) { // if enemy is to the right of the player
            _enemyAnim.SetTrigger("Afraid");
            this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position
                    + new Vector3(6, transform.position.y, 0), _speed * Time.deltaTime);
            CorrectUmbraSpriteDirection(dir, 5, 7);
        } else if (dir.x >= 0) { // if enemy is to the left of the player
            _enemyAnim.SetTrigger("Afraid");
            this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position
                    - new Vector3(6, transform.position.y, 0), _speed * Time.deltaTime);
            CorrectUmbraSpriteDirection(dir, 5, 7);
        }
    }

    private void CorrectUmbraSpriteDirection(Vector3 dir, int beamDistInner, int beamDistOuter) {
        if (dir.x < 0) {
            if (this.transform.position.x > _player.transform.position.x + beamDistInner
                    && this.transform.position.x < _player.transform.position.x + beamDistOuter) { // if the enemy is on the edge of the flashlight beam
                _speed = _slowSpeed; //if (flashlight)
                transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
            } else if (this.transform.position.x < _player.transform.position.x + beamDistInner) { // if the enemy is within the flashlight beam
                _speed = _fastSpeed;
                transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
            } else if (this.transform.position.x > _player.transform.position.x + beamDistOuter) { // if the enemy is far right of the flashlight beam
                _speed = _fastSpeed;
                transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
            }
        } else if (dir.x >= 0) {
            if (this.transform.position.x < _player.transform.position.x - beamDistInner
                    && this.transform.position.x > _player.transform.position.x - beamDistOuter) { // if the enemy is on the edge of the flashlight beam
                _speed = _slowSpeed;
                transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
            } else if (this.transform.position.x > _player.transform.position.x - beamDistInner) { // if the enemy is within the flashlight beam
                _speed = _fastSpeed;
                transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
            } else if (this.transform.position.x < _player.transform.position.x - beamDistOuter) { // if the enemy is far left of the flashlight beam
                _speed = _fastSpeed;
                transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
            }
        }
    }

    private void KillEnemy() {
        _enemyAnim.SetTrigger("Death");
        _enemyDeathSource.Play();
        _isDead = true;
        _enemyEyes.SetActive(false);
        Destroy(this.gameObject.GetComponent<Rigidbody2D>());
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        //this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(DestroyGameObject(7.1f));
    }

    private void PlayEnemyNoise() {
        int rand = (int) Random.Range(1, 4 );
        if(rand == 1 || rand == 2) { 
            _enemyNoiseSource.pitch = Random.Range(0.8f, 1.1f);
            _enemyNoiseSource.Play(); 
        }
    }

    IEnumerator DestroyGameObject(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

    IEnumerator EnableJump() {
        yield return new WaitForSeconds(1);
        _isJumpActive = false;
    }
}
