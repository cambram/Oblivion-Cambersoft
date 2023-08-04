using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _flashChargePrefab;
    [SerializeField]
    private GameObject _batteryPrefab;
    [SerializeField]
    private GameObject _collectablesContainer;

    void Start(){
        enemiesInit();
        collectablesInit();        
    }
    private void enemiesInit() {
        SpawnUmbra(22.23f, 0.78f);
    }

    private void collectablesInit() {
        SpawnFlashCharge(24.3f, -1.87f);
        SpawnFlashCharge(46.4f, -0.4f);
        SpawnBattery(-30.1f, -0.7f);
    }
    private void SpawnUmbra(float x, float y) {
        GameObject enemy = Instantiate(_enemyPrefab, new Vector3(x, y, 0), Quaternion.identity);
        enemy.transform.parent = _enemyContainer.transform;
    }

    private void SpawnFlashCharge(float x, float y) {
        GameObject flashCharge = Instantiate(_flashChargePrefab, new Vector3(x, y, 0), Quaternion.identity);
        flashCharge.transform.parent = _collectablesContainer.transform;
    }
    private void SpawnBattery(float x, float y) {
        GameObject battery = Instantiate(_batteryPrefab, new Vector3(x, y, 0), Quaternion.identity);
        battery.transform.parent = _collectablesContainer.transform;
    }
}
