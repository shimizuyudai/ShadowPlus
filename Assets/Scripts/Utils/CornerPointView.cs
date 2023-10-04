using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerPointView : MonoBehaviour {
    [SerializeField]
    WarpablePlane warpPlane;
    [SerializeField]
    ParticleSystem ps;
    [SerializeField]
    Color color;

    private void Awake()
    {
        ps.Stop();
    }

    private void Update()
    {
        
        if (warpPlane.IsEnable)
        {
            var particles = new List<ParticleSystem.Particle>();
            foreach (var cornerPoint in warpPlane.CornerPoints)
            {
                var p = new ParticleSystem.Particle();
                p.startSize = warpPlane.TouchDistanceThreshold * 2f;
                p.startColor = color;
                p.position = cornerPoint.position;
                particles.Add(p);
            }
            ps.SetParticles(particles.ToArray(), particles.Count);
        }else
        {
            if (ps.particleCount > 0)
            {
                ps.Clear();
            }
        }
    }
}
