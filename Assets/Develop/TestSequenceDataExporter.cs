using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;

public class TestSequenceDataExporter : MonoBehaviour
{
    [SerializeField]
    TestSequenceData[] sequenceArray;
    [SerializeField]
    string fileName;
    [SerializeField]
    bool isExport;

    private void Awake()
    {
        if (isExport)
        {
            var json = JsonConvert.SerializeObject(sequenceArray);
            var path = Path.Combine(Application.streamingAssetsPath, fileName);
            File.WriteAllText(path, json);
        }
    }
}
