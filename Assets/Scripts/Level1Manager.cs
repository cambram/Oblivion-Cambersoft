using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Level1Manager : MonoBehaviour
{
    private Camera _camera;
    private Player _player;

    void Start() {
        _camera = Camera.main;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.transform.position = new Vector3(-119, -2, 0);
    }

    // Update is called once per frame
    void Update() {
        ConstrainCamera();
    }

    private void ConstrainCamera() {
        if (_player.transform.position.x < -113) {
            _camera.transform.position = new Vector3(-108, 0, -10);
        } else if (_player.transform.position.x > 1000.3f) {
            _camera.transform.position = new Vector3(65.3f, 0, -10);
        } else {
            _camera.transform.position = new Vector3(_player.transform.position.x + 5, 0, -10);
        }
    }
}
