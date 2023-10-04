using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AlertManager : MonoBehaviour {
    [SerializeField]
    GameObject alertObject;
    [SerializeField]
    AlertController alertController;

    private void Awake()
    {
        Hide();
    }

    public void DisplayAlert(string message, string buttonLabel, Action callback = null)
    {
        alertObject.SetActive(true);
        alertController.Display(message, buttonLabel, callback);
    }

    public void Hide()
    {
        alertController.Clear();
        alertObject.SetActive(false);
    }
}
