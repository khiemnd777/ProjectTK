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
    #endregion

    #region Public non-serialized fields
    [System.NonSerialized]
    public Transform reachedRoad;
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
    RectTransform actionRoadRect;

    float startTime;
    bool isStopped = true;
    float percent;
    #endregion

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        reachedRoadRect = reachedRoad.GetComponent<RectTransform>();
        actionRoadRect = actionRoad.GetComponent<RectTransform>();
    }

    void Update()
    {
        Run();
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

    void Run()
    {
        if(character.isDeath){
            gameObject.SetActive(false);
            ResetRunningPosition();
            return;
        }
        gameObject.SetActive(true);
        // run if non-stop
        if (isStopped)
            return;
        // check character is in turn. True if it reachs
        character.isTurn = false;
        transform.localScale = Vector3.one;
        // Move position towards the end of road
        var journeyLength = reachedRoadRect.GetWidth();
        var targetAnchoredPosition = new Vector2(journeyLength, 0f);

        percent += character.dexterity * deltaSpeed / 1000f;

        rectTransform.anchoredPosition = Vector2.Lerp(Vector2.zero, targetAnchoredPosition, percent);

        if (percent >= 1f)
        {
            percent = 0f;
            character.isTurn = true;
            transform.localScale = Vector3.one * deltaScale;
            if (onRunnerReachedCallback != null)
            {
                onRunnerReachedCallback.Invoke(this);
            }
        }
    }
}