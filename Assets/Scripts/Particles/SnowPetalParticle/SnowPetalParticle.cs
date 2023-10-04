using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class SnowPetalParticle : InteractiveParticleController
{

    struct Particle
    {
        Vector3 rotation;
        Vector3 defaultRotation;
        Vector3 startPos;
        Vector3 pos;
        Vector3 velocity;
        float size;
        int textureId;
        float rotationSpeed;

        public Particle(Vector3 rotation, Vector3 defaultRotation, Vector3 startPos, Vector3 pos, Vector3 velocity, float size, int textureId, float rotationSpeed)
        {
            this.rotation = Vector3.zero;
            this.defaultRotation = defaultRotation;
            this.startPos = startPos;
            this.pos = pos;
            this.velocity = velocity;
            this.size = size;
            this.textureId = textureId;
            this.rotationSpeed = rotationSpeed;
        }
    }

    [SerializeField]
    int num;

    [SerializeField]
    Shader particleShader;
    [SerializeField]
    Texture particleTexture;
    [SerializeField]
    Texture petalTexture;
    [SerializeField]
    Color petalColor;

    [SerializeField]
    ComputeShader particleComputeShader;

    Material particleMaterial;

    [SerializeField]
    Cam2RenderTex shadowTexture;
    ComputeBuffer particleBuffer;

    [SerializeField]
    float size;
    [SerializeField]
    float speed;
    [SerializeField]
    Color color;
    [SerializeField]
    float angle;

    [SerializeField]
    float minSize, maxSize, minRotationSpeed, maxRotationSpeed;

    [SerializeField]
    float minAngle, maxAngle;

    [SerializeField]
    float timeSpeed, maxTime;
    float elapsedTime;
    [SerializeField]
    float minSpeedY, maxSpeedY;

    public override void Init()
    {
        Particle[] particle = new Particle[particleBuffer.count];
        for (int i = 0; i < particleBuffer.count; i++)
        {
            var startPos = new Vector3(Random.Range(-10f, 10f), Random.Range(4.5f, 20f), 0f);
            var pos = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
            var angle = Random.Range(minAngle,maxAngle);
            var direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle) - Random.Range(minSpeedY,maxSpeedY), 0f);
            particle[i] =
                new Particle(
                    Vector3.zero,
                    Vector3.zero,
                    startPos,
                    pos,
                    direction,
                    Random.Range(minSize, maxSize),
                    0,
                    Random.Range(minRotationSpeed,maxRotationSpeed)
                    );
        }
        particleBuffer.SetData(particle);
    }

    public override void Play()
    {

    }

    public override void Stop()
    {
        elapsedTime = 0f;
    }


    void OnDestroy()
    {
        particleBuffer.Release();
    }

    void Awake()
    {
        particleMaterial = new Material(particleShader);
        InitializeComputeBuffer();
    }

    void Update()
    {
        var dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        particleComputeShader.SetBuffer(0, "Particles", particleBuffer);
        particleComputeShader.SetTexture(0, "shadowTexture", shadowTexture.RT);
        particleComputeShader.SetFloat("DeltaTime", Time.deltaTime);
        particleComputeShader.SetFloat("ElapsedTime", elapsedTime);
        particleComputeShader.SetFloat("speed", speed);
        particleComputeShader.Dispatch(0, num / 8 + 1, 1, 1);

        elapsedTime += timeSpeed * Time.deltaTime;
        if (elapsedTime > maxTime || elapsedTime < -maxTime)
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
        particleMaterial.SetTexture("_PetalTex", petalTexture);
        particleMaterial.SetBuffer("Particles", particleBuffer);
        particleMaterial.SetColor("_Color", color);
        particleMaterial.SetColor("_SubColor", petalColor);
        particleMaterial.SetFloat("_Angle", angle);
        particleMaterial.SetPass(0);
        Graphics.DrawProcedural(MeshTopology.Points, particleBuffer.count);
    }
}