using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    private Player _player;
    private Camera _camera;
    private bool _respawn = false, _soundEffect1 = false, _umbraAttack = false, _surpriseAttack = false, _cliffFall = false;
    [SerializeField]
    private GameObject _cliff;
    [SerializeField]
    private GameObject _electricDeath;
    private SuspenseAudioManager _suspenseAudioManager;
    private SpawnManager _spawnManager;

    private UIManager _uiManager;
    private CheckpointManager _checkpointManager;


    void Start() {
        Cursor.visible = false;
        _player = GameObject.Find("Player").GetComponent<Player>();
        _player.transform.position = new Vector3(-125, 1.2f, 0);
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
        _suspenseAudioManager = GameObject.Find("Suspense_Audio_Manager").GetComponent<SuspenseAudioManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _camera = Camera.main;
        _checkpointManager = GameObject.Find("Checkpoint_Manager").GetComponent<CheckpointManager>();


        if (_checkpointManager.GetCurrentCheckpoint() == Vector3.zero) {
            _checkpointManager.SetCurrentCheckpoint(new Vector3(-125, 1.2f, 0));
        }
        _player.transform.position = _checkpointManager.GetCurrentCheckpoint();

        InitialisePrefabsForLevel();
    }

    void Update() {
        if (_player != null) {
            ConstrainCamera();

            if (_player.transform.position.y < -12 && !_respawn) {
                _respawn = true;
                _uiManager.FadeOut(4, false);
            }

            if (_player.transform.position.x > -107 && _player.transform.position.x < -105 && !_soundEffect1) {
                _soundEffect1 = true;
                _suspenseAudioManager.PlaySuspense2();
            }

            if (_player.transform.position.x > 83 && _player.transform.position.x < 85 && !_umbraAttack) {
                _umbraAttack = true;
                _suspenseAudioManager.PlaySuspense1();
                //_spawnManager.SpawnUmbra(61.67f, 2.76f);
                //_spawnManager.SpawnUmbra(63.91f, 2.55f);
                //_spawnManager.SpawnUmbra(66.17f, 2.63f);
            }

            if (_player.transform.position.x > 112 && _player.transform.position.x < 114 && !_surpriseAttack) {
                _surpriseAttack = true;
                _suspenseAudioManager.PlaySuspense1();
                _spawnManager.SpawnUmbra(97f, 2.5f);
            }

            if(_player.transform.position.x >= 72 && _player.transform.position.x < 74 && !_cliffFall) {
                _cliffFall = true;
                _cliff.GetComponent<AudioSource>().Play();
                _cliff.GetComponent<Rigidbody2D>().gravityScale = 0.05f;

                StartCoroutine(CliffFall());
            }

            //mid point of these zones is where a lux should be attracted to with the attarction zone being turned on and off according to the flicker
            /*if((_player.transform.position.x > -3.8f && _player.transform.position.x < 3.8f) || (_player.transform.position.x > 10.2f && _player.transform.position.x < 17.2f) || (_player.transform.position.x > 25.3f && _player.transform.position.x < 33.2f)) {
                //lux zone
            } else {
                //no lux zone
            }*/
        }
    }

    private void ConstrainCamera() {
        if (_player.transform.position.x < -121) {
            _camera.transform.position = new Vector3(-116, 0, -10);
        } else if (_player.transform.position.x > 275) {
            _camera.transform.position = new Vector3(280, 0, -10);
        } else {
            _camera.transform.position = new Vector3(_player.transform.position.x + 5, 0, -10);
        }
    }

    private void InitialisePrefabsForLevel() {
        /* Enemies */
        _spawnManager.SpawnUmbra(-86.44f, -0.76f);
        _spawnManager.SpawnLux(-64f, 3.45f);
        _spawnManager.SpawnLux(-58.57f, 3.88f);
        _spawnManager.SpawnLux(-50.58f, 3.78f);
        _spawnManager.SpawnUmbra(-20.95f, 1.53f);
        _spawnManager.SpawnLux(3.83f, -0.68f);
        _spawnManager.SpawnLux(16.5f, -0.3f);
        _spawnManager.SpawnLux(25.11f, 0f);
        _spawnManager.SpawnUmbra(76f, 4.6f);
        _spawnManager.SpawnLux(105.4f, 2.4f);
        _spawnManager.SpawnUmbra(111f, 2.4f);
        _spawnManager.SpawnUmbra(115.8f, 2.4f);
        _spawnManager.SpawnLux(120.4f, 2.4f);
        _spawnManager.SpawnLux(126.4f, 2.4f);

        /* Collectables */
        _spawnManager.SpawnFlashCharge(-107.66f, -1f);
        _spawnManager.SpawnFlashCharge(-95.1f, -3.74f);
        _spawnManager.SpawnFlashCharge(-91.2f, -3.78f);
        _spawnManager.SpawnFlashCharge(-57.17f, 2.32f);
        _spawnManager.SpawnFlashCharge(-37.63f, 1.43f);
        _spawnManager.SpawnBattery(-12f, -2.33f);
        _spawnManager.SpawnFlashCharge(-6.46f, -2.29f);
        _spawnManager.SpawnFlashCharge(0.85f, -2.3f);
        _spawnManager.SpawnFlashCharge(35.16f, -0.2f);
        _spawnManager.SpawnFlashCharge(42.45f, -0.23f);
        _spawnManager.SpawnFlashCharge(44.29f, -0.22f);
    }

    IEnumerator CliffFall() {
        yield return new WaitForSeconds(1f);
        _cliff.GetComponent<Rigidbody2D>().gravityScale = 0.1f;
        yield return new WaitForSeconds(0.5f);
        _cliff.GetComponent<Rigidbody2D>().gravityScale = 0.8f;
    }
}
