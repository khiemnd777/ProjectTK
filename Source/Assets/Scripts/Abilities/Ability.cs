using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    new public string name = "New skill";
    public string description = "New description of ability";
    public bool isDefault;
    public int displayOrder;
    public Tactical defaultTacticPrefab;
    public Character character;
    public bool isUsing;
    public List<Tactical> tactics = new List<Tactical>();

    public virtual IEnumerator Use(Tactical tactic)
    {
        Debug.Log(character.name + " is using " + name + " with tactic " + tactic.description);
        yield return null;
    }

    public void AddTactic(Tactical tactic)
    {
        tactic.ability = this;
        tactic.transform.SetParent(transform);
        tactics.Add(tactic);
    }

    public Character[] GetCharacters()
    {
        var manager = BattleFieldManager.instance;
        var ownFieldSlots = !character.isEnemy
            ? manager.characters.Where(x => !x.isEnemy)
            : manager.characters.Where(x => x.isEnemy);
        ownFieldSlots = ownFieldSlots.OrderBy(x => x.slot);
        manager = null;
        return ownFieldSlots.ToArray();
    }

    public Character[] GetOpponentCharacters()
    {
        var manager = BattleFieldManager.instance;
        var ownFieldSlots = !character.isEnemy
            ? manager.characters.Where(x => x.isEnemy)
            : manager.characters.Where(x => !x.isEnemy);
        ownFieldSlots = ownFieldSlots.OrderBy(x => x.slot);
        manager = null;
        return ownFieldSlots.ToArray();
    }

    public FieldSlot[] GetFieldSlots()
    {
        var manager = BattleFieldManager.instance;
        var ownFieldSlots = !character.isEnemy
            ? manager.playerFieldSlots
            : manager.opponentFieldSlots;
        manager = null;
        return ownFieldSlots;
    }

    public FieldSlot[] GetOpponentFieldSlots()
    {
        var manager = BattleFieldManager.instance;
        var opponentFieldSlots = !character.isEnemy
            ? manager.opponentFieldSlots
            : manager.playerFieldSlots;
        manager = null;
        return opponentFieldSlots;
    }

    public FieldSlot GetOwnFieldSlot()
    {
        var fieldSlots = GetFieldSlots();
        var single = fieldSlots.FirstOrDefault(x => x.character == character);
        fieldSlots = null;
        return single;
    }
}