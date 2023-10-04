using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwitcher : MonoBehaviour {
    [SerializeField]
    KeyCode keyCode;
    [SerializeField]
    GameObject[] gameObjects;
    [SerializeField]
    int index;
	// Use this for initialization
	void Start () {
        switchObject();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(keyCode))
        {
            switchObject();
        }
	}

    void switchObject()
    {
        for (var i = 0; i < gameObjects.Length; i++)
        {
            if (i == index)
            {
                gameObjects[i].SetActive(true);
            }else
            {
                gameObjects[i].SetActive(false);
            }
        }

        index++;
        if (index >= gameObjects.Length)
        {
            index = 0;
        }
    }
}
