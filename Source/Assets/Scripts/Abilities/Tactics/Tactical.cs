using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tactical : MonoBehaviour
{
    public string description = "New description of tactic";
    public int displayOrder;
    public bool isDefault;
    public Ability ability;
    public Tactical owner;
    public List<Tactical> tactics = new List<Tactical>();
    public int[] priorityPositions;

    public virtual bool Define()
    {
        return false;
    }

    public void AddTactic(Tactical tactic){
        tactic.owner = this;
        tactic.ability = ability;
        tactics.Add(tactic);
    }
}