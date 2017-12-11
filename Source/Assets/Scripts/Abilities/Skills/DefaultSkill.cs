using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DefaultSkill : Skill
{
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

        StartCoroutine(MoveToTarget(ownField, opponentField, deltaWaitingTime / 2f));
        yield return new WaitForSeconds(deltaWaitingTime / 2f);
        
        StartCoroutine(MoveToTarget(opponentField, ownField, deltaWaitingTime / 2f));
        yield return new WaitForSeconds(deltaWaitingTime / 2f);

        opponentImage.color = Color.white;
        ownImage.color = Color.white;
        
        opponentImage = null;
        ownImage = null;
        positions = null;
        opponentFieldSlots = null;
        opponentFieldSlot = null;
    }

    IEnumerator MoveToTarget(CharacterField start, CharacterField end, float runningTime)
    {
        var percent = 0f;
        var startPosition = start.spawner.transform.position;
        var endPosition = end.spawner.transform.position;
        while (percent <= 1f)
        {
            percent += Time.deltaTime / runningTime;
            character.model.transform.position = Mathfx.Sinerp(startPosition, endPosition, percent);
            yield return null;
        }
    }
}