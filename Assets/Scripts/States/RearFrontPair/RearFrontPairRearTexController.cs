using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearFrontPairRearTexController : MonoBehaviour {
    [SerializeField]
    Cam2RenderTex shadowTex;
    [SerializeField]
    string shadowTexParamName;
    [SerializeField]
    Renderer rearRenderer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rearRenderer.material.SetTexture(shadowTexParamName, shadowTex.RT);
	}
}
