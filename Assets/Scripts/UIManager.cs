using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _deathText;
    private bool _isDead;
    private Animator _fadeOutAnim;
    private GameManager _gameManager;
    private TutorialManager _tutorialManager;

    void Start(){
        _deathText.gameObject.SetActive(false);
        _isDead = false;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _tutorialManager = GameObject.Find("Tutorial_Manager").GetComponent<TutorialManager>();
        _fadeOutAnim = GetComponent<Animator>();
    }

    private void Update() {
        if(_isDead && Input.GetKeyDown(KeyCode.R)) {
            FadeOut(2);        
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            FadeOut(1);
        }
    }

    public void DisplayDeath() {
        _deathText.gameObject.SetActive(true);
        _isDead = true;
    }

    public void FadeOut(int x) {
        switch (x) {
            case 1:
                _fadeOutAnim.SetTrigger("FadeOut");
                StartCoroutine(BackToMainRoutine());
                break;
            case 2:
                _fadeOutAnim.SetTrigger("FadeOut");
                StartCoroutine(RespawnRoutine());
                break;
            default: break;
        }
    }
    IEnumerator BackToMainRoutine() {
        yield return new WaitForSeconds(1);
        _gameManager.BackToMainMenu();
    }

    IEnumerator RespawnRoutine() {
        yield return new WaitForSeconds(1);
        _gameManager.RestartGame();
    }
}
