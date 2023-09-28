using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenLog : MonoBehaviour
{
    [SerializeField]
    private AudioClip _impactClip;
    private bool _disableDeath = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("LogImpact")) {
            this.GetComponent<AudioSource>().clip = _impactClip;
            this.GetComponent<AudioSource>().Play();
            _disableDeath = true;
        } else if (collision.CompareTag("Player")) {
            if (!_disableDeath) {
                Debug.Log("player hit");
            }
        }
    }
}
