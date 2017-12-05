﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarathonRunner : MonoBehaviour
{
    public Transform characterArea;
    public Transform enemyArea;

    public CharacterRunner characterRunnerPrefab;

    List<CharacterRunner> characterRunners = new List<CharacterRunner>();

    public bool isStopped = true;

    void Update()
    {
        if (isStopped)
            return;
        OrderCharacterRunners(characterRunners);
    }

    public void AddToCharacterArea(Character character)
    {
        var runner = CreateRunner(character, characterArea);
        AddCharacterRunner(runner);
        runner = null;
    }

    public void AddToEnemyArea(Character character)
    {
        var runner = CreateRunner(character, enemyArea);
        AddCharacterRunner(runner);
        runner = null;
    }

    public void StartOrStopRunner()
    {
        if (isStopped)
        {
            StartRunner();
        }
        else
        {
            StopRunner();
        }
    }

    public void StartRunner()
    {
        characterRunners.ForEach(x => x.StartRunning());
        isStopped = false;
    }

    public void StopRunner()
    {
        characterRunners.ForEach(x => x.StopRunning());
        isStopped = true;
    }

    public CharacterRunner CreateRunner(Character character, Transform parent)
    {
        var runner = Instantiate<CharacterRunner>(characterRunnerPrefab, parent.position, Quaternion.identity, parent);
        runner.icon.sprite = character.icon;
        runner.character = character;

        // register event of reached
        runner.onRunnerReachedCallback += OnSingleRunnerReachedCallback;
        character.onAbilityHandledCallback += OnAbilityHandledCallback;

        var rt = runner.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0f, 0f);
        rt.anchorMin = new Vector2(0f, .5f);
        rt.anchorMax = new Vector2(0f, .5f);
        rt = null;

        return runner;
    }

    public void AddCharacterRunner(CharacterRunner characterRunner)
    {
        characterRunners.Add(characterRunner);
    }

    void OnSingleRunnerReachedCallback(CharacterRunner runner)
    {
        // Debug.Log(runner.character.name + " has reached!");
    }

    void OnAbilityHandledCallback(Character character)
    {
        // Start runner after tactic process of character is finished
        StartRunner();
    }

    void OrderCharacterRunners(List<CharacterRunner> characterRunners)
    {
        var orderedCharacterRunners = characterRunners.OrderBy(x =>
        {
            return x.GetComponent<RectTransform>().anchoredPosition.x;
        }).ToArray();

        for (var i = 0; i < orderedCharacterRunners.Length; i++)
        {
            orderedCharacterRunners[i].transform.SetSiblingIndex(i);
        }

        orderedCharacterRunners = null;
    }
}