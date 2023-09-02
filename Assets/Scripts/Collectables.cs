using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Collectables : MonoBehaviour
{
    [SerializeField]
    private int _collectableID; //0 = battery; 1 = flash charge
    private PlayerLightSources _lightSources;
    private AudioSource _audioSource;

    private void Start() {
        _lightSources = GameObject.Find("Player").GetComponent<PlayerLightSources>();
        _audioSource = GetComponent<AudioSource>();
    }

    IEnumerator DestroyGameObject(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            switch (_collectableID) {
                case 0:
                    _lightSources.CollectBattery();
                    _audioSource.Play();
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    this.gameObject.GetComponent<Light2D>().intensity = 0f;
                    StartCoroutine(DestroyGameObject(0.46f));
                    break;
                case 1:
                    _lightSources.CollectFlashCharge();
                    _audioSource.Play();
                    this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    this.gameObject.GetComponent<Light2D>().intensity = 0f;
                    StartCoroutine(DestroyGameObject(0.37f));
                    break;
            }
        }
    }
}
