using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AlertController : MonoBehaviour {
    [SerializeField]
    Text message;
    [SerializeField]
    Text buttonLabel;
    Action callback;

	public void Display(string message, string buttonLabel,Action callback = null)
    {
        this.message.text = message;
        this.buttonLabel.text = buttonLabel;
        this.callback = callback;
    }

    public void Clear()
    {
        this.message.text = string.Empty;
        this.buttonLabel.text = string.Empty;
        callback = null;
    }

    public void OnClickedButton()
    {
        if (this.callback != null)
        {
            callback();
        }
    }
}
