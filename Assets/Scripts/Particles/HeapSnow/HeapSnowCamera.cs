using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeapSnowCamera : ImageEffectApplier {
    [SerializeField]
    Cam2RenderTex shadow;
    void Start () {
		
	}

    private void Update()
    {
        material.SetTexture("_ShadowTex", shadow.RT);
    }
}
