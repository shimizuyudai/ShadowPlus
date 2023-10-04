using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearImageController : MonoBehaviour
{
    [SerializeField]
    Renderer Renderer;

    RenderTexture renderTexture;
    public RenderTexture RenderTexture
    {
        get
        {
            return renderTexture;
        }
        set
        {
            this.Renderer.material.SetTexture("_MainTex",value);
            renderTexture = value;
        }
    }

    [Space(10)]
    [Header("マスクに使用するイメージ")]
    [SerializeField]
    string shadowTextureParm;
    public Cam2RenderTex ShadowTexture;


    [Space(10)]
    [Header("マスクのオン/オフ")]
    [SerializeField]
    string isUseMaskKeyword;
    public bool IsUseMask
    {
        set
        {
            if (value)
            {
                this.Renderer.material.EnableKeyword(isUseMaskKeyword);
            }
            else
            {
                this.Renderer.material.DisableKeyword(isUseMaskKeyword);
            }

        }
    }

    public Color ShadowColor
    {
        set
        {
            this.Renderer.material.SetColor("_ShadowColor",value);
        }
    }

    private void Update()
    {
        if (ShadowTexture != null)
        {
            this.Renderer.material.SetTexture(shadowTextureParm, ShadowTexture.RT);
        }
       
        
    }

    //[SerializeField]
    //string isUseGrayScaleKeyword;
    //public bool IsUseGrayScale
    //{
    //    set
    //    {
    //        if (value)
    //        {
    //            this.Renderer.material.EnableKeyword(isUseGrayScaleKeyword);
    //        }
    //        else
    //        {
    //            this.Renderer.material.DisableKeyword(isUseGrayScaleKeyword);
    //        }
    //    }
    //}

}
