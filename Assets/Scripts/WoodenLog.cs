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
        this.GetComponent<AudioSource>().loop = true;
    }

    private void Update() {
        if(_player != null && _player.transform.position.x >= -119 && _player.transform.position.x <= -44) {
            this.GetComponent<AudioSource>().volume = SlopeIntercept(new Vector2(-119, 0.01f), new Vector2(-44, 1));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("LogImpact")) {
            this.GetComponent<AudioSource>().loop = false;
            this.GetComponent<AudioSource>().clip = _impactClip;
            this.GetComponent<AudioSource>().Play();
        } else if (collision.CompareTag("PreventDeath")) {
            _disableDeath = true;
        } else if (collision.CompareTag("Player")) {
            if (!_disableDeath) {
                _player.KillPlayer();
            }
        }
    }

    private float SlopeIntercept(Vector2 one, Vector2 two) {
        float m = (two.y - one.y) / (two.x - one.x);
        float b = one.y + (-(m * one.x));
        return (m * _player.transform.position.x) + b;
    }
}
