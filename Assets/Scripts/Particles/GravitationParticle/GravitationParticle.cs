using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class GravitationParticle : InteractiveParticleController
{

    struct Particle
    {
        public Vector3 pos;
        public Vector3 velocity;
        float size;
        public Particle(Vector3 pos, Vector3 velocity, float size)
        {
            this.pos = pos;
            this.velocity = velocity;
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
    Cam2RenderTex cam2RenderTex;
    ComputeBuffer particleBuffer;
    
    [SerializeField]
    float size;

    [SerializeField]
    float speed;
    [SerializeField]
    float radius;
    [SerializeField]
    Color color;
    [SerializeField]
    int invert;

    Vector3 target;
    [SerializeField]
    float minTime;
    [SerializeField]
    float maxTime;

    IEnumerator coroutine;

    public override void Init()
    {
        Particle[] particle = new Particle[particleBuffer.count];
        for (int i = 0; i < particleBuffer.count; i++)
        {
            particle[i] =
                new Particle(
                    new Vector3(Random.Range(-8f, 8f), Random.Range(-4.5f, 4.5f), 0f),
                    Vector3.zero,
                    size
                    );
        }

        particleBuffer.SetData(particle);
    }

    public override void Play()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = moveTarget();
        StartCoroutine(coroutine);
    }

    public override void Stop()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = null;
    }

    IEnumerator moveTarget()
    {
        while (true)
        {
            target = new Vector3(Random.Range(-6f, 6f), Random.Range(-3f, 3f), 0f);
            var waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void OnDestroy()
    {
        particleBuffer.Release();
    }

    void Start()
    {
        particleMaterial = new Material(particleShader);
        particleComputeShader.SetFloat("radius", radius);
        InitializeComputeBuffer();
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            invert = 1;
        }else
        {
            invert = 0;
        }
        particleComputeShader.SetVector("target",target);
        particleComputeShader.SetBuffer(0, "Particles", particleBuffer);
        particleComputeShader.SetTexture(0, "tex",cam2RenderTex.RT);
        particleComputeShader.SetFloat("DeltaTime", Time.deltaTime);
        particleComputeShader.SetFloat("speed", speed);
        particleComputeShader.Dispatch(0, num/8+1, 1, 1);
    }
    
    void InitializeComputeBuffer()
    {
        particleBuffer = new ComputeBuffer(num, Marshal.SizeOf(typeof(Particle)));
    }

    void OnRenderObject()
    {
        particleMaterial.SetTexture("_MainTex", particleTexture);
        particleMaterial.SetBuffer("Particles", particleBuffer);
        particleMaterial.SetColor("_Color",color);
        particleMaterial.SetFloat("_Size", size);
        particleMaterial.SetPass(0);
        Graphics.DrawProcedural(MeshTopology.Points, particleBuffer.count);
    }

}