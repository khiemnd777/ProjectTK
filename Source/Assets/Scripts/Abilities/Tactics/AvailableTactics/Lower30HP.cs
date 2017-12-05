using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Lower30HP : Tactical
{
    public override bool Define()
    {
        base.Define();
        priorityPositions = FindPriorityPositions();
        if(priorityPositions.Length <= 0)
            return false;
        return true;
    }

    int[] FindPriorityPositions()
    {
        var ownFieldSlots = ability.GetFieldSlots();
        var opponentFieldSlots = ability.GetOpponentFieldSlots();

        var priorityIndexes = new List<int>();

        for(var i = 0; i< opponentFieldSlots.Length;i++){
            var opponentCharacter = opponentFieldSlots[i].character;
            if(!opponentCharacter.IsNull() && !opponentCharacter.isDeath){
                if(opponentCharacter.health / opponentCharacter.maxHealth <= .3f){
                    priorityIndexes.Add(i);
                }
            }
        }
        
        return priorityIndexes.ToArray();
    }
}