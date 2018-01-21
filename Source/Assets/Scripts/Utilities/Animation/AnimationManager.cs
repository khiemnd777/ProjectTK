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

    public struct AnimationPlayingParams
    {
        public string name { get; set; }
        public float frameLength { get; set; }
    }

    public static IEnumerator Play(Animator animator, params AnimationPlayingParams[] animationPlayingParams)
    {
        foreach (var p in animationPlayingParams)
        {
            if (!animator.runtimeAnimatorController.animationClips.Any(x => x.name == p.name))
                continue;
            animator.Play(p.name);
            var frameRate = animator.runtimeAnimatorController.animationClips.FirstOrDefault(x => x.name == p.name).frameRate;
            var length = p.frameLength <= 0 ? 0 : CalculatorUtility.TimeByFrame(p.frameLength, frameRate);
            yield return new WaitForSeconds(length);
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
        if (!animations.Any())
            return;
        StartCoroutine(Playing());
    }

    public void Play(string name)
    {
        var animPart = animations.FirstOrDefault(x => name.Equals(x.animation.name));
        if (animPart.animation.IsNull())
            return;
        StartCoroutine(Playing(animPart));
    }

    IEnumerator Playing()
    {
        foreach (var animPart in animations)
        {
            if (_isStop)
                break;
            if (animPart.animation.IsNull())
                continue;

            yield return StartCoroutine(Playing(animPart));
        }
    }

    IEnumerator Playing(AnimationPart animPart)
    {
        var animClip = animPart.animation;
        var frameRate = animClip.frameRate;
        var startTime = CalculatorUtility.TimeByFrame(animPart.startFrame, frameRate);
        var endTime = animPart.endFrame <= 0 ? animClip.length : CalculatorUtility.TimeByFrame(animPart.endFrame, frameRate);
        var length = animPart.frameLength <= 0 ? animPart.animation.length : CalculatorUtility.TimeByFrame(animPart.frameLength, frameRate);
        var animator = _ability.character.animator;
        StartCoroutine(ExecuteEvents(animPart.events, length, frameRate));
        StartCoroutine(ExecuteEffects(animPart.effects, length, frameRate));
        animator.Play(animPart.animation.name, 0, startTime / endTime);
        yield return new WaitForSeconds(length);
    }

    IEnumerator ExecuteEvents(AnimationPartEvent[] events, float animlength, float frameRate)
    {
        foreach (var evt in events)
        {
            Debug.Log("Event name: " + evt.eventName);
            if (evt.action.IsNull())
                continue;
            var startTime = CalculatorUtility.TimeByFrame(evt.startFrame, frameRate);
            yield return new WaitForSeconds(startTime);
            var length = animlength - startTime;
            evt.action.Invoke(length);
            yield return new WaitForSeconds(length);
        }
    }

    IEnumerator ExecuteEffects(AnimationPartEffect[] effects, float animlength, float frameRate)
    {
        foreach (var effect in effects)
        {
            if(effect.effectObject.IsNull())
                continue;
            var startTime = CalculatorUtility.TimeByFrame(effect.startFrame, frameRate);
            yield return new WaitForSeconds(startTime);
            var length = animlength - startTime;
            var effectObj = Instantiate(effect.effectObject, _ability.character.model.transform.position + effect.position, Quaternion.identity, _ability.character.model.transform);
            var isEnemy = _ability.character.isEnemy;
            if(!isEnemy){
                var originScale = effectObj.transform.localScale;
                originScale.x *= -1;
                effectObj.transform.localScale = originScale;
            }
            Destroy(effectObj, length);
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