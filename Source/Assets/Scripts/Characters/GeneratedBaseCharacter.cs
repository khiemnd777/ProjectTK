using UnityEngine;

[System.Serializable]
public class BaseCharacterElement
{
    public SpriteRenderer hair;
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

    void Awake()
    {
        id = transform.GetInstanceID();
    }
}