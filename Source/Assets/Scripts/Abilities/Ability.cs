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
    public float deltaWaitingTime = .25f;
    public float executedTime;
    public bool isUsing;
    public List<Tactical> tactics = new List<Tactical>();

    public virtual void Setup(){
        
    }

    public virtual void Exit(){

    }

    public virtual IEnumerator Use(AbilityUsingParams args)
    {
        Debug.Log(character.name + " is using " + name + " with tactic " + args.tactic.description);
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

    public CharacterField[] GetFields()
    {
        var manager = BattleFieldManager.instance;
        var ownFieldSlots = !character.isEnemy
            ? manager.playerFields
            : manager.opponentFields;
        manager = null;
        return ownFieldSlots;
    }

    public CharacterField[] GetOpponentFields()
    {
        var manager = BattleFieldManager.instance;
        var opponentFieldSlots = !character.isEnemy
            ? manager.opponentFields
            : manager.playerFields;
        manager = null;
        return opponentFieldSlots;
    }

    public CharacterField GetOwnField()
    {
        var fields = GetFields();
        var single = fields.FirstOrDefault(x => x.character == character);
        fields = null;
        return single;
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

public struct AbilityUsingParams
{
    public Tactical tactic { get; set; }
    public MarathonRunner marathonRunner { get; set; }
}