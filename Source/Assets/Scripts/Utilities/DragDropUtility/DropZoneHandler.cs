using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DragDropHandler))]
public class DropZoneHandler : MonoBehaviour
{
    public delegate void OnDragInZoneEvent(GameObject draggableZone, PointerEventData eventData);
    public OnDragInZoneEvent onDragInZoneEvent;

    public delegate void OnDropInZoneEvent(GameObject droppableZone, PointerEventData eventData);
    public OnDropInZoneEvent onDropInZoneEvent;

    DragDropHandler handler;
    DroppableZone lastDraggableZone;
    Canvas canvas;

    void Start()
    {
        handler = GetComponent<DragDropHandler>();
        handler.onBeginDragEvent += OnBeginDrag;
        handler.onDragEvent += OnDrag;
        handler.onEndDragEvent += OnEndDrag;
    }

    void OnBeginDrag(PointerEventData eventData){
        canvas = GetComponentInParent<Canvas>();
    }

    void OnDrag(PointerEventData eventData)
    {
        var position = eventData.position;
        if(canvas.renderMode == RenderMode.ScreenSpaceCamera 
            || canvas.renderMode == RenderMode.WorldSpace){
            position = canvas.worldCamera.ScreenToWorldPoint(position);
        }
        // interactable cases
        var interactableZones = FindObjectsOfType<InteractableZone>();
        if (interactableZones.Length == 0)
            return;
        foreach (var interactableZone in interactableZones)
        {
            var rectInteractableZone = interactableZone.GetComponent<RectTransform>();
            if (rectInteractableZone.IsNull())
                continue;
            if (RectTransformUtility.RectangleContainsScreenPoint(rectInteractableZone, position))
            {
                var droppableZones = rectInteractableZone.GetComponentsInChildren<DroppableZone>(true);
                if (droppableZones.Length == 0)
                    continue;
                lastDraggableZone = droppableZones[0];
                foreach (var zone in droppableZones)
                {
                    var rectZone = zone.GetComponent<RectTransform>();
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectZone, position))
                    {
                        lastDraggableZone = zone;
                        if (onDragInZoneEvent != null)
                        {
                            onDragInZoneEvent.Invoke(zone.gameObject, eventData);
                        }
                    }
                    rectZone = null;
                }
                droppableZones = null;
            }
            rectInteractableZone = null;
        }
    }

    void OnEndDrag(PointerEventData eventData)
    {
        if (lastDraggableZone == null)
            return;
        if (onDropInZoneEvent != null)
            onDropInZoneEvent.Invoke(lastDraggableZone.gameObject, eventData);
        lastDraggableZone = null;
    }
}
