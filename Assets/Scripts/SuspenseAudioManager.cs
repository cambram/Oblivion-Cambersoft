using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspenseAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource _suspenseSource;
    [SerializeField]
    private AudioClip _suspense1;
    [SerializeField]
    private AudioClip _suspense2;

    public void PlaySuspense1() {
        _suspenseSource.clip = _suspense1;
        _suspenseSource.Play();
    }

    public void PlaySuspense2() {
        _suspenseSource.clip = _suspense2;
        _suspenseSource.Play();
    }
}
