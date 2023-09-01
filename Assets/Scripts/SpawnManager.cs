using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _umbraPrefab;
    [SerializeField]
    private GameObject _luxPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _flashChargePrefab;
    [SerializeField]
    private GameObject _batteryPrefab;
    [SerializeField]
    private GameObject _collectablesContainer;

    /// <summary>
    /// Spawns an umbra at the specified coordinates 
    /// </summary>
    /// <param name="x">x coordinate for umbra</param>
    /// <param name="y">y coordinate for umbra</param>
    public void SpawnUmbra(float x, float y) {
        GameObject umbra = Instantiate(_umbraPrefab, new Vector3(x, y, 0), Quaternion.identity);
        umbra.transform.parent = _enemyContainer.transform;
    }

    /// <summary>
    /// Spawns an lux at the specified coordinates 
    /// </summary>
    /// <param name="x">x coordinate for lux</param>
    /// <param name="y">y coordinate for lux</param>
    public void SpawnLux(float x, float y) {
        GameObject lux = Instantiate(_luxPrefab, new Vector3(x, y, 0), Quaternion.identity);
        lux.transform.parent = _enemyContainer.transform;
    }

    /// <summary>
    /// Spawns a flash charge at the specified coordinates 
    /// </summary>
    /// <param name="x">x coordinate for flash charge</param>
    /// <param name="y">y coordinate for flash charge</param>
    public void SpawnFlashCharge(float x, float y) {
        GameObject flashCharge = Instantiate(_flashChargePrefab, new Vector3(x, y, 0), Quaternion.identity);
        flashCharge.transform.parent = _collectablesContainer.transform;
    }

    /// <summary>
    /// Spawns a battery at the specified coordinates 
    /// </summary>
    /// <param name="x">x coordinate for battery</param>
    /// <param name="y">y coordinate for battery</param>
    public void SpawnBattery(float x, float y) {
        GameObject battery = Instantiate(_batteryPrefab, new Vector3(x, y, 0), Quaternion.identity);
        battery.transform.parent = _collectablesContainer.transform;
    }
}
