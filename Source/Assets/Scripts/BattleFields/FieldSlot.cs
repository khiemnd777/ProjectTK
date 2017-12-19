using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldSlot : MonoBehaviour
{
    public Image icon;
    public bool flip;
    [Space]
    public Character character;

    DragDropHandler dragDropHandler;

    void Start()
    {
        dragDropHandler = GetComponent<DragDropHandler>();
        dragDropHandler.onDragged += OnUpdateSlot;
        Flip();
    }

    void Update()
    {
        if(!character.IsNull() && character.isDeath){
            ClearSlot();
        }
        Flip();
    }

    public void AddField(Character character)
    {
        this.character = character;
        icon.sprite = character.icon;
        icon.enabled = true;
    }

    public bool CanAdd()
    {
        return character.IsNull();
    }

    public void ClearSlot()
    {
        this.character = null;
        icon.enabled = false;
    }

    public void OnShowSkillPanelButtonClick()
    {
        if (character.IsNull())
            return;
        var abilityList = AbilityList.instance;
        abilityList.Clear();
        foreach (var skill in character.learnedSkills)
        {
            if (skill.isDefault)
                continue;
            abilityList.AddItem(skill);
        }
        abilityList.OpenPanel(transform.position);
        abilityList = null;
    }

    void OnUpdateSlot(GameObject item, int index, bool isAlternative)
    {
        var oldFieldSlot = item.GetComponent<FieldSlot>();
        if (isAlternative)
        {
            var currentCharacter = character;
            AddField(oldFieldSlot.character);
            oldFieldSlot.AddField(currentCharacter);
            oldFieldSlot.character.slot = character.slot;
            character.slot = index;
        }
        else
        {
            AddField(oldFieldSlot.character);
            oldFieldSlot.ClearSlot();
            character.slot = index;
        }
        BattleFieldManager.instance.ArrageSquad();
    }

    void Flip()
    {
        if(character.IsNull())
            return;
        if (!flip)
            return;
        if (character.isEnemy)
        {
            var scale = icon.transform.localScale;
            var deltaFlip = scale.x > 0 ? -1 : 1;
            icon.transform.localScale = new Vector3(scale.x * deltaFlip, scale.y, scale.z);
        }
    }
}