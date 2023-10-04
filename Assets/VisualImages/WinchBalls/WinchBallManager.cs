using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinchBallManager : MonoBehaviour {
    [SerializeField]
    int num;
    [SerializeField]
    float margin, randomRange, minFrequency, maxFrequency, maxTime, timeSpeed, saturation, brightness;
    [SerializeField]
    GameObject winchPrefab;
    
    List<WinchBall> winchBalls = new List<WinchBall>();
    float elapsedTime;
    

    private void Awake()
    {
        generate();
    }

    void generate()
    {
        winchBalls = new List<WinchBall>();
        var area = 16f - margin * (float)(num + 1);
        var defaultScale = area / (float)num;
        var scaleList = new List<float>();
        for (var i = 0; i < num; i++)
        {
            scaleList.Add(defaultScale);
        }

        for (var i = 0; i < scaleList.Count - 1; i++)
        {
            var randomValue = Random.Range(-randomRange, randomRange);
            scaleList[i] += randomValue;
            scaleList[(int)Random.Range(0,num)] -= randomValue;
        }
        

        var positionXList = new List<float>();
        positionXList.Add(-8f + margin + scaleList[0] / 2f);
        for (var i = 1; i < num; i++)
        {
            positionXList.Add(positionXList[i-1] + scaleList[i-1]/2f + margin + scaleList[i] / 2f);
        }
        print(positionXList[num - 1] + scaleList[num - 1] / 2f + margin);
        for (var i = 0; i < num; i++)
        {
            var obj = GameObject.Instantiate(winchPrefab) as GameObject;
            obj.transform.SetParent(this.transform, false);
            var component = obj.GetComponent<WinchBall>();
            var color = Color.HSVToRGB(Random.Range(0f,1f),saturation,brightness);
            component.Frequency = Random.Range(minFrequency, maxFrequency);
            component.Init(positionXList[i], new Vector3(scaleList[i], scaleList[i], scaleList[i]), color);
            winchBalls.Add(component);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        foreach (var winchBall in winchBalls)
        {
            winchBall.Refresh(elapsedTime);
        }
        elapsedTime += timeSpeed * Time.deltaTime;
        if (elapsedTime > maxTime || elapsedTime < -maxTime)
        {
            timeSpeed *= -1f;
        }
	}
}
