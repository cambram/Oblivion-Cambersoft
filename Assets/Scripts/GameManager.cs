using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    /*private static GameManager instance;
    private Vector3 _currentCheckpoint;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
        } else {
            Destroy(instance);
        }
    }

    public void SetCurrentCheckpoint(Vector3 checkpoint) {
        _currentCheckpoint = checkpoint;
    }

    public Vector3 GetCurrentCheckpoint() {
        return _currentCheckpoint;
    }*/

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
