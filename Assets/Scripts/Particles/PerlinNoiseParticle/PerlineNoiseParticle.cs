using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class PerlineNoiseParticle : InteractiveParticleController
{

    struct Particle
    {
        Vector3 startPos;
        Vector3 pos;
        Vector3 velocity;
        float brightness;
        public Particle(Vector3 startPos, Vector3 pos, Vector3 velocity, float brightness)
        {
            this.startPos = startPos;
            this.pos = pos;
            this.velocity = velocity;
            this.brightness = brightness;
        }
    }

    [SerializeField]
    int num;

    [SerializeField]
    Shader particleShader;
    [SerializeField]
    Texture particleTexture;
    [SerializeField]
    ComputeShader particleComputeShader;

    Material particleMaterial;

    [SerializeField]
    Cam2RenderTex shadowTexture;
    ComputeBuffer particleBuffer;
    [SerializeField]
    Cam2RenderTex perlinNoiseTexture;
    [SerializeField]
    float size;

    [SerializeField]
    float speed;
    [SerializeField]
    Color color;

    [SerializeField]
    float timeSpeed, maxTime;
    [SerializeField]
    float elapsedTime;
    [SerializeField]
    float ratio;

    public override void Init()
    {
        Particle[] particle = new Particle[particleBuffer.count];
        for (int i = 0; i < particleBuffer.count; i++)
        {
            var pos = new Vector3(Random.Range(-8f, 8f), Random.Range(-4.5f, 4.5f), 0f);
            particle[i] =
                new Particle(
                    pos,
                    pos,
                    Vector3.zero,
                    0f
                    );
        }
        particleBuffer.SetData(particle);
    }

    public override void Play()
    {

    }

    public override void Stop()
    {

    }

    void OnDestroy()
    {
        particleBuffer.Release();
    }

    void Awake()
    {
        particleMaterial = new Material(particleShader);
        InitializeComputeBuffer();
        particleMaterial.SetColor("_Color", color);
    }

    void Update()
    {
        particleComputeShader.SetFloat("ElapsedTime", elapsedTime);
        particleComputeShader.SetBuffer(0, "Particles", particleBuffer);
        particleComputeShader.SetTexture(0, "shadowTexture", shadowTexture.RT);
        particleComputeShader.SetFloat("DeltaTime", Time.deltaTime);
        particleComputeShader.SetTexture(0, "noiseTexture", perlinNoiseTexture.RT);
        particleComputeShader.SetFloat("speed", speed);
        particleComputeShader.SetFloat("ratio",ratio);
        
        particleComputeShader.Dispatch(0, num / 8 + 1, 1, 1);
        elapsedTime += timeSpeed * Time.deltaTime;
        if (elapsedTime < -maxTime || elapsedTime > maxTime)
        {
            timeSpeed *= -1f;
        }
    }

    void InitializeComputeBuffer()
    {
        particleBuffer = new ComputeBuffer(num, Marshal.SizeOf(typeof(Particle)));


    }

    void OnRenderObject()
    {
        particleMaterial.SetTexture("_MainTex", particleTexture);
        particleMaterial.SetBuffer("Particles", particleBuffer);
        particleMaterial.SetFloat("_Size", size);
        particleMaterial.SetColor("_Color", color);
        particleMaterial.SetPass(0);
        Graphics.DrawProcedural(MeshTopology.Points, particleBuffer.count);
    }

}