using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLightSources : MonoBehaviour {
    [SerializeField]
    private GameObject _flashlight;
    [SerializeField]
    private GameObject _lantern;
    [SerializeField]
    private GameObject _flashlightSprite;
    [SerializeField]
    private GameObject _lanternSprite;
    [SerializeField]
    private GameObject _flashCamera;
    [SerializeField]
    private GameObject _FC;
    [SerializeField]
    private Animator _flickerFlashlightAnim;
    [SerializeField]
    private Animator _flickerLanternAnim;
    [SerializeField]
    private GameObject _batteryLife;
    [SerializeField]
    private Animator _batteryLifeAnim;

    private int _flashChargeCount = 0;
    private int _currentLightSource = 0; //0 = flashlight, 1 = lantern;
    private bool _isFlashlightActive = false, _isLanternActive = false, _isFlashCameraActive = false, _toggleFC = false;

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
    private AudioClip _swapClip;
    [SerializeField]
    private AudioSource _flashlightSource;

    private UIManager _uiManager;

    private void Start() {
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _flashlight.SetActive(false);
        _flashCamera.SetActive(false);
        _flashlightSprite.SetActive(true);
        _FC.SetActive(false);
        _batteryLife.GetComponent<Slider>().value = _BATTERY;
        _batteryLife.SetActive(false);
        _batteryCount = _BATTERY; // 1 battery = 400 units
        if (_lantern != null) {
            _lanternSprite.SetActive(false);
            _lantern.SetActive(false);
        }
    }

    private void Update() {
        //cheat codes
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            CollectBattery();
        }
        IndicateFlashCharge();

        if (!_uiManager.GetIsPaused()) {        
            if(_lantern != null && Input.GetKeyDown(KeyCode.F)) {
                _toggleFC = !_toggleFC;
                Toggle();
            }
            BatteryChecker();
            if (Input.GetKeyDown(KeyCode.K)) {
                if (_isFlashlightActive || _isLanternActive) { // flashlight gets turned off
                    Flashlight(false);
                } else { // flashlight gets turned on
                    if (_batteryCount > 0) { //this prevents light turning on when battery is flat
                        _flashlightSource.clip = _flashlightOnClip;
                        _flashlightSource.Play();
                        Flashlight(true);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.L)) {
                FlashCamera();
            }
        }
            //Mouse follow action
            /*Vector3 mpos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
            _flashlight.transform.eulerAngles = new Vector3(0, 0, mpos.y*130);*/
    }

    private void IndicateFlashCharge() {
        if (_flashChargeCount > 0 && !_toggleFC) {
            if (_currentLightSource == 0) {
                _toggleFC = true;
                _FC.SetActive(true);
            } else {
                _toggleFC = true;
                _FC.SetActive(false);
            }
        } else if (_flashChargeCount <= 0 && _toggleFC) {
            if (_currentLightSource == 0) {
                _toggleFC = false;
                _FC.SetActive(false);
            } else {
                _toggleFC = false;
                _FC.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Checks the players battery count and flickers the battery based on the battery level.
    /// </summary>
    private void BatteryChecker() {
        _batteryLife.GetComponent<Slider>().value = _batteryCount;
        if (_batteryCount > 200 && _batteryCount < 300 && !_battP1) {
            _battP1 = true;
            CheckFlicker();
        } else if (_batteryCount > 100 && _batteryCount < 200 && !_battP2) {
            _battP2 = true;
            CheckFlicker();
        } else if (_batteryCount > 0 && _batteryCount < 100 && !_battP3) {
            _battP3 = true;
            CheckFlicker();
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

    private void Toggle() {
        if (_currentLightSource == 0) { // means current is flashlight 
            if (_isFlashlightActive) {
                _isFlashlightActive = false;
                _flashlight.SetActive(false);
                _currentLightSource = 1; // toggles to lantern
                _isLanternActive = true;
                _lantern.SetActive(true);
                _flashlightSprite.SetActive(false);
                _lanternSprite.SetActive(true);
            } else {
                _currentLightSource = 1; // toggles to lantern
                _flashlightSprite.SetActive(false);
                _lanternSprite.SetActive(true);
            }
            _flashlightSource.clip = _swapClip;
            _flashlightSource.Play();
        } else { // means current is lantern
            if (_isLanternActive) {
                _isLanternActive = false;
                _lantern.SetActive(false);
                _currentLightSource = 0; // toggles to flashlight
                _isFlashlightActive = true;
                _flashlight.SetActive(true);
                _flashlightSprite.SetActive(true);
                _lanternSprite.SetActive(false);
            } else {
                _currentLightSource = 0; // toggles to flashlight
                _flashlightSprite.SetActive(true);
                _lanternSprite.SetActive(false);
            }
            _flashlightSource.clip = _swapClip;
            _flashlightSource.Play();
        }
    }

    public void CollectFlashCharge() {
        _flashChargeCount++;
    }

    public void FlickerFlashlight(int light) {
        if(light == 0) {
            _flickerFlashlightAnim.SetTrigger("Flicker");
            _batterySource.Play();
            StartCoroutine(BatteryFlickerTriggerReset());
        } else if(light == 1) {
            _flickerLanternAnim.SetTrigger("Flicker");
            _batterySource.Play();
            StartCoroutine(BatteryFlickerTriggerReset());
        }

    }

    private void CheckFlicker() {
        switch(_currentLightSource) {
            case 0: // flashlight
                FlickerFlashlight(0);
                break;
            case 1: // lantern
                FlickerFlashlight(1);
                break;
            default: break;
        }
    }

    public bool GetIsFlashlightActive() {
        return _isFlashlightActive;
    }

    public bool GetIsLanternActive() {
        return _isLanternActive;
    }

    public bool GetIsAnyLightActive() {
        return _isLanternActive || _isFlashlightActive;
    }

    public bool GetIsFlashCameraActive() {
        return _isFlashCameraActive;
    }

    public void SetIsFlashCameraActive(bool x) {
        _isFlashCameraActive = x;
    }

    /// <summary>
    /// Returns the current light source
    /// </summary>
    /// <returns>0 = flashlight; 1 = lantern</returns>
    public int GetCurrentLightSource() {
        return _currentLightSource;
    }

    /// <summary>
    /// Turns the players flashlight on or off. 
    /// </summary>
    /// <param name="x">true = on; false = off</param>
    public void Flashlight(bool x) {
        _batteryLife.SetActive(x);
        if (!x) _batteryLifeAnim.SetTrigger("FadeOut");
        switch (_currentLightSource) {
            case 0:
                _isFlashlightActive = x;
                _flashlight.SetActive(x);
                if (x) StartCoroutine(BatteryCountdownRoutine());
                break;
            case 1:
                _isLanternActive = x;
                _lantern.SetActive(x);
                if (x) StartCoroutine(BatteryCountdownRoutine());
                break;
            default: break;
        }
    }

    private void FlashCamera() {
        if (_flashChargeCount > 0) {
            _flashChargeCount--;
            if (_isFlashlightActive || _isLanternActive) {
                Flashlight(false);
            }
            _flashCamera.SetActive(true);
            _flashlightSource.clip = _flashChargeClip;
            _flashlightSource.Play();
            _isFlashCameraActive = true;
            StartCoroutine(FlashCameraOffRoutine());
        }
    }

    //IEnumerators
    IEnumerator BatteryFlickerTriggerReset() {
        yield return new WaitForSeconds(0.8f);
        switch (_currentLightSource) {
            case 0: // flashlight
                _flickerFlashlightAnim.ResetTrigger("Flicker");
                break;
            case 1: // lantern
                _flickerLanternAnim.ResetTrigger("Flicker");
                break;
            default: break;
        }
    }

    IEnumerator FlashCameraOffRoutine() {
        yield return new WaitForSeconds(0.2f);
        _flashCamera.SetActive(false);
        _isFlashCameraActive = false;
    }

    IEnumerator BatteryCountdownRoutine() {
        while (true) {
            if (_isFlashlightActive || _isLanternActive) {
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

