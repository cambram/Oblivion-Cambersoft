using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{
    private float _speed = 2.5f; //3.5f
    private bool _isJumpActive = false;
    private bool _direction = true; //true is facing right and false is facing left
    private bool _moving = false;
    private bool _disableJump = false, _disableMovement = false, _attraction = false;
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
    private GameObject _deathHandler;
    [SerializeField]
    private GameObject _playerGlowLight;

    private Vector3 _deathPos, _facingLeft, _facingRight;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();

        _deathPos = new Vector3(0,0,0);
        _facingLeft = new Vector3(-0.17f, 0.17f, 0.17f);
        _facingRight = new Vector3(0.17f, 0.17f, 0.17f);

        if (SceneManager.GetActiveScene().buildIndex == 1) {
            _disableJump = true; 
            _disableMovement = true;
        } else if (SceneManager.GetActiveScene().buildIndex == 2) {
            _level1Manager = GameObject.Find("Level_1_Manager").GetComponent<Level1Manager>();
        }

        for(int i = 0; i < Object.FindObjectsOfType<Enemy>().Length; i++) {
            if (Vector3.Distance(this.transform.position, Object.FindObjectsOfType<Enemy>()[i].transform.position) < 22) {
                Destroy(Object.FindObjectsOfType<Enemy>()[i].gameObject);
            }
        }
    } 

    private void Update() {
        if (!_uiManager.GetIsPaused()) {
            if (!_disableMovement) { CalculateMovement(); }            
            if (Input.GetKeyDown(KeyCode.Space) && !_isJumpActive && !_disableJump) { JumpSequence(); }
            _deathPos.x = this.transform.position.x + 1.183f;
            _deathPos.y = this.transform.position.y - 0.049f;
            _deathHandler.transform.position = _deathPos;
            _playerGlowLight.transform.position = this.transform.position;
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
                    _uiManager.FadeOut(3, true);
                    break;
                case 2:
                    _uiManager.FadeOut(4, true);
                    break;
                case 3:
                    _uiManager.FadeOut(1, true);
                    break;
            }
        } else if (collision.CompareTag("Death")) {
            switch (SceneManager.GetActiveScene().buildIndex) {
                case 1:
                    _uiManager.FadeOut(2, false);
                    break;
                case 2:
                    _uiManager.FadeOut(3, false);
                    break;
                case 3:
                    _uiManager.FadeOut(4, false);
                    _deathHandler.GetComponent<AudioSource>().Play();
                    _deathHandler.GetComponent<Animator>().SetTrigger("Death");
                    break;
                default: break;
            }
        } else if (collision.CompareTag("Instruction")) {
            _level1Manager.PlayLightOffInstruction();
        } else if (collision.CompareTag("LampSpace")) {
            _attraction = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("LampSpace")) {
            _attraction = false;
        }
    }

    public bool GetAttraction() {
        return _attraction;
    }

    public void EnableJump() {
        _disableJump = false;
    }  
    public void EnableMovement() {
        _disableMovement = false;
    }

    public void DisableJump() {
        _disableJump = true;
        if (_isJumpActive) {
            _isJumpActive = false;
            _playerAnim.ResetTrigger("Jumping");
        }
    }
    public void DisableMovement() {
        _disableMovement = true;
        _footstepSource.Pause();
        _playerAnim.ResetTrigger("Walking");
        _moving = false;
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
            transform.localScale = _facingLeft;
            _deathHandler.transform.localScale = _facingLeft;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D)) {
            if(!_footstepSource.isPlaying && !_isJumpActive) {
                _footstepSource.Play();
                _playerAnim.SetTrigger("Walking");
            }
            _direction = true; // facing right
            _moving = true;
            transform.localScale = _facingRight;
            _deathHandler.transform.localScale = _facingRight;
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) {
            _footstepSource.Pause();
            _playerAnim.ResetTrigger("Walking");
            _moving = false;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(_speed * Time.deltaTime * new Vector3(horizontalInput, 0, 0));
    }

    public void KillPlayer() {
        _deathHandler.GetComponent<AudioSource>().Play();
        _deathHandler.GetComponent<Animator>().SetTrigger("Death");
        switch (SceneManager.GetActiveScene().buildIndex) {
            case 1:
                _uiManager.FadeOut(2, false);
                break;
            case 2:
                _uiManager.FadeOut(3, false);
                break;
            case 3:
                _uiManager.FadeOut(4, false);
                break;
        }
        Destroy(this.gameObject);
    }
}
