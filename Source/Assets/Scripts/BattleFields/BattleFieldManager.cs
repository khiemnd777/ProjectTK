﻿using System.Linq;
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

    public Character[] characters;
    [Space]
    public Transform playerField;
    public Transform opponentField;
    [Space]
    public Transform playerField3D;
    public Transform opponentField3D;
    [Space]
    public MarathonRunner marathonRunner;

    [System.NonSerialized]
    public CharacterField[] playerFields;
    [System.NonSerialized]
    public CharacterField[] opponentFields;

    [System.NonSerialized]
    public FieldSlot[] playerFieldSlots;
    [System.NonSerialized]
    public FieldSlot[] opponentFieldSlots;

    void Start()
    {
        CreateNewBattle();
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
        // if character is in a turn
        if (marathonRunner.isStopped)
            return;
        var singleCharacterInTurn = characters.FirstOrDefault(x => x.isTurn);
        if (singleCharacterInTurn != null)
        {
            Debug.Log(singleCharacterInTurn.name + " has been turned!");
            marathonRunner.StopRunner();
            singleCharacterInTurn.HandleAbilities();
        }
    }

    public void CreateNewBattle()
    {
        // Add skill for player's characters (It's a hijack)
        foreach (var character in characters)
        {
            character.ClearAllLearnedSkills();
            character.ClearAllTactics();
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