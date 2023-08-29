using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour{
    private float _speed = 2.5f; //3.5f
    [SerializeField]
    private GameObject _flashlight;
    [SerializeField]
    private GameObject _lantern;
    private bool _isFlashlightActive = false, _isJumpActive = false, _isFlashCameraActive = false, _isLanternActive = false;
    [SerializeField]
    private GameObject _flashCamera;
    private int _flashChargeCount = 0;
    private bool _direction = true; //true is facing right and false is facing left
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private Animator _flickerAnim;
    [SerializeField]
    private Animator _playerAnim;

    //Battery Percentage Variables
    private int _batteryCount;
    private const int _BATTERY = 400;
    private bool _battP1 = false, _battP2 = false, _battP3 = false;

    private UIManager _uiManager;

    // Flashlight/Charge Audio Variables
    [SerializeField]
    private AudioClip _flashChargeClip;
    [SerializeField]
    private AudioSource _batterySource;
    [SerializeField]
    private AudioClip _flashlightOnClip;
    [SerializeField]
    private AudioSource _flashlightSource;

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

    private void Start() {
        _flashlight.SetActive(false);
        _flashCamera.SetActive(false);
        _lantern.SetActive(false);
        _batteryCount = _BATTERY; // 1 battery = 400 units
        _rigidbody = GetComponent<Rigidbody2D>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    private void Update() {
        CalculateMovement();
        BatteryChecker();
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
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumpActive) {
            JumpSequence();
        }

        //Mouse follow action
        /*Vector3 mpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        _flashlight.transform.eulerAngles = new Vector3(0, 0, mpos.y*130);*/
    }

    /* Play jump landing animation correctly with ground edge collider
     and end the level when player finishes the level */
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Ground") {
            if(_isJumpActive) {
                _jumpAudioSource.clip = _jumpLandClip;
                _jumpAudioSource.Play();
                _isJumpActive = false;
                _playerAnim.ResetTrigger("Jumping");
            }
        } else if(collision.tag == "Finish") {
            switch (SceneManager.GetActiveScene().buildIndex) {
                case 1:
                    _uiManager.FadeOut(3);
                    break;
                case 2:
                    _uiManager.FadeOut(1);
                    break;
            }
        } else if (collision.tag == "Death") {
            switch (SceneManager.GetActiveScene().buildIndex) {
                case 1:
                    _uiManager.FadeOut(2);
                    break;
                case 2:
                    _uiManager.FadeOut(3);
                    break;
            }
        }
    }

    /// <summary>
    /// Checks the players battery count and flickers the battery based on the battery level.
    /// </summary>
    private void BatteryChecker() {
        if (_batteryCount > 200 && _batteryCount < 300 && !_battP1) {
            _battP1 = true;
            FlickerFlashlight();
        } else if (_batteryCount > 100 && _batteryCount < 200 && !_battP2) {
            _battP2 = true;
            FlickerFlashlight();
        } else if (_batteryCount > 0 && _batteryCount < 100 && !_battP3) {
            _battP3 = true;
            FlickerFlashlight();
        }
    }

    /// <summary>
    /// Resets the players battery level to full.
    /// </summary>
    public void CollectBattery() {
        _batteryCount = _BATTERY;
        _battP1 = false;
        _battP2 = false;
        _battP3 = false;
    }

    public void FlickerFlashlight() {
        _flickerAnim.SetTrigger("Flicker");
        _batterySource.Play();
        StartCoroutine(BatteryFlickerTriggerReset());
    }

    public bool GetIsFlashlightActive() {
        return _isFlashlightActive;
    }

    public bool GetIsFlashCameraActive() {
        return _isFlashCameraActive;
    }

    public bool GetIsLanternActive() {
        return _isLanternActive;
    }

    public void JumpSequence() {
        if (_footstepSource.isPlaying) {
            _footstepSource.Pause();
        }
        _jumpAudioSource.clip = _jumpTakeoffClip;
        _jumpAudioSource.Play();
        _isJumpActive = true;
        _playerAnim.SetTrigger("Jumping");
        _rigidbody.AddForce(new Vector2(_rigidbody.velocity.x, 500));
    }

    /// <summary>
    /// Determines which direction the player is facing.
    /// </summary>
    /// <returns>false if the player is facing left and true if the player is facing right.</returns>
    public bool GetDirection() {
        return _direction;
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

    /// <summary>
    /// Turns the players flashlight on or off. 
    /// </summary>
    /// <param name="x">true = on; false = off</param>
    private void Flashlight(bool x) {
        _isFlashlightActive = x;
        _flashlight.SetActive(x);
        if (x) StartCoroutine(BatteryCountdownRoutine());
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

    public void CollectFlashCharge() {
        _flashChargeCount++;
    }

    public void KillPlayer() {
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

    //IEnumerators
    IEnumerator BatteryFlickerTriggerReset() {
        yield return new WaitForSeconds(0.8f);
        _flickerAnim.ResetTrigger("Flicker");
    }

    IEnumerator FlashCameraOffRoutine() {
        yield return new WaitForSeconds(0.2f);
        _flashCamera.SetActive(false);
        _isFlashCameraActive = false;
    }

    IEnumerator BatteryCountdownRoutine() {
        while (true) {
            if (_isFlashlightActive) {
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
}
