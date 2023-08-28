using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Animator _fadeOutAnim;
    private GameManager _gameManager;
    private GameObject _environment;

    void Start(){
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _environment = GameObject.Find("Environment");
        _fadeOutAnim = GetComponent<Animator>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            FadeOut(1);
        }
    }

    //SceneManager.GetActiveScene().buildIndex

    /// <summary>
    /// Fades out from the game
    /// </summary>
    /// <param name="x">1 = fade to main menu; 2 = fade to respawn player</param>
    public void FadeOut(int x) {
        switch (x) {
            case 1:
                _fadeOutAnim.SetTrigger("FadeOut");
                _environment.GetComponent<Animator>().SetTrigger("FadeOut");
                StartCoroutine(BackToMainRoutine());
                break;
            case 2:
                _fadeOutAnim.SetTrigger("FadeOut");
                _environment.GetComponent<Animator>().SetTrigger("FadeOut");
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
        _gameManager.RestartTutorial();
    }
}
