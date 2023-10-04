using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneHandler : MonoBehaviour {
    
    [SerializeField]
    SceneController[] scenes;
    public SceneController[] Scenes
    {
        get
        {
            return scenes;
        }
    }
    [Space(10)]
    [SerializeField]
    SceneSettingsHandler sceneSettingsHandler;

    [ContextMenu("GetSceneControllers")]
    void GetSceneControllers()
    {
        this.scenes = GetComponentsInChildren<SceneController>();
        this.scenes = this.scenes.OrderBy(e => e.Id).ToArray();
    }

    void Awake()
    {
        foreach (var scene in scenes)
        {
            if (scenes.Count(e => e.Id == scene.Id) > 1)
            {
                Debug.LogWarning("duplicate scene id");
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void Play(int id)
    {
        var scene = scenes.FirstOrDefault(e => e.Id == id);
        if (scene != null)
        {
            scene.Play();
        }
    }

    public void SetScene(int id)
    {
        if (scenes.Count(e => e.Id == id) < 1)
        {
            id = 0;
        }
        //SceneController selectedScene = null;
        foreach (var scene in scenes)
        {
            if (scene.Id == id)
            {
                scene.Display();
                sceneSettingsHandler.SetScene(scene);
                //selectedScene = scene;
            }
            else
            {
                scene.Stop();
                scene.Hide();
            }
        }

    }
}
