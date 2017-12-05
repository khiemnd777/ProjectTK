using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvailableTacticItem : MonoBehaviour
{
    public Tactical tactic;
    public TacticItem tacticItemPrefab;

    DropZoneHandler dropZoneHandler;

    Text title;

    void Start()
    {
        dropZoneHandler = GetComponent<DropZoneHandler>();
        dropZoneHandler.onDragInZoneEvent += OnItemDropInZone;
        dropZoneHandler.onDropInZoneEvent += OnItemDroppedInZone;
    }

    void OnItemDropInZone(GameObject draggableZone, PointerEventData eventData)
    {

    }

    void OnItemDroppedInZone(GameObject droppableZone, PointerEventData eventData)
    {
        droppableZone.SetActive(true);
        var abilityItem = droppableZone.GetComponentInParent<AbilityItem>();

        if (abilityItem == null)
            return;

        var instanceTacticItem = Instantiate(tacticItemPrefab, Vector2.zero, Quaternion.identity, droppableZone.transform);
        var instanceTactic = Instantiate(tactic, Vector2.zero, Quaternion.identity);
        
        abilityItem.AddTacticItem(instanceTactic, instanceTacticItem);
        abilityItem.SetTacticDisplayOrder();
        abilityItem.ShowTacticContainer();

        instanceTacticItem = null;
        instanceTactic = null;
        abilityItem = null;
    }

    public void HandleTitle()
    {
        title = GetComponentInChildren<Text>();
        title.text = tactic.description;
    }

    void OnDrawGizmos()
    {
        if (tactic.IsNull())
            return;
        HandleTitle();
    }
}