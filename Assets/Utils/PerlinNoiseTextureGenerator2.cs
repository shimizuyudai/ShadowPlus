using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseTextureGenerator2 : MonoBehaviour {
	[SerializeField]
	Shader shader;
	Material material;
	float elapsedTime;
	[SerializeField]
	Vector2 Rate;
	[SerializeField]
	float max;
	[SerializeField]
	float speed;

	[Range(0,1)]
	[SerializeField]
	float brightnessRate;

	void Awake(){
		material = new Material (shader);
	}

	// Use this for initialization
	void Start () {
		
	}

    private void OnDisable()
    {
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update () {
		material.SetFloat ("_ElapsedTime",elapsedTime);
		material.SetVector ("_Rate",Rate);
		material.SetFloat ("_BrightnessRate",brightnessRate);
		elapsedTime += speed*Time.deltaTime;
		if(elapsedTime > max || elapsedTime < -max){
			speed *= -1f;
		}
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, material);
	}
}
