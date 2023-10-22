using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    private Player _player;
    private Camera _camera;
    private bool _respawn = false, _umbraAttack1 = false, _soundEffect1 = false, _umbraAttack = false, _surpriseAttack = false, _cliffFall = false, _luxAttack1 = false, _luxAttack2 = false, _openMassiveDoors = false;
    [SerializeField]
    private GameObject _cliff;
    [SerializeField]
    private GameObject _electricDeath;
    private SuspenseAudioManager _suspenseAudioManager;
    private SpawnManager _spawnManager;

    private UIManager _uiManager;
    private CheckpointManager _checkpointManager;
    [SerializeField]
    private Animator _massiveDoorsAnim;


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

            if (_player.transform.position.x > -93 && _player.transform.position.x < -91 && !_umbraAttack1) {
                _umbraAttack1 = true;
                _spawnManager.SpawnUmbra(-109.23f, 2.7f);
            }

            if (_player.transform.position.x > -107 && _player.transform.position.x < -105 && !_soundEffect1) {
                _soundEffect1 = true;
                _suspenseAudioManager.PlaySuspense2();
            }

            if (_player.transform.position.x > 10 && _player.transform.position.x < 12 && !_luxAttack1) {
                _luxAttack1 = true;
                _spawnManager.SpawnLux(-6.56f, -0.4f);
            }

            if (_player.transform.position.x > 12 && _player.transform.position.x < 14 && !_luxAttack2) {
                _luxAttack2 = true;
                _spawnManager.SpawnLux(-4f, -0.4f);
                _spawnManager.SpawnLux(-6.56f, -0.4f);
            }

            if (_player.transform.position.x > 48 && _player.transform.position.x < 50 && !_umbraAttack) {
                _umbraAttack = true;
                _suspenseAudioManager.PlaySuspense1();
                _spawnManager.SpawnUmbra(34.89f, 2.49f);
            }

            if (_player.transform.position.x > 112 && _player.transform.position.x < 114 && !_surpriseAttack) {
                _surpriseAttack = true;
                _suspenseAudioManager.PlaySuspense1();
                _spawnManager.SpawnUmbra(99f, 2.5f);
            }

            if(_player.transform.position.x >= 72 && _player.transform.position.x < 74 && !_cliffFall) {
                _cliffFall = true;
                _cliff.GetComponent<AudioSource>().Play();
                _cliff.GetComponent<Rigidbody2D>().gravityScale = 0.05f;
                StartCoroutine(CliffFall());
            }

            if(_player.transform.position.x > 276 && !_openMassiveDoors) {
                _openMassiveDoors = true;
                _massiveDoorsAnim.SetTrigger("Open");
                _player.DisableJump();
                _player.DisableMovement();
                StartCoroutine(ReenablePlayer());
            }
        }
    }

    IEnumerator ReenablePlayer() {
        yield return new WaitForSeconds(8);
        _player.EnableJump();
        _player.EnableMovement();
    }

    private void ConstrainCamera() {
        if (_player.transform.position.x < -121) {
            _camera.transform.position = new Vector3(-116, 0, -10);
        } else if (_player.transform.position.x > 274) {
            _camera.transform.position = new Vector3(279, 0, -10);
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
        _spawnManager.SpawnLux(60.5f, 3.7f);
        _spawnManager.SpawnUmbra(76f, 4.6f);
        _spawnManager.SpawnLux(105.4f, 2.4f);
        _spawnManager.SpawnUmbra(111f, 2.4f);
        _spawnManager.SpawnUmbra(115.8f, 2.4f);
        _spawnManager.SpawnLux(120.4f, 2.4f);
        _spawnManager.SpawnLux(126.4f, 2.4f);
        _spawnManager.SpawnLux(146f, 1f);
        _spawnManager.SpawnUmbra(151f, 2f);
        _spawnManager.SpawnLux(162.2f, 1.47f);
        _spawnManager.SpawnUmbra(178.87f, 5.7f);
        _spawnManager.SpawnLux(194.56f, 6.4f);
        _spawnManager.SpawnLux(207.62f, -2.4f);
        _spawnManager.SpawnUmbra(210.43f, -1.2f);
        _spawnManager.SpawnUmbra(238.63f, 2.4f);
        _spawnManager.SpawnLux(251.44f, 5.1f);
        _spawnManager.SpawnUmbra(264.45f, 0.1f);

        /* Collectables */
        _spawnManager.SpawnFlashCharge(-107.66f, -1f, -14.747f);
        _spawnManager.SpawnFlashCharge(-95.1f, -3.9f);
        _spawnManager.SpawnFlashCharge(-91.2f, -3.91f);
        _spawnManager.SpawnFlashCharge(-64.47f, 1.49f);
        _spawnManager.SpawnFlashCharge(-57.17f, 2.38f);
        _spawnManager.SpawnFlashCharge(-37.63f, 1.04f);
        _spawnManager.SpawnBattery(-12f, -2.33f);
        _spawnManager.SpawnFlashCharge(-6.46f, -2.367f);
        _spawnManager.SpawnFlashCharge(0.85f, -2.44f);
        _spawnManager.SpawnFlashCharge(35.16f, -0.443f);
        _spawnManager.SpawnFlashCharge(42.45f, -0.375f);
        _spawnManager.SpawnFlashCharge(44.29f, -0.438f);
        _spawnManager.SpawnFlashCharge(88.05f, -0.71f);
        _spawnManager.SpawnFlashCharge(100.27f, 0.18f);
        _spawnManager.SpawnFlashCharge(103.515f, -0.743f);
        _spawnManager.SpawnBattery(133.89f, -0.62f);
        _spawnManager.SpawnFlashCharge(137.44f, -0.557f);
        _spawnManager.SpawnFlashCharge(158.608f, -0.764f);
        _spawnManager.SpawnFlashCharge(172.96f, 1.53f);
        _spawnManager.SpawnFlashCharge(198.484f, 6.801f, 38.072f);
        _spawnManager.SpawnFlashCharge(208.52f, -3.96f);
        _spawnManager.SpawnFlashCharge(215.0061f, -3.483238f);
        _spawnManager.SpawnFlashCharge(255.11f, 2.86f, -33.02f);
    }

    IEnumerator CliffFall() {
        yield return new WaitForSeconds(1f);
        _spawnManager.SpawnLux(100.88f, 3.22f);
        _cliff.GetComponent<Rigidbody2D>().gravityScale = 0.1f;
        yield return new WaitForSeconds(0.5f);
        _cliff.GetComponent<Rigidbody2D>().gravityScale = 0.8f;
    }
}
