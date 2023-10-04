using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SequenceData {
    //識別ID
    public int Id;
    //継続時間
    public float Duration;

    //トランジションの時の種類
    public int TransitionType;

    //トランジションの時の色
    public TypeUtil.Json.Color shadowColor;
    public Color TransitionShadowColor
    {
        get
        {
            return new Color(shadowColor.r, shadowColor.g, shadowColor.b, shadowColor.a);
        }
    }

    public TypeUtil.Json.Color transitionColor;
    public Color TransitionColor
    {
        get
        {
            return new Color(transitionColor.r, transitionColor.g, transitionColor.b, transitionColor.a);
        }
    }
}

