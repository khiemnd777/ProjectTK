using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GeneratedBaseCharacter : BaseCharacter
{
    public BaseGeneratedCharacterElement elements;
    public BaseJob baseJob;
    public BaseClass baseClass;
    public GeneratedBaseCharacterData savedData;
    public BaseSkillHandler skillHandler;

    public override AvatarInfo GetAvatarInfo()
    {
        return new AvatarInfo
        {
            Avatar = elements.head.transform,
            AvatarStyle = SpriteHelper.instance.Get("Sprites/UI/action_bar => action_bar_3")
        };
    }

    public override ActionInfo DoAction()
    {
        var baseSkills = skillHandler.baseSkills;
        if(!baseSkills.Any())
            return new ActionInfo
            {
                time = 0f
            };
        var skill = baseSkills.FirstOrDefault();
        if(skill == null || skill is Object && skill.Equals(null))
            return new ActionInfo{
                time = 0f
            };
        // Execute the skill
        skill.Execute();
        return new ActionInfo
        {
            time = skill.GetLength()
        };
    }

    public void LoadFromData(GeneratedBaseCharacterData data)
    {
        savedData = data;
        characterName = data.name;
        baseJob = data.baseJob;
        baseClass = data.baseClass;
        elements.LoadFromData(data.elements);
    }

    public void SaveToData(GeneratedBaseCharacterData data = null)
    {
        if (data == null)
            data = savedData;
        data.baseJob = baseJob;
        data.baseClass = baseClass;
        data.name = characterName;
        data.elements.SaveFromDisplay(elements);
    }
}