using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFieldManager : MonoBehaviour
{
    #region Singleton

    static BattleFieldManager manager;
    public static BattleFieldManager instance
    {
        get
        {
            manager = FindObjectOfType<BattleFieldManager>();
            if (manager == null)
            {
                Debug.LogError("There needs to be one active BattleFieldManager script on a GameObject in your scene.");
            }
            else
            {
                // If it exists then init
            }
            return manager;
        }
    }

    #endregion

    public int numOfFields = 9;

    [System.Obsolete]
    public Character[] characters;
    [System.NonSerialized]
    public BaseCharacter[] baseCharacters;

    [Space]
    public Transform playerField;
    public Transform opponentField;
    [Space]
    public Transform playerField3D;
    public Transform opponentField3D;
    [Space]
    public MarathonRunner marathonRunner;
    public HeroPositions heroPositions;
    public MonsterPositions monsterPositions;

    [System.NonSerialized]
    public CharacterField[] playerFields;
    [System.NonSerialized]
    public CharacterField[] opponentFields;

    [System.NonSerialized]
    public FieldSlot[] playerFieldSlots;
    [System.NonSerialized]
    public FieldSlot[] opponentFieldSlots;

    Queue<Character> queueHandledAbilities = new Queue<Character>();

    void Start()
    {
        Init();
        // CreateNewBattle();
        // StartCoroutine(DequeueHandledAbilities());
    }

    void Update()
    {
        Run();

        if(Input.GetButtonDown("Arrange Squad")){
            playerField.gameObject.SetActive(!playerField.gameObject.activeSelf);
            opponentField.gameObject.SetActive(!opponentField.gameObject.activeSelf);
        }
    }

    void Run()
    {
        if (marathonRunner.isStopped)
            return;
        // if any characters reached turn
        // they will be enqueued to handled ability list
        // EnqueueHandledAbilityListWhenCharacterTurned();
    }

    // Todo: remove
    void EnqueueHandledAbilityListWhenCharacterTurned(){
        // find any character when turned
        var singleCharacterInTurn = GetCharacterInTurn();
        if (singleCharacterInTurn.IsNull())
            return;
        Debug.Log(singleCharacterInTurn.name + " has been turned!");
        marathonRunner.StopSingleRunner(singleCharacterInTurn);
        queueHandledAbilities.Enqueue(singleCharacterInTurn);
        singleCharacterInTurn = null;
    }

    // Todo: remove
    Character GetCharacterInTurn(){
        
        var character = characters.FirstOrDefault(x => x.isTurn);
        if(character.IsNull())
            return null;
        character.isTurn = false;
        return character;
    }

    // Todo: remove
    IEnumerator DequeueHandledAbilities(){
        while(!gameObject.IsNull()){
            if(queueHandledAbilities.Count == 0){
                yield return null;
                continue;
            }
            var character = queueHandledAbilities.Dequeue();
            if(!character.isDeath)
                yield return StartCoroutine(character.HandleAbilities(marathonRunner));
        }
    }

    public void CreateNewBattle()
    {
        // Add skill for player's characters (It's a bit hijack)
        foreach (var character in characters)
        {
            // character.Setup();
            foreach (var skill in character.skills)
            {
                if (skill.IsNull())
                    continue;
                var learnedSkill = character.LearnSkill(skill);
                character.AddAbility(learnedSkill);
            }
            if (!character.isEnemy)
            {
                marathonRunner.AddToCharacterArea(character);
            }
            else
            {
                marathonRunner.AddToEnemyArea(character);
            }
        }

        playerFieldSlots = playerField.GetComponentsInChildren<FieldSlot>();
        opponentFieldSlots = opponentField.GetComponentsInChildren<FieldSlot>();

        playerFields = playerField3D.GetComponentsInChildren<CharacterField>();
        opponentFields = opponentField3D.GetComponentsInChildren<CharacterField>();

        // Add character into field slots
        for (var i = 0; i < characters.Length; i++)
        {
            AddCharacterToField(characters[i]);
        }
    }

    void Init()
    {
        // Add hero list
        var characterList = CharacterList.instance;
        if(!characterList.squadCharacters.Any())
            return;
        foreach(var character in characterList.squadCharacters)
        {
            marathonRunner.AddToRunner(character);
            heroPositions.AddToPosition(character);
            // Active the animation layer by hero's job.
            // This will be removed out because of temporary.
            AnimatorUtility.ActiveLayer(character.animator, character.baseJob.label.ToString());
        }
        // Add monster list
        var monsterList = MonsterList.instance;
        if(!monsterList.monsters.Any())
            return;
        foreach(var monster in monsterList.monsters)
        {
            monsterPositions.AddToPosition(monster);
            AnimatorUtility.ActiveLayer(monster.animator, monster.baseJob.label.ToString());
        }
    }

    public void ArrageSquad(){
        foreach(var field in playerFields){
            field.ClearSlot();
        }
        foreach(var field in opponentFields){
            field.ClearSlot();
        }
        foreach(var character in characters){
            var characterFields = character.isEnemy ? opponentFields : playerFields;
            characterFields[character.slot].AddField(character);
            characterFields = null;
        }
    }

    void AddCharacterToField(Character character)
    {
        var fieldSlots = character.isEnemy ? opponentFieldSlots : playerFieldSlots;
        var characterFields = character.isEnemy ? opponentFields : playerFields;
        for (var i = 0; i < fieldSlots.Length; i++)
        {
            var fieldSlot = fieldSlots[i];
            var characterField = characterFields[i];
            if (character.slot == i)
            {
                if (!fieldSlot.CanAdd())
                {
                    ++character.slot;
                    if(character.slot == fieldSlots.Length)
                        character.slot = 0;
                    fieldSlot = null;
                    characterField = null;
                    AddCharacterToField(character);
                    break;
                }
                fieldSlot.AddField(character);
                characterField.AddField(character);

                fieldSlot = null;
                characterField = null;

                break;
            }
            fieldSlot = null;
        }
        fieldSlots = null;
    }
}