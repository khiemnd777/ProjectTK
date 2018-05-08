using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroPositions : MonoBehaviour
{
    public HeroPosition heroPositionPrefab;
    public List<HeroPosition> heroPositions
    {
        get
        {
            return _heroPositions ?? (_heroPositions = new List<HeroPosition>());
        }
    }
    List<HeroPosition> _heroPositions = new List<HeroPosition>();

    public void AddToPosition(BaseCharacter baseCharacter)
    {
        var heroPosition = CreateHeroPosition(baseCharacter);
        _heroPositions.Add(heroPosition);
    }

    HeroPosition CreateHeroPosition(BaseCharacter baseCharacter)
    {
        var heroPosition = Instantiate<HeroPosition>(heroPositionPrefab, Vector3.zero, Quaternion.identity, transform);
        var posX = _heroPositions.Count * -2.5f;
        heroPosition.transform.localPosition = new Vector3(posX, 0f, 0f);
        heroPosition.AddHero(baseCharacter);
        return heroPosition;
    }
}