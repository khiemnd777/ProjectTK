using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public enum SquadType
{
    Squad, PreparatorySquad
}

public class SquadItem : MonoBehaviour
{
    public Text displayOrder;
    public SquadType squadType;
    [System.NonSerialized]
    public GeneratedBaseCharacter character;
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
        if (squadType == SquadType.Squad)
        {
            SelectionCharacterUI.instance.OrderSquadByDragged();
        }
        else if(squadType == SquadType.PreparatorySquad)
        {
            SelectionCharacterUI.instance.OrderPreparatorySquadByDragged();
        }
    }

    void OnItemDroppedInZone(GameObject droppableZone, PointerEventData eventData)
    {
        var position = eventData.position;
        if (_canvas.renderMode == RenderMode.ScreenSpaceCamera
            || _canvas.renderMode == RenderMode.WorldSpace)
        {
            position = _canvas.worldCamera.ScreenToWorldPoint(position);
        }
        var thisDroppableZone = GetComponentInParent<DroppableZone>();
        if (droppableZone.gameObject.GetInstanceID() == thisDroppableZone.gameObject.GetInstanceID())
            return;
        // alternative between old and current position
        var itemsInDroppableZone = droppableZone.GetComponentsInChildren<DragDropHandler>();
        if (itemsInDroppableZone.Any())
        {
            var matchedItem = itemsInDroppableZone.FirstOrDefault(item => RectTransformUtility.RectangleContainsScreenPoint(item.GetComponent<RectTransform>(), position));
            if (matchedItem != null)
            {
                var siblingIndexOfMatchedItem = matchedItem.transform.GetSiblingIndex();
                var siblingIndexOfThis = transform.GetSiblingIndex();
                matchedItem.transform.SetParent(thisDroppableZone.transform);
                matchedItem.transform.SetSiblingIndex(siblingIndexOfThis);
                transform.SetParent(droppableZone.transform);
                transform.SetSiblingIndex(siblingIndexOfMatchedItem);
                if (squadType == SquadType.Squad)
                {
                    squadType = SquadType.PreparatorySquad;
                    CharacterList.instance.AddToPreparatory(character);
                    // add matched item to opposite
                    var matchedSquadItem = matchedItem.GetComponent<SquadItem>();
                    matchedSquadItem.squadType = SquadType.Squad;
                    CharacterList.instance.AddToSquad(matchedSquadItem.character);
                }
                else if (squadType == SquadType.PreparatorySquad)
                {
                    squadType = SquadType.Squad;
                    CharacterList.instance.AddToSquad(character);
                    // add matched item to opposite
                    var matchedSquadItem = matchedItem.GetComponent<SquadItem>();
                    matchedSquadItem.squadType = SquadType.PreparatorySquad;
                    CharacterList.instance.AddToPreparatory(matchedSquadItem.character);
                }
                return;
            }
        }
        if (squadType == SquadType.Squad)
        {
            if (itemsInDroppableZone.Length < CharacterList.instance.preparatoryNumber)
            {
                transform.SetParent(droppableZone.transform);
                transform.SetAsLastSibling();
                squadType = SquadType.PreparatorySquad;
                CharacterList.instance.AddToPreparatory(character);
            }
        }
        else if (squadType == SquadType.PreparatorySquad)
        {
            if (itemsInDroppableZone.Length < CharacterList.instance.squadNumber)
            {
                transform.SetParent(droppableZone.transform);
                transform.SetAsLastSibling();
                squadType = SquadType.Squad;
                CharacterList.instance.AddToSquad(character);
            }
        }
    }
}