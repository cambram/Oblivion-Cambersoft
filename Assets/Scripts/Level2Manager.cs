using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Level2Manager : MonoBehaviour
{
    private Player _player;
    private Camera _camera;
    private bool _respawn = false;

    private UIManager _uiManager;

    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.transform.position = new Vector3(-119, -1.2f, 0);
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _camera = Camera.main;
    }

    void Update() {
        ConstrainCamera();

        if (_player.transform.position.y < -12 && !_respawn) {
            _respawn = true;
            _uiManager.FadeOut(4);
        }
    }

    private void ConstrainCamera() {
        if (_player.transform.position.x < -113) {
            _camera.transform.position = new Vector3(-108, 0, -10);
        } else if (_player.transform.position.x > 275) { //105
            _camera.transform.position = new Vector3(280, 0, -10);
        } else {
            _camera.transform.position = new Vector3(_player.transform.position.x + 5, 0, -10);
        }
    }
}
