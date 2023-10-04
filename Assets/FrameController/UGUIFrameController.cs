using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

public class UGUIFrameController : MonoBehaviour
{
    [SerializeField]
    Image top, bottom, left, right;
    [SerializeField]
    KeyCode topKey, bottomKey, leftKey, rightKey, toggleKey;

    [SerializeField]
    KeyCode expandKey, shrinkKey;

    [SerializeField]
    int width, height;
    public enum ControlMode
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [SerializeField]
    ControlMode controlMode;
    private bool isVisible;

    public bool IsVisible
    {
        get {
            return isVisible;
        }
        private set {
            top.enabled = value;
            bottom.enabled = value;
            right.enabled = value;
            left.enabled = value;
            this.isVisible = value;
        }
    }
    [SerializeField]
    KeyCode saveKey;

    [SerializeField]
    float controlStep;

    [SerializeField]
    string fileName;

    private void Awake()
    {
        Load();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(topKey))
        {
            this.controlMode = ControlMode.Top;
        }
        else if (Input.GetKeyDown(bottomKey))
        {
            this.controlMode = ControlMode.Bottom;
        }
        else if (Input.GetKeyDown(leftKey))
        {
            this.controlMode = ControlMode.Left;
        }
        else if (Input.GetKeyDown(rightKey))
        {
            this.controlMode = ControlMode.Right;
        }
        else if (Input.GetKeyDown(saveKey))
        {
            Save();
        }
        else if (Input.GetKeyDown(toggleKey))
        {
            Toggle();
        }


        if (Input.GetKey(expandKey))
        {
            ControlSize(controlStep);
        }
        else if (Input.GetKey(shrinkKey))
        {
            ControlSize(-controlStep);
        }
    }

    void ControlSize(float step)
    {
        switch (controlMode)
        {
            case ControlMode.Top:
                {
                    var size = top.rectTransform.sizeDelta;
                    size.y += step * Time.deltaTime;
                    size.y = Mathf.Clamp(size.y, 0f, height / 2f);
                    top.rectTransform.sizeDelta = size;
                }
                break;

            case ControlMode.Bottom:
                {
                    var size = bottom.rectTransform.sizeDelta;
                    size.y += step * Time.deltaTime;
                    size.y = Mathf.Clamp(size.y, 0f, height / 2f);
                    bottom.rectTransform.sizeDelta = size;
                }
                break;

            case ControlMode.Left:
                {
                    var size = left.rectTransform.sizeDelta;
                    size.x += step * Time.deltaTime;
                    size.x = Mathf.Clamp(size.x, 0f, width / 2f);
                    left.rectTransform.sizeDelta = size;
                }
                break;

            case ControlMode.Right:
                {
                    var size = right.rectTransform.sizeDelta;
                    size.x += step * Time.deltaTime;
                    size.x = Mathf.Clamp(size.x, 0f, width / 2f);
                    right.rectTransform.sizeDelta = size;
                }
                break;
        }
    }

    void Load()
    {
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            var settings = JsonConvert.DeserializeObject<FrameSettings>(json);
            top.rectTransform.sizeDelta = new Vector2(settings.TopSettings.sizeInfo.x, settings.TopSettings.sizeInfo.y);
            bottom.rectTransform.sizeDelta = new Vector2(settings.BottomSettings.sizeInfo.x, settings.BottomSettings.sizeInfo.y);
            left.rectTransform.sizeDelta = new Vector2(settings.LeftSettings.sizeInfo.x, settings.LeftSettings.sizeInfo.y);
            right.rectTransform.sizeDelta = new Vector2(settings.RightSettings.sizeInfo.x, settings.RightSettings.sizeInfo.y);
            this.IsVisible = settings.IsVisible;
        }
        else
        {
            top.rectTransform.sizeDelta = new Vector2(width,0f);
            bottom.rectTransform.sizeDelta = new Vector2(width, 0f);
            left.rectTransform.sizeDelta = new Vector2(0f, height);
            right.rectTransform.sizeDelta = new Vector2(0f, height);
            this.IsVisible = false;
        }
    }

    void Toggle()
    {
        IsVisible = !IsVisible;
    }



    void Save()
    {
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
        var settings = new FrameSettings();
        settings.IsVisible = this.IsVisible;
        settings.TopSettings = new RectSettings(top.rectTransform);
        settings.BottomSettings = new RectSettings(bottom.rectTransform);
        settings.LeftSettings = new RectSettings(left.rectTransform);
        settings.RightSettings = new RectSettings(right.rectTransform);
        var json = JsonConvert.SerializeObject(settings);
        File.WriteAllText(path, json);
    }

    public class FrameSettings
    {
        public bool IsVisible;
        public RectSettings TopSettings;
        public RectSettings BottomSettings;
        public RectSettings LeftSettings;
        public RectSettings RightSettings;
    }

    public class RectSettings
    {
        public TypeUtil.Json.Vec2 sizeInfo;

        public RectSettings()
        {

        }

        public RectSettings(RectTransform rectTransform)
        {
            this.sizeInfo = new TypeUtil.Json.Vec2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        }
    }
}
