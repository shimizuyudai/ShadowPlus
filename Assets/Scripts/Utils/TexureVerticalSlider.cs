using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexureVerticalSlider : MonoBehaviour {
    [SerializeField]
    float speed;
    float v;
    [SerializeField]
    Renderer renderer;
    [SerializeField]
    float maxV;
	// Use this for initialization
	void Start () {
		
	}

    private void OnDisable()
    {
        v = 0f;
    }
    // Update is called once per frame
    void Update () {
        v += speed * Time.deltaTime;
        if (v >= maxV || v <= -maxV)
        {
            v = 0f;
        }
        renderer.material.SetFloat("_VerticalScroll",v);

        print("called");
	}
}
