using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTex2Material : MonoBehaviour {
	[SerializeField]
	Cam2RenderTex cam2RenderTex;
	[SerializeField]
	Material material;
	// Use this for initialization
	void Start () {
		material.mainTexture = cam2RenderTex.RT;
	}
	
	// Update is called once per frame
	void Update () {
		material.mainTexture = cam2RenderTex.RT;
	}
}
