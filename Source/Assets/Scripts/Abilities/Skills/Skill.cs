using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : Ability
{
    [Header("Modifier")]
    public float damageModifier;
    public float armorModifier;
    [Space]
    public Sprite icon;
    public Color markColor;
    public Color selectColor;

    public override void Setup()
    {
        var animManager = GetComponent<AnimationManager>();
        if (!animManager.IsNull())
        {
            executedTime = animManager.GetLength();
        }
    }
    
    public override IEnumerator Use(AbilityUsingParams args) 
    {
        // yield return base.Use(args);

        var positions = args.tactic.priorityPositions;
        var opponentFieldSlots = GetOpponentFieldSlots();
        var opponentFieldSlot = opponentFieldSlots[positions[0]];
        var opponentImage = opponentFieldSlot.GetComponent<Image>();
        var ownFieldSlot = GetOwnFieldSlot(); 
        var ownImage = ownFieldSlot.GetComponent<Image>();
        var opponentField = GetOpponentFields()[positions[0]];
        var ownField = GetOwnField();

        opponentImage.color = markColor;
        ownImage.color = selectColor;

        var direction = character.isEnemy ? -1 : 1;
        var ownFieldPosition = ownField.spawner.transform.position;
        var opponentFieldPosition = opponentField.spawner.transform.position - (direction * new Vector3(5f, 0, 0));
        var animManager = GetComponent<AnimationManager>();

        if(!animManager.IsNull()){
            animManager.AddEvent("MoveToOpponent", (length) => {
                StartCoroutine(TransformUtility.MoveToTarget(character.model.transform, ownFieldPosition, opponentFieldPosition, length));
            });
            animManager.AddEvent("TakeDamage", (length) => {
                TakeDamage(new[] { opponentField.character });
            });
            animManager.AddEvent("MoveBack", (length) => {
                StartCoroutine(TransformUtility.MoveToTarget(character.model.transform, opponentFieldPosition, ownFieldPosition, length));
            });
            animManager.Play();
            yield return new WaitForSeconds(animManager.GetLength());
            animManager.Stop();
        }
        
        opponentImage.color = Color.white;
        ownImage.color = Color.white;

        opponentImage = null;
        ownImage = null;
        positions = null;
        opponentFieldSlots = null;
        opponentFieldSlot = null;
    }

    public virtual IEnumerator TakingDamage(float damage, Character[] opponents)
    {
        foreach (var opponent in opponents)
        {
            var stats = opponent.GetComponent<CharacterStats>();
            stats.TakeDamage(damage);
            StartCoroutine(TakingDamageAnimation(opponent));
            stats = null;
            yield return null;
        }
    }

    IEnumerator TakingDamageAnimation(Character opponent){
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

    public virtual void TakeDamage(Character[] characters){
        var stats = character.GetComponent<CharacterStats>();
        stats.damage.AddModifier(damageModifier);
        StartCoroutine(TakingDamage(stats.damage.GetValue(), characters));
        stats.damage.RemoveModifier(damageModifier);   
    }
}