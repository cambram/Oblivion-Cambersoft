using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    //A vars
    [SerializeField]
    private GameObject _A;
    [SerializeField]
    private Animator _AAnim;

    //D vars
    [SerializeField]
    private GameObject _D;
    [SerializeField]
    private Animator _DAnim;

    //E vars
    [SerializeField]
    private GameObject _E;
    [SerializeField]
    private Animator _EAnim;

    //space vars
    [SerializeField]
    private GameObject _space;
    [SerializeField]
    private Animator _spaceAnim;

    //Left Click vars
    [SerializeField]
    private GameObject _leftClick;
    [SerializeField]
    private Animator _leftClickAnim;

    //Flash charge instructions
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

    //battery instructions
    private bool _batteryBypass = true;
    [SerializeField]
    private GameObject _batteryDying;
    [SerializeField]
    private Animator _batteryDyingAnim;

    private bool _flashlightInstructionComplete = false, _movementInstructionComplete = false, 
        _jumpInstructionStart = false, _jumpInstructionComplete = false, _firstPickup = false,
        _firstPickupComplete = false, _flash1 = false, _secondPickupComplete = false, _flash2 = false;

    private Player _player;
    private Vector3 _checkpoint1, _checkpoint2;

    void Start() {
        Cursor.visible = false;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _A.SetActive(false);
        _E.SetActive(false);
        _D.SetActive(false);
        _space.SetActive(false);
        _leftClick.SetActive(false);
        _flashChargeIntrcn1.SetActive(false);
        _flashChargeIntrcn2.SetActive(false);
        _flashChargeIntrcn3.SetActive(false);
        _batteryDying.SetActive(false);
        _checkpoint1 = new Vector3(-10.4f, 2.7f, 0);
        _checkpoint2 = new Vector3(-1.84f, -1.44f, 0);
        StartCoroutine(FlashlightInstruction());
    }

    void Update() {
        if (_player != null) {
            if(_player.transform.position.x < -48 && _batteryBypass) {
                _player.CollectBattery();
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

            //First pick up show instruction for battery
            if (_player.transform.position.x > -48 && !_firstPickup) {
                _batteryBypass = false;
                _batteryDying.SetActive(true);
                _player.FlickerFlashlight();
                _firstPickup = true;
                _E.SetActive(true);
            }
            //Hide first pickup instruction
            if (Input.GetKeyDown(KeyCode.E) && _firstPickup && !_firstPickupComplete) {
                _firstPickupComplete = true;
                _EAnim.SetTrigger("FadeOut");
                _batteryDyingAnim.SetTrigger("FadeOut");
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
                StartCoroutine(FlashChargeInstcn2());
            }

            if (_player.transform.position.x > 23.3f && Input.GetKeyDown(KeyCode.E) && !_secondPickupComplete) {
                _secondPickupComplete = true;
                _EAnim.SetTrigger("FadeOut");
                _flashChargeIntrcn2Anim.SetTrigger("FadeOut");
                _flashChargeIntrcn3.SetActive(true);
            }

            if (_secondPickupComplete && Input.GetMouseButtonDown(1) && !_flash2) { 
                _flash2 = true;
                _flashChargeIntrcn3Anim.SetTrigger("FadeOut");
            }
        }
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

    IEnumerator FlashChargeInstcn2() {
        yield return new WaitForSeconds(1f);
        _E.SetActive(false);
        _E.transform.position = new Vector3(24.3f, -3.64f, 0);
        _E.SetActive(true);
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
}
