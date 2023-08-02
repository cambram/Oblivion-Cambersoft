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

    void Start()
    {
        //enemiesInit();
        //collectablesInit();        
    }
    private void enemiesInit() {
        GameObject enemy1 = Instantiate(_enemyPrefab, new Vector3(-44.6f, -6, 0), Quaternion.identity);
        enemy1.transform.parent = _enemyContainer.transform;
        GameObject enemy2 = Instantiate(_enemyPrefab, new Vector3(-42.4f, -5.4f, 0), Quaternion.identity);
        enemy2.transform.parent = _enemyContainer.transform;
        GameObject enemy3 = Instantiate(_enemyPrefab, new Vector3(17, -6, 0), Quaternion.identity);
        enemy3.transform.parent = _enemyContainer.transform;
        GameObject enemy4 = Instantiate(_enemyPrefab, new Vector3(19, -6, 0), Quaternion.identity);
        enemy4.transform.parent = _enemyContainer.transform;
        GameObject enemy5 = Instantiate(_enemyPrefab, new Vector3(50, -2.5f, 0), Quaternion.identity);
        enemy5.transform.parent = _enemyContainer.transform;
        GameObject enemy6 = Instantiate(_enemyPrefab, new Vector3(52, -2.5f, 0), Quaternion.identity);
        enemy6.transform.parent = _enemyContainer.transform;
    }

    private void collectablesInit() {
        GameObject flashCharge1 = Instantiate(_flashChargePrefab, new Vector3(-59, -9, 0), Quaternion.identity);
        flashCharge1.transform.parent = _collectablesContainer.transform;
        GameObject flashCharge2 = Instantiate(_flashChargePrefab, new Vector3(0.6f, -5.8f, 0), Quaternion.identity);
        flashCharge2.transform.parent = _collectablesContainer.transform;
        GameObject flashCharge3 = Instantiate(_flashChargePrefab, new Vector3(52, -5.5f, 0), Quaternion.identity);
        flashCharge3.transform.parent = _collectablesContainer.transform;
        GameObject battery1 = Instantiate(_batteryPrefab, new Vector3(34.4f, -6.2f, 0), Quaternion.identity);
        battery1.transform.parent = _collectablesContainer.transform;
    }
}
