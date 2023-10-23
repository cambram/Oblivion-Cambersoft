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
    [SerializeField]
    private AudioClip _suspenseDrone1;

    public void PlaySuspense1() {
        if(!_suspenseSource.isPlaying) {
            _suspenseSource.clip = _suspense1;
            _suspenseSource.Play();
        }
    }

    public void PlaySuspense2() {
        if (!_suspenseSource.isPlaying) {
            _suspenseSource.clip = _suspense2;
            _suspenseSource.Play();
        }
    }

    public void PlaySuspenseDrone1() {
        if (!_suspenseSource.isPlaying) {
            _suspenseSource.clip = _suspenseDrone1;
            _suspenseSource.Play();
        }
    }
}
