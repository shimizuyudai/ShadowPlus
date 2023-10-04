using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRearImageController : MonoBehaviour
{
    [SerializeField]
    KeyCode toggleKey;
    [SerializeField]
    KeyCode incrementKey;
    [SerializeField]
    KeyCode decrementKey;

    [SerializeField]
    int debugModeNUM;

    [SerializeField]
    GameObject debugObject;
    [SerializeField]
    Renderer Renderer;
    [SerializeField]
    Cam2RenderTex captureImage;

    int debugMode;

    bool isDebug;
    public bool IsDebug
    {
        get
        {
            return isDebug;
        }
        set
        {
            debugObject.SetActive(value);
            isDebug = value;
        }
    }

    private void Awake()
    {
        Renderer.material.SetInt("_Mode", debugMode); ;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            IsDebug = !IsDebug;
        }
        if (!IsDebug) return;

        if (Input.GetKeyDown(incrementKey))
        {
            debugMode++;
            if (debugMode >= debugModeNUM)
            {
                debugMode = 0;
            }
            Renderer.material.SetInt("_Mode", debugMode);
        }
        else if (Input.GetKeyDown(decrementKey))
        {
            debugMode--;
            if (debugMode < 0)
            {
                debugMode = debugModeNUM - 1;
            }
            Renderer.material.SetInt("_Mode", debugMode);
        }

        switch (debugMode)
        {
            case 0:
                this.Renderer.material.mainTexture = captureImage.RT;
                break;

            case 1:

                break;

            default:

                break;
        }

    }
}
