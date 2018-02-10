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

    public void Generate()
    {
        var countOfHairSprite = SpriteHelper.instance.Count(hairSprite);
        var countOfHeadSprite = SpriteHelper.instance.Count(headSprite);
        var countOfBodySprite = SpriteHelper.instance.Count(bodySprite);
        var countOfLeftArmSprite = SpriteHelper.instance.Count(leftArmSprite);
        var countOfRightArmSprite = SpriteHelper.instance.Count(rightArmSprite);
        var countOfLeftLegSprite = SpriteHelper.instance.Count(leftLegSprite);
        var countOfRightLegSprite = SpriteHelper.instance.Count(rightLegSprite);
    }
}