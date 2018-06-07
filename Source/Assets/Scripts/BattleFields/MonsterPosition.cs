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
        var spawnPoint = monsterTransform.Find("Spawn Point");
        monsterTransform.SetParent(transform);
        monsterTransform.localPosition = new Vector3(0f, -spawnPoint.localPosition.y, 0f);
        // monsterTransform.localPosition = Vector3.zero;
        monsterTransform.localScale = monsterTransform.localScale;
        monsterTransform.gameObject.SetActive(true);
    }
}