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
    bool _isDead = false, _disableNoise = false;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start() {
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _lightSources = GameObject.Find("Player").GetComponent<PlayerLightSources>();
        _enemyAnim = GetComponent<Animator>();
        _enemyEyes.GetComponent<Light2D>().intensity = 0.1f;
        switch (_enemyID) {
            case 0:
                _slowSpeed = Random.Range(2, 4);
                _fastSpeed = Random.Range(4, 6);
                break;
            case 1:
                _slowSpeed = 0;
                _fastSpeed = Random.Range(5, 8);
                break;
        }
    }

    void Update() {
        if(_player != null && !_uiManager.GetIsPaused()) {
            switch (_enemyID) {
                case 0:
                    Umbra();
                    break;
                case 1:
                    Lux();
                    break;
            }
        }
    }

    private void Umbra() {
        _distance = Vector3.Distance(this.transform.position, _player.transform.position);
        Vector3 _direction = _player.transform.position - this.transform.position;
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
            if (_distance < 13 && _lightSources.GetIsFlashCameraActive()) { // change to 7
                if (_lightSources.GetCurrentLightSource() == 0) {
                    if (_direction.x < 0 && _player.GetDirection()) {
                        KillEnemy();
                    } else if (_direction.x > 0 && !_player.GetDirection()) {
                        KillEnemy();
                    }
                } else {
                    KillEnemy();
                }
            }
        }
    }

    private void Lux() {
        _distance = Vector3.Distance(this.transform.position, _player.transform.position);
        Vector3 _direction = _player.transform.position - this.transform.position;
        if (!_disableNoise && _distance <= Random.Range(22, 31)) {
            _disableNoise = true;
            PlayEnemyNoise();
        }
        if (_distance < 30 && !_isDead) {
            if (_lightSources.GetIsAnyLightActive()) { // if player flashlight is on, lux approches
                _enemyEyes.GetComponent<Light2D>().intensity = 0.1f;
                _enemyAnim.SetTrigger("Approach");
                _speed = _fastSpeed;
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
                if (_direction.x < 0) {
                    transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
                } else if (_direction.x >= 0) {
                    transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
                }
            } else { // if player flashlight is off, lux approaches very slowly
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
            if (_distance < 13 && _lightSources.GetIsFlashCameraActive()) { // change to 7
                if (_lightSources.GetCurrentLightSource() == 0) {
                    if (_direction.x < 0 && _player.GetDirection()) {
                        KillEnemy();
                    } else if (_direction.x > 0 && !_player.GetDirection()) {
                        KillEnemy();
                    }
                } else {
                    KillEnemy();
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
                        //_enemyAnim.ResetTrigger("Walking");
                        _player.KillPlayer();
                        break;
                }
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
        _enemyDeathSource.Play();
        _isDead = true;
        _enemyEyes.SetActive(false);
        Destroy(this.gameObject.GetComponent<Rigidbody2D>());
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
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
}
