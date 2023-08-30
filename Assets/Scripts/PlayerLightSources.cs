using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerLightSources : MonoBehaviour {
    [SerializeField]
    private GameObject _flashlight;
    [SerializeField]
    private GameObject _flashCamera;
    [SerializeField]
    private Animator _flickerAnim;

    private int _flashChargeCount = 0;
    private bool _isFlashlightActive = false, _isFlashCameraActive = false;

    //Battery Percentage Variables
    private int _batteryCount;
    private const int _BATTERY = 400;
    private bool _battP1 = false, _battP2 = false, _battP3 = false;

    // Flashlight/Charge Audio Variables
    [SerializeField]
    private AudioClip _flashChargeClip;
    [SerializeField]
    private AudioSource _batterySource;
    [SerializeField]
    private AudioClip _flashlightOnClip;
    [SerializeField]
    private AudioSource _flashlightSource;

    private void Start() {
        _flashlight.SetActive(false);
        _flashCamera.SetActive(false);
        _batteryCount = _BATTERY; // 1 battery = 400 units
    }

    private void Update() {
        BatteryChecker();
        if (Input.GetMouseButtonDown(0)) {
            if (_isFlashlightActive) { // flashlight gets turned off
                Flashlight(false);
            } else { // flashlight gets turned on
                if (_batteryCount > 0) { //this prevents light turning on when battery is flat
                    _flashlightSource.clip = _flashlightOnClip;
                    _flashlightSource.Play();
                    Flashlight(true);
                }
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            FlashCamera();
        }

        //Mouse follow action
        /*Vector3 mpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        _flashlight.transform.eulerAngles = new Vector3(0, 0, mpos.y*130);*/
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

    public void CollectFlashCharge() {
        _flashChargeCount++;
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
        if (_flashChargeCount > 0) {
            _flashChargeCount--;
            if (_isFlashlightActive) {
                Flashlight(false);
            }
            _flashCamera.SetActive(true);
            _flashlightSource.clip = _flashChargeClip;
            _flashlightSource.Play();
            StartCoroutine(FlashCameraOffRoutine());
            _isFlashCameraActive = true;
        }
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

