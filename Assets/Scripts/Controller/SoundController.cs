using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    [SerializeField]
    bool isEnable;

    [SerializeField]
    AudioSource tickAudioSource, transitionAudioSource;
    [SerializeField]
    AudioClip tickSoundClip, transitionSoundClip;

    [SerializeField]
    TransitionController transitionController;

    [SerializeField]
    float interval;

    [SerializeField]
    IEnumerator coroutine;

    private void Awake()
    {
        if (!isEnable) return;
        transitionController.OnStartTransitionEvent += () =>
        {
            //if (coroutine != null)
            //{
            //    StopCoroutine(coroutine);
            //}
            //coroutine = null;
            transitionAudioSource.PlayOneShot(transitionSoundClip);
        };
        transitionController.OnCompleteTransitionEvent += () =>
        {
            //if (coroutine != null)
            //{
            //    StopCoroutine(coroutine);
            //}
            //coroutine = PlayTickSound();
            //StartCoroutine(coroutine);
        };
        coroutine = PlayTickSound();
        StartCoroutine(coroutine);
    }

    IEnumerator PlayTickSound()
    {
        while (true)
        {
            tickAudioSource.PlayOneShot(tickSoundClip);
            yield return new WaitForSeconds(interval);
        }
    }
}
