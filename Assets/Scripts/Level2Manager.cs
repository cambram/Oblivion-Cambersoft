using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Level2Manager : MonoBehaviour
{
    private Player _player;
    private Camera _camera;
    private bool _respawn = false, _soundEffect1 = false;
    private SuspenseAudioManager _suspenseAudioManager;


    private UIManager _uiManager;

    void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.transform.position = new Vector3(-125, -1.2f, 0);
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _suspenseAudioManager = GameObject.Find("Suspense_Audio_Manager").GetComponent<SuspenseAudioManager>();
        _camera = Camera.main;
    }

    void Update() {
        ConstrainCamera();

        if (_player.transform.position.y < -12 && !_respawn) {
            _respawn = true;
            _uiManager.FadeOut(4);
        }

        if(_player.transform.position.x > -107 && !_soundEffect1) {
            _soundEffect1 = true;
            _suspenseAudioManager.PlaySuspense2();
        }
    }

    private void ConstrainCamera() {
        if (_player.transform.position.x < -121) {
            _camera.transform.position = new Vector3(-116, 0, -10);
        } else if (_player.transform.position.x > 275) { //105
            _camera.transform.position = new Vector3(280, 0, -10);
        } else {
            _camera.transform.position = new Vector3(_player.transform.position.x + 5, 0, -10);
        }
    }
}
