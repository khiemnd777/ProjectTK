using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ReynerDefaultSkill : Skill
{
    public override void Setup()
    {
        var animManager = GetComponent<AnimationManager>();
        if(!animManager.IsNull()){
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

        // var timeMoveTo = executedTime / 2f;
        // var timePrepareIdleToMove = .02f;
        // var timeTotalMoving = timeMoveTo - timePrepareIdleToMove;
        // var timeSlash = .25f;
        // var timeSlashDelay = .025f;
        // var timeMoving = timeTotalMoving - timeSlash - timeSlashDelay;
        // var timeBack = executedTime - timeMoveTo;
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
}