using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void LoadGame() {
        animator.SetTrigger("FadeOut");
        StartCoroutine(SceneChangeRoutine());
    }

    public void QuitGame() {
        Application.Quit();
    }
    IEnumerator SceneChangeRoutine() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
