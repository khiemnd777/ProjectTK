using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public AnimationPart[] animations;

    bool _isStop;
    Ability _ability;

    void Start()
    {
        _ability = GetComponent<Ability>();
    }

    void Update()
    {

    }

    void Setup()
    {
        if (!animations.Any())
            return;
        foreach (var animPart in animations)
        {
            var animClip = animPart.animation;

            animClip = null;
        }
    }

    public void AddEvent(string eventName, System.Action<float> action)
    {
        foreach (var animPart in animations)
        {
            foreach (var evt in animPart.events)
            {
                if (eventName.Equals(evt.eventName))
                {
                    evt.action = action;
                }
            }
        }
    }

    public void Play()
    {
        _isStop = false;
        Setup();
        if (!animations.Any())
            return;
        StartCoroutine(Playing());
    }

    IEnumerator Playing()
    {
        foreach (var animPart in animations)
        {
            if (_isStop)
            {
                break;
            }
            if (animPart.animation.IsNull())
                continue;
            var animClip = animPart.animation;
            var frameRate = animClip.frameRate;
            var startTime = CalculatorUtility.TimeByFrame(animPart.startFrame, frameRate);
            var endTime = animPart.endFrame <= 0 ? animClip.length : CalculatorUtility.TimeByFrame(animPart.endFrame, frameRate);
            var length = animPart.frameLength <= 0 ? animPart.animation.length : CalculatorUtility.TimeByFrame(animPart.frameLength, frameRate);
            var animator = _ability.character.animator;
            animator.Play(animPart.animation.name, 0, startTime / endTime);
            StartCoroutine(ExecuteEvents(animPart.events, length, frameRate));
            yield return new WaitForSeconds(length);
        }
    }

    IEnumerator ExecuteEvents(AnimationPartEvent[] events, float animlength, float frameRate)
    {
        foreach(var evt in events)
        {
            if(evt.action.IsNull())
                continue;
            var startTime = CalculatorUtility.TimeByFrame(evt.startFrame, frameRate);
            yield return new WaitForSeconds(startTime);
            var length = animlength - startTime;
            evt.action.Invoke(length);
            yield return new WaitForSeconds(length);
        }
    }

    public float GetLength()
    {
        return animations
            .Where(x => !x.animation.IsNull())
            .Sum(x => x.frameLength <= 0 ? x.animation.length : CalculatorUtility.TimeByFrame(x.frameLength, x.animation.frameRate));
    }

    public void Stop()
    {
        _ability.character.animator.Play(animations[animations.Length - 1].animation.name);
        _isStop = true;
    }
}