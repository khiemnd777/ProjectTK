using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityItem : MonoBehaviour
{
    public Ability ability;
    public RectTransform titleContainer;
    public Transform tacticContainer;
    public TacticItem tacticItemPrefab;

    DragDropHandler dragDropHandler;
    RectTransform rectTransform;
    float minHeight;

    void Start()
    {
        dragDropHandler = GetComponent<DragDropHandler>();
        rectTransform = GetComponent<RectTransform>();
        minHeight = titleContainer.GetHeight() + 10f;
        FitWithTacticContainer();
        
        dragDropHandler.onDragged += OnItemDragged;
    }

    void Update()
    {
        FitWithTacticContainer();
    }

    void OnItemDragged(GameObject item, int index, bool isAlternative){
        AbilityList.instance.SetDisplayOrder();
    }

    public void AddTacticItem(Tactical tactic, TacticItem tacticItem){
        ability.AddTactic(tactic);
        tacticItem.transform.SetParent(tacticContainer);
        tacticItem.AddTactic(tactic, this);
    }

    public void HandleTitle()
    {
        var title = GetComponentInChildren<Text>();
        if (title.IsNull())
            return;
        title.text = ability.name +
            (!string.IsNullOrEmpty(ability.description)
                && !string.IsNullOrEmpty(ability.name) ? "\n" : "") +
                (!string.IsNullOrEmpty(ability.description) ? ability.description : "");
    }

    public void InstantiateTacticItems(){
        var tactics = ability.tactics;
        foreach(var tactic in tactics){
            if(tactic.isDefault)
                continue;
            tactic.displayOrder = GetNextTacticDisplayOrder();
            var tacticItem = Instantiate<TacticItem>(tacticItemPrefab, Vector2.zero, Quaternion.identity, tacticContainer);
            tacticItem.AddTactic(tactic, this);
            tacticItem = null;
        }
        tactics = null;
    }

    int GetNextTacticDisplayOrder()
    {
        var tacticItems = tacticContainer.GetComponentsInChildren<TacticItem>();
        if(tacticItems.Length == 0){
            return 1;
        }
        var nextDisplayOrder = tacticItems.Max(x => x.tactic.displayOrder) + 1;
        tacticItems = null;
        return nextDisplayOrder;
    }

    public void SetTacticDisplayOrder()
    {
        var tacticItems = tacticContainer.GetComponentsInChildren<TacticItem>();
        for (var i = 0; i < tacticItems.Length; i++)
        {
            var item = tacticItems[i];
            item.tactic.displayOrder = i + 1;
            item = null;
        }
        tacticItems = null;
    }

    public void ClearAllTacticItems(){
        var tacticItems = tacticContainer.GetComponentsInChildren<TacticItem>();
        foreach(var tacticItem in tacticItems){
            DestroyImmediate(tacticItem.gameObject);
        }
    }

    public void ToggleTacticContainer()
    {
        tacticContainer.gameObject.SetActive(!tacticContainer.gameObject.activeSelf);
    }

    public void ShowTacticContainer(){
        tacticContainer.gameObject.SetActive(true);
    }

    public void CloseTacticContainer(){
        tacticContainer.gameObject.SetActive(false);
    }

    void FitWithTacticContainer()
    {
        var tacticItems = tacticContainer.GetComponentsInChildren<TacticItem>();
        if(tacticItems.Length == 0){
            CloseTacticContainer();
        }
        if(!tacticContainer.gameObject.activeSelf){
            rectTransform.SetHeight(minHeight);
            return;
        }
        var totalTacticContainerHeight = GetTacticContainerHeight();
        var paddingBottom = (tacticItems.Length > 0 ? 1 : -1) * 5f;
        rectTransform.SetHeight(minHeight + totalTacticContainerHeight + paddingBottom);
        tacticItems = null;
    }

    float GetTacticContainerHeight(){
        var totalTacticContainerHeight = 0f;
        var tacticItems = tacticContainer.GetComponentsInChildren<TacticItem>();
        var verticalLayoutGroup = tacticContainer.GetComponent<VerticalLayoutGroup>();
        var index = 0;
        foreach (var tacticItem in tacticItems)
        {
            var deltaBottomHeight = index == 0 ? verticalLayoutGroup.padding.bottom : verticalLayoutGroup.spacing;
            var tacticItemRectTransform = tacticItem.GetComponent<RectTransform>();
            var singleHeight = tacticItemRectTransform.GetHeight();
            totalTacticContainerHeight += singleHeight + deltaBottomHeight;
            tacticItemRectTransform = null;
            ++index;
        }
        tacticItems = null;
        verticalLayoutGroup = null;
        return totalTacticContainerHeight;
    }

    void OnDrawGizmos()
    {
        if (ability.IsNull())
            return;
        HandleTitle();
    }
}