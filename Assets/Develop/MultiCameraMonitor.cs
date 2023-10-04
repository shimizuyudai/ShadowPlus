using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCameraMonitor : MonoBehaviour {
    [SerializeField]
    Camera[] cameras;
    [SerializeField]
    GameObject monitorPrefab;
    [SerializeField]
    int w, h;
    [SerializeField]
    Transform parent;

    private void Start()
    {
        foreach (var cam in cameras)
        {
            var obj = GameObject.Instantiate(monitorPrefab) as GameObject;
            var component = obj.GetComponent<CameraMonitor>();
            var cam2RT = cam.GetComponent<Cam2RenderTex>();
            RenderTexture rt = null;
            if (cam2RT == null)
            {
                rt = new RenderTexture(w, h, 24);
                cam.targetTexture = rt;
            }
            else
            {
                rt = cam2RT.RT;
            }
            component.rt = rt;
            component.Text = cam.name;
            obj.transform.SetParent(parent, false);
            obj.name = cam.name;
        }
    }
}
