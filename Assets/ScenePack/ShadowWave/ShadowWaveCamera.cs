using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowWaveCamera : MonoBehaviour {

    [SerializeField]
    protected Shader imageEffectShader;
    protected Material material;
    [SerializeField]
    Cam2RenderTex shadowBlurTex;
    [SerializeField]
    float amplitude, frequency, shadowBrightnessThreshold, timeSpeed, maxTime;
    float elapsedTime;

    void Awake()
    {
        material = new Material(imageEffectShader);
        
    }

    private void OnDisable()
    {
        elapsedTime = 0f;
    }

    private void Update()
    {
        material.SetFloat("_Amplitude", amplitude);
        material.SetFloat("_Frequency", frequency);
        material.SetFloat("_ShadowBrightnessThreshold", shadowBrightnessThreshold);
        material.SetFloat("_ElapsedTime", elapsedTime);
        material.SetTexture("_BlurredShadowTex", shadowBlurTex.RT);

        elapsedTime += Time.deltaTime * timeSpeed;
        if (elapsedTime < -maxTime || elapsedTime > maxTime)
        {
            timeSpeed *= -1f;
        }
    }

    protected void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
