using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinchBall : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;
    [SerializeField]
    Renderer renderer;

    public Vector3 Scale
    {
        get
        {
            return this.transform.localScale;
        }
        set
        {
            this.transform.localScale = value;
        }
    }

    public Color Color
    {
        set
        {
            this.renderer.material.color = value;
        }
    }

    float minY, maxY;
    public float Frequency;

    public void Init(float x, Vector3 scale, Color color)
    {
        this.Scale = scale;
        this.renderer.material.color = color;
        //var a = Random.Range(-4.5f + this.Scale.y/2f, 4.5f - this.Scale.y/2f);
        //var b = Random.Range(-4.5f + this.Scale.y/2f, 4.5f - this.Scale.y/2f);
        minY = Random.Range(-4.5f + this.Scale.y / 2f, 0f);
        maxY = Random.Range(0f, 4.5f - this.Scale.y / 2f);
        lineRenderer.SetPosition(0, new Vector3(x, 4.5f, this.transform.position.z + 1f));
        var val = Mathf.PerlinNoise(this.transform.position.x, 0f * Frequency);
        var y = EMath.Map(val, 0f, 1f, minY, maxY);
        var pos = new Vector3(x, y, this.transform.position.z);
        this.transform.position = pos;
        this.lineRenderer.SetPosition(1, new Vector3(pos.x, pos.y, pos.z + 1f));
    }

    public void Refresh(float elapsedTime)
    {
        var val = Mathf.PerlinNoise(this.transform.position.x, elapsedTime * Frequency);
        var y = EMath.Map(val, 0f, 1f, minY, maxY);
        var pos = new Vector3(this.transform.position.x, y, this.transform.position.z);
        this.transform.position = pos;
        this.lineRenderer.SetPosition(1, new Vector3(pos.x, pos.y, pos.z + 1f));
    }
}
