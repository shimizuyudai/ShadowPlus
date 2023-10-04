using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSettingsHandler : MonoBehaviour {
    [SerializeField]
    RearImageController rearImageController;

	public void SetScene(SceneController sceneController)
    {
        rearImageController.IsUseMask = sceneController.IsUseMask;
        if (sceneController.IsUseMask && sceneController.ShadowTexture != null)
        {
            rearImageController.ShadowTexture = sceneController.ShadowTexture;
        }
    }
}
