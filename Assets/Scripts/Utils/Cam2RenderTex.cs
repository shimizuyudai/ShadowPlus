using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam2RenderTex : MonoBehaviour {
    [SerializeField]
    int w = 1920;
    [SerializeField]
    int h = 1080;
    [SerializeField]
    protected Camera cam;
    [SerializeField]
    protected RenderTexture rt;
	public RenderTexture RT{
		get{
			return rt;
		}
	}

	protected virtual void Awake()
	{
        if (rt == null)
        {
            rt = new RenderTexture(w, h, 24);
        }
        cam.targetTexture = rt;
    }
}
