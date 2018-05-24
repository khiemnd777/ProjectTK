using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BaseSkill : MonoBehaviour
{
    [Header("Modifiers")]
    public float damageModifier;
    public float armorModifier;
    [Header("Animation Clips")]
    public AnimationClip[] animationClips;

    // Main function
    public virtual void Execute(Animator animator, GeneratedBaseCharacter baseCharacter)
    {
        StartCoroutine(PlayingAnimationClips(animator, baseCharacter));
    }

    public virtual void ActivateEffect(string effectName, GeneratedBaseCharacter baseCharacter, BaseCharacter target = null)
    {
        var effect = GetEffect(effectName, baseCharacter);
        if(effect == null || effect is Object && effect.Equals(null))
            return;
        if(target != null){
            effect.transform.SetParent(null);
            effect.transform.position = target.hitPoint.position;
        }
        var animatorEffect = effect.GetComponent<Animator>();
        if(animatorEffect == null || animatorEffect is Object && animatorEffect.Equals(null)){
            Destroy(effect.gameObject);    
            return;
        }
        var fxStateInfo = animatorEffect.GetCurrentAnimatorStateInfo(0);
        if(fxStateInfo.Equals(null)){
            Destroy(effect.gameObject);    
            return;
        }
        Destroy(effect.gameObject, fxStateInfo.length);
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

    IEnumerator PlayingAnimationClips(Animator animator, GeneratedBaseCharacter baseCharacter)
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