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

    const string iconJobUILocation = "Sprites/UI/selection-ui";
    const string recruitUILoc = "Sprites/UI/recruit-ui";

    [SerializeField]
    RectTransform squadPanel;
    [SerializeField]
    RectTransform preparatorySquadPanel;
    [SerializeField]
    SquadItem squadItemPrefab;

    CharacterList _list;
    SquadItem[] _squadCharacterSquadItems;
    SquadItem[] _preparatorySquadCharacterSquadItems;
    SpriteHelper _spriteHelper;

    void Awake()
    {
        _spriteHelper = SpriteHelper.instance;
        _list = CharacterList.instance;
    }

    void Start()
    {
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
        var colors = squadType == SquadType.Squad ? new [] {"22b573", "0071bc", "006837"} : new [] {"603813", "603813"};
        // assigns
        for (var i = 0; i < squadCharacters.Count; i++)
        {
            var character = squadCharacters.ElementAtOrDefault(i);
            if(character == null)
                continue;
            var squadItem = Instantiate<SquadItem>(squadItemPrefab, Vector3.zero, Quaternion.identity, panel.transform);
            squadItem.character = character;
            squadItem.squadType = squadType;
            // display order
            squadItem.displayOrder.text = ((squadType == SquadType.Squad ? 0 : _list.squadCharacters.Count) + i + 1).ToString();
            // display order text's color
            var color = new Color();
            ColorUtility.TryParseHtmlString("#" + colors[i], out color);
            squadItem.displayOrder.color = color;
            // icon of job
            squadItem.iconJob.sprite = GetIconJobSprite(character.baseJob.label);
            // icon of class
            squadItem.iconClass.sprite = GetClassSpriteByLabel(character.baseClass.label);
            // job text
            squadItem.jobText.text = character.baseJob.label.ToString();
            // character name
            squadItem.characterName.text = character.characterName;
            // level
            squadItem.level.text = character.baseClass.level.ToString();
            // health
            squadItem.healthText.text = Mathf.RoundToInt(character.stats.currentHealth) + "/" + Mathf.RoundToInt(character.stats.maxHealth.GetValue());
        }
    }

    Sprite GetIconJobSprite(JobLabel label)
    {
        var pattern = "{0} => {1}";
        switch (label)
        {
            case JobLabel.Swordman:
                return _spriteHelper.Get(string.Format(pattern, iconJobUILocation, "icon_swordman"));
            case JobLabel.Archer:
                return _spriteHelper.Get(string.Format(pattern, iconJobUILocation, "icon_archer"));
            case JobLabel.Healer:
                return _spriteHelper.Get(string.Format(pattern, iconJobUILocation, "icon_healer"));
            case JobLabel.Mage:
                return _spriteHelper.Get(string.Format(pattern, iconJobUILocation, "icon_mage"));
            default:
                break;
        }
        return null;
    }

    public Sprite GetClassSpriteByLabel(ClassLabel label)
    {
        var pattern = "{0} => {1}";
        switch (label)
        {
            case ClassLabel.A:
                return _spriteHelper.Get(string.Format(pattern, recruitUILoc, "a-class"));
            case ClassLabel.B:
                return _spriteHelper.Get(string.Format(pattern, recruitUILoc, "b-class"));
            case ClassLabel.C:
                return _spriteHelper.Get(string.Format(pattern, recruitUILoc, "c-class"));
            case ClassLabel.S:
                return _spriteHelper.Get(string.Format(pattern, recruitUILoc, "s-class"));
            default:
                break;
        }
        return null;
    }

    public void AssignSquad(){
        AssignSquadCharacter(_list.squadCharacters, squadPanel, SquadType.Squad);
        // NumberDisplayOrder(squadPanel, "22b573", "0071bc", "006837");
    }

    public void AssignPreparatorySquad(){
        AssignSquadCharacter(_list.preparatoryCharacters, preparatorySquadPanel, SquadType.PreparatorySquad);
        // NumberDisplayOrder(preparatorySquadPanel, "603813", "603813");
    }

    public void NumberDisplayOrder(SquadType squadType)
    {
        var colors = squadType == SquadType.Squad ? new [] {"22b573", "0071bc", "006837"} : new [] {"603813", "603813"};
        var panel = squadType == SquadType.Squad ? squadPanel : preparatorySquadPanel;
        var squadItems = panel.GetComponentsInChildren<SquadItem>();
        for (var i = 0; i < squadItems.Length; i++)
        {
            var squadItem = squadItems[i];
            if (!squadItem.gameObject.activeSelf)
                continue;
            var number = i + 1;
            squadItem.displayOrder.text = ((squadType == SquadType.Squad ? 0 : _list.squadCharacters.Count) + i + 1).ToString();
            var color = new Color();
            ColorUtility.TryParseHtmlString("#" + colors[i], out color);
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
        NumberDisplayOrder(SquadType.Squad);
    }

    public void OrderPreparatorySquadByDragged()
    {
        var items = preparatorySquadPanel.GetComponentsInChildren<SquadItem>();
        _list.preparatoryCharacters.Clear();
        foreach(var i in items){
            _list.AddToPreparatory(i.character);
        }
        NumberDisplayOrder(SquadType.PreparatorySquad);
    }
}