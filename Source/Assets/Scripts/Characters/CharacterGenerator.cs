using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    const string hairSprite = "Sprites/hairs";
    const string headSprite = "Sprites/heads";
    const string bodySprite = "Sprites/bodys";
    const string leftArmSprite = "Sprites/arms";
    const string rightArmSprite = "Sprites/arms";
    const string leftLegSprite = "Sprites/legs";
    const string rightLegSprite = "Sprites/legs";

    SpriteHelper spriteHelper;

    void Awake()
    {
        spriteHelper = SpriteHelper.instance;
    }

    public void Generate()
    {
        var countOfHairSprite = spriteHelper.Count(hairSprite);
        var countOfHeadSprite = spriteHelper.Count(headSprite);
        var countOfBodySprite = spriteHelper.Count(bodySprite);
        var countOfLeftArmSprite = spriteHelper.Count(leftArmSprite);
        var countOfRightArmSprite = spriteHelper.Count(rightArmSprite);
        var countOfLeftLegSprite = spriteHelper.Count(leftLegSprite);
        var countOfRightLegSprite = spriteHelper.Count(rightLegSprite);
    }
}