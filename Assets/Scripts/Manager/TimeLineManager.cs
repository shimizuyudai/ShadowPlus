using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

public class TimeLineManager : MonoBehaviour
{

    [SerializeField]
    string timelineFileName;
    [SerializeField]
    TransitionController transitionController;
    [SerializeField]
    SceneHandler sceneHandler;
    [SerializeField]
    Timer timer;

    List<SequenceData> timeline;
    int index;

    [SerializeField]
    bool isAutoPlay;

    SequenceData sequence;


    // Use this for initialization
    void Start()
    {
        loadFile();
        Play(0);
        if (isAutoPlay)
        {
            autoPlay(0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void loadFile()
    {
        var path = Path.Combine(Application.streamingAssetsPath, timelineFileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            timeline = JsonConvert.DeserializeObject<List<SequenceData>>(json);
            foreach (var sequence in timeline)
            {
                if (sceneHandler.Scenes.Count(e => e.Id == sequence.Id) < 1)
                {
                    sequence.Id = 0;
                }
            }
            print(timeline.Count);
        }
    }

    void nextSequence()
    {
        sequence = timeline[index];
        index++;
        if (index >= timeline.Count)
        {
            index = 0;
        }
    }

    SequenceData getSequence(int index)
    {
        if (index < 0 || index >= timeline.Count)
        {
            index = 0;
        }
        var sequence = timeline[index];
        return sequence;
    }

    public void Play(int index)
    {
        //if (sequence != null)
        //{
        var sequence = getSequence(index);
        print(sequence.Id);
        Action onHide = () =>
        {
            GC.Collect();
            sceneHandler.SetScene(sequence.Id);
        };
        Action onCompleteTransition = () =>
        {
            sceneHandler.Play(sequence.Id);
        };
        transitionController.SetStyle(sequence.TransitionType, sequence.TransitionColor, sequence.TransitionShadowColor);
        transitionController.Play(onCompleteTransition, onHide);
        //print("index : " + index + " sequenceId : " + sequence.Id);
        //}
        //else
        //{
        //    sequence = getSequence(index);
        //    sceneHandler.SetScene(sequence.Id);
        //    sceneHandler.Play(sequence.Id);
        //    print("index : " + index + " sequenceId : " + sequence.Id);
        //}
    }

    void autoPlay(int index)
    {
        var sequence = getSequence(index);
        Action onHide = () =>
        {
            GC.Collect();
            sceneHandler.SetScene(sequence.Id);
        };
        Action onCompleteTransition = () =>
        {
            sceneHandler.Play(sequence.Id);
            Action onCompletePlay = () =>
            {
                index++;
                if (index >= timeline.Count)
                {
                    index = 0;
                }
                autoPlay(index);
            };
            timer.StartCount(sequence.Duration, onCompletePlay);
        };
        transitionController.SetStyle(sequence.TransitionType, sequence.TransitionColor, sequence.TransitionShadowColor);
        transitionController.Play(onCompleteTransition, onHide);
    }

    public void PlayNext()
    {
        //タイマーが時間を測り終えた後の処理
        Action callback = () =>
        {
            Action onHide = () =>
            {
                GC.Collect();
                nextSequence();
                sceneHandler.SetScene(sequence.Id);
            };
            Action onCompleteTransition = () =>
            {
                PlayNext();
            };
            transitionController.SetStyle(sequence.TransitionType, sequence.TransitionColor, sequence.TransitionShadowColor);
            transitionController.Play(onCompleteTransition, onHide);
        };
        sceneHandler.Play(sequence.Id);
        timer.StartCount(sequence.Duration, callback);
    }

}
