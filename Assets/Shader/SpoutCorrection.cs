using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoutCorrection : MonoBehaviour {
    [SerializeField]
    protected Shader imageEffectShader;
    protected Material material;
    [Range(0, 1)]
    [SerializeField]
    float brightnessThreshold;
    [Range(0,1080)]
    [SerializeField]
    float bottomThreshold;
    [SerializeField]
    float resolutionH;

    protected virtual void Awake()
    {
        if (resolutionH < 1f)
        {
            resolutionH = 1080f;
        }
        material = new Material(imageEffectShader);
    }

    void Update()
    {
        material.SetFloat("_BrightnessThreshold", brightnessThreshold);
        material.SetFloat("_BottomThreshold", bottomThreshold/resolutionH);
    }

    protected void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
