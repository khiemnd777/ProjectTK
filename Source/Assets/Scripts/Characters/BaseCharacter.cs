using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [System.NonSerialized]
    public int id;

    public string characterName;
    public bool isDeath;

    BaseCharacterStat _stats;
    public BaseCharacterStat stats
    {
        get
        {
            return _stats ?? (_stats = GetComponent<BaseCharacterStat>());
        }
    }
    
    protected virtual void Awake()
    {
        id = transform.GetInstanceID();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    public virtual Transform GetAvatar()
    {
        return null;
    }
}