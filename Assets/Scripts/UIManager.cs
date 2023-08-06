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

    void Start(){
        _deathText.gameObject.SetActive(false);
        _isDead = false;
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _fadeOutAnim = GetComponent<Animator>();
    }

    private void Update() {
        if(_isDead && Input.GetKeyDown(KeyCode.R)) {
            _gameManager.RestartGame();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            FadeOut();
        }
    }

    public void DisplayDeath() {
        _deathText.gameObject.SetActive(true);
        _isDead = true;
    }

    public void FadeOut() {
        _fadeOutAnim.SetTrigger("FadeOut");
        StartCoroutine(FadeOutRoutine());
    }
    IEnumerator FadeOutRoutine() {
        yield return new WaitForSeconds(1);
        _gameManager.BackToMainMenu();
    }
}
