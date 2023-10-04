using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseTextureGenerator : MonoBehaviour {
	[SerializeField]
	Shader shader;
	Material material;
	[SerializeField]
	float elapsedTime;
	[SerializeField]
	Vector2 Rate;
	[SerializeField]
	float max;
	[SerializeField]
	float speed;

	void Awake(){
		material = new Material (shader);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		material.SetFloat ("_ElapsedTime",elapsedTime);
		material.SetVector ("_Rate",Rate);
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
