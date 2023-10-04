using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MemoryChecker : MonoBehaviour {
    [SerializeField]
    float interval;
    int count;
	// Use this for initialization
	void Start () {
        StartCoroutine(checkMemory());
	}
	
	// Update is called once per frame
	void Update () {
        var time = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
        
    }

    IEnumerator checkMemory()
    {
        while (true)
        {
            count++;
            var time = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
            var memoryValue1 = count.ToString() + "回目 : " + time + "---" + (GC.GetTotalMemory(false) / 1048576).ToString();
            var memoryValue2 = count.ToString() + "回目 : " + time + "---" + (UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory() / 1048576).ToString();
            print(memoryValue1);
            print(memoryValue2);
            yield return new WaitForSeconds(interval);
        }
    }


}
