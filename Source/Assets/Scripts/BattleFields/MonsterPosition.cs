using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterPosition : MonoBehaviour
{
    BaseCharacter _monster;

    public BaseCharacter monster
    {
        get
        {
            return _monster;
        }
    }

    public void AddMonster(BaseCharacter monster)
    {
        _monster = monster;
        var monsterTransform = monster.transform;
        monsterTransform.SetParent(transform);
        monsterTransform.localPosition = Vector3.zero;
        monsterTransform.localScale = Vector3.one * .375f;
        monsterTransform.gameObject.SetActive(true);
    }
}