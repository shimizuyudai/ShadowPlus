using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransitionController : MonoBehaviour {
    [SerializeField]
    float attackDuration;
    [SerializeField]
    float sustainDuration;
    [SerializeField]
    float releaseDuration;
    [SerializeField]
    Transform curtainTransform;

    [SerializeField]
    Sequencer sequencer;
    [SerializeField]
    CurtainController curtainController;

    TransitionSetting transitionSetting;

    [SerializeField]
    TransitionSetting[] transitionSettings;
    [SerializeField]
    RearImageController rearImageController;

    public delegate void OnTransition();
    public event OnTransition OnStartTransitionEvent;
    public event OnTransition OnCompleteTransitionEvent;

    [SerializeField]
    [Range(0, 1)]
    float saturation, brightness;
    // Use this for initialization
    void Start () {
        transitionSetting = transitionSettings[0];

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetStyle(int transitionIndex, Color transitionColor, Color shadowColor)
    {
        if(transitionIndex >= 0 && transitionIndex < transitionSettings.Length)
        {
            var transitionTypeIndex = (int)Mathf.Floor(UnityEngine.Random.Range(0,transitionSettings.Length));
            this.transitionSetting = this.transitionSettings[transitionTypeIndex];
        }
        curtainController.Color = transitionColor;
        var hue = UnityEngine.Random.Range(0f,1f);
        rearImageController.ShadowColor = Color.HSVToRGB(hue, saturation, brightness);
    }

    public void Play(Action onComplete = null, Action onHide = null)
    {
        curtainTransform.localPosition = transitionSetting.startPosition;

        Action<float> onUpdateRelease = (value) =>
        {
            curtainTransform.localPosition = Vector3.Lerp(transitionSetting.destinationPosition, transitionSetting.endPosition, transitionSetting.outCurve.Evaluate(value));
        };

        Action onCompleteRelease = () =>
        {
            curtainTransform.localPosition = transitionSetting.endPosition;
            if (onComplete != null)
            {
                onComplete();
            }
            if (OnCompleteTransitionEvent != null)
            {
                OnCompleteTransitionEvent();
            }
        };

        Action onCompleteSustain = () =>
        {
            curtainTransform.localPosition = transitionSetting.destinationPosition;
            sequencer.StartTimeline(releaseDuration,onCompleteRelease,onUpdateRelease);
        };

        Action<float> onUpdateAttack = (value) =>
        {
            curtainTransform.localPosition = Vector3.Lerp(transitionSetting.startPosition,transitionSetting.destinationPosition,transitionSetting.inCurve.Evaluate(value));
        };

        Action onCompleteAttack = () =>
        {
            if (onHide != null)
            {
                onHide();
            }
            sequencer.StartTimeline(sustainDuration,onCompleteSustain);
        };
        sequencer.StartTimeline(attackDuration, onCompleteAttack, onUpdateAttack);
        if (OnStartTransitionEvent != null)
        {
            OnStartTransitionEvent();
        }
    }


    [Serializable]
    struct TransitionSetting
    {
        public Vector3 startPosition;
        public Vector3 destinationPosition;
        public Vector3 endPosition;
        public AnimationCurve inCurve;
        public AnimationCurve outCurve;
    }

}
