using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {
    [SerializeField]
    protected int id;
    [SerializeField]
    protected GameObject sceneObject;
    [SerializeField]
    protected bool isUseMask;
    public bool IsUseMask
    {
        get
        {
            return isUseMask;
        }
    }
    [SerializeField]
    protected Cam2RenderTex shadowTexture;
    public Cam2RenderTex ShadowTexture
    {
        get
        {
            return shadowTexture;
        }
    }
        
    public int Id
    {
        get
        {
            return id;
        }
    }

    public virtual void Display()
    {
        sceneObject.SetActive(true);
    }

    public virtual void Hide()
    {
        sceneObject.SetActive(false);
    }

    public virtual void Play()
    {

    }

    public virtual void Stop()
    {
        
    }
    
}
