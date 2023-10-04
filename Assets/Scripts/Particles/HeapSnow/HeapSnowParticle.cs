using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class HeapSnowParticle : InteractiveParticleController
{

    struct Particle
    {
        public Vector3 startPos;
        public Vector3 pos;
        public Vector3 velocity;
        public Vector3 startVelocity;
        public Color color;
        float size;
        public Particle(Vector3 startPos, Vector3 pos, Vector3 velocity, Vector3 startVelocity, Color color,float size)
        {
            this.startPos = startPos;
            this.pos = pos;
            this.velocity = velocity;
            this.startVelocity = startVelocity;
            this.color = color;
            this.size = size;
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
    Cam2RenderTex heapTexture;
    ComputeBuffer particleBuffer;

    [SerializeField]
    float size;
    [SerializeField]
    float speed;
    [SerializeField]
    Color color;

    [SerializeField]
    float minAngle, maxAngle;

    [SerializeField]
    Cam2RenderTex rt;

    [SerializeField]
    float minSpeedY, maxSpeedY, minSize, maxSize;

    int mainKernel;
    [SerializeField]
    float offsetY;

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
                    startPos,
                    pos,
                    direction,
                    direction,
                    Color.magenta,
                    Random.Range(minSize, maxSize)
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

    void Start()
    {
        mainKernel = particleComputeShader.FindKernel("CSMain");
        particleMaterial = new Material(particleShader);
        InitializeComputeBuffer();
    }

    void Update()
    {

        //Graphics.SetRenderTarget(result);
       // GL.Clear(false, true, Color.black);
       
        particleComputeShader.SetFloat("size", size);
        particleComputeShader.SetBuffer(mainKernel, "Particles", particleBuffer);
        particleComputeShader.SetTexture(mainKernel, "heapTexture", heapTexture.RT);
       // particleComputeShader.SetTexture(mainKernel, "shadowTexture", shadowTexture.RT);
        particleComputeShader.SetFloat("DeltaTime", Time.deltaTime);
        particleComputeShader.SetFloat("speed", speed);
        particleComputeShader.SetFloat("offsetY", offsetY);
        particleComputeShader.Dispatch(mainKernel, num / 8 + 1, 1, 1);
        
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
        particleMaterial.SetPass(0);
        Graphics.DrawProcedural(MeshTopology.Points, particleBuffer.count);
    }
}