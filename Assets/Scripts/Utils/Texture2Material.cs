using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture2Material : MonoBehaviour {
    [SerializeField]
    Renderer renderer;
    [SerializeField]
    Texture texture;

    private void Awake()
    {
        renderer.material.mainTexture = texture;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        renderer.material.mainTexture = texture;
    }
}
