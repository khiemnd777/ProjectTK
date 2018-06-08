using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BaseSkill : MonoBehaviour
{
    [Header("Modifiers")]
    public float damageModifier;
    public float armorModifier;
    [Space]
    public Transform hitEffectPrefab;
    public string effectName;
    public AnimationClip[] animationClips;

    // Main function
    public virtual void Execute(Animator animator, GeneratedBaseCharacter baseCharacter, BaseCharacter target = null)
    {
        StartCoroutine(PlayingAnimationClips(animator, baseCharacter, target));
    }

    public virtual Transform ActivateEffect(string effectName, GeneratedBaseCharacter baseCharacter, BaseCharacter target = null)
    {
        var effect = GetEffect(effectName, baseCharacter);
        if(effect == null || effect is Object && effect.Equals(null))
            return null;
        var animatorEffect = effect.GetComponent<Animator>();
        if(animatorEffect == null || animatorEffect is Object && animatorEffect.Equals(null)){
            Destroy(effect.gameObject);    
            return effect;
        }
        var fxStateInfo = animatorEffect.GetCurrentAnimatorStateInfo(0);
        if(fxStateInfo.Equals(null)){
            Destroy(effect.gameObject);    
            return effect;
        }
        StartCoroutine(GenerateHitPointEvent(animatorEffect, baseCharacter, target));
        Destroy(effect.gameObject, fxStateInfo.length);
        return effect;
    }

    public virtual Transform GetEffect(string effectName, GeneratedBaseCharacter baseCharacter)
    {
        var effectContainer = baseCharacter.transform.Find("Effects");
        if(effectContainer == null || effectContainer is Object && effectContainer.Equals(null))
            return null;
        var effectPref = effectContainer.Find(effectName);
        if(effectPref == null || effectPref is Object && effectPref.Equals(null))
            return null;
        var effect = Instantiate<Transform>(effectPref, effectPref.transform.position, Quaternion.identity, baseCharacter.transform);
        effect.gameObject.SetActive(true);
        return effect;
    }

    IEnumerator PlayingAnimationClips(Animator animator, GeneratedBaseCharacter baseCharacter, BaseCharacter target = null)
    {
        for (var i = 0; i < animationClips.Length; i++)
        {
            var animClip = animationClips[i];
            // Debug.Log(animClip.name);
            var frameRate = animClip.frameRate;
            var startTime = 0f;
            var endTime = animClip.length;
            var length = animClip.length;
            var animLayerIndex = animator.GetLayerIndex(baseCharacter.baseJob.label.ToString());
            animator.Play(animClip.name, animLayerIndex, startTime / endTime);
            yield return new WaitForSeconds(length);
            animator.Play(baseCharacter.idlingAnimation.name, animLayerIndex, startTime / endTime);
        }
    }

    IEnumerator GenerateHitPointEvent(Animator effectAnim, GeneratedBaseCharacter baseCharacter, BaseCharacter target)
    {
        if(effectAnim == null || effectAnim is Object && effectAnim.Equals(null))
            yield break;
        var animClipInfo = effectAnim.GetCurrentAnimatorClipInfo(0)[0];
        var animEvents = animClipInfo.clip.events;
        foreach(var animEvent in animEvents)
        {
            var time = animEvent.time;
            CreateHitEffect(baseCharacter, target);
            yield return new WaitForSeconds(time);
            StartCoroutine(ActiveHurtAnimation(target));
        }
    }

    public IEnumerator ActiveHurtAnimation(BaseCharacter target)
    {
        var animLayerIndex = target.animator.GetLayerIndex(target.baseJob.label.ToString());
        target.animator.Play(target.hurtingAnimation.name, animLayerIndex);
        yield return new WaitForSeconds(target.hurtingAnimation.length);
        target.animator.Play(target.idlingAnimation.name, animLayerIndex);
    }

    void CreateHitEffect(GeneratedBaseCharacter baseCharacter, BaseCharacter target)
    {
        if(target == null || target is Object && target.Equals(null))
            return;
        var hitFx = Instantiate<Transform>(hitEffectPrefab, target.hitPoint.transform.position, Quaternion.identity, baseCharacter.transform);
        hitFx.gameObject.SetActive(true);
        hitFx.transform.position =  target.hitPoint.position;
        var hitFxAnim = hitFx.GetComponent<Animator>();
        var length = hitFxAnim.GetCurrentAnimatorStateInfo(0).length;
        Destroy(hitFxAnim.gameObject, length);
    }

    public virtual float GetLength()
    {
        var totalLength = animationClips.Any() ? animationClips.Sum(x => x.length) : 0f;
        return totalLength;
    }

    public virtual BaseCharacter[] DetermineOpponents()
    {
        // Get default opponent in list
        return BattleFieldManager.instance
            .monsterPositions
            .monsterPositions
            .Select(x => x.monster)
            .ToArray();
    }

    // public virtual void TakeDamage(BaseCharacter[] baseCharacters)
    // {
    //     var stats = baseCharacter.stats;
    //     stats.damage.AddModifier(damageModifier);
    //     StartCoroutine(TakingDamage(stats.damage.GetValue(), baseCharacters));
    //     stats.damage.RemoveModifier(damageModifier);
    // }

    // public virtual IEnumerator TakingDamage(float damage, BaseCharacter[] opponents)
    // {
    //     foreach (var opponent in opponents)
    //     {
    //         var stats = opponent.stats;
    //         stats.TakeDamage(damage);
    //         StartCoroutine(TakingDamageAnimation(opponent));
    //         stats = null;
    //         yield return null;
    //     }
    // }

    // IEnumerator TakingDamageAnimation(BaseCharacter opponent)
    // {
    //     if (!opponent.hurtingAnimation.IsNull())
    //     {
    //         var animClip = opponent.hurtingAnimation;
    //         var animator = opponent.animator;
    //         animator.Play(animClip.name, 0);
    //         yield return new WaitForSeconds(animClip.length);
    //     }
    //     if (!opponent.idlingAnimation.IsNull())
    //     {
    //         var animClip = opponent.idlingAnimation;
    //         var animator = opponent.animator;
    //         animator.Play(animClip.name, 0);
    //     }
    // }
}