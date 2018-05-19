using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseSkill : MonoBehaviour
{
    [Header("Modifier")]
    public float damageModifier;
    public float armorModifier;

    [System.NonSerialized]
    public BaseCharacter baseCharacter;

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    public virtual float GetLength()
    {
        return 0f;
    }

    public virtual void Execute()
    {
        
    }

    public virtual void TakeDamage(BaseCharacter[] baseCharacters){
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

    IEnumerator TakingDamageAnimation(BaseCharacter opponent){
        if(!opponent.hurtingAnimation.IsNull()){
            var animClip = opponent.hurtingAnimation;
            var animator = opponent.animator;
            animator.Play(animClip.name, 0);
            yield return new WaitForSeconds(animClip.length);
        }
        if(!opponent.idlingAnimation.IsNull()){
            var animClip = opponent.idlingAnimation;
            var animator = opponent.animator;
            animator.Play(animClip.name, 0);
        }
    }
}