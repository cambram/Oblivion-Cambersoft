using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void RestartTutorial() {
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene(0);
    }
}
