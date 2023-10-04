using UnityEngine;
using System.Collections;

public static class EMath
{//計算処理クラス

    public static float Map(float currentVal, float tempAStart, float tempAGoal, float tempBStart, float tempBGoal)
    {

        return ((currentVal - tempAStart) / (tempAGoal - tempAStart)) * (tempBGoal - tempBStart) + tempBStart;

    }

    public static float ConvertToRadians(float angle)//degreeからradianに変換
    {
        return (Mathf.PI / 180.0f) * angle;
    }

    public static float Constrain(float baseVal, float minVal, float maxVal)
    {
        float tempVal = Mathf.Min(baseVal, maxVal);
        tempVal = Mathf.Max(tempVal, minVal);
        return tempVal;
    }

    public static float EaseIn(float tempCurrentVal, float tempOverallValue, int tempEaseIndex, float destinatonVal)//イージング処理
    {
        float temp = tempCurrentVal / tempOverallValue/*時間(操作する軸)*/;
        temp = destinatonVal/*目的値*/* Mathf.Pow(temp, tempEaseIndex);
        return temp;//操作される側の値
    }

    public static float EaseOut(float tempCurrentVal, float tempOverallValue, int tempEaseIndex, float destinatonVal)
    {

        float temp = tempCurrentVal / tempOverallValue/*時間(操作する軸)*/;
        temp = 1 - temp;
        temp = destinatonVal/*目的値*/* Mathf.Pow(temp, tempEaseIndex);
        return temp;//操作される側の値
    }

}
