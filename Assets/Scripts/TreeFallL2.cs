using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFallL2 : MonoBehaviour
{
    [SerializeField]
    private AudioClip _impactClip;
    private bool _impactPlayed = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !_impactPlayed) {
            this.GetComponent<Rigidbody2D>().gravityScale = 1;
            this.GetComponent<AudioSource>().PlayDelayed(0.54f);
            _impactPlayed = true;
        }
    }
}
