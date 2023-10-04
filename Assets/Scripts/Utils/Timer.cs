using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour {
    [SerializeField]
    bool isMulti;

    IEnumerator coroutine;
    public bool IsCount
    {
        get;
        private set;
    }

    public void Stop()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
            IsCount = false;
        }
    }

    public void StartCount(float duration, Action callback = null, Action<float> updateCallback = null)
    {
        if (isMulti)
        {
            StartCoroutine(Count(duration, callback, updateCallback));
        }
        else
        {
            Stop();
            coroutine = Count(duration, callback, updateCallback);
            StartCoroutine(coroutine);
        }
        
    }

    IEnumerator Count(float duration, Action callback = null, Action<float> updateCallback = null)
    {
        IsCount = true;
        float time = duration;
        while(time > 0f)
        {
            time -= Time.deltaTime;
            time = Mathf.Clamp(time,0f,duration);
            if (updateCallback != null)
            {
                updateCallback(time);
            }
            yield return new WaitForEndOfFrame();
        }
        IsCount = false;
        if (callback != null)
        {
            callback();
        }
        yield break;
    }
}
