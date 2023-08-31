using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public void RestartTutorial() {
        SceneManager.LoadScene(1);
    }

    public void RestartLevelOne() {
        SceneManager.LoadScene(2);
    }
 
    public void BackToMainMenu() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
