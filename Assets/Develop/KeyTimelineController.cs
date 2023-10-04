using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTimelineController : MonoBehaviour {
    [SerializeField]
    TimeLineManager timelineManager;

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            timelineManager.Play(1);
        }
        else if (Input.GetKeyDown("2"))
        {
            timelineManager.Play(2);
        }
        else if (Input.GetKeyDown("3"))
        {
            timelineManager.Play(3);
        }
        else if (Input.GetKeyDown("4"))
        {
            timelineManager.Play(4);
        }
        else if (Input.GetKeyDown("5"))
        {
            timelineManager.Play(5);
        }
        else if (Input.GetKeyDown("6"))
        {
            timelineManager.Play(6);
        }
        else if (Input.GetKeyDown("7"))
        {
            timelineManager.Play(7);
        }
        else if (Input.GetKeyDown("8"))
        {
            timelineManager.Play(8);
        }
        else if (Input.GetKeyDown("9"))
        {
            timelineManager.Play(9);
        }
        else if (Input.GetKeyDown("0"))
        {
            timelineManager.Play(0);
        }
        else if (Input.GetKeyDown("a"))
        {
            timelineManager.Play(10);
        }
        else if (Input.GetKeyDown("b"))
        {
            timelineManager.Play(11);
        }
    }
}
