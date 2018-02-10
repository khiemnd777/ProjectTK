using UnityEngine;

[System.Serializable]
public class BaseCharacterElement
{
    public SpriteRenderer hair;
    public SpriteRenderer head;
    public SpriteRenderer body;
    public SpriteRenderer leftArm;
    public SpriteRenderer rightArm;
    public SpriteRenderer leftLeg;
    public SpriteRenderer rightLeg;
}

public class GeneratedBaseCharacter : MonoBehaviour
{
    public BaseCharacterElement elements;
}