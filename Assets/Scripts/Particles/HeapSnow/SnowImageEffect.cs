using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowImageEffect : ImageEffectApplier
{
    [SerializeField]
    Color color;
    [Range(0,5)]
    [SerializeField]
    float rate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        material.SetColor("_Color",color);
        material.SetFloat("_Rate",rate);
	}
}
