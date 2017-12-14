using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : Ability
{
    [Header("Modifier")]
    public float damageModifier;
    public float armorModifier;
    [Space]
    public Sprite icon;
    public Color markColor;
    public Color selectColor;
    
    public override IEnumerator Use(AbilityUsingParams args) 
    {
        yield return base.Use(args);
    }

    public virtual IEnumerator TakingDamage(float damage, Character[] opponents)
    {
        foreach (var opponent in opponents)
        {
            var stats = opponent.GetComponent<CharacterStats>();
            stats.TakeDamage(damage);
            stats = null;
            yield return null;
        }
    }

    public virtual void TakeDamage(Character[] characters){
        var stats = character.GetComponent<CharacterStats>();
        stats.damage.AddModifier(damageModifier);
        StartCoroutine(TakingDamage(stats.damage.GetValue(), characters));
        stats.damage.RemoveModifier(damageModifier);
    }
}