using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam2WarpablePlane : Cam2RenderTex
{

    [SerializeField]
    Renderer renderer;

    [SerializeField]
    WarpablePlane warpablePlane;

    [SerializeField]
    Texture calibrationTexture;

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	void Update()
    {
        if (warpablePlane.IsEnable)
        {
            renderer.material.mainTexture = calibrationTexture;
        }else
        {
            renderer.material.mainTexture = RT;
        }
        
    }
}
