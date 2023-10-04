using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class DotPattern : InteractiveParticleController
{

    struct Particle
    {
        Vector2 pos;
        Color color;
        public Particle(Vector2 pos, Color color)
        {
            this.pos = pos;
            this.color = color;
        }
    }

    [SerializeField]
    Shader particleShader;
    [SerializeField]
    Texture particleTexture;
    [SerializeField]
    ComputeShader particleComputeShader;
    
    Material particleMaterial;
    ComputeBuffer particleBuffer;

    int num;
    [SerializeField]
    Vector2 area;
    [SerializeField]
    int segmentW, segmentH;
    float size;
    [SerializeField]
    float saturation, brightness;
    [SerializeField]
    float timeSpeed;
    float elapsedTime;
    [SerializeField]
    float maxTime;

    public override void Init()
    {
        Particle[] particle = new Particle[particleBuffer.count];
        print((float)segmentH*size);
        var start = new Vector2(-8f + (16f - area.x)/2f + size/2f, 4.5f - (9f - area.y)/2f - size/2f) + new Vector2(this.transform.position.x, this.transform.position.y);
        int index = 0;
        for (var y = 0; y < segmentH; y++)
        {
            var length = segmentW;
            var offsetX = 0f;
            if (y % 2 == 1)
            {
                length = segmentW - 1;
                offsetX = size / 2f;
            }
            
            for (var x = 0; x < length; x++)
            {
                var pos = new Vector2(start.x + size*(float)x + offsetX, start.y - size*(float)y);
                var color = Color.HSVToRGB(Random.Range(0f,1f),saturation,brightness);
                //color = new Color(color.r,color.g,color.b,1f);
                particle[index] =
                new Particle(
                    pos,
                    color
                    );
                index++;
            }
        }

        //for (int i = 0; i < particleBuffer.count; i++)
        //{
        //    var pos = new Vector3(Random.Range(-10f, 10f), Random.Range(-4.5f * 2f, 4.5f * 3f), 0f);
        //    particle[i] =
        //        new Particle(
        //            pos,
        //            Color.magenta
        //            );
        //}
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
        num = 0;
        size = area.x / (float)segmentW;
        for (var y = 0; y < segmentH; y++)
        {
            if (y % 2 == 1)
            {
                num += segmentW - 1;
            }
            else
            {
                num += segmentW;
            }
        }

        particleMaterial = new Material(particleShader);
        particleMaterial.SetFloat("_Size", size);
        InitializeComputeBuffer();
    }
    
    void Update()
    {
        
        particleComputeShader.SetBuffer(0, "Particles", particleBuffer);
        particleComputeShader.SetFloat("ElapsedTime", elapsedTime);
        particleComputeShader.Dispatch(0, num/8+1, 1, 1);

        elapsedTime += timeSpeed * Time.deltaTime;
        if (elapsedTime >= maxTime || elapsedTime <= -maxTime)
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
        particleMaterial.SetPass(0);
        Graphics.DrawProcedural(MeshTopology.Points, particleBuffer.count);
    }

}