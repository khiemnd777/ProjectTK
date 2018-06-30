using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MarathonRunner : MonoBehaviour
{
    public Transform reachedRoad;
    public Transform affectiveRoad;
    public Transform actionRoad;
    public Transform characterArea;
    public Transform enemyArea;
    public CharacterRunner characterRunnerPrefab;

    Queue<CharacterRunner> characterRunnersInTurn = new Queue<CharacterRunner>();

    List<CharacterRunner> characterRunners = new List<CharacterRunner>();

    public bool isStopped = true;

    // Main function
    IEnumerator DequeueCharacterRunnerInTurnForAction()
    {
        while (gameObject.activeSelf)
        {
            if (characterRunnersInTurn.Count == 0)
            {
                yield return null;
                continue;
            }
            var characterRunner = characterRunnersInTurn.Dequeue();
            var character = characterRunner.baseCharacter;
            if (character.isDeath)
                continue;
            // Run for action
            yield return StartCoroutine(characterRunner.RunForAction());
            // Debug.Log(character.characterName + " is done yet");
            // After successfully running on action, then restarting on reached road.
            StartSingleRunner(characterRunner);
        }
    }

    void Start()
    {
        StartCoroutine(DequeueCharacterRunnerInTurnForAction());
    }

    void Update()
    {
        if (isStopped)
            return;
        OrderCharacterRunners(characterRunners);
    }

    public void AddToRunner(BaseCharacter baseCharacter)
    {
        var runner = CreateCharacterRunner(baseCharacter, characterArea);
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

    public void StartSingleRunner(CharacterRunner runner)
    {
        if (runner.IsNull())
            return;
        runner.StartRunning();
        runner = null;
    }

    public void StopSingleRunner(CharacterRunner runner)
    {
        if (runner.IsNull())
            return;
        runner.StopRunning();
        runner = null;
    }

    public CharacterRunner CreateCharacterRunner(BaseCharacter baseCharacter, Transform runnerArea)
    {
        var runner = Instantiate<CharacterRunner>(characterRunnerPrefab, runnerArea.position, Quaternion.identity, runnerArea);
        // Avatar
        var avatarInfo = baseCharacter.GetAvatarInfo();
        var avatar = avatarInfo.Avatar;
        if (avatar != null && avatar is Object && !avatar.Equals(null))
        {
            var avatarPerformance = runner.avatar.transform.Find("Performance");
            var instantiatedAvatar = Instantiate<Transform>(avatar, Vector3.zero, Quaternion.identity, avatarPerformance);
            instantiatedAvatar.transform.localPosition = Vector3.zero;
            instantiatedAvatar.transform.localScale = Vector3.one * 7.5f;
            // icon's style
            var avatarStyle = avatarInfo.AvatarStyle;
            if (avatarStyle != null && avatarStyle is Object && !avatarStyle.Equals(null))
            {
                var avatarImage = runner.avatar.GetComponent<Image>();
                avatarImage.sprite = avatarStyle;
            }
        }
        runner.baseCharacter = baseCharacter;
        runner.reachedRoad = reachedRoad;
        runner.affectiveRoad = affectiveRoad;
        runner.actionRoad = actionRoad;

        // register event of reaching
        runner.onRunnerReachedCallback += OnSingleRunnerReachedCallback;
        // character.onAbilityHandledCallback += OnAbilityHandledCallback;

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

    public CharacterRunner GetBaseCharacterRunner(BaseCharacter baseCharacter)
    {
        var runner = characterRunners.FirstOrDefault(x => x.baseCharacter == baseCharacter);
        return runner;
    }

    void OnSingleRunnerReachedCallback(CharacterRunner runner)
    {
        // Debug.Log(runner.baseCharacter.characterName + " has been in turn!");
        StopSingleRunner(runner);
        characterRunnersInTurn.Enqueue(runner);
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