using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CharacterGenerator : MonoBehaviour
{
    #region Singleton
    static CharacterGenerator _instance;

    public static CharacterGenerator instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<CharacterGenerator>();
                if (!_instance)
                {
                    Debug.LogError("There needs to be one active CharacterGenerator script on a GameObject in your scene.");
                }
                else
                {
                    
                }
            }
            return _instance;
        }
    }
    #endregion

    public string headSpriteLoc = "Sprites/Characters/Generated Characters/heads";
    public string eyeSpriteLoc = "Sprites/Characters/Generated Characters/eyes";
    public string mouthSpriteLoc = "Sprites/Characters/Generated Characters/mouths";
    public string bodySpriteLoc = "Sprites/Characters/Generated Characters/bodys";
    public string armSpriteLoc = "Sprites/Characters/Generated Characters/arms";
    public string legSpriteLoc = "Sprites/Characters/Generated Characters/legs";
    [Space]
    public GeneratedBaseCharacter generatedBaseCharacterPrefab;
    public GeneratedBaseCharacter currentGeneratedBaseCharacter;
    public Text characterName;

    List<GeneratedBaseCharacter> _generatedCharaters;

    public ICollection<GeneratedBaseCharacter> generatedCharaters
    {
        get
        {
            return _generatedCharaters ?? (_generatedCharaters = new List<GeneratedBaseCharacter>());
        }
    }

    SpriteHelper spriteHelper;
    CharacterNameGenerator nameGenerator;

    int countOfHeadSprite;
    int countOfEyeSprite;
    int countOfMouthSprite;
    int countOfBodySprite;
    int countOfArmSprite;
    int countOfLegSprite;

    List<string> _listOfCharacterName;

    void Awake()
    {
        spriteHelper = SpriteHelper.instance;
        nameGenerator = CharacterNameGenerator.instance;
        _generatedCharaters = new List<GeneratedBaseCharacter>();
    }

    void Start()
    {
        countOfHeadSprite = spriteHelper.Count(headSpriteLoc);
        countOfEyeSprite = spriteHelper.Count(eyeSpriteLoc);
        countOfMouthSprite = spriteHelper.Count(mouthSpriteLoc);
        countOfBodySprite = spriteHelper.Count(bodySpriteLoc);
        countOfArmSprite = spriteHelper.Count(armSpriteLoc) / 2;
        countOfLegSprite = spriteHelper.Count(legSpriteLoc) / 2;
    }

    public void Generate()
    {
        var pattern = "{0} => {1}";
        var headIndex = Random.Range(0, countOfHeadSprite);
        var eyeIndex = Random.Range(0, countOfEyeSprite);
        var mouthIndex = Random.Range(0, countOfMouthSprite);
        var bodyIndex = Random.Range(0, countOfBodySprite);
        var armIndex = Random.Range(0, countOfArmSprite);
        var legIndex = Random.Range(0, countOfLegSprite);

        var headSprite = spriteHelper.Get(string.Format(pattern, headSpriteLoc, headIndex));
        var eyeSprite = spriteHelper.Get(string.Format(pattern, eyeSpriteLoc, eyeIndex));
        var mouthSprite = spriteHelper.Get(string.Format(pattern, mouthSpriteLoc, mouthIndex));
        var bodySprite = spriteHelper.Get(string.Format(pattern, bodySpriteLoc, bodyIndex));
        var leftArmSprite = spriteHelper.Get(string.Format(pattern, armSpriteLoc, "l_" + bodyIndex));
        var rightArmSprite = spriteHelper.Get(string.Format(pattern, armSpriteLoc, "r_" + bodyIndex));
        var leftLegSprite = spriteHelper.Get(string.Format(pattern, legSpriteLoc, "l_" + legIndex));
        var rightLegSprite = spriteHelper.Get(string.Format(pattern, legSpriteLoc, "r_" + legIndex));

        // var instanceOfBaseCharacter = Instantiate<GeneratedBaseCharacter>(generatedBaseCharacterPrefab, Vector3.zero, Quaternion.identity);
        // _generatedCharaters.Add(instanceOfBaseCharacter);

        var characterElements = currentGeneratedBaseCharacter.elements;
        var characterEyes = characterElements.eye;
        var characterMouth = characterElements.mouth;
        characterElements.head.sprite = headSprite;
        characterEyes.sprite = eyeSprite;
        characterMouth.sprite = mouthSprite;
        characterElements.body.sprite = bodySprite;
        characterElements.leftArm.sprite = leftArmSprite;
        characterElements.rightArm.sprite = rightArmSprite;
        characterElements.leftLeg.sprite = leftLegSprite;
        characterElements.rightLeg.sprite = rightLegSprite;

        // transform eyes and mouth according to pivot of Y
        // range of mouth
        var rangeYOfMouth = Random.Range(.2f, .6f);
        var mouthPosition = characterMouth.transform.localPosition;
        mouthPosition.y = rangeYOfMouth;
        characterMouth.transform.localPosition = mouthPosition;
        // range of eyes
        var rangeYOfEyes = Random.Range(1f, 1.3f);
        var eyesPosition = characterEyes.transform.localPosition;
        eyesPosition.y = rangeYOfEyes;
        characterEyes.transform.localPosition = eyesPosition;
        

        var genName = nameGenerator.Generate();
        currentGeneratedBaseCharacter.name = genName;
        characterName.text = genName;
    }
}