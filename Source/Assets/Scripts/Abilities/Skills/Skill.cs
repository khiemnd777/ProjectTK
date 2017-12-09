using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : Ability
{
    public Sprite icon;
    public Color markColor;
    public Color selectColor;
    
    public override IEnumerator Use(AbilityUsingParams args) 
    {
        yield return base.Use(args);
    }
}