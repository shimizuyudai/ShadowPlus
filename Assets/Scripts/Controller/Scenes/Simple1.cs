using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simple1 : SceneController {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Display()
    {
        sceneObject.SetActive(true);
    }

    public override void Hide()
    {
        sceneObject.SetActive(false);
    }

    public override void Play()
    {

    }

    public override void Stop()
    {

    }
}
