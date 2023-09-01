using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private PlayerLightSources _lightSources;
    private float _speed = 4f, _distance;
    [SerializeField]
    private int _enemyID; //0 = umbra; 1 = lux

    private Animator _umbraAnim;
    
    [SerializeField]
    private AudioSource _enemyNoiseSource;
    [SerializeField]
    private AudioSource _enemyDeathSource;
    bool _isDead = false, _disableNoise = false;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start() {
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _lightSources = GameObject.Find("Player").GetComponent<PlayerLightSources>();
        _umbraAnim = GetComponent<Animator>();
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
        if (!_disableNoise && _distance <= 30) {
            _disableNoise = true;
            PlayUmbraNoise();
        }
        if (_distance < 22 && !_isDead) {
            _umbraAnim.SetTrigger("Walking");
            if (!_lightSources.GetIsFlashlightActive()) { // if player flashlight is off, enemy approaches
                _umbraAnim.ResetTrigger("Afraid");
                _speed = 4f;
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
                if(_direction.x < 0) {
                    transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
                } else if(_direction.x >= 0) {
                    transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
                }
            } else { // if player flashlight is on, umbra runs away...
                CalculateCorrectEnemyMovement(_direction);
            }
            if (_distance < 13 && _lightSources.GetIsFlashCameraActive()) { // change to 7
                if (_direction.x < 0 && _player.GetDirection()) {
                    KillUmbra();
                }
            }
        }
    }

    private void Lux() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (!_isDead) {
                switch (_enemyID) {
                    case 0:
                        _umbraAnim.ResetTrigger("Walking");
                        _player.KillPlayer();
                        break;
                    case 1:
                        break;
                }
            }
        }
    }

    private void CalculateCorrectEnemyMovement(Vector3 dir) {
        if (dir.x < 0) { // if enemy is to the right of the player
            if (_player.GetDirection()) { // ... only if the player is pointing the flashlight in the correct direction
                _umbraAnim.SetTrigger("Afraid");
                _speed = 2f;
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position 
                    + new Vector3(13, transform.position.y, 0), _speed * Time.deltaTime);
                CorrectUmbraSpriteDirection(dir);
            } else {
                _umbraAnim.ResetTrigger("Afraid");
                _speed = 4f;
                transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
            }
        } else if (dir.x >= 0) { // if enemy is to the left of the player
            if (!_player.GetDirection()) { // ... only if the player is pointing the flashlight in the correct direction
                _umbraAnim.SetTrigger("Afraid");
                _speed = 2f;
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position 
                    - new Vector3(13, transform.position.y, 0), _speed * Time.deltaTime);
                CorrectUmbraSpriteDirection(dir);
            } else {
                _umbraAnim.ResetTrigger("Afraid");
                _speed = 4f;
                transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
                this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
            }
        }
    }

    private void CorrectUmbraSpriteDirection(Vector3 dir) {
        if (dir.x < 0) {
            if (this.transform.position.x > _player.transform.position.x + 12
                    && this.transform.position.x < _player.transform.position.x + 14) { // if the enemy is on the edge of the flashlight beam
                transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
            } else if (this.transform.position.x < _player.transform.position.x + 12) { // if the enemy is within the flashlight beam
                transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
            } else if (this.transform.position.x > _player.transform.position.x + 14) { // if the enemy is far right of the flashlight beam
                transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
            }
        } else if (dir.x >= 0) {
            if (this.transform.position.x < _player.transform.position.x - 12
                    && this.transform.position.x > _player.transform.position.x - 14) { // if the enemy is on the edge of the flashlight beam
                transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
            } else if (this.transform.position.x > _player.transform.position.x - 12) { // if the enemy is within the flashlight beam
                transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
            } else if (this.transform.position.x < _player.transform.position.x - 14) { // if the enemy is far left of the flashlight beam
                transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
            }
        }
    }

    private void KillUmbra() {
        _enemyDeathSource.Play();
        _isDead = true;
        GameObject.Find("Enemy_Eyes").SetActive(false);
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine(DestroyGameObject(7.1f));
    }

    private void PlayUmbraNoise() {
        _enemyNoiseSource.Play();
    }

    IEnumerator DestroyGameObject(float seconds) {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
