using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySceneController : MonoBehaviour
{
    [SerializeField]
    SceneHandler sceneHandler;

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            setScene(1);
        }
        else if (Input.GetKeyDown("2"))
        {
            setScene(2);
        }
        else if (Input.GetKeyDown("3"))
        {
            setScene(3);
        }
        else if (Input.GetKeyDown("4"))
        {
            setScene(4);
        }
        else if (Input.GetKeyDown("5"))
        {
            setScene(5);
        }
        else if (Input.GetKeyDown("6"))
        {
            setScene(6);
        }
        else if (Input.GetKeyDown("7"))
        {
            setScene(7);
        }
        else if (Input.GetKeyDown("8"))
        {
            setScene(8);
        }
        else if (Input.GetKeyDown("9"))
        {
            setScene(9);
        }
        else if (Input.GetKeyDown("0"))
        {
            setScene(0);
        }
        else if (Input.GetKeyDown("a"))
        {
            setScene(10);
        }
        else if (Input.GetKeyDown("b"))
        {
            setScene(11);
        }
    }

    void setScene(int id)
    {
        sceneHandler.SetScene(id);
        sceneHandler.Play(id);
    }
}
