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
    [SerializeField]
    private GameObject _caveDrips;
    private bool _respawn = false;

    private bool _lanternInstruction = false, _secondEncounter = false, _logFallen = false, _lanternComplete = false, _kKeyEnabled = false, _luxApproaches = false, _umbraCaveApproaches = false, _lightningStrike = false, _secondLuxEncounter = false;
    private bool _soundEffect1 = false, _soundEffect2 = false, _soundEffect3 = false, _soundEffect4 = false, _suspenseDrone = false, _umbraCaveOut = false, _umbraApproach1 = false;

    //A Instruction Variables
    [SerializeField]
    private GameObject _I;
    [SerializeField]
    private Animator _IAnim;

    //D Instruction Variables
    [SerializeField]
    private GameObject _lightOff;
    [SerializeField]
    private Animator _lightOffAnim;

    //Left Click Instruction Variables
    [SerializeField]
    private GameObject _J;
    [SerializeField]
    private Animator _JAnim;

    //Lightning
    [SerializeField]
    private GameObject _lightning;
    [SerializeField]
    private GameObject _effects;

    [SerializeField]
    private GameObject _woodLog;
    private SuspenseAudioManager _suspenseAudioManager;

    private Vector2 _caveCutoff1 = new Vector2(13, 22000);
    private Vector2 _caveCutoff2 = new Vector2(25, 4000);
    private Vector2 _caveCutoff3 = new Vector2(76, 4000);
    private Vector2 _caveCutoff4 = new Vector2(87, 22000);

    private CheckpointManager _checkpointManager;

    private Vector3 _cameraLeftBounds, _cameraRightBounds, _cameraPlayerBounds, _effectsBounds1, _effectsBounds2;


    void Start() {
        Cursor.visible = false;
        _camera = Camera.main;
        _I.SetActive(false);
        _lightOff.SetActive(false);
        _J.SetActive(false);
        _lightning.SetActive(true);
        _lightning.GetComponent<Light2D>().enabled = false;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _environment = GameObject.Find("Environment");
        _lightSources = GameObject.Find("Player").GetComponent<PlayerLightSources>();
        _suspenseAudioManager = GameObject.Find("Suspense_Audio_Manager").GetComponent<SuspenseAudioManager>();
        _lightSources.SetLanternDisabled(true);
        _checkpointManager = GameObject.Find("Checkpoint_Manager").GetComponent<CheckpointManager>();

        _cameraLeftBounds = new Vector3(-108, 0, -10);
        _cameraRightBounds = new Vector3(280, 0, -10);
        _cameraPlayerBounds = new Vector3(_player.transform.position.x + 5, 0, -10);
        _effectsBounds1 = new Vector3(-9, 0, -10);
        _effectsBounds2 = new Vector3(117, 0, -10);

        if (_checkpointManager.GetCurrentCheckpoint() == Vector3.zero) {
            _checkpointManager.SetCurrentCheckpoint(new Vector3(-119, -2f, 0));
        }
        _player.transform.position = _checkpointManager.GetCurrentCheckpoint();

        InitialisePrefabsForLevel();
    }

    void Update() {
        if (_player != null) {
            ConstrainEffects();
            ConstrainCamera();
            //respawn player if they fall
            if (_player.transform.position.y < -12 && !_respawn) {
                _respawn = true;
                _player.KillPlayer();
                _uiManager.FadeOut(3, false);
            }

            if (_player.transform.position.x > 0 && _player.transform.position.x < 2 && !_secondLuxEncounter) {
                _secondLuxEncounter = true;
                _spawnManager.SpawnLux(-18.67f, -0.26f);
                _spawnManager.SpawnLux(-13.22f, 0.56f);
                _spawnManager.SpawnLux(-15.8f, 0.23f);
            }

            if (_player.transform.position.x > -42 && _player.transform.position.x < -20 && !_logFallen) {
                _logFallen = true;
                _woodLog.GetComponent<Rigidbody2D>().gravityScale = 1;
            }

            if(_player.transform.position.x > -86 && _player.transform.position.x < -84 && !_soundEffect1) {
                _soundEffect1 = true;
                _suspenseAudioManager.PlaySuspense1();
            }

            if (_player.transform.position.x > 36 && _player.transform.position.x < 38 && !_soundEffect2) {
                _soundEffect2 = true;
                _suspenseAudioManager.PlaySuspense2();
            }

            if (_player.transform.position.x > 101 && _player.transform.position.x < 103 && !_umbraCaveOut) {
                _umbraCaveOut = true;
                _spawnManager.SpawnUmbra(85.94f, -0.49f);
            }

            if (_player.transform.position.x > 112 && _player.transform.position.x < 114 && !_soundEffect3) {
                _suspenseAudioManager.transform.position = new Vector3(100, 0);
                _soundEffect3 = true;
                _suspenseAudioManager.PlaySuspense1();
            }

            if (_player.transform.position.x > 160 && _player.transform.position.x < 162 && !_soundEffect4) {
                _soundEffect4 = true;
                _suspenseAudioManager.PlaySuspenseDrone2();
            }

            if (_player.transform.position.x > -106 && !_lanternInstruction) {
                _I.SetActive(true);
                _lightSources.SetLanternDisabled(false);
                _lanternInstruction = true;
            }

            if (_player.transform.position.x > 6 && _player.transform.position.x < 8 && !_suspenseDrone) {
                _suspenseDrone = true;
                _suspenseAudioManager.PlaySuspenseDrone1();
            }

            if (_player.transform.position.x > -77 && _player.transform.position.x < -75 && !_secondEncounter) {
                if(_lightSources.GetCurrentLightSource() == 0) {
                    _I.SetActive(false);
                    _I.transform.position = new Vector3(-74, -3);
                    _I.SetActive(true);
                }
                _secondEncounter = true;
                _spawnManager.SpawnUmbra(-54f, 2f);
                _spawnManager.SpawnUmbra(-93f, 0.5f);
                _spawnManager.SpawnUmbra(-95f, 1f);
            }

            if (_player.transform.position.x > 13 && _player.transform.position.x < 25) {
                _environment.GetComponent<AudioLowPassFilter>().cutoffFrequency = SlopeIntercept(_caveCutoff1, _caveCutoff2); //y = mx + b
                _caveDrips.GetComponent<AudioSource>().volume = SlopeIntercept(new Vector2(13, 0), new Vector2(25, 0.65f));
            }
            if (_player.transform.position.x >= 25 && _player.transform.position.x <= 76) {
                _environment.GetComponent<AudioLowPassFilter>().cutoffFrequency = 4000.2f;
            }
            if (_player.transform.position.x > 76 && _player.transform.position.x < 87) {
                _environment.GetComponent<AudioLowPassFilter>().cutoffFrequency = SlopeIntercept(_caveCutoff3, _caveCutoff4); //y = mx + b
                _caveDrips.GetComponent<AudioSource>().volume = SlopeIntercept(new Vector2(76, 0.65f), new Vector2(87, 0));
            }

            if (_player.transform.position.x > -65 && !_lanternComplete) {
                _lanternComplete = true;
                _IAnim.SetTrigger("FadeOut");
                if(_kKeyEnabled) {
                    _JAnim.SetTrigger("FadeOut");
                }
            }

            if (_player.transform.position.x > 34 && _player.transform.position.x < 36 && !_umbraCaveApproaches) {
                _umbraCaveApproaches = true;
                _spawnManager.SpawnUmbra(15f, -3.6f);
                _spawnManager.SpawnLux(13f, -3.6f);
            }

            //lightning strike
            if (_player.transform.position.x > 85 && _player.transform.position.x < 87 && !_lightningStrike) {
                _lightningStrike = true;
                _lightSources.SetIsFlashCameraActive(true);
                _lightning.GetComponent<AudioSource>().Play();
                _lightning.GetComponent<Light2D>().enabled = true;
                StartCoroutine(LightningRoutine());
            }

            if (_player.transform.position.x > 267 && _player.transform.position.x < 269 && !_umbraApproach1) {
                _umbraApproach1 = true;
                _spawnManager.SpawnUmbra(247.89f, 0f);
            }
        }
    }

    public void PlayLightOffInstruction() {
        if (!_luxApproaches) {
            _luxApproaches = true;
            _lightOff.transform.position = new Vector3(_player.transform.position.x + 10, _player.transform.position.y - 5, 0);
            _lightOff.SetActive(true);
        }
    }

    private void ConstrainCamera() {
        if (_player.transform.position.x < -113) {
            _camera.transform.position = _cameraLeftBounds;
        } else if (_player.transform.position.x > 275) {
            _camera.transform.position = _cameraRightBounds;
        } else {
            _cameraPlayerBounds.x = _player.transform.position.x + 5;
            _camera.transform.position = _cameraPlayerBounds;
        }
    }
    private void ConstrainEffects() {
        if(_player.transform.position.x < -14 || _player.transform.position.x >= 112) {
            _effects.transform.position = _camera.transform.position;
        } else if(_player.transform.position.x >= -14 && _player.transform.position.x <= 55) {
            _effects.transform.position = _effectsBounds1;
        } else if(_player.transform.position.x > 55 && _player.transform.position.x < 112) {
            _effects.transform.position = _effectsBounds2;
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
        _spawnManager.SpawnUmbra(-68f, 2.2f);
        _spawnManager.SpawnUmbra(41.15f, -3.81f);
        _spawnManager.SpawnLux(10.6f, 3.2f);
        _spawnManager.SpawnUmbra(125.98f, 1.92f);
        _spawnManager.SpawnUmbra(133.34f, 2.15f);
        _spawnManager.SpawnUmbra(254.05f, -3.2f);
        _spawnManager.SpawnLux(174.19f, 1.21f);
        _spawnManager.SpawnLux(181.35f, 4.71f);
        _spawnManager.SpawnLux(203.2f, -3f);
        _spawnManager.SpawnLux(206.84f, -3f);
        _spawnManager.SpawnLux(273.2f, -3.56f);
        
        /* Collectables */
        _spawnManager.SpawnFlashCharge(-94.04f, -2.76f);
        _spawnManager.SpawnBattery(-74.12f, -0.85f);
        _spawnManager.SpawnFlashCharge(-59f, -0.38f);
        _spawnManager.SpawnFlashCharge(-54f, -0.33f);
        _spawnManager.SpawnBattery(-41f, -1.5f);
        _spawnManager.SpawnFlashCharge(-16.436f, -1.935f);
        _spawnManager.SpawnFlashCharge(13.08f, -6.37f);
        _spawnManager.SpawnFlashCharge(25.04f, -6.67f);
        _spawnManager.SpawnFlashCharge(46.47f, -6.04f);
        _spawnManager.SpawnBattery(75.88f, -3.55f);
        _spawnManager.SpawnFlashCharge(122f, -0.87f);
        _spawnManager.SpawnFlashCharge(139.5f, -1.1f);
        _spawnManager.SpawnBattery(151.28f, -0.97f);
        _spawnManager.SpawnFlashCharge(158f, -0.99f);
        _spawnManager.SpawnFlashCharge(178.55f, 1.48f);
        _spawnManager.SpawnFlashCharge(206, -5.2f);
        _spawnManager.SpawnFlashCharge(238.1f, 3.6f);
        _spawnManager.SpawnFlashCharge(263, -4.8f);
    }

    IEnumerator LightningRoutine() {
        yield return new WaitForSeconds(0.2f);
        _lightning.GetComponent<Light2D>().enabled = false;
        _lightSources.SetIsFlashCameraActive(false);
    }
}
