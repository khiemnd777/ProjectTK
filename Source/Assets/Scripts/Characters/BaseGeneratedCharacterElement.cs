using UnityEngine;

[System.Serializable]
public class BaseGeneratedCharacterElement
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

    public void LoadFromData(BaseGeneratedCharacterElementData data)
    {
        head.sprite = data.head;
        eye.sprite = data.eye;
        mouth.sprite = data.mouth;
        body.sprite = data.body;
        leftArm.sprite = data.leftArm;
        leftWeapon.sprite = data.leftWeapon;
        rightArm.sprite = data.rightArm;
        rightWeapon.sprite = data.rightWeapon;
        leftLeg.sprite = data.leftLeg;
        rightLeg.sprite = data.rightLeg;
    }
}

[System.Serializable]
public class BaseGeneratedCharacterElementData
{
    public Sprite head;
    public Sprite eye;
    public Sprite mouth;
    public Sprite body;
    public Sprite leftArm;
    public Sprite leftWeapon;
    public Sprite rightArm;
    public Sprite rightWeapon;
    public Sprite leftLeg;
    public Sprite rightLeg;

    public void SaveFromDisplay(BaseGeneratedCharacterElement display)
    {
        head = display.head.sprite;
        eye = display.eye.sprite;
        mouth = display.mouth.sprite;
        body = display.body.sprite;
        leftArm = display.leftArm.sprite;
        leftWeapon = display.leftWeapon.sprite;
        rightArm = display.rightArm.sprite;
        rightWeapon = display.rightWeapon.sprite;
        leftLeg = display.leftLeg.sprite;
        rightLeg = display.rightLeg.sprite;
    }
}