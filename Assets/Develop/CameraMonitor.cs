using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraMonitor : MonoBehaviour {
    [SerializeField]
    RawImage rawImage;
    [SerializeField]
    Text text;
    public string Text
    {
        get
        {
            return text.text;
        }
        set
        {
            this.text.text = value;
        }
    }
    public RenderTexture rt;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rawImage.texture = rt;
	}
}
