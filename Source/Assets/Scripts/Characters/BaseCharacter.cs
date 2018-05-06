using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [System.NonSerialized]
    public int id;

    public string characterName;
    public bool isDeath;

    [Space]
    [Header("Default Animations")]
    public AnimationClip idlingAnimation;
    public AnimationClip hurtingAnimation;
    public AnimationClip dodgingAnimation;

    BaseCharacterStat _stats;
    public BaseCharacterStat stats
    {
        get
        {
            return _stats ?? (_stats = GetComponent<BaseCharacterStat>());
        }
    }

    Animator _animator;
    public Animator animator
    {
        get
        {
            return _animator ?? (_animator = GetComponent<Animator>());
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