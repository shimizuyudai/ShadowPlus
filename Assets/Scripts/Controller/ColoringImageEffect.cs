using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoringImageEffect : ImageEffectApplier {

    [SerializeField]
    private Color color;

    protected override void Awake()
    {
        base.Awake();
        material.SetColor("_Color",color);
    }
}
