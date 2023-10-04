using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#if UNITY_EDITOR
using UnityEditor;
#endif



public class ProteanSnow : InteractiveParticleController
{

    struct Particle
    {
        public Vector3 startPos;
        public Vector3 pos;
        public Vector3 velocity;
        Vector3 angle;
        float size;
        int textureId;
        public Particle(Vector3 startPos, Vector3 pos, Vector3 velocity, Vector3 angle, float size, int textureId)
        {
            this.startPos = startPos;
            this.pos = pos;
            this.velocity = velocity;
            this.angle = angle;
            this.size = size;
            this.textureId = textureId;
        }
    }

    const int MAX_VERTEX_NUM = 65534;
    [SerializeField]
    Camera targetCamera;
    [SerializeField, Tooltip("This cannot be changed while running.")]
    int maxParticleNum;
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    Shader shader;
    [SerializeField]
    ComputeShader computeShader;

    Mesh combinedMesh_;
    ComputeBuffer particlesBuffer;
    Material material;
    List<MaterialPropertyBlock> propertyBlocks_ = new List<MaterialPropertyBlock>();
    int particleNumPerMesh_;
    int meshNum_;

    [SerializeField]
    string layerName;

    [SerializeField]
    float speed;
    [SerializeField]
    Color color;
    [SerializeField]
    float size;
    [SerializeField]
    Vector3 angle;

    [SerializeField]
    float minAngle, maxAngle;

    [SerializeField]
    Cam2RenderTex shadowTexture;

    [SerializeField]
    Texture mainTexture;
    [SerializeField]
    Texture subTexture;

    [SerializeField]
    float minSize, maxSize;

    [SerializeField]
    Vector3 rotationSpeed;

    void Start()
    {
        initialize();
    }

    Mesh CreateCombinedMesh(Mesh mesh, int num)
    {
        Assert.IsTrue(mesh.vertexCount * num <= MAX_VERTEX_NUM);

        var meshIndices = mesh.GetIndices(0);
        var indexNum = meshIndices.Length;

        var vertices = new List<Vector3>();
        var indices = new int[num * indexNum];
        var normals = new List<Vector3>();
        var tangents = new List<Vector4>();
        var uv0 = new List<Vector2>();
        var uv1 = new List<Vector2>();

        for (int id = 0; id < num; ++id)
        {
            vertices.AddRange(mesh.vertices);
            normals.AddRange(mesh.normals);
            tangents.AddRange(mesh.tangents);
            uv0.AddRange(mesh.uv);
            
            for (int n = 0; n < indexNum; ++n)
            {
                indices[id * indexNum + n] = id * mesh.vertexCount + meshIndices[n];
            }
            
            for (int n = 0; n < mesh.uv.Length; ++n)
            {
                uv1.Add(new Vector2(id, id));
            }
        }

        var combinedMesh = new Mesh();
        combinedMesh.SetVertices(vertices);
        combinedMesh.SetIndices(indices, MeshTopology.Triangles, 0);
        combinedMesh.SetNormals(normals);
        combinedMesh.RecalculateNormals();
        combinedMesh.SetTangents(tangents);
        combinedMesh.SetUVs(0, uv0);
        combinedMesh.SetUVs(1, uv1);
        combinedMesh.RecalculateBounds();
        combinedMesh.bounds.SetMinMax(Vector3.one * -100f, Vector3.one * 100f);

        return combinedMesh;
    }

    void initialize()
    {
        // メッシュの結合
        {
            particleNumPerMesh_ = MAX_VERTEX_NUM / mesh.vertexCount;
            meshNum_ = (int)Mathf.Ceil((float)maxParticleNum / particleNumPerMesh_);
            combinedMesh_ = CreateCombinedMesh(mesh, particleNumPerMesh_);
        }

        // 必要な数だけマテリアルを作成
        material = new Material(shader);
        
        for (int i = 0; i < meshNum_; ++i)
        {
            var props = new MaterialPropertyBlock();
            props.SetFloat("_IdOffset", particleNumPerMesh_ * i);
            propertyBlocks_.Add(props);
        }

        // ComputeBuffer の初期化
        {
            particlesBuffer = new ComputeBuffer(maxParticleNum, Marshal.SizeOf(typeof(Particle)));
            Particle[] particle = new Particle[particlesBuffer.count];
            for (int i = 0; i < particlesBuffer.count; i++)
            {
                var startPos = new Vector3(Random.Range(-10f, 10f), Random.Range(4.5f, 20f), 0f);
                var pos = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
                var angle = Random.Range(minAngle, maxAngle);
                var direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
                particle[i] =
                    new Particle(
                        startPos,
                        pos,
                        direction,
                        Vector3.zero,
                       size,// Random.Range(minSize,maxSize),
                        0
                        );
            }
            particlesBuffer.SetData(particle);
        }
    }

    void OnDestroy()
    {
        particlesBuffer.Release();
    }

    void Update()
    {
        DispatchUpdate();
       RegisterDraw(targetCamera);
#if UNITY_EDITOR
        if (SceneView.lastActiveSceneView)
        {
            RegisterDraw(SceneView.lastActiveSceneView.camera);
        }
#endif
    }


    void DispatchUpdate()
    {
        computeShader.SetBuffer(0, "Particles", particlesBuffer);
        computeShader.SetTexture(0, "shadowTexture", shadowTexture.RT);
        computeShader.SetFloat("DeltaTime", Time.deltaTime);
        computeShader.SetFloat("speed", speed);
        computeShader.SetVector("rotationSpeed", rotationSpeed);
        computeShader.Dispatch(0, maxParticleNum / 8 + 1, 1, 1);
    }

    void RegisterDraw(Camera camera)
    {
        material.SetBuffer("_Particles", particlesBuffer);
        material.SetColor("_Color", color);
        material.SetTexture("_MainTex", mainTexture);
        material.SetTexture("_SubTex", subTexture);
        for (int i = 0; i < meshNum_; ++i)
        {
            var props = propertyBlocks_[i]; 
            props.SetFloat("_IdOffset", particleNumPerMesh_ * i);
            
            Graphics.DrawMesh(combinedMesh_, transform.position, transform.rotation, material, LayerMask.NameToLayer(layerName), camera, 0, props);
        }
    }
}
