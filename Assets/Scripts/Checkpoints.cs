using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private CheckpointManager _checkpointManager;

    private void Start() {
        _checkpointManager = GameObject.Find("Checkpoint_Manager").GetComponent<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            _checkpointManager.SetCurrentCheckpoint(this.transform.position);
        }
    } 
}
