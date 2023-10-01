using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour{
    private float _speed = 2.5f; //3.5f
    private bool _isJumpActive = false;
    private bool _direction = true; //true is facing right and false is facing left
    private bool _moving = false;
    private Rigidbody2D _rigidbody;
    private Level1Manager _level1Manager;
    [SerializeField]
    private Animator _playerAnim;

    private UIManager _uiManager;

    // Footsteps Audio Variables
    [SerializeField]
    private AudioSource _footstepSource;

    // Jump Audio Variables
    [SerializeField]
    private AudioSource _jumpAudioSource;
    [SerializeField]
    private AudioClip _jumpLandClip;
    [SerializeField]
    private AudioClip _jumpTakeoffClip;
    [SerializeField]
    private GameObject _deathAudioSource;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        if (SceneManager.GetActiveScene().buildIndex == 2) {
            _level1Manager = GameObject.Find("Level_1_Manager").GetComponent<Level1Manager>();
        }
    }

    private void Update() {
        if (!_uiManager.GetIsPaused()) {
            CalculateMovement();
            if (Input.GetKeyDown(KeyCode.Space) && !_isJumpActive) {
                JumpSequence();
            }
            _deathAudioSource.transform.position = this.transform.position;
        }
    }

    /* Play jump landing animation correctly with ground edge collider
     and end the level when player finishes the level */
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Ground")) {
            if(_isJumpActive) {
                _jumpAudioSource.clip = _jumpLandClip;
                _jumpAudioSource.Play();
                _isJumpActive = false;
                _playerAnim.ResetTrigger("Jumping");
            }
        } else if(collision.CompareTag("Finish")) {
            switch (SceneManager.GetActiveScene().buildIndex) {
                case 1:
                    _uiManager.FadeOut(3);
                    break;
                case 2:
                    _uiManager.FadeOut(4);
                    break;
                case 3:
                    _uiManager.FadeOut(1);
                    break;
            }
        } else if (collision.CompareTag("Death")) {
            switch (SceneManager.GetActiveScene().buildIndex) {
                case 1:
                    _uiManager.FadeOut(2);
                    break;
                case 2:
                    _uiManager.FadeOut(3);
                    break;
            }
        } else if (collision.CompareTag("Instruction")) {
            _level1Manager.PlayLightOffInstruction();
        }
    }

    public void JumpSequence() {
        if (_footstepSource.isPlaying) {
            _footstepSource.Pause();
        }
        _jumpAudioSource.clip = _jumpTakeoffClip;
        _jumpAudioSource.Play();
        _isJumpActive = true;
        _playerAnim.SetTrigger("Jumping");
        if (GetDirection()) {
            _rigidbody.AddForce(new Vector2(25, 500)); //_rigidbody.velocity.x
        } else {
            _rigidbody.AddForce(new Vector2(-25, 500)); //_rigidbody.velocity.x
        }       
    }

    /// <summary>
    /// Determines which direction the player is facing
    /// </summary>
    /// <returns>false if the player is facing left and true if the player is facing right</returns>
    public bool GetDirection() {
        return _direction;
    }

    /// <summary>
    /// Determines if the player is moving
    /// </summary>
    /// <returns>false if the player is idle and true if the player is moving</returns>
    public bool GetMoving() {
        return _moving;
    }

    /// <summary>
    /// Calculates the players movement based on input. Audio clips and animations relating to the player are also played here.
    /// </summary>
    private void CalculateMovement() {
        // player controlled light sources change direction depending on players direction of movement
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A)) {
            if (!_footstepSource.isPlaying && !_isJumpActive) {
                _footstepSource.Play();
                _playerAnim.SetTrigger("Walking");
            }
            _direction = false; // facing left
            _moving = true;
            transform.localScale = new Vector3(-0.17f, 0.17f, 0.17f);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D)) {
            if(!_footstepSource.isPlaying && !_isJumpActive) {
                _footstepSource.Play();
                _playerAnim.SetTrigger("Walking");
            }
            _direction = true; // facing right
            _moving = true;
            transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
        }
        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) {
            _footstepSource.Pause();
            _playerAnim.ResetTrigger("Walking");
            _moving = false;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(_speed * Time.deltaTime * new Vector3(horizontalInput, 0, 0));
    }

    public void KillPlayer() {
        _deathAudioSource.GetComponent<AudioSource>().Play();
        switch (SceneManager.GetActiveScene().buildIndex) {
            case 1:
                _uiManager.FadeOut(2);
                break;
            case 2:
                _uiManager.FadeOut(3);
                break;
        }
        Destroy(this.gameObject);
    }
}
