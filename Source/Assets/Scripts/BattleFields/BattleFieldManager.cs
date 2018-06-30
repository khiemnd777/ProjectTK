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
            marathonRunner.AddToRunner(monster);
            monsterPositions.AddToPosition(monster);
            AnimatorUtility.ActiveLayer(monster.animator, monster.baseJob.label.ToString());
        }
    }
}