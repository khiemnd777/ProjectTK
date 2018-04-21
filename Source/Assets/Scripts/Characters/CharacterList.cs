using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterList : MonoBehaviour
{
    #region Singleton
    static CharacterList _instance;

    public static CharacterList instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<CharacterList>();
                if (!_instance)
                {
                    Debug.LogError("There needs to be one active CharacterList script on a GameObject in your scene.");
                }
                else
                {

                }
            }
            return _instance;
        }
    }
    #endregion

    public int squadNumber = 3;
    public int preparatoryNumber = 2;
    [System.NonSerialized]
    public List<GeneratedBaseCharacter> squadCharacters;
    [System.NonSerialized]
    public List<GeneratedBaseCharacter> preparatoryCharacters;

    void Start()
    {
        squadCharacters = new List<GeneratedBaseCharacter>();
        preparatoryCharacters = new List<GeneratedBaseCharacter>();
    }

    public void AddCharacter(GeneratedBaseCharacter character)
    {
        if (squadCharacters.Count < squadNumber)
        {
            squadCharacters.Add(character);
        }
        else
        {
            if (preparatoryCharacters.Count < preparatoryNumber)
            {
                preparatoryCharacters.Add(character);
            }
            else
            {
                // both lists are full
                Debug.Log("Both lists are full");
            }
        }
    }
}