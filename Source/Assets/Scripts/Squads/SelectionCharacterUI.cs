using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class SelectionCharacterUI : MonoBehaviour
{
    #region Singleton
    static SelectionCharacterUI _instance;

    public static SelectionCharacterUI instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<SelectionCharacterUI>();
                if (!_instance)
                {
                    Debug.LogError("There needs to be one active SelectionCharacterUI script on a GameObject in your scene.");
                }
                else
                {

                }
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField]
    RectTransform squadPanel;
    [SerializeField]
    RectTransform preparatorySquadPanel;
    [SerializeField]
    SquadItem squadItemPrefab;

    CharacterList _list;
    SquadItem[] _squadCharacterSquadItems;
    SquadItem[] _preparatorySquadCharacterSquadItems;

    void Start()
    {
        _list = CharacterList.instance;
        _squadCharacterSquadItems = squadPanel.GetComponentsInChildren<SquadItem>();
        _preparatorySquadCharacterSquadItems = preparatorySquadPanel.GetComponentsInChildren<SquadItem>();
        AssignSquadCharacter(_list.squadCharacters, squadPanel, SquadType.Squad);
        AssignSquadCharacter(_list.preparatoryCharacters, preparatorySquadPanel, SquadType.PreparatorySquad);
    }

    void AssignSquadCharacter(List<GeneratedBaseCharacter> squadCharacters, Transform panel, SquadType squadType)
    {
        // clear all items
        var children = panel.GetComponentsInChildren<SquadItem>();
        foreach(var child in children){
            DestroyImmediate(child.gameObject);
        }
        // assigns
        for (var i = 0; i < squadCharacters.Count; i++)
        {
            var character = squadCharacters.ElementAtOrDefault(i);
            if(character == null)
                continue;
            var squadItem = Instantiate<SquadItem>(squadItemPrefab, Vector3.zero, Quaternion.identity, panel.transform);
            squadItem.character = character;
            squadItem.squadType = squadType;
        }

    }

    public void AssignSquad(){
        AssignSquadCharacter(_list.squadCharacters, squadPanel, SquadType.Squad);
        // NumberDisplayOrder(squadPanel, "22b573", "0071bc", "006837");
    }

    public void AssignPreparatorySquad(){
        AssignSquadCharacter(_list.preparatoryCharacters, preparatorySquadPanel, SquadType.PreparatorySquad);
        // NumberDisplayOrder(preparatorySquadPanel, "603813", "603813");
    }

    public void NumberDisplayOrder(Transform panel, params string[] squadColors)
    {
        var squadItems = panel.GetComponentsInChildren<SquadItem>();
        squadItems = squadItems.Where(x => !x.Equals(null)).ToArray();
        for (var i = 0; i < squadItems.Length; i++)
        {
            var squadItem = squadItems[i];
            if (!squadItem.gameObject.activeSelf)
                continue;
            var number = i + 1;
            squadItem.displayOrder.text = (i + 1).ToString();
            var color = new Color();
            ColorUtility.TryParseHtmlString("#" + squadColors[i], out color);
            squadItem.displayOrder.color = color;
        }
    }

    public void OrderSquadByDragged()
    {
        var items = squadPanel.GetComponentsInChildren<SquadItem>();
        _list.squadCharacters.Clear();
        foreach(var i in items){
            _list.AddToSquad(i.character);
        }
    }

    public void OrderPreparatorySquadByDragged()
    {
        var items = preparatorySquadPanel.GetComponentsInChildren<SquadItem>();
        _list.preparatoryCharacters.Clear();
        foreach(var i in items){
            _list.AddToPreparatory(i.character);
        }
    }
}