using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Player : MonoBehaviour{
    private float _speed = 3f; //3.5f
    [SerializeField]
    private GameObject _flashlight;
    private bool _isFlashlightActive = false, _isJumpActive = false, _isFlashCameraActive = false;
    [SerializeField]
    private GameObject _flashCamera;
    private int _flashChargeCount = 0;
    private bool _direction = true; //true is facing right and false is facing left
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private Animator _flickerAnim;
    [SerializeField]
    private Animator _playerAnim;

    private int _batteryCount;
    private const int _BATTERY = 400;
    private bool _battP1 = false, _battP2 = false, _battP3 = false;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _flashChargeClip;
    [SerializeField]
    private AudioClip _flashlightOnClip;
    [SerializeField]
    private AudioSource _flashlightSource;

    [SerializeField]
    private AudioSource _footstepSource;

    [SerializeField]
    private AudioSource _batterySource;

    private Camera _camera;
    private GameManager _gameManager;

    private void Start() {
        _camera = Camera.main;
        transform.position = new Vector3(-77, -1.7f, 0);
        _flashlight.SetActive(false);
        _flashCamera.SetActive(false);
        _batteryCount = _BATTERY; // 1 battery = 400 units
        _rigidbody = GetComponent<Rigidbody2D>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    private void Update() {
        //camera must not be able to travel left of x = -66 or right of x = 60.3
        if (this.transform.position.x < -71)
            _camera.transform.position = new Vector3(-66, 0, -10);
        else if(this.transform.position.x > 60.3f)
            _camera.transform.position = new Vector3(65.3f, 0, -10);
        else
            _camera.transform.position = new Vector3(transform.position.x + 5, 0, -10);

            CalculateMovement();

        //Mouse follow action
        /*Vector3 mpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        _flashlight.transform.eulerAngles = new Vector3(0, 0, mpos.y*130);*/

        if (Input.GetMouseButtonDown(0)) {
            if(_isFlashlightActive) { // flashlight gets turned off
                Flashlight(false);
            } else { // flashlight gets turned on
                if(_batteryCount > 0) { //this prevents light turning on when battery is flat
                    _flashlightSource.clip = _flashlightOnClip;
                    _flashlightSource.Play();
                    Flashlight(true);
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            FlashCamera();
        }

        BatteryChecker();

        if (Input.GetKeyDown(KeyCode.Space) && !_isJumpActive) {
            if (_footstepSource.isPlaying) {
                _footstepSource.Pause();
            }
            _isJumpActive = true;
            _playerAnim.SetTrigger("Jumping");
            _rigidbody.AddForce(new Vector2(_rigidbody.velocity.x, 500));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Ground") {
            if(_isJumpActive) {
                _footstepSource.Play();
                _isJumpActive = false;
                _playerAnim.ResetTrigger("Jumping");
            }
        } else if(collision.tag == "Finish") {
            _gameManager.BackToMainMenu();
        }
    }

    private void BatteryChecker() {
        if (_batteryCount > 200 && _batteryCount < 300 && !_battP1) {
            _battP1 = true;
            _flickerAnim.SetTrigger("Flicker");
            _batterySource.Play();
            StartCoroutine(BatteryFlickerTriggerReset());
        } else if (_batteryCount > 100 && _batteryCount < 200 && !_battP2) {
            _battP2 = true;
            _flickerAnim.SetTrigger("Flicker");
            _batterySource.Play();
            StartCoroutine(BatteryFlickerTriggerReset());
        } else if (_batteryCount > 0 && _batteryCount < 100 && !_battP3) {
            _battP3 = true;
            _flickerAnim.SetTrigger("Flicker");
            _batterySource.Play();
            StartCoroutine(BatteryFlickerTriggerReset());
        }
    }

    IEnumerator BatteryFlickerTriggerReset() {
        yield return new WaitForSeconds(0.8f);
        _flickerAnim.ResetTrigger("Flicker");
    }

    public bool GetIsFlashlightActive() {
        return _isFlashlightActive;
    }

    public bool GetIsFlashCameraActive() {
        return _isFlashCameraActive;
    }

    /// <summary>
    /// Determines which direction the player is facing.
    /// </summary>
    /// <returns>false if the player is facing left and true if the player is facing right.</returns>
    public bool GetDirection() {
        return _direction;
    }

    private void CalculateMovement() {
        // player controlled light sources change direction depending on players direction of movement
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A)) {
            if (!_footstepSource.isPlaying && !_isJumpActive) {
                _footstepSource.Play();
                _playerAnim.SetTrigger("Walking");
            }
            _direction = false; // facing left
            transform.localScale = new Vector3(-0.17f, 0.17f, 0.17f);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D)) {
            if(!_footstepSource.isPlaying && !_isJumpActive) {
                _footstepSource.Play();
                _playerAnim.SetTrigger("Walking");
            }
            _direction = true; // facing right
            transform.localScale = new Vector3(0.17f, 0.17f, 0.17f);
        }

        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)) {
            _footstepSource.Pause();
            _playerAnim.ResetTrigger("Walking");
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    private void Flashlight(bool x) { // on = true; off = false
        _isFlashlightActive = x;
        _flashlight.SetActive(x);
        if (x) StartCoroutine(BatteryCountdownRoutine());
    }

    IEnumerator BatteryCountdownRoutine() {
        while(true) {
            if(_isFlashlightActive) {
                yield return new WaitForSeconds(0.1f);
                _batteryCount--;
                if (_batteryCount <= 0) { // this is where the flashlight is turned off when battery is flat
                    Flashlight(false);
                    yield break;
                }
            } else {     
                yield break;                
            }
        }
    }

    private void FlashCamera() {
        if(_flashChargeCount > 0) {
            _flashChargeCount--;
            if(_isFlashlightActive) {
                Flashlight(false);
            }
            _flashCamera.SetActive(true);
            _flashlightSource.clip = _flashChargeClip;
            _flashlightSource.Play();
            StartCoroutine(FlashCameraOffRoutine());
            _isFlashCameraActive = true;
        }
    }

    IEnumerator FlashCameraOffRoutine() {
        yield return new WaitForSeconds(0.2f);
        _flashCamera.SetActive(false);
        _isFlashCameraActive = false;
    }

    public void CollectFlashCharge() {
        _flashChargeCount++;
    }

    public void CollectBattery() {
        _batteryCount = _BATTERY;
        _battP1 = false;
        _battP2 = false;
        _battP3 = false;
    }

    public void KillPlayer() {
        _uiManager.DisplayDeath();
        Destroy(this.gameObject);
    }
}
