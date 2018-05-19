using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterList : MonoBehaviour
{
    #region Singleton
    static MonsterList _instance;

    public static MonsterList instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<MonsterList>();
                if (!_instance)
                {
                    Debug.LogError("There needs to be one active MonsterList script on a GameObject in your scene.");
                }
                else
                {

                }
            }
            return _instance;
        }
    }
    #endregion

    public List<BaseCharacter> monsters;

    void Start()
    {
        monsters = monsters ?? (monsters = new List<BaseCharacter>());
    }
}