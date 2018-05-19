using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseSkill : MonoBehaviour
{
    [Header("Modifiers")]
    public float damageModifier;
    public float armorModifier;
    [Header("Animation Clips")]
    public AnimationClip[] animationClips;

    [System.NonSerialized]
    public BaseCharacter baseCharacter;

    // Main function
    public virtual void Execute(Animator animator)
    {
        StartCoroutine(PlayingAnimationClips(animator));
    }

    IEnumerator PlayingAnimationClips(Animator animator)
    {
        for (var i = 0; i < animationClips.Length; i++)
        {
            var animClip = animationClips[i];
            var frameRate = animClip.frameRate;
            var startTime = 0f;
            var endTime = animClip.length;
            var length = animClip.length;
            animator.Play(animClip.name, 0, startTime / endTime);
            yield return new WaitForSeconds(length);
        }
    }

    public virtual float GetLength()
    {
        var totalLength = animationClips.Any() ? animationClips.Sum(x => x.length) : 0f;
        return totalLength;
    }

    public virtual BaseCharacter[] DetermineOpponents()
    {
        return null;
    }

    public virtual void TakeDamage(BaseCharacter[] baseCharacters)
    {
        var stats = baseCharacter.stats;
        stats.damage.AddModifier(damageModifier);
        StartCoroutine(TakingDamage(stats.damage.GetValue(), baseCharacters));
        stats.damage.RemoveModifier(damageModifier);
    }

    public virtual IEnumerator TakingDamage(float damage, BaseCharacter[] opponents)
    {
        foreach (var opponent in opponents)
        {
            var stats = opponent.stats;
            stats.TakeDamage(damage);
            StartCoroutine(TakingDamageAnimation(opponent));
            stats = null;
            yield return null;
        }
    }

    IEnumerator TakingDamageAnimation(BaseCharacter opponent)
    {
        if (!opponent.hurtingAnimation.IsNull())
        {
            var animClip = opponent.hurtingAnimation;
            var animator = opponent.animator;
            animator.Play(animClip.name, 0);
            yield return new WaitForSeconds(animClip.length);
        }
        if (!opponent.idlingAnimation.IsNull())
        {
            var animClip = opponent.idlingAnimation;
            var animator = opponent.animator;
            animator.Play(animClip.name, 0);
        }
    }
}