using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GeneratedBaseCharacter : BaseCharacter
{
    public BaseGeneratedCharacterElement elements;
    public BaseClass baseClass;
    public GeneratedBaseCharacterData savedData;

    protected override void Awake()
    {
        base.Awake();
    }

    public override AvatarInfo GetAvatarInfo()
    {
        return new AvatarInfo
        {
            Avatar = elements.head.transform,
            AvatarStyle = SpriteHelper.instance.Get("Sprites/UI/action_bar => action_bar_3")
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