using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DefaultTactic : Tactical
{
    public override bool Define()
    {
        base.Define();
        priorityPositions = FindPriorityPositions();
        if (priorityPositions.Length <= 0)
            return false;
        return true;
    }

    int[] FindPriorityPositions()
    {
        var currentOwnRow = 0;
        var col = 3;
        var row = 3;
        var characters = ability.GetCharacters();
        var opponentCharacters = ability.GetOpponentCharacters();

        for (var i = 0; i < characters.Length; i++)
        {
            var character = characters[i];
            if (character == ability.character)
            {
                currentOwnRow = Mathf.FloorToInt(character.slot / row);
                character = null;
                break;
            }
            character = null;
        }

        var priorityIndexes = new List<int>();

        for (var i = 0; i < row; i++)
        {
            var minIndex = row * currentOwnRow;
            var maxIndex = row * currentOwnRow + (col - 1);
            var appropriateIndexes = opponentCharacters.Where(x => !x.isDeath && x.slot >= minIndex && x.slot <= maxIndex)
                .OrderByDescending(x => x.slot)
                .Select(x => x.slot)
                .ToArray();
            if(appropriateIndexes.Length > 0)
                priorityIndexes.AddRange(appropriateIndexes);
            ++currentOwnRow;
            if (currentOwnRow > (row - 1))
                currentOwnRow = 0;
            appropriateIndexes = null;
        }

        characters = null;
        opponentCharacters = null;

        return priorityIndexes.ToArray();
    }
}