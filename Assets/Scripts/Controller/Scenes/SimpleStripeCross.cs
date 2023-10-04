using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class SimpleStripeCross : SceneController {
    [SerializeField]
    MediaPlayer frontMovie, rearMovie;


    public override void Play()
    {
        frontMovie.Play();
        rearMovie.Play();
    }

    public override void Stop()
    {
        frontMovie.Stop();
        rearMovie.Stop();
    }
}
