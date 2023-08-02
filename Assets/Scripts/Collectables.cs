using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField]
    private int _collectableID; //0 = battery; 1 = flash charge
    private bool _isInProximity;
    private Player _player;
    private void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }
    private void Update() {
        switch (_collectableID) {
            case 0:
                if (Input.GetKeyDown(KeyCode.E) && _isInProximity) {
                    _player.CollectBattery();
                    Destroy(this.gameObject);
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.E) && _isInProximity) {
                    _player.CollectFlashCharge();
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    private void SetIsInProximity(bool x) {
        _isInProximity = x;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            switch (_collectableID) {
                case 0:
                    SetIsInProximity(true);
                    break;
                case 1:
                    SetIsInProximity(true);
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            switch (_collectableID) {
                case 0:
                    SetIsInProximity(false);
                    break;
                case 1:
                    SetIsInProximity(false);
                    break;
            }
        }
    }
}
