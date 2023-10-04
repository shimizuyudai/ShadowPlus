using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RenderHeads.Media.AVProVideo;

public class MultiMovie : SceneController {
    [SerializeField]
    MediaPlayer[] movies;
    bool isPlay;

    public override void Display()
    {
        base.Display();
        Play();
    }

    public override void Play()
    {
        if (isPlay) return;
        foreach (var movie in movies)
        {
            movie.Play();
        }
        isPlay = true;
    }

    public override void Stop()
    {
        foreach (var movie in movies)
        {
            movie.Stop();
        }
        isPlay = false;
    }
}
