using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenLog : MonoBehaviour
{
    [SerializeField]
    private AudioClip _impactClip;
    private bool _disableDeath = false;
    private Player _player;

    private void Start() {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("LogImpact")) {
            this.GetComponent<AudioSource>().Play();
        } else if (collision.CompareTag("PreventDeath")) {
            _disableDeath = true;
        } else if (collision.CompareTag("Player")) {
            if (!_disableDeath) {
                _player.KillPlayer();
            }
        }
    }
}
