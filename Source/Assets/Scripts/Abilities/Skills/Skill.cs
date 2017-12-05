using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : Ability
{
    public Sprite icon;
    public Color markColor;
    public Color selectColor;
    
    public override IEnumerator Use(Tactical tactic) 
    {
        yield return base.Use(tactic);
    }
}