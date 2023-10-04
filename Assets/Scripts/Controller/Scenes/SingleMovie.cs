using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class SingleMovie : SceneController
{
    [SerializeField]
    MediaPlayer movie;
    bool isPlay;

    public override void Display()
    {
        base.Display();
        Play();
    }

    public override void Play()
    {
        if (isPlay) return;
        movie.Play();
        isPlay = true;
    }

    public override void Stop()
    {
        movie.Stop();
        isPlay = false;
    }
}
