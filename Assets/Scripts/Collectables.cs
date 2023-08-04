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
    private AudioSource _audioSource;
    private void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void Update() {
        if (Input.GetKeyUp(KeyCode.L) && _collectableID == 1) {
            _audioSource.Play();
        }

        switch (_collectableID) {
            case 0:
                if (Input.GetKeyDown(KeyCode.E) && _isInProximity) {
                    _player.CollectBattery();
                    _audioSource.Play();
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    StartCoroutine(DestroyGameObject(0.46f));
                }
                break;
            case 1:
                if (Input.GetKeyDown(KeyCode.E) && _isInProximity) {
                    _player.CollectFlashCharge();
                    _audioSource.Play();
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    StartCoroutine(DestroyGameObject(0.37f));
                }
                break;
        }
    }

    IEnumerator DestroyGameObject(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
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
