using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiInteractiveParticleSceneController : SceneController {
    [SerializeField]
    protected InteractiveParticleController[] interactiveParticleController;

    public override void Display()
    {
        base.Display();
        foreach (var interactiveParticleController in interactiveParticleController)
        {
            interactiveParticleController.Init();
        }
        
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Play()
    {
        foreach (var interactiveParticleController in interactiveParticleController)
        {
            interactiveParticleController.Play();
        }
    }

    public override void Stop()
    {
        foreach (var interactiveParticleController in interactiveParticleController)
        {
            interactiveParticleController.Stop();
        }
    }
}
