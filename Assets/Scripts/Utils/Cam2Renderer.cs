using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam2Renderer : Cam2RenderTex {
    [SerializeField]
    Renderer renderer;
    
    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        renderer.material.mainTexture = RT;
	}
}
