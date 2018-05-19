using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPositions : MonoBehaviour
{
	public MonsterPosition monsterPositionPrefab;
    public List<MonsterPosition> monsterPositions
    {
        get
        {
            return _monsterPositions ?? (_monsterPositions = new List<MonsterPosition>());
        }
    }
    List<MonsterPosition> _monsterPositions = new List<MonsterPosition>();

    public void AddToPosition(BaseCharacter baseCharacter)
    {
        var monsterPosition = CreateMonsterPosition(baseCharacter);
        _monsterPositions.Add(monsterPosition);
    }

    MonsterPosition CreateMonsterPosition(BaseCharacter baseCharacter)
    {
        var monsterPosition = Instantiate<MonsterPosition>(monsterPositionPrefab, Vector3.zero, Quaternion.identity, transform);
        var posX = _monsterPositions.Count * -2.5f;
        monsterPosition.transform.localPosition = new Vector3(posX, 0f, 0f);
        monsterPosition.AddMonster(baseCharacter);
        return monsterPosition;
    }
}