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

    public override Transform GetAvatar()
    {
        return elements.head.transform;
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
        if(data == null)
            data = savedData;
        data.baseJob = baseJob;
        data.baseClass = baseClass;
        data.name = characterName;
        data.elements.SaveFromDisplay(elements);
    }
}