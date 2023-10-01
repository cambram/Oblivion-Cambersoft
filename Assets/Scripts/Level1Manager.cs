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

    private bool _secondEncounter = false, _logFallen = false, _lanternComplete = false, _kKeyEnabled = false, _luxApproaches = false, _umbraCaveApproaches = false, _lightningStrike = false, _secondLuxEncounter = false;

    //A Instruction Variables
    [SerializeField]
    private GameObject _W;
    [SerializeField]
    private Animator _WAnim;

    //D Instruction Variables
    [SerializeField]
    private GameObject _lightOff;
    [SerializeField]
    private Animator _lightOffAnim;

    //Left Click Instruction Variables
    [SerializeField]
    private GameObject _K;
    [SerializeField]
    private Animator _KAnim;

    //Lightning
    [SerializeField]
    private GameObject _lightning;
    [SerializeField]
    private GameObject _effects;

    [SerializeField]
    private GameObject _woodLog;

    private Vector2 _caveCutoff1 = new Vector2(13, 22000);
    private Vector2 _caveCutoff2 = new Vector2(25, 4000);
    private Vector2 _caveCutoff3 = new Vector2(76, 4000);
    private Vector2 _caveCutoff4 = new Vector2(87, 22000);

    private Vector3 _checkpoint1, _checkpoint2, _checkpoint3, _checkpoint4;

    void Start() {
        Cursor.visible = false;
        _camera = Camera.main;
        _W.SetActive(false);
        _lightOff.SetActive(false);
        _K.SetActive(false);
        _lightning.SetActive(true);
        _lightning.GetComponent<Light2D>().enabled = false;
        _checkpoint1 = new Vector3(-52.95f, 0.7f, 0);
        _checkpoint2 = new Vector3(15.65f, -6.5f, 0);
        _checkpoint3 = new Vector3(89.51f, -2.25f, 0);
        _checkpoint4 = new Vector3(191.15f, 5.9f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _environment = GameObject.Find("Environment");
        _lightSources = GameObject.Find("Player").GetComponent<PlayerLightSources>();
        _player.transform.position = new Vector3(-119, -2, 0);
        _lightSources.SetLanternDisabled(true);
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

            if (_player.transform.position.x > 0 && !_secondLuxEncounter) {
                _secondLuxEncounter = true;
                _spawnManager.SpawnLux(-18.67f, -0.26f);
                _spawnManager.SpawnLux(-13.22f, 0.56f);
                _spawnManager.SpawnLux(-15.8f, 0.23f);
            }

            if ( _player.transform.position.x > -42 && !_logFallen) {
                _logFallen = true;
                _woodLog.GetComponent<Rigidbody2D>().gravityScale = 1;
            }

            if (_player.transform.position.x > -77 && !_secondEncounter) {
                _W.SetActive(true);
                _lightSources.SetLanternDisabled(false);
                if(!_lightSources.GetIsAnyLightActive()) {
                    _kKeyEnabled = true;
                    _K.SetActive(true); 
                }
                _secondEncounter = true;
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
                _WAnim.SetTrigger("FadeOut");
                if(_kKeyEnabled) {
                    _KAnim.SetTrigger("FadeOut");
                }
            }

            if (_player.transform.position.x > 34 && !_umbraCaveApproaches) {
                _umbraCaveApproaches = true;
                _spawnManager.SpawnUmbra(15f, -3.6f);
                _spawnManager.SpawnLux(13f, -3.6f);
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

    private void PlayerDeath() { //call this method instead of kill player from enemy script using current build index to see which level to call the methdo on
        if(_player.transform.position.x < _checkpoint1.x) {
            _uiManager.FadeOut(3);
        } else if(_player.transform.position.x >= _checkpoint1.x && _player.transform.position.x < _checkpoint2.x) {
            _player.transform.position = _checkpoint1;
        } else if (_player.transform.position.x >= _checkpoint2.x && _player.transform.position.x < _checkpoint3.x) {
            _player.transform.position = _checkpoint2;
        } else if (_player.transform.position.x >= _checkpoint3.x && _player.transform.position.x < _checkpoint4.x) {
            _player.transform.position = _checkpoint3;
        } else if (_player.transform.position.x >= _checkpoint4.x) {
            _player.transform.position = _checkpoint4;
        }  
    }

    public void PlayLightOffInstruction() {
        if (!_luxApproaches) {
            _luxApproaches = true;
            _lightOff.transform.position = new Vector3(_player.transform.position.x + 10, _player.transform.position.y - 4, 0);
            _lightOff.SetActive(true);
        }
    }

    private void ConstrainCamera() {
        if (_player.transform.position.x < -113) {
            _camera.transform.position = new Vector3(-108, 0, -10);
        } else if (_player.transform.position.x > 275) { //105
            _camera.transform.position = new Vector3(280, 0, -10);
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
        _spawnManager.SpawnUmbra(-80f, 2.2f);
        _spawnManager.SpawnUmbra(-68f, 2.2f);
        _spawnManager.SpawnUmbra(41.15f, -3.81f);
        _spawnManager.SpawnLux(10.6f, 3.2f);
        //_spawnManager.SpawnUmbra(112.93f, 1.2f);
        _spawnManager.SpawnUmbra(125.98f, 1.92f);
        _spawnManager.SpawnUmbra(133.34f, 2.15f);
        _spawnManager.SpawnUmbra(240.05f, -0.16f);
        _spawnManager.SpawnLux(174.19f, 1.21f);
        _spawnManager.SpawnLux(181.35f, 4.71f);
        _spawnManager.SpawnLux(200.04f, -2.6f);
        _spawnManager.SpawnLux(202.73f, -2.6f);
        _spawnManager.SpawnLux(273.2f, -3.56f);
        /* Collectables */
        _spawnManager.SpawnFlashCharge(-59f, -0.15f);
        _spawnManager.SpawnFlashCharge(-54f, 0f);
        _spawnManager.SpawnFlashCharge(46.47f, -5.64f);
        _spawnManager.SpawnFlashCharge(-9.4f, -1.32f);
        _spawnManager.SpawnFlashCharge(-94.04f, -2.76f);
        _spawnManager.SpawnFlashCharge(25.04f, -6.4f);
        _spawnManager.SpawnFlashCharge(13.08f, -6.37f);
        _spawnManager.SpawnBattery(-41f, -1.5f);
        _spawnManager.SpawnBattery(-74.12f, -0.85f);
        _spawnManager.SpawnBattery(75.88f, -3.55f);
        _spawnManager.SpawnFlashCharge(178.55f, 1.66f);
        _spawnManager.SpawnFlashCharge(158f, -0.7f);
        _spawnManager.SpawnBattery(151.28f, -0.97f);
        _spawnManager.SpawnFlashCharge(139.5f, -0.88f);
        _spawnManager.SpawnFlashCharge(122f, -0.138f);
    }

    IEnumerator LightningRoutine() {
        yield return new WaitForSeconds(0.2f);
        _lightning.GetComponent<Light2D>().enabled = false;
        _lightSources.SetIsFlashCameraActive(false);
    }
}
