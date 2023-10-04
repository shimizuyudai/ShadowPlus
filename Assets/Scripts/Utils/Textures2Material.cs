using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Textures2Material : MonoBehaviour {
    [SerializeField]
    Renderer renderer;
    [SerializeField]
    TextureInfo[] textureInfos;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [Serializable]
    public struct TextureInfo
    {
        public string Name;
        public Texture texture;
    }
}




