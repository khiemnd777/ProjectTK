using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticItem : MonoBehaviour
{
    public Tactical tactic;
    public AbilityItem abilityItem;

    DragDropHandler dragDropHandler;

    void Start()
    {
        dragDropHandler = GetComponent<DragDropHandler>();
        dragDropHandler.onDragged += OnItemDragged;
    }

    void OnItemDragged(GameObject item, int index, bool isAlternative)
    {
        abilityItem.SetTacticDisplayOrder();
    }

    public void AddTactic(Tactical tactic, AbilityItem abilityItem = null)
    {
        this.tactic = tactic;
        this.abilityItem = abilityItem ?? this.abilityItem;
        
        HandleTitle();
        FitHeight();
    }

    public void HandleTitle()
    {
        var title = GetComponentInChildren<Text>();
        title.text = tactic.description;
    }

    public void FitHeight()
    {
        var title = GetComponentInChildren<Text>();
        var fitHeight = title.GetComponent<RectTransform>().GetHeight() + 15f;
        GetComponent<RectTransform>().SetHeight(fitHeight);
    }

    void OnDrawGizmos()
    {
        if (tactic.IsNull())
            return;
        HandleTitle();
    }
}