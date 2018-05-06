using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
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
    // [System.NonSerialized]
    public List<GeneratedBaseCharacter> squadCharacters;
    // [System.NonSerialized]
    public List<GeneratedBaseCharacter> preparatoryCharacters;
    [System.NonSerialized]
    public List<GeneratedBaseCharacter> freeCharacters;

    void Awake()
    {
        if (!_instance)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        squadCharacters = new List<GeneratedBaseCharacter>();
        preparatoryCharacters = new List<GeneratedBaseCharacter>();
    }

    public void AddCharacter(GeneratedBaseCharacter character)
    {
        if (squadCharacters.Count < squadNumber)
        {
            var clone = Instantiate<GeneratedBaseCharacter>(character, Vector3.zero, Quaternion.identity, transform);
            clone.gameObject.SetActive(false);
            squadCharacters.Add(clone);
        }
        else
        {
            if (preparatoryCharacters.Count < preparatoryNumber)
            {
                var clone = Instantiate<GeneratedBaseCharacter>(character, Vector3.zero, Quaternion.identity, transform);
                clone.gameObject.SetActive(false);
                preparatoryCharacters.Add(clone);
            }
            else
            {
                // both lists are full
                Debug.Log("Both lists are full");
                var clone = Instantiate<GeneratedBaseCharacter>(character, Vector3.zero, Quaternion.identity, transform);
                clone.gameObject.SetActive(false);
                freeCharacters.Add(clone);
            }
        }
    }

    public void AddToSquad(GeneratedBaseCharacter character, bool justAdd = false)
    {
        if (!justAdd)
        {
            if (preparatoryCharacters.Any(x => x.id == character.id))
            {
                preparatoryCharacters.RemoveAll(x => x.id == character.id);
            }
            if (squadCharacters.Any(x => x.id == character.id))
                return;
        }
        squadCharacters.Add(character);
    }

    public void AddToPreparatory(GeneratedBaseCharacter character, bool justAdd = false)
    {
        if (!justAdd)
        {
            if (squadCharacters.Any(x => x.id == character.id))
            {
                squadCharacters.RemoveAll(x => x.id == character.id);
            }
            if (preparatoryCharacters.Any(x => x.id == character.id))
                return;
        }
        preparatoryCharacters.Add(character);
    }

    public void MoveToSence(string name)
    {
        var scene = SceneManager.GetSceneByName(name);
        if (scene.isLoaded)
        {
            SceneManager.MoveGameObjectToScene(gameObject, scene);
        }
    }
}