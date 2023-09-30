using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public void RestartTutorial() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void RestartLevelOne() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(2);
    }

    public void RestartLevelTwo() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(3);
    }

    public void BackToMainMenu() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
