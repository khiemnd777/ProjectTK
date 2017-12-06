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

        opponentImage.color = markColor;
        ownImage.color = selectColor;
        
        yield return new WaitForSeconds(.125f);

        opponentImage.color = Color.white;
        ownImage.color = Color.white;
        
        opponentImage = null;
        ownImage = null;
        positions = null;
        opponentFieldSlots = null;
        opponentFieldSlot = null;
    }
}