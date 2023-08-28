using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Level1Manager : MonoBehaviour
{
    private Camera _camera;
    private Player _player;
    private UIManager _uiManager;
    private bool _respawn = false;

    void Start() {
        _camera = Camera.main;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _player.transform.position = new Vector3(-119, -2, 0);
    }

    void Update() {
        ConstrainCamera();

        //respawn player if they fall
        if (_player.transform.position.y < -12 && !_respawn) {
            _respawn = true;
            _uiManager.FadeOut(2);
        }
    }

    private void ConstrainCamera() {
        if (_player.transform.position.x < -113) {
            _camera.transform.position = new Vector3(-108, 0, -10);
        } else if (_player.transform.position.x > 105) {
            _camera.transform.position = new Vector3(110, 0, -10);
        } else {
            _camera.transform.position = new Vector3(_player.transform.position.x + 5, 0, -10);
        }
    }
}
