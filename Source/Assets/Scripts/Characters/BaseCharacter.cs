using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    [System.NonSerialized]
    public int id;
    public string characterName;
    public bool isDeath;
    [Header("Default Animations")]
    public AnimationClip idlingAnimation;
    public AnimationClip hurtingAnimation;
    public AnimationClip dodgingAnimation;

    BaseSkillHandler _skillHandler;
    public BaseSkillHandler skillHandler
    {
        get
        {
            return _skillHandler ?? (_skillHandler = GetComponent<BaseSkillHandler>());
        }
    }

    // non-serialized
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

    public virtual AvatarInfo GetAvatarInfo()
    {
        return default(AvatarInfo);
    }
}

public struct AvatarInfo
{
    public Transform Avatar { get; set; }
    public Sprite AvatarStyle { get; set; }
}

public struct ActionInfo
{
    public float time { get; set; }
}