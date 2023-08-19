using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player _player;
    private float _speed = 4f, _distance;

    private SpriteRenderer _spriteRenderer;
    private Animator _enemyAnim;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _enemySoundClip;
    [SerializeField]
    private AudioClip _enemyDeathClip;
    bool _isDead = false, _disableNoise = false;

    // Start is called before the first frame update
    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _enemyAnim = GetComponent<Animator>();
    }

    void Update() {
        if(_player != null) {
            _distance = Vector3.Distance(this.transform.position, _player.transform.position);
            Vector3 _direction = _player.transform.position - this.transform.position; 
            if(!_disableNoise && _distance <= 30) {
                _disableNoise = true;
                PlayEnemyNoise();
            }
            if (_distance < 22 && !_isDead) {
                _enemyAnim.SetTrigger("Walking");
                if (!_player.GetIsFlashlightActive()) { // if player flashlight is off, enemy approaches
                    _enemyAnim.ResetTrigger("Afraid");
                    _speed = 4f;
                    this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
                    transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
                } else { // if player flashlight is on, enemy runs away...
                    if(_direction.x < 0 && _player.GetDirection()) { // ... only if the player is pointing the flashlight in the correct direction
                        _enemyAnim.SetTrigger("Afraid");
                        _speed = 2f;
                        this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position + new Vector3(13, transform.position.y, 0), _speed * Time.deltaTime);
                        checkIfEnemyIsMovingCorrectly();
                    } else {
                        _enemyAnim.ResetTrigger("Afraid");
                        _speed = 4f;
                        transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
                        this.transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
                    }
                }
                if (_distance < 13 && _player.GetIsFlashCameraActive()) { // change to 7
                    if (_direction.x < 0 && _player.GetDirection()) {
                        KillEnemy();
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (!_isDead) {
                _enemyAnim.ResetTrigger("Walking");
                _player.KillPlayer();
            }
        }
    }

    public void checkIfEnemyIsMovingCorrectly() {
        if(this.transform.position.x > _player.transform.position.x + 12 && this.transform.position.x < _player.transform.position.x + 14) {
            transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
        } else if(this.transform.position.x < _player.transform.position.x + 12) {
            transform.localScale = new Vector3(0.29428f, 0.29428f, 0.29428f);
        } else if(this.transform.position.x > _player.transform.position.x + 14) {
            transform.localScale = new Vector3(-0.29428f, 0.29428f, 0.29428f);
        }
    }

    private void KillEnemy() {
        _audioSource.clip = _enemyDeathClip;
        _audioSource.Play();
        _isDead = true;
        GameObject.Find("Enemy_Eyes").SetActive(false);
        this.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine(DestroyGameObject(7.1f));
    }

    private void PlayEnemyNoise() {
        _audioSource.clip = _enemySoundClip;
        _audioSource.Play();
    }

    IEnumerator DestroyGameObject(float seconds) {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
