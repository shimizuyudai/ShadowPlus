using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTexture2Renderere : MonoBehaviour {
    [SerializeField]
    RenderTexture rt;
    [SerializeField]
    Renderer renderer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        renderer.material.mainTexture = rt;
	}
}
