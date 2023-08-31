using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Level1Manager : MonoBehaviour
{
    private Camera _camera;
    private Player _player;
    private UIManager _uiManager;
    private GameObject _environment;
    private bool _respawn = false;

    private Vector2 _caveCutoff1 = new Vector2(13, 22000);
    private Vector2 _caveCutoff2 = new Vector2(25, 4000);

    void Start() {
        _camera = Camera.main;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _environment = GameObject.Find("Environment");
        _player.transform.position = new Vector3(-119, -2, 0);
    }

    void Update() {
        ConstrainCamera();

        //respawn player if they fall
        if (_player.transform.position.y < -12 && !_respawn) {
            _respawn = true;
            _uiManager.FadeOut(3);
        }

        if(_player.transform.position.x > 13 && _player.transform.position.x < 25) {
            _environment.GetComponent<AudioLowPassFilter>().cutoffFrequency = -1500 * _player.transform.position.x + 41500; //y = mx + b
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

    /*private float SlopeIntercept(Vector2 a, Vector2 c) {
        float m = (c.y - a.y) / (c.x - a.x);
        float b = a.y + (m * a.x);
        return (m * _player.transform.position.x) + b;
    }*/
}
