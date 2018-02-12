using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNameGenerator : MonoBehaviour
{
    #region Singleton
    static CharacterNameGenerator _instance;

    public static CharacterNameGenerator instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<CharacterNameGenerator>();
                if (!_instance)
                {
                    Debug.LogError("There needs to be one active CharacterNameGenerator script on a GameObject in your scene.");
                }
                else
                {
                    
                }
            }
            return _instance;
        }
    }
    #endregion

    public string listOfName;

    List<string> _names;
    int range;
    
    void Start()
    {
        _names = new List<string>();
        Init();
    }

    void Init()
    {
        listOfName = "Ariel, Jamin, Abdiel, Cyrus, Nicodemas, Hamath, Elah, Jeshiah, Zachariah, Marcus, Shem, Eliezer, Simon, Arioch, Jared, Joshua, Peres, El'azar, Adonijah, Luke, Ithiel, Malachi, Janoah, Adlai, Pagiel, Abiel, Jakin, Jahaziel, Baruch, Johanan, Anayah, Areli, Edrei, Joachim, Othniel";
        var it = listOfName.Split(',');
        foreach(var name in it)
        {
            _names.Add(name.Trim());
        }
        it = null;
        range = _names.Count;
    }

    public string Generate()
    {
        var index = Random.Range(0, range);
        return _names[index];
    }
}