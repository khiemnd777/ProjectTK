using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FireSlash : Skill
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

        var timeMoveTo = deltaWaitingTime / 4f;
        var timeBack = deltaWaitingTime - timeMoveTo;
        var direction = character.isEnemy ? -1 : 1;
        var ownFieldPosition = ownField.spawner.transform.position;
        var opponentFieldPosition = opponentField.spawner.transform.position - (direction * new Vector3(2f,0,0));

        StartCoroutine(MoveToTarget(ownFieldPosition, opponentFieldPosition, timeMoveTo));
        yield return new WaitForSeconds(timeMoveTo);
        
        StartCoroutine(MoveToTarget(opponentFieldPosition, ownFieldPosition, timeBack));
        yield return new WaitForSeconds(timeBack);

        opponentImage.color = Color.white;
        ownImage.color = Color.white;
        
        opponentImage = null;
        ownImage = null;
        positions = null;
        opponentFieldSlots = null;
        opponentFieldSlot = null;
    }

    IEnumerator MoveToTarget(Vector3 start, Vector3 end, float runningTime)
    {
        var percent = 0f;
        while (percent <= 1f)
        {
            percent += Time.deltaTime / runningTime;
            character.model.transform.position = Mathfx.Sinerp(start, end, percent);
            yield return null;
        }
    }
}