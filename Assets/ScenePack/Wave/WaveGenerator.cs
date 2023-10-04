using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {
    [SerializeField]
    float phase0, amplitude0, frequency0;
    [Space(10)]
    [SerializeField]
    float phase1, amplitude1, frequency1;
    [SerializeField]
    float lineWidth;
    [SerializeField]
    Color lineColor;

    float elapsedTime;
    [SerializeField]
    Renderer renderer;
    [SerializeField]
    float speed, maxTime;

    [SerializeField]
    Texture background, overlap, shadowOverride, shadowOverlap;
    [SerializeField]
    Cam2RenderTex shadow;
    protected virtual void Awake()
    {

    }

    private void OnDisable()
    {
        elapsedTime = 0f;
    }

    private void Update()
    {
        renderer.material.SetFloat("_Amplitude0", Mathf.Sin(elapsedTime) * amplitude0);
        renderer.material.SetFloat("_Frequency0", frequency0);
        renderer.material.SetFloat("_Amplitude1", Mathf.Sin(elapsedTime) * amplitude1);
        renderer.material.SetFloat("_Frequency1", frequency1);
        renderer.material.SetFloat("_Phase0", phase0 * Mathf.Deg2Rad);
        renderer.material.SetFloat("_Phase1", phase1 * Mathf.Deg2Rad);
        renderer.material.SetFloat("_LineWidth", lineWidth);

        renderer.material.mainTexture = background;
        renderer.material.SetTexture("_ShadowTex", shadow.RT);
        renderer.material.SetTexture("_ShadowOverlapTex", shadowOverlap);
        renderer.material.SetTexture("_OverlapTex", overlap);
        renderer.material.SetTexture("_ShadowOverrideTex", shadowOverride);

        renderer.material.SetFloat("_ElapsedTime", elapsedTime);
        
        renderer.material.SetColor("_LineColor", lineColor);

        elapsedTime += speed * Time.deltaTime;
        if (elapsedTime > maxTime || elapsedTime < -maxTime)
        {
            speed *= -1f;
        }
    }
}
