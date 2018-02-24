using UnityEngine;

[System.Serializable]
public class BaseCharacterElement
{
    public SpriteRenderer head;
    public SpriteRenderer eye;
    public SpriteRenderer mouth;
    public SpriteRenderer body;
    public SpriteRenderer leftArm;
    public SpriteRenderer rightArm;
    public SpriteRenderer leftLeg;
    public SpriteRenderer rightLeg;
}

public class GeneratedBaseCharacter : MonoBehaviour
{
    public BaseCharacterElement elements;

    [System.NonSerialized]
    public int id;
    public JobLabel jobLabel;
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

    void Awake()
    {
        id = transform.GetInstanceID();
        jobLabel = JobLabel.Swordman;
    }
}