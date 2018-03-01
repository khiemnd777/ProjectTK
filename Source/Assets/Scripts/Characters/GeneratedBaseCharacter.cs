using UnityEngine;

[System.Serializable]
public class BaseCharacterElement
{
    public SpriteRenderer head;
    public SpriteRenderer eye;
    public SpriteRenderer mouth;
    public SpriteRenderer body;
    public SpriteRenderer leftArm;
    public SpriteRenderer leftWeapon;
    public SpriteRenderer rightArm;
    public SpriteRenderer rightWeapon;
    public SpriteRenderer leftLeg;
    public SpriteRenderer rightLeg;
}

public class GeneratedBaseCharacter : MonoBehaviour
{
    public BaseCharacterElement elements;

    [System.NonSerialized]
    public int id;
    public BaseJob baseJob;
    public BaseClass baseClass;
    public bool isDeath;

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

    void Awake()
    {
        id = transform.GetInstanceID();
        baseJob.label = JobLabel.Swordman;
    }
}