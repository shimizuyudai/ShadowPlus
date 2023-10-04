using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPassImageEffect : MonoBehaviour {

    [SerializeField]
    protected Shader imageEffectShader;
    protected Material material;
    [SerializeField]
    RenderTexture rt;
    [Range(0,1)]
    [SerializeField]
    float ratio;
    [SerializeField]
    Camera cam;
    [SerializeField]

    protected virtual void Awake()
    {
        material = new Material(imageEffectShader);
        if (rt == null)
        {
            rt = new RenderTexture(1920, 1080, 24);
        }
    }

    private void OnDisable()
    {
        RenderTexture.active = rt;
        GL.Clear(false, true, Color.black);
        RenderTexture.active = null;
    }

    private void Update()
    {
        material.SetFloat("_Ratio",ratio);
    }


    protected void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetTexture("_PreTex", rt);
        Graphics.Blit(source, destination, material);
        Graphics.Blit(destination, rt);
    }
}
