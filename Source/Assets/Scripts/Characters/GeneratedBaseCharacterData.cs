using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName="Generated Character Data")]
public class GeneratedBaseCharacterData : ScriptableObject
{
    public new string name;
    public BaseGeneratedCharacterElementData elements;    
    public BaseJob baseJob;
    public BaseClass baseClass;

    public void CreateDisplay()
    {
        const string prefabPath = "Prefabs/Characters/Generated Base Character";
        var resource = Resources.Load<GeneratedBaseCharacter>(prefabPath);
        var instance = Instantiate<GeneratedBaseCharacter>(resource, Vector3.zero, Quaternion.identity);
        instance.LoadFromData(this);
        instance.gameObject.SetActive(false);
    }
}