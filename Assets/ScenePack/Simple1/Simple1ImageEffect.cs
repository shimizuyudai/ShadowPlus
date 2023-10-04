using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple1ImageEffect : MonoBehaviour {
    [SerializeField]
    Material material;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual void OnRenderImage(RenderTexture src, RenderTexture dst)
    {

        Graphics.Blit(src, dst, material);
    }
}
