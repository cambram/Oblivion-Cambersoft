using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {
    private static CheckpointManager instance;
    private Vector3 _currentCheckpoint;
    //private int _debugTool = 0;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetCurrentCheckpoint(Vector3 checkpoint) {
        _currentCheckpoint = checkpoint;
        //_debugTool++;
        //Debug.Log(_debugTool + " Current checkpoint is: " + _currentCheckpoint);
    }

    public Vector3 GetCurrentCheckpoint() {
        return _currentCheckpoint;
    }
}
