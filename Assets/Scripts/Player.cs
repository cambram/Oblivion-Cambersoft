using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float _speed = 3.0f;
    [SerializeField]
    private GameObject _flashlight;
    private bool _isFlashlightActive = false;
    [SerializeField]
    private GameObject _flashCamera;
    private int _flashChargeCount = 0;
    private bool _isFlashCameraActive = false;
    private bool _direction = true; //true is facing right and false is facing left

    private int _batteryCount;
    private const int _BATTERY = 400;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _flashlightOnClip;
    private AudioSource _audioSource;

    private Camera _camera;

    private void Start() {
        _camera = Camera.main;
        transform.position = new Vector3(-77, -2.3f, 0);
        _flashlight.SetActive(false);
        _flashCamera.SetActive(false);
        _batteryCount = _BATTERY; // 1 battery = 40 units

        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_audioSource != null) {
            _audioSource.clip = _flashlightOnClip;
        }
    }

    private void Update() {
        _camera.transform.position = new Vector3(transform.position.x, 0, -10);
        CalculateMovement();
        if (Input.GetMouseButtonDown(0)) {
            if(_isFlashlightActive) { // flashlight gets turned off
                Flashlight(false);
            } else { // flashlight gets turned on
                if(_batteryCount > 0) {
                    _audioSource.Play();
                    Flashlight(true);
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            FlashCamera();
        }

        if(_batteryCount > 300) {
            _uiManager.UpdateBattery(4);
        } else if(_batteryCount > 200 && _batteryCount < 300) {
            _uiManager.UpdateBattery(3);
        } else if(_batteryCount > 100 && _batteryCount < 200) {
            _uiManager.UpdateBattery(2);
        } else if(_batteryCount > 0 && _batteryCount < 100) {
            _uiManager.UpdateBattery(1);
        } else if(_batteryCount == 0) {
            _uiManager.UpdateBattery(0);
        }
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
            _direction = false; // facing left
            transform.localScale = new Vector3(-0.15f, 0.15f, 0.15f);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D)) {
            _direction = true; // facing right
            transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3(horizontalInput, 0, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        //camera must not be able to travel further than x = -66
        if (_camera.transform.position.x < -66)
            _camera.transform.position = new Vector3(-66, 0, -10);
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
                if (_batteryCount <= 0) {
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
    }

    public void KillPlayer() {
        _uiManager.DisplayDeath();
        Destroy(this.gameObject);
    }
}
