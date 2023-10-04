using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class SplashParticle : InteractiveParticleController
{

    struct Particle
    {
        Vector3 pos;
        Vector3 defaultPosition;
        Vector3 velocity;
        Vector3 splashVelocity;
        float destination;
        float speed;
        float size;
        float splashRate;
        int isRaise;
        public Particle(Vector3 pos, Vector3 defaultPosition, float destination, Vector3 velocity, Vector3 splashVelocity, float speed, float size, float splashRate, int isRaise)
        {
            this.pos = pos;
            this.defaultPosition = defaultPosition;
            this.destination = destination;
            this.velocity = velocity;
            this.splashVelocity = splashVelocity;
            this.speed = speed;
            this.size = size;
            this.splashRate = splashRate;
            this.isRaise = isRaise;
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
    Color color;

    [SerializeField]
    float minSize, maxSize;
    [SerializeField]
    float minSplashRate, maxSplashRate;
    [SerializeField]
    float minSpeed, maxSpeed, minAngle, maxAngle;

    [SerializeField]
    float gravity;

    [SerializeField]
    float splashSpeed;

    [SerializeField]
    float minSplashSpeed, maxSplashSpeed;

    public override void Init()
    {
        Particle[] particle = new Particle[particleBuffer.count];
        for (int i = 0; i < particleBuffer.count; i++)
        {
            var pos = new Vector3(Random.Range(-8f, 8f), 0f, 0f);
            var speedRate = Random.Range(minSplashSpeed, maxSplashSpeed);
            var angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
            particle[i] =
                new Particle(
                    pos,
                    pos,
                    0f,
                    Vector3.zero,
                    new Vector3(Mathf.Cos(angle) * speedRate, Mathf.Sin(angle) * speedRate,0f),
                    Random.Range(minSpeed, maxSpeed),
                    Random.Range(minSize, maxSize),
                    Random.Range(minSplashRate, maxSplashRate),
                    0
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
        particleMaterial.SetTexture("_MainTex", particleTexture);
        InitializeComputeBuffer();
    }
    
    void Update()
    {
        particleComputeShader.SetBuffer(0, "Particles", particleBuffer);
        particleComputeShader.SetFloat("DeltaTime", Time.deltaTime);
        particleComputeShader.SetTexture(0, "shadowTexture", shadowTexture.RT);
        particleComputeShader.Dispatch(0, num/8+1, 1, 1);
    }
    
    void InitializeComputeBuffer()
    {
        particleBuffer = new ComputeBuffer(num, Marshal.SizeOf(typeof(Particle)));
        particleComputeShader.SetFloat("gravity",gravity);
        particleComputeShader.SetFloat("splashSpeed",splashSpeed);
    }

    void OnRenderObject()
    {
        
        particleMaterial.SetBuffer("Particles", particleBuffer);
        particleMaterial.SetColor("_Color",color);
        particleMaterial.SetPass(0);
        Graphics.DrawProcedural(MeshTopology.Points, particleBuffer.count);
    }

}