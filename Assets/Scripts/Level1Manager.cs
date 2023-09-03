using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Level1Manager : MonoBehaviour
{
    private Camera _camera;
    private Player _player;
    private UIManager _uiManager;
    private GameObject _environment;
    private SpawnManager _spawnManager;
    private PlayerLightSources _lightSources;
    private bool _respawn = false;

    private bool _firstEncounter = false, _lanternComplete = false, _leftClickEnabled = false, _luxApproaches = false, _umbraCaveApproaches = false, _lightningStrike = false;

    //A Instruction Variables
    [SerializeField]
    private GameObject _F;
    [SerializeField]
    private Animator _FAnim;

    //D Instruction Variables
    [SerializeField]
    private GameObject _lightOff;
    [SerializeField]
    private Animator _lightOffAnim;

    //Left Click Instruction Variables
    [SerializeField]
    private GameObject _leftClick;
    [SerializeField]
    private Animator _leftClickAnim;

    //Lightning
    [SerializeField]
    private GameObject _lightning;
    [SerializeField]
    private GameObject _effects;

    private Vector2 _caveCutoff1 = new Vector2(13, 22000);
    private Vector2 _caveCutoff2 = new Vector2(25, 4000);
    private Vector2 _caveCutoff3 = new Vector2(76, 4000);
    private Vector2 _caveCutoff4 = new Vector2(87, 22000);

    void Start() {
        Cursor.visible = false;
        _camera = Camera.main;
        _F.SetActive(false);
        _lightOff.SetActive(false);
        _leftClick.SetActive(false);
        _lightning.SetActive(true);
        _lightning.GetComponent<Light2D>().enabled = false;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _environment = GameObject.Find("Environment");
        _lightSources = GameObject.Find("Player").GetComponent<PlayerLightSources>();
        _player.transform.position = new Vector3(-119, -2, 0);
        InitialisePrefabsForLevel();
    }

    void Update() {
        if (_player != null) {
            ConstrainEffects();
            ConstrainCamera();
            //respawn player if they fall
            if (_player.transform.position.y < -12 && !_respawn) {
                _respawn = true;
                _uiManager.FadeOut(3);
            }

            if (_player.transform.position.x > -77 && !_firstEncounter) {
                _F.SetActive(true);
                if(!_lightSources.GetIsAnyLightActive()) { 
                    _leftClickEnabled = true;
                    _leftClick.SetActive(true); 
                }
                _firstEncounter = true;
                _spawnManager.SpawnUmbra(-54f, 2f);
                _spawnManager.SpawnUmbra(-93f, 0.5f);
            }

            if (_player.transform.position.x > 13 && _player.transform.position.x < 25) {
                _environment.GetComponent<AudioLowPassFilter>().cutoffFrequency = SlopeIntercept(_caveCutoff1, _caveCutoff2); //y = mx + b
            }
            if (_player.transform.position.x > 76 && _player.transform.position.x < 87) {
                _environment.GetComponent<AudioLowPassFilter>().cutoffFrequency = SlopeIntercept(_caveCutoff3, _caveCutoff4); //y = mx + b
            }

            if (_player.transform.position.x > -65 && !_lanternComplete) {
                _lanternComplete = true;
                _FAnim.SetTrigger("FadeOut");
                if(_leftClickEnabled) {
                    _leftClickAnim.SetTrigger("FadeOut");
                }
            }

            if (_player.transform.position.x > 34 && !_umbraCaveApproaches) {
                _umbraCaveApproaches = true;
                _spawnManager.SpawnUmbra(15f, -3.6f);
            }

            if (_player.transform.position.x > 85 && !_lightningStrike) {
                _lightningStrike = true;
                _lightSources.SetIsFlashCameraActive(true);
                _lightning.GetComponent<AudioSource>().Play();
                _lightning.GetComponent<Light2D>().enabled = true;
                StartCoroutine(LightningRoutine());
            }
        }
    }

    public void PlayLightOffInstruction() {
        if (!_luxApproaches) {
            _luxApproaches = true;
            _lightOff.transform.position = new Vector3(_player.transform.position.x + 4, _player.transform.position.y - 2, 0);
            _lightOff.SetActive(true);
        }
    }

    private void ConstrainCamera() {
        if (_player.transform.position.x < -113) {
            _camera.transform.position = new Vector3(-108, 0, -10);
        } else if (_player.transform.position.x > 105) {
            _camera.transform.position = new Vector3(110, 0, -10);
        } else {
            _camera.transform.position = new Vector3(_player.transform.position.x + 5, 0, -10);
        }
    }
    private void ConstrainEffects() {
        if(_player.transform.position.x < -14 || _player.transform.position.x >= 112) {
            _effects.transform.position = _camera.transform.position;
        } else if(_player.transform.position.x >= -14 && _player.transform.position.x <= 55) {
            _effects.transform.position = new Vector3(-9, 0, -10);
        } else if(_player.transform.position.x > 55 && _player.transform.position.x < 112) {
            _effects.transform.position = new Vector3(117, 0, -10);
        } 
    }

    private float SlopeIntercept(Vector2 one, Vector2 two) {
        float m = (two.y - one.y) / (two.x - one.x);
        float b = one.y + (-(m * one.x));
        return (m * _player.transform.position.x) + b;
    }

    private void InitialisePrefabsForLevel() {
        /* Enemies */
        _spawnManager.SpawnUmbra(88f, -0.3f);
        _spawnManager.SpawnUmbra(91f, -0.3f);
        _spawnManager.SpawnLux(10.6f, 3.2f);
        //_spawnManager.SpawnUmbra(94f, 0.36f);
        /* Collectables */
        _spawnManager.SpawnFlashCharge(-59f, -0.15f);
        _spawnManager.SpawnFlashCharge(-54f, 0f);
        _spawnManager.SpawnFlashCharge(46.47f, -5.64f);
        _spawnManager.SpawnFlashCharge(-1.87f, -0.19f);
        _spawnManager.SpawnBattery(-41f, -1.5f);
    }

    IEnumerator LightningRoutine() {
        yield return new WaitForSeconds(0.2f);
        _lightning.GetComponent<Light2D>().enabled = false;
        _lightSources.SetIsFlashCameraActive(false);
    }
}
