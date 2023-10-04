using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveParticleSceneController : SceneController {
    [SerializeField]
    protected InteractiveParticleController interactiveParticleController;

    public override void Display()
    {
        base.Display();
        interactiveParticleController.Init();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Play()
    {
        interactiveParticleController.Play();
    }

    public override void Stop()
    {
        interactiveParticleController.Stop();
    }
}
