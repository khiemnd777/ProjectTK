using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class SquadItem : MonoBehaviour
{
    DragDropHandler _dragDropHandler;
    DropZoneHandler _dropZoneHandler;
    Canvas _canvas;

    void Start()
    {
        // drag drop handler
        _dragDropHandler = GetComponent<DragDropHandler>();
        _dragDropHandler.onDragged += OnItemDragged;
        _dragDropHandler.onBeginDragEvent += OnItemBeginDrag;
        // drop zone handler
        _dropZoneHandler = GetComponent<DropZoneHandler>();
        _dropZoneHandler.onDropInZoneEvent += OnItemDroppedInZone;
    }

    void OnItemBeginDrag(PointerEventData eventData)
    {
        _canvas = GetComponentInParent<Canvas>();
    }

    void OnItemDragged(GameObject item, int index, bool isAlternative)
    {

    }

    void OnItemDroppedInZone(GameObject droppableZone, PointerEventData eventData)
    {
        var position = eventData.position;
        if(_canvas.renderMode == RenderMode.ScreenSpaceCamera 
            || _canvas.renderMode == RenderMode.WorldSpace){
            position = _canvas.worldCamera.ScreenToWorldPoint(position);
        }
        // alternative between old and current position
        var itemsInDroppableZone = droppableZone.GetComponentsInChildren<DragDropHandler>();
        if(itemsInDroppableZone.Any())
        {
            var thisDroppableZone = GetComponentInParent<DroppableZone>();
            var matchedItem = itemsInDroppableZone.FirstOrDefault(item => RectTransformUtility.RectangleContainsScreenPoint(item.GetComponent<RectTransform>(), position));
            if(matchedItem != null){
                var siblingIndexOfMatchedItem = matchedItem.transform.GetSiblingIndex();
                var siblingIndexOfThis = transform.GetSiblingIndex();
                matchedItem.transform.SetParent(thisDroppableZone.transform);
                matchedItem.transform.SetSiblingIndex(siblingIndexOfThis);
                transform.SetParent(droppableZone.transform);    
                transform.SetSiblingIndex(siblingIndexOfMatchedItem);
            }
        }
        else{
            transform.SetParent(droppableZone.transform);
        }
    }
}