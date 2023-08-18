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

    //Flash Charge Instruction Variables
    [SerializeField]
    private GameObject _flashChargeIntrcn1;
    [SerializeField]
    private Animator _flashChargeIntrcn1Anim;
    [SerializeField]
    private GameObject _flashChargeIntrcn2;
    [SerializeField]
    private Animator _flashChargeIntrcn2Anim;
    [SerializeField]
    private GameObject _flashChargeIntrcn3;
    [SerializeField]
    private Animator _flashChargeIntrcn3Anim;

    //Battery Instruction Variables
    private bool _batteryBypass = true;
    [SerializeField]
    private GameObject _batteryDying;
    [SerializeField]
    private Animator _batteryDyingAnim;

    private bool _flashlightInstructionComplete = false, _movementInstructionComplete = false, _jumpInstructionStart = false, _jumpInstructionComplete = false, _flash1 = false;

    private Player _player;
    private Camera _camera;
    private SpawnManager _spawnManager;
    private Vector3 _checkpoint1, _checkpoint2;

    void Start() {
        Cursor.visible = false;
        _camera = Camera.main;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _checkpoint1 = new Vector3(-10.4f, 2.7f, 0);
        _checkpoint2 = new Vector3(-1.84f, -1.44f, 0);
        InitialisePrefabsForLevel();
        SetAllInstructionsActiveFalse();
        StartCoroutine(FlashlightInstruction());
    }

    void Update() {
        if (_player != null) {
            ConstrainCamera();
            //battery bypass at beginning for battery dying instruction
            if (_player.transform.position.x < -48 && _batteryBypass) {
                _player.CollectBattery();
            }

            if (_player.transform.position.x > -48 && _batteryBypass) {
                _batteryBypass = false;
                _player.FlickerFlashlight();
            }

            //respawn player if they fall
            if (_player.transform.position.y < -12) {
                RespawnPlayer(_checkpoint1);
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

            //show jump instruction
            if (_player.transform.position.x > -20 && !_jumpInstructionStart) {
                _jumpInstructionStart = true;
                _space.SetActive(true);
            }

            //Get rid of jump instruction
            if (_player.transform.position.x > -1 && !_jumpInstructionComplete) {
                _jumpInstructionComplete = true;
                _spaceAnim.SetTrigger("FadeOut");
                _flashChargeIntrcn1.SetActive(true);
            }

            if (_player.transform.position.x > 10 && !_flash1) {
                _flash1 = true;
                _flashChargeIntrcn1Anim.SetTrigger("FadeOut");
                _flashChargeIntrcn2.SetActive(true);
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
        _flashChargeIntrcn1.SetActive(false);
        _flashChargeIntrcn2.SetActive(false);
        _flashChargeIntrcn3.SetActive(false);
        _batteryDying.SetActive(false);
    }

    public Vector3 GetCheckpoint(int x) {
        switch(x) {
            case 1: return _checkpoint1;
            case 2: return _checkpoint2;
            default: return new Vector3(0,0,0);
        }
    }

    public void RespawnPlayer(Vector3 s) {
        _player.transform.position = s;
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
