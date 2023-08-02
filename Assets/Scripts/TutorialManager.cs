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

    //Right Click vars
    [SerializeField]
    private GameObject _rightClick;
    [SerializeField]
    private Animator _rightClickAnim;

    //Left Click vars
    [SerializeField]
    private GameObject _leftClick;
    [SerializeField]
    private Animator _leftClickAnim;

    private bool _flashlightInstructionComplete = false, _movementInstructionComplete = false, 
        _jumpInstructionStart = false, _jumpInstructionComplete = false, _firstPickup = false,
        _firstPickupComplete = false;

    private Player _player;

    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _A.SetActive(false);
        _E.SetActive(false);
        _D.SetActive(false);
        _space.SetActive(false);
        _rightClick.SetActive(false);
        _leftClick.SetActive(false);
        StartCoroutine(FlashlightInstruction());
    }

    void Update() {
        //respawn player if they fall
        if(_player.transform.position.y < -12) {
            _player.transform.position = new Vector3(-10.4f, 2.7f, 0);
        }

        //remove flashlight instruction
        if (Input.GetMouseButtonDown(0) && !_flashlightInstructionComplete) {
            _flashlightInstructionComplete = true;
            _leftClickAnim.SetTrigger("FadeOut");
            StartCoroutine(MovementInstruction());
        }

        //Get rid of movement instructions
        if(_player.transform.position.x > -58 && !_movementInstructionComplete) {
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
        if (_player.transform.position.x > 1 && !_jumpInstructionComplete) {
            _jumpInstructionComplete = true;
            _spaceAnim.SetTrigger("FadeOut");
        }

        //First pick up show instruction for battery
        if (_player.transform.position.x > -48 && !_firstPickup) {
            _firstPickup = true;
            _E.SetActive(true);
        }
        //Hide first pickup instruction
        if (Input.GetKeyDown(KeyCode.E) && _firstPickup && !_firstPickupComplete) {
            _firstPickupComplete = true;
            _EAnim.SetTrigger("FadeOut");
        }

        //show right click instruction
        if (_player.transform.position.x > -20 && !_jumpInstructionStart) {
            _jumpInstructionStart = true;
            _space.SetActive(true);
        }

        //Get rid of right click instruction
        if (_player.transform.position.x > 1 && !_jumpInstructionComplete) {
            _jumpInstructionComplete = true;
            _spaceAnim.SetTrigger("FadeOut");
        }
    }
    
    IEnumerator FlashlightInstruction() {
        yield return new WaitForSeconds(1);
        _leftClick.SetActive(true);
    }

    IEnumerator MovementInstruction() {
        yield return new WaitForSeconds(1);
        _A.SetActive(true);
        _D.SetActive(true);
    }
}
