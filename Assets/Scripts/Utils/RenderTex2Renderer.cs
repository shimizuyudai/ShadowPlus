using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTex2Renderer : MonoBehaviour {
	[SerializeField]
	Cam2RenderTex cam2RenderTex;
	[SerializeField]
	Renderer renderer;
	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = cam2RenderTex.RT;
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material.mainTexture = cam2RenderTex.RT;
	}
}
