using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera4RearImage : Cam2RenderTex
{
    [SerializeField]
    RearImageController rearImageController;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        rearImageController.RenderTexture = this.rt;
    }


}
