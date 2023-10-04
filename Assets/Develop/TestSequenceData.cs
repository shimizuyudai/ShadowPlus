using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TestSequenceData {
    //識別ID
    public int Id;
    //継続時間
    public float Duration;
    //トランジションの時の種類
    public int TransitionType;
    //トランジションの時の色
    public TypeUtil.Json.Color shadowColor;
    public TypeUtil.Json.Color transitionColor;
}
