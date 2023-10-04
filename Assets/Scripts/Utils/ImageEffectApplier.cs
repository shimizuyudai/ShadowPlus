using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEffectApplier : MonoBehaviour {
    [SerializeField]
    protected Shader imageEffectShader;
    protected Material material;
    

    protected virtual void Awake()
    {
        material = new Material(imageEffectShader);
    }

    protected void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
