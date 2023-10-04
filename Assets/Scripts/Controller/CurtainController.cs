using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurtainController : MonoBehaviour {
    [SerializeField]
    Image image;

    public Color Color
    {
        get
        {
            return image.color;
        }
        set
        {
            image.color = value;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
