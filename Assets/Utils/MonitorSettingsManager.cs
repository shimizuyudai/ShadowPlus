using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class MonitorSettingsManager : MonoBehaviour {
    [SerializeField]
    AlertManager alertManager;
    MonitorSettings settings;
    [SerializeField]
    string fileName;

    private void Awake()
    {
        var path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (File.Exists(path))
        {

            var json = File.ReadAllText(path);
            settings = JsonConvert.DeserializeObject<MonitorSettings>(json);
            if (settings.SelectMonitor < 0 || settings.SelectMonitor >= Display.displays.Length)
            {
                alertManager.DisplayAlert("選択されたモニター番号が正しくありません。", "OK", () =>
                {
                    alertManager.Hide();
                });
                return;
            }

            PlayerPrefs.SetInt("UnitySelectMonitor", settings.SelectMonitor);
            var display = Display.displays[settings.SelectMonitor];
            int w = display.systemWidth;
            int h = display.systemHeight;
            Screen.SetResolution(w, h, Screen.fullScreen);
        }
        else
        {
            alertManager.DisplayAlert("設定ファイルがありません。", "OK", () =>
            {
                alertManager.Hide();
            });
            return;
        }

        
        //var msg = "w : " + w.ToString() + " h :" + h.ToString();
        //msg += "\n" + "設定されました。";
        //msg += "\n" + "選択されたモニター : " + settings.SelectMonitor.ToString();
        //alertManager.DisplayAlert(msg, "OK", () =>
        //{
        //    alertManager.Hide();
        //});
    }
}
