using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexureHorizontalSlider : MonoBehaviour {
    [SerializeField]
    float speed;
    float u;
    [SerializeField]
    Renderer renderer;
    [SerializeField]
    float maxU;
	// Use this for initialization
	void Start () {
		
	}

    private void OnDisable()
    {
        u = 0f;
    }

    // Update is called once per frame
    void Update () {
        u += speed * Time.deltaTime;
        if (u >= maxU || u <= -maxU)
        {
            u = 0f;
        }
        renderer.material.SetFloat("_HorizontalScroll",u);
	}
}
