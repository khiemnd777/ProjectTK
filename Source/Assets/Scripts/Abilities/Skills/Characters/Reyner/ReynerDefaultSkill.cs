using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReynerDefaultSkill : Skill
{
    public override void Setup()
    {
        base.Setup();
        foreach(var x in character.animator.runtimeAnimatorController.animationClips){
            
        }
    }

    public override IEnumerator Use(AbilityUsingParams args)
    {
        yield return base.Use(args);

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

        var timeMoveTo = executedTime / 2f;
        var timePrepareIdleToMove = .02f;
        var timeTotalMoving = timeMoveTo - timePrepareIdleToMove;
        var timeSlash = .25f;
        var timeSlashDelay = .025f;
        var timeMoving = timeTotalMoving - timeSlash - timeSlashDelay;
        var timeBack = executedTime - timeMoveTo;
        var direction = character.isEnemy ? -1 : 1;
        var ownFieldPosition = ownField.spawner.transform.position;
        var opponentFieldPosition = opponentField.spawner.transform.position - (direction * new Vector3(2f, 0, 0));

        // Move to opponent
        character.animator.Play("reyner_dash");
        yield return new WaitForSeconds(timePrepareIdleToMove);
        StartCoroutine(TransformUtility.MoveToTarget(character.model.transform, ownFieldPosition, opponentFieldPosition, timeMoveTo));
        yield return new WaitForSeconds(timeMoving);

        character.animator.Play("reyner_slash");
        yield return new WaitForSeconds(timeSlash);

        // Take damage
        TakeDamage(new[] { opponentField.character });
        yield return new WaitForSeconds(timeSlashDelay);

        // Back own field
        character.animator.Play("reyner_get_back");
        StartCoroutine(TransformUtility.MoveToTarget(character.model.transform, opponentFieldPosition, ownFieldPosition, timeBack));
        yield return new WaitForSeconds(timeBack);

        character.animator.Play("reyner_idle");
        opponentImage.color = Color.white;
        ownImage.color = Color.white;

        opponentImage = null;
        ownImage = null;
        positions = null;
        opponentFieldSlots = null;
        opponentFieldSlot = null;
    }
}