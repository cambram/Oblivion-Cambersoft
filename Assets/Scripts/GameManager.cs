using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private CheckpointManager _checkpointManager;

    private void Start() {
        _checkpointManager = GameObject.Find("Checkpoint_Manager").GetComponent<CheckpointManager>();
    }
    public void RestartTutorial() {
        _checkpointManager.SetCurrentCheckpoint(new Vector3(-77, -1.7f, 0));
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void RestartLevelOne() {
        _checkpointManager.SetCurrentCheckpoint(new Vector3(-119, -2f, 0));
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(2);
    }

    public void RestartLevelTwo() {
        _checkpointManager.SetCurrentCheckpoint(new Vector3(-125, -1.2f, 0));
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(3);
    }

    public void BackToMainMenu() {
        _checkpointManager.SetCurrentCheckpoint(Vector3.zero);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void RespawnTutorial() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void RespawnLevelOne() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(2);
    }

    public void RespawnLevelTwo() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(3);
    }
}
