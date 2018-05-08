using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRunner : MonoBehaviour
{
    #region Unity fields
    public Transform avatar;
    [Range(0f, 1f)]
    public float deltaSpeed = .5f;
    [Range(1f, 2f)]
    public float deltaScale = 1.25f;
    [Space]
    [System.Obsolete]
    public Character character;
    [System.NonSerialized]
    public BaseCharacter baseCharacter;
    #endregion

    #region Public non-serialized fields
    public bool isTurn
    {
        get; private set;
    }
    public bool isOnActionRoad
    {
        get; private set;
    }
    [System.NonSerialized]
    public Transform reachedRoad;
    [System.NonSerialized]
    public Transform affectiveRoad;
    [System.NonSerialized]
    public Transform actionRoad;
    #endregion

    #region Events
    // invoked if the runner has reached
    public delegate void OnRunnerReached(CharacterRunner runner);
    public OnRunnerReached onRunnerReachedCallback;
    #endregion

    #region Private fields
    MarathonRunner marathonRunner;
    RectTransform runnerRectTranform;
    RectTransform rectTransform;
    RectTransform reachedRoadRect;
    RectTransform affectiveRoadRect;
    RectTransform actionRoadRect;

    bool isStopped = true;
    bool isRunningOnActionRoad = false;
    float runOnReachedRoadPercent;

    #endregion

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        marathonRunner = GetComponentInParent<MarathonRunner>();
        reachedRoadRect = reachedRoad.GetComponent<RectTransform>();
        actionRoadRect = actionRoad.GetComponent<RectTransform>();
        affectiveRoadRect = affectiveRoad.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (marathonRunner.isStopped)
            return;
        RunOnReachedRoad();
    }

    public void ResetRunningPosition()
    {
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void StopRunning()
    {
        isStopped = true;
    }

    public void StartRunning()
    {
        isStopped = false;
    }

    void RunOnReachedRoad()
    {
        if (IsCharacterDied())
            return;

        // run if non-stop
        if (isStopped)
            return;
        if(isOnActionRoad)
            return;
        // check character is in turn. True if it reachs
        // isTurn = false;
        transform.localScale = Vector3.one;
        // Move position towards the end of road
        var journeyLength = reachedRoadRect.GetWidth();
        var affectiveLength = affectiveRoadRect.GetWidth();
        var targetAnchoredPosition = new Vector2(journeyLength, 0f);
        var stats = baseCharacter.stats;
        runOnReachedRoadPercent += (stats.speed.GetValue() / 30f) * Time.deltaTime;
        // moving the character runner on road
        rectTransform.anchoredPosition = Vector2.Lerp(Vector2.zero, targetAnchoredPosition, runOnReachedRoadPercent);
        // if position is through affective road
        if (rectTransform.anchoredPosition.x >= journeyLength - affectiveLength)
        {
            // Debug.Log("The character was infected the positive/negative effects");
        }
        if (runOnReachedRoadPercent >= 1f)
        {
            runOnReachedRoadPercent = 0f;
            // isTurn = true;
            transform.localScale = Vector3.one * deltaScale;
            if (onRunnerReachedCallback != null)
            {
                onRunnerReachedCallback.Invoke(this);
            }
        }
    }

    // Todo: remove
    [System.Obsolete]
    public void RunOnActionRoad(float executedAbilityTime)
    {
        if (IsCharacterDied())
            return;
        isRunningOnActionRoad = true;
        StartCoroutine(RunningOnActionRoad(executedAbilityTime));
    }

    // Todo: remove
    [System.Obsolete]
    IEnumerator RunningOnActionRoad(float executedAbilityTime)
    {
        var reachedRoadLength = reachedRoadRect.GetWidth();
        var journeyLength = reachedRoadLength + actionRoadRect.GetWidth();
        var startPosition = new Vector2(reachedRoadLength, 0f);
        var endPosition = new Vector2(journeyLength, 0f);
        Debug.Log(baseCharacter.characterName + " executes skill in " + executedAbilityTime + "s");
        var percent = 0f;
        while (!IsCharacterDied() && percent <= 1f)
        {
            percent += Time.deltaTime / executedAbilityTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }
        isRunningOnActionRoad = false;
        StopCoroutine("RunningOnActionRoad");
    }

    public IEnumerator RunForAction()
    {
        isOnActionRoad = true;
        isTurn = false;
        var reachedRoadLength = reachedRoadRect.GetWidth();
        var journeyLength = reachedRoadLength + actionRoadRect.GetWidth();
        var startPosition = new Vector2(reachedRoadLength, 0f);
        var endPosition = new Vector2(journeyLength, 0f);
        var actionInfo = baseCharacter.DoAction();
        var runningTime = actionInfo.time; // It must be got by skill but 1 second is default
        Debug.Log(baseCharacter.characterName + " executes skill in " + runningTime + "s");
        var percent = 0f;
        while (!IsCharacterDied() && percent <= 1f)
        {
            percent += Time.deltaTime / runningTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }
        isOnActionRoad = false;
        StopCoroutine("RunForAction");
    }

    bool IsCharacterDied()
    {
        if (gameObject.activeSelf && baseCharacter.isDeath)
        {
            gameObject.SetActive(false);
            ResetRunningPosition();
            return true;
        }
        gameObject.SetActive(true);
        return false;
    }
}