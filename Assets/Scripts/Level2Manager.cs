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
    private SpawnManager _spawnManager;


    private UIManager _uiManager;

    void Start() {
        Cursor.visible = false;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.transform.position = new Vector3(-125, -1.2f, 0);
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _suspenseAudioManager = GameObject.Find("Suspense_Audio_Manager").GetComponent<SuspenseAudioManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _camera = Camera.main;
        InitialisePrefabsForLevel();
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
        } else if (_player.transform.position.x > 112) {
            _camera.transform.position = new Vector3(117, 0, -10);
        } else {
            _camera.transform.position = new Vector3(_player.transform.position.x + 5, 0, -10);
        }
    }

    private void InitialisePrefabsForLevel() {
        /* Enemies */
        _spawnManager.SpawnUmbra(68.7f, 2.7f);
        _spawnManager.SpawnUmbra(43.13f, 2.27f);
        _spawnManager.SpawnLux(25.11f, 0f);
        _spawnManager.SpawnLux(16.5f, -0.3f);
        _spawnManager.SpawnLux(3.83f, -0.68f);
        _spawnManager.SpawnUmbra(-20.95f, 1.53f);
        _spawnManager.SpawnUmbra(-86.44f, -0.76f);
        _spawnManager.SpawnLux(-50.58f, 3.78f);
        _spawnManager.SpawnLux(-58.57f, 3.88f);
        _spawnManager.SpawnLux(-64f, 3.45f);

        /* Collectables */
        _spawnManager.SpawnBattery(-12f, -2.33f);
        _spawnManager.SpawnFlashCharge(-6.46f, -2.29f);
        _spawnManager.SpawnFlashCharge(35.16f, -0.2f);
        _spawnManager.SpawnFlashCharge(0.85f, -2.3f);
        _spawnManager.SpawnFlashCharge(-91.2f, -3.78f);
        _spawnManager.SpawnFlashCharge(-57.17f, 2.32f);
        _spawnManager.SpawnFlashCharge(-107.66f, -1f);
        _spawnManager.SpawnFlashCharge(51.39f, -0.21f);
    }
}
