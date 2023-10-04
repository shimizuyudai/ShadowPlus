using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderTex2RawImg : MonoBehaviour {
	[SerializeField]
	Cam2RenderTex cam2RendetTex;
	[SerializeField]
	RawImage rawImg;

	// Use this for initialization
	void Start () {
		rawImg.texture = cam2RendetTex.RT;
	}
	
	// Update is called once per frame
	void Update () {
		rawImg.texture = cam2RendetTex.RT;
	}
}
