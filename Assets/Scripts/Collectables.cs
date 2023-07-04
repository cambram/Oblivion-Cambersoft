using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField]
    private int _collectableID; //0 = battery; 1 = flash charge

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Player player = other.transform.GetComponent<Player>();
            if(player != null) {
                switch (_collectableID) {
                    case 0:
                        player.CollectBattery();
                        break;
                    case 1:
                        player.CollectFlashCharge();
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
