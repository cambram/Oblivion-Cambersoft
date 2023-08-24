using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField]
    private int _collectableID; //0 = battery; 1 = flash charge
    private Player _player;
    private AudioSource _audioSource;

    private void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
    }

    IEnumerator DestroyGameObject(float seconds) {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            switch (_collectableID) {
                case 0:
                    _player.CollectBattery();
                    _audioSource.Play();
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    StartCoroutine(DestroyGameObject(0.46f));
                    break;
                case 1:
                    _player.CollectFlashCharge();
                    _audioSource.Play();
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    StartCoroutine(DestroyGameObject(0.37f));
                    break;
            }
        }
    }
}
