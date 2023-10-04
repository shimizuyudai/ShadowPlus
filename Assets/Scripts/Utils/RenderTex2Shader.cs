using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RenderTex2Shader : MonoBehaviour {
    [SerializeField]
    ShaderParm[] shaderParms;
    [SerializeField]
    Renderer Renderer;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        foreach (var shaderParm in shaderParms)
        {
            this.Renderer.material.SetTexture(shaderParm.Name,shaderParm.Cam2RenderTex.RT);
        }
	}
}

[Serializable]
public struct ShaderParm
{
    public string Name;
    public Cam2RenderTex Cam2RenderTex;
}