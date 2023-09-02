using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    //A Instruction Variables
    [SerializeField]
    private GameObject _A;
    [SerializeField]
    private Animator _AAnim;

    //D Instruction Variables
    [SerializeField]
    private GameObject _D;
    [SerializeField]
    private Animator _DAnim;

    //Space Instruction Variables
    [SerializeField]
    private GameObject _space;
    [SerializeField]
    private Animator _spaceAnim;

    //Left Click Instruction Variables
    [SerializeField]
    private GameObject _leftClick;
    [SerializeField]
    private Animator _leftClickAnim;

    //Right Click Instruction Variables
    [SerializeField]
    private GameObject _rightClick;
    [SerializeField]
    private Animator _rightClickAnim;

    //Battery Variable
    private bool _batteryBypass = true;

    private bool _flashlightInstructionComplete = false, _movementInstructionComplete = false, _jumpInstructionShow = false, _jumpInstructionComplete = false, _scareOffEnemyInstruction = false;

    private bool _respawn = false;
    private Player _player;
    private PlayerLightSources _lightSources;
    private Camera _camera;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameObject _environment;
    //private Vector3 _checkpoint1, _checkpoint2;

    private Vector2 _caveCutoff1 = new Vector2(4, 22000);
    private Vector2 _caveCutoff2 = new Vector2(14, 1000);
    private Vector2 _caveCutoff3 = new Vector2(30, 1000);
    private Vector2 _caveCutoff4 = new Vector2(44, 22000);

    void Start() {
        Cursor.visible = false;
        _camera = Camera.main;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _lightSources = GameObject.Find("Player").GetComponent<PlayerLightSources>();
        _environment = GameObject.Find("Environment");
        _player.transform.position = new Vector3(-77, -1.7f, 0);
        //_checkpoint1 = new Vector3(-10.4f, 2.7f, 0);
        //_checkpoint2 = new Vector3(-1.84f, -1.44f, 0);
        InitialisePrefabsForLevel();
        SetAllInstructionsActiveFalse();
        StartCoroutine(FlashlightInstruction());
    }

    void Update() {
        if (_player != null) {
            ConstrainCamera();
            //battery bypass at beginning for battery dying instruction
            if (_player.transform.position.x < -48 && _batteryBypass) {
                _lightSources.CollectBattery();
            }

            if (_player.transform.position.x > -48 && _batteryBypass) {
                if (_lightSources.GetIsFlashlightActive()) {
                    _batteryBypass = false;
                    _lightSources.FlickerFlashlight();
                } else {
                    _lightSources.Flashlight(true);
                    _batteryBypass = false;
                    _lightSources.FlickerFlashlight();
                }
                
            }

            //respawn player if they fall
            if (_player.transform.position.y < -12 && !_respawn) {
                _respawn = true;
                _uiManager.FadeOut(2);
            }

            //remove flashlight instruction
            if (Input.GetMouseButtonDown(0) && !_flashlightInstructionComplete) {
                _flashlightInstructionComplete = true;
                _leftClickAnim.SetTrigger("FadeOut");
                StartCoroutine(MovementInstruction());
            }

            //Fade out movement instructions
            if (_player.transform.position.x > -59 && !_movementInstructionComplete) {
                _movementInstructionComplete = true;
                _AAnim.SetTrigger("FadeOut");
                _DAnim.SetTrigger("FadeOut");
            }

            //Show jump instruction
            if (_player.transform.position.x > -20 && !_jumpInstructionShow) {
                _jumpInstructionShow = true;
                _space.SetActive(true);
            }

            //Fade out jump instruction
            if (_player.transform.position.x > -1 && !_jumpInstructionComplete) {
                _jumpInstructionComplete = true;
                _spaceAnim.SetTrigger("FadeOut");
                _leftClick.SetActive(false);
                _leftClick.transform.position = new Vector3(17.5f, -3.8f, 0);
                _leftClick.SetActive(true);
            }

            //Show the right click icon to kill enemy (link this with flash charge somehow)
            if (_player.transform.position.x > 10 && !_scareOffEnemyInstruction) {
                _scareOffEnemyInstruction = true;
                _leftClickAnim.SetTrigger("FadeOut");
                _rightClick.SetActive(true);
            }

            //Hide right click icon
            if (_player.transform.position.x > 33) {
                _rightClickAnim.SetTrigger("FadeOut");
            }

            if (_player.transform.position.x >= 4 && _player.transform.position.x < 14) {
                _environment.GetComponent<AudioLowPassFilter>().cutoffFrequency = SlopeIntercept(_caveCutoff1, _caveCutoff2); //y = mx + b
            }
            if (_player.transform.position.x > 30 && _player.transform.position.x < 44) {
                _environment.GetComponent<AudioLowPassFilter>().cutoffFrequency = SlopeIntercept(_caveCutoff3, _caveCutoff4); //y = mx + b
            }
        }
    }

    private void InitialisePrefabsForLevel() {
        /* Enemies */
        _spawnManager.SpawnUmbra(22.23f, 0.78f);
        _spawnManager.SpawnUmbra(74f, 0.5f);
        _spawnManager.SpawnUmbra(69f, 1f);
        /* Collectables */
        _spawnManager.SpawnFlashCharge(24.3f, -1.87f);
        _spawnManager.SpawnFlashCharge(46.4f, -0.4f);
        _spawnManager.SpawnBattery(-30.1f, -0.7f);
    }

    private void SetAllInstructionsActiveFalse() {
        _A.SetActive(false);
        _D.SetActive(false);
        _space.SetActive(false);
        _leftClick.SetActive(false);
        _rightClick.SetActive(false);
    }

    private float SlopeIntercept(Vector2 one, Vector2 two) {
        float m = (two.y - one.y) / (two.x - one.x);
        float b = one.y + (-(m * one.x));
        return (m * _player.transform.position.x) + b;
    }

    IEnumerator FlashlightInstruction() {
        yield return new WaitForSeconds(0.5f);
        _leftClick.SetActive(true);
    }

    IEnumerator MovementInstruction() {
        yield return new WaitForSeconds(1);
        _A.SetActive(true);
        _D.SetActive(true);
    }

    private void ConstrainCamera() {
        if (_player.transform.position.x < -71) {
            _camera.transform.position = new Vector3(-66, 0, -10);
        } else if (_player.transform.position.x > 60.3f) {
            _camera.transform.position = new Vector3(65.3f, 0, -10);
        } else {
            _camera.transform.position = new Vector3(_player.transform.position.x + 5, 0, -10);
        }
    }
}
