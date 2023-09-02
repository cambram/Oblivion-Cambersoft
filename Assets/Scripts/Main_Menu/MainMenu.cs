using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _mainMenuButtons;
    [SerializeField]
    private GameObject _selectLevel;

    public void Start() {
        _mainMenuButtons.SetActive(true);
        _selectLevel.SetActive(false);
        Cursor.visible = true;
    }

    public void StartTutorial() {
        _animator.SetTrigger("FadeOut");
        StartCoroutine(StartGameRoutine(1));
    }

    public void StartLevelOne() {
        _animator.SetTrigger("FadeOut");
        StartCoroutine(StartGameRoutine(2));
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void SelectLevelButton() {
        _mainMenuButtons.SetActive(false);
        _selectLevel.SetActive(true);
    }

    public void BackButton() {
        _mainMenuButtons.SetActive(true);
        _selectLevel.SetActive(false);
    }

    IEnumerator StartGameRoutine(int level) {
        yield return new WaitForSeconds(4.5f);
        SceneManager.LoadScene(level);
    }
}
