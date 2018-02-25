using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum ClassLabel
{
    S, A, B, C
}

public enum JobLabel
{
    Swordman, Archer, Mage, Healer
}

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
    [Header("Element Location")]
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
    [Header("Class's Generated Percent")]
    [Range(0, 1)]
    public float sPercent = .01f;
    [Range(0, 1)]
    public float aPercent = .05f;
    [Range(0, 1)]
    public float bPercent = .6f;
    [Range(0, 1)]
    public float cPercent = .34f;
    [Header("Job's Generated Percent")]
    [Range(0, 1)]
    public float swordmanPercent = 1 / 4f;
    [Range(0, 1)]
    public float archerPercent = 1 / 4f;
    [Range(0, 1)]
    public float magePercent = 1 / 4f;
    [Range(0, 1)]
    public float healerPercent = 1 / 4f;
    [Header("Base Generated Level")]
    public int baseLevelMin = 1;
    public int baseLevelMax = 1;
    [Header("Point/level of Class")]
    public MinMax sPointPerLevel;
    public MinMax aPointPerLevel;
    public MinMax bPointPerLevel;
    public MinMax cPointPerLevel;

    [Header("Temporary")]
    public TendencyPoint tendencyPoint;

    ClassLabel[] _cachedClassLabels;
    JobLabel[] _cachedJobLabels;
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
    // int countOfArmSprite;
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
        countOfLegSprite = spriteHelper.Count(legSpriteLoc) / 2;
    }

    public void Generate()
    {
        var pattern = "{0} => {1}";
        var headIndex = Random.Range(0, countOfHeadSprite);
        var eyeIndex = Random.Range(0, countOfEyeSprite);
        var mouthIndex = Random.Range(0, countOfMouthSprite);
        var bodyIndex = Random.Range(0, countOfBodySprite);
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

        // generating names
        var genName = nameGenerator.Generate();
        currentGeneratedBaseCharacter.name = genName;
        characterName.text = genName;

        // generating jobs
        currentGeneratedBaseCharacter.baseJob.label = GenerateJob();

        // generating classes and base point per level
        var baseClass = currentGeneratedBaseCharacter.baseClass;
        baseClass.label = GenerateClass();

        // generating tendency points
        tendencyPoint.GeneratePoints();
        var stats = currentGeneratedBaseCharacter.stats;
        stats.damage.growthPercent = tendencyPoint.damagePoint;
        stats.speed.growthPercent = tendencyPoint.speedPoint;
        stats.hp.growthPercent = tendencyPoint.hpPoint;

        // generating stats, level and point per level
        var baseLevel = Random.Range(baseLevelMin, baseLevelMax + 1);
        baseClass.pointPerLevel = 0;
        for (var i = 0; i < baseLevel; i++)
        {
            var basePointPerLevel = AssignBasePointPerLevelOfClass(baseClass.label);
            var pointPerLevel = basePointPerLevel;
            baseClass.pointPerLevel += pointPerLevel;
        }
        var baseClassPointPerLevel = baseClass.pointPerLevel;
        stats.damage.baseValue = Mathf.Round(baseClassPointPerLevel * tendencyPoint.damagePoint);
        stats.speed.baseValue = Mathf.Round(baseClassPointPerLevel * tendencyPoint.speedPoint);
        stats.hp.baseValue = baseClassPointPerLevel - (stats.damage.baseValue + stats.speed.baseValue); //Mathf.Round(basePointPerLevel * tendencyPoint.hpPoint);
        baseClass.level = baseLevel;
        stats.TransformValues();
    }

    public ClassLabel GenerateClass()
    {
        if (_cachedClassLabels == null || _cachedClassLabels.Length == 0)
        {
            var classLabels = new[] { ClassLabel.S, ClassLabel.A, ClassLabel.B, ClassLabel.C };
            var percents = new[] { sPercent * 100, aPercent * 100, bPercent * 100, cPercent * 100 };
            _cachedClassLabels = Probability.Initialize(classLabels, percents);
        }
        return Probability.GetValueInProbability(_cachedClassLabels);
    }

    public JobLabel GenerateJob()
    {
        if (_cachedJobLabels == null || _cachedJobLabels.Length == 0)
        {
            var jobLabels = new[] { JobLabel.Swordman, JobLabel.Archer, JobLabel.Mage, JobLabel.Healer };
            var percents = new[] { swordmanPercent * 100, archerPercent * 100, magePercent * 100, healerPercent * 100 };
            _cachedJobLabels = Probability.Initialize(jobLabels, percents);
        }
        return Probability.GetValueInProbability(_cachedJobLabels);
    }

    public int AssignBasePointPerLevelOfClass(ClassLabel classLabel)
    {
        var point = 0;
        switch (classLabel)
        {
            case ClassLabel.S:
                point = (int) Random.Range(sPointPerLevel.min, sPointPerLevel.max + 1);
                break;
            case ClassLabel.A:
                point = (int) Random.Range(aPointPerLevel.min, aPointPerLevel.max + 1);
                break;
            case ClassLabel.B:
                point = (int) Random.Range(bPointPerLevel.min, bPointPerLevel.max + 1);
                break;
            case ClassLabel.C:
                point = (int) Random.Range(cPointPerLevel.min, cPointPerLevel.max + 1);
                break;
            default:
                break;
        }
        return point;
    }
}