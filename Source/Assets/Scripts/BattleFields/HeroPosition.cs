using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroPosition : MonoBehaviour
{
    BaseCharacter _hero;

    public BaseCharacter hero
    {
        get
        {
            return _hero;
        }
    }

    public void AddHero(BaseCharacter hero)
    {
        _hero = hero;
        var heroTransform = hero.transform;
        heroTransform.SetParent(transform);
        heroTransform.localPosition = Vector3.zero;
        heroTransform.localScale = new Vector3(-.375f, .375f, .375f);
        heroTransform.gameObject.SetActive(true);
    }
}