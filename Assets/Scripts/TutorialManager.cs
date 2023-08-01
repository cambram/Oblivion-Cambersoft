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
    //space vars
    [SerializeField]
    private GameObject _space;
    //Right Click vars
    [SerializeField]
    private GameObject _rightClick;
    //Left Click vars
    [SerializeField]
    private GameObject _leftClick;
    [SerializeField]
    private Animator _leftClickAnim;

    private bool _flashlightInstructionComplete = false, _movementInstructionComplete = false, _jumpInstructionComplete = false;

    private Player _player;

    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _A.SetActive(false);
        _E.SetActive(false);
        _D.SetActive(false);
        _space.SetActive(true);
        _rightClick.SetActive(false);
        _leftClick.SetActive(false);
        StartCoroutine(FlashlightInstruction());
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && !_flashlightInstructionComplete) {
            _flashlightInstructionComplete = true;
            _leftClickAnim.SetTrigger("FadeOut");
            StartCoroutine(MovementInstruction());
        }

        if(_player.transform.position.x > -58 && !_movementInstructionComplete) {
            _movementInstructionComplete = true;
            _AAnim.SetTrigger("FadeOut");
            _DAnim.SetTrigger("FadeOut");
        }

        if (_player.transform.position.x > 1 && !_jumpInstructionComplete) {
            _jumpInstructionComplete = true;
            //_AAnim.SetTrigger("FadeOut");
            //_DAnim.SetTrigger("FadeOut");
        }
    }
    
    IEnumerator FlashlightInstruction() {
        yield return new WaitForSeconds(1);
        _leftClick.SetActive(true);
    }

    IEnumerator MovementInstruction() {
        yield return new WaitForSeconds(1f);
        _A.SetActive(true);
        _D.SetActive(true);
    }
}
