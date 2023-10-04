using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sequencer : MonoBehaviour
{
    [SerializeField]
    bool isMulti;
    IEnumerator coroutine;
    public void StartTimeline(float duration, Action callback = null, Action<float> updateCallback = null)
    {
        //if (duration <= 0f) return;
        
        if (isMulti)
        {
            StartCoroutine(PlayTimeline(duration, callback, updateCallback));
        }
        else
        {
            StopTimeline();
            coroutine = PlayTimeline(duration, callback, updateCallback);
            StartCoroutine(coroutine);
        }
        
    }

    public void StopTimeline()
    {
        if (coroutine != null)
        {
            try
            {
                StopCoroutine(coroutine);
            }
            catch
            {

            }
        }
    }

    IEnumerator PlayTimeline(float duration, Action callback = null, Action<float> updateCallback = null)
    {
        float time = duration;
        while (time > 0f)
        {
            time = Mathf.Clamp(time, 0f, duration);
            if (updateCallback != null)
            {
                updateCallback(1f - (time / duration));
            }
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if (updateCallback != null)
        {
            updateCallback(1f);
        }
        if (callback != null)
        {
            callback();
        }
        yield break;
    }

}

