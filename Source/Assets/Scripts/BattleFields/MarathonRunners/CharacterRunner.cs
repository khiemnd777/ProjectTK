using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRunner : MonoBehaviour
{
    #region Unity fields
    public Image icon;
    [Range(0f, 1f)]
    public float deltaSpeed = .5f;
    [Range(1f, 2f)]
    public float deltaScale = 1.25f;
    [Space]
    public Character character;
    public GeneratedBaseCharacter baseCharacter;
    #endregion

    #region Public non-serialized fields
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
        if(marathonRunner.isStopped)
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
        if(IsCharacterDied())
            return;

        // run if non-stop
        if (isStopped)
            return;
        if (isRunningOnActionRoad)
            return;
        // check character is in turn. True if it reachs
        character.isTurn = false;
        transform.localScale = Vector3.one;
        // Move position towards the end of road
        var journeyLength = reachedRoadRect.GetWidth();
        var affectiveLength = affectiveRoadRect.GetWidth();
        var targetAnchoredPosition = new Vector2(journeyLength, 0f);
        var stats = character.GetComponent<CharacterStats>();
        runOnReachedRoadPercent += (stats.dexterity.GetValue() / 60f) * Time.deltaTime;
        // moving the character runner on road
        rectTransform.anchoredPosition = Vector2.Lerp(Vector2.zero, targetAnchoredPosition, runOnReachedRoadPercent);
        // if position is through affective road
        if(rectTransform.anchoredPosition.x >= journeyLength - affectiveLength)
        {
            Debug.Log("Character");
        }
        if (runOnReachedRoadPercent >= 1f)
        {
            runOnReachedRoadPercent = 0f;
            character.isTurn = true;
            transform.localScale = Vector3.one * deltaScale;
            if (onRunnerReachedCallback != null)
            {
                onRunnerReachedCallback.Invoke(this);
            }
        }
    }

    public void RunOnActionRoad(float executedAbilityTime){
        if(IsCharacterDied())
            return;
        isRunningOnActionRoad = true;
        StartCoroutine(RunningOnActionRoad(executedAbilityTime));
    }

    IEnumerator RunningOnActionRoad(float executedAbilityTime){
        var reachedRoadLength = reachedRoadRect.GetWidth();
        var journeyLength = reachedRoadLength + actionRoadRect.GetWidth();
        var startPosition = new Vector2(reachedRoadLength, 0f);
        var endPosition = new Vector2(journeyLength, 0f);
        Debug.Log(character.name + " executes skill in " + executedAbilityTime + "s");
        var percent = 0f;
        while(!IsCharacterDied() && percent <= 1f){
            percent += Time.deltaTime / executedAbilityTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, percent);
            yield return null;
        }
        isRunningOnActionRoad = false;
        StopCoroutine("RunningOnActionRoad");
    }

    bool IsCharacterDied(){
        if(gameObject.activeSelf && character.isDeath){
            gameObject.SetActive(false);
            ResetRunningPosition();
            return true;
        }
        gameObject.SetActive(true);
        return false;
    }
}