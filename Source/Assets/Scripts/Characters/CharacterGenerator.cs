using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum ClassLabel
{
    S, A, B, C, Common
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
    [Header("List of Generated Characters")]
    [SerializeField]
    GeneratedCharacterBlock[] generatedCharaterBlocks;
    [Header("Element's Sprite Locations")]
    public string headSpriteLoc = "Sprites/Characters/Generated Characters/{0}/heads";
    public string eyeSpriteLoc = "Sprites/Characters/Generated Characters/{0}/eyes";
    public string mouthSpriteLoc = "Sprites/Characters/Generated Characters/{0}/mouths";
    public string bodySpriteLoc = "Sprites/Characters/Generated Characters/{0}/bodys";
    public string armSpriteLoc = "Sprites/Characters/Generated Characters/{0}/arms";
    public string legSpriteLoc = "Sprites/Characters/Generated Characters/{0}/legs";
    [Header("Recruitment UI's Sprite Location")]
    public string recruitUILoc = "Sprites/UI/recruit-ui";
    [Space]
    public ClassLabel generatedCommonClassLabel = ClassLabel.Common;
    public ClassLabel generatedAClassLabel = ClassLabel.A;
    public ClassLabel generatedSClassLabel = ClassLabel.A;
    [Header("Weapon's Sprite Location")]
    public string weaponSpriteLoc = "Sprites/Weapons/basic weapons";
    [Space]
    [SerializeField]
    GeneratedBaseCharacter generatedBaseCharacterPrefab;
    // [SerializeField]
    // GeneratedBaseCharacter currentGeneratedBaseCharacter;
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
    [Header("Money bag")]
    public int gold = 9999;
    public int diamond = 9999;
    public int baseGoldAmount;
    public int baseDiamondAmount;
    public float baseAmountOnTime = 1.5f;
    public int amountOfCall = 800;
    public Text goldText;
    public Text diamondText;
    public Text amountForCallText;
    [Header("Misc")]
    public Rotator frogEyeLeft;
    public Rotator frogEyeRight;
    public RectTransform mainPanel;
    public RectTransform tavernBanner;
    public Button generatedButton;

    // [Header("Temporary")]
    // public TendencyPoint tendencyPoint;

    ClassLabel[] _cachedClassLabels;
    JobLabel[] _cachedJobLabels;

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
    }

    void Start()
    {
        amountForCallText.text = amountOfCall.ToString();
        FillElementSkinColor(Color.black);
        StartCoroutine(MainPanelShowing());
        tavernBanner.anchoredPosition = new Vector2(tavernBanner.anchoredPosition.x, 193f);
    }

    void Update()
    {
        // Updating gold and diamond's amount
        goldText.text = gold.ToString();
        diamondText.text = diamond.ToString();
    }

    IEnumerator MainPanelShowing()
    {
        var percent = 0f;
        var originalMainPanelAnchoredPosLerp = new Vector2(Screen.width, 0f);
        var deltaSlowdownPercent = 1.5f;
        var isInSlowdown = false;
        while(percent <= 1f)
        {
            percent += Time.deltaTime * deltaSlowdownPercent;
            if (percent >= .7f && percent < .85f && !isInSlowdown)
            {
                deltaSlowdownPercent /= 2f;
                isInSlowdown = true;
            }
            else if (percent >= .85f && percent < .9f)
            {
                if(isInSlowdown){
                    isInSlowdown = false;
                    deltaSlowdownPercent /= 2f;
                }
            }
            else if (percent >= .9f && !isInSlowdown)
            {
                deltaSlowdownPercent /= 2f;
                isInSlowdown = true;
            }
            mainPanel.anchoredPosition = Vector3.Lerp(originalMainPanelAnchoredPosLerp, Vector2.zero, percent);
            yield return null;
        }

        percent = 0f;

        while(percent <= 1f)
        {
            percent += Time.deltaTime * 2.5f;
            tavernBanner.anchoredPosition = Vector2.Lerp(new Vector2(0, 193), new Vector2(0, 60), percent);
            yield return null;
        }

        percent = 0f;

        while(percent <= 1f)
        {
            percent += Time.deltaTime * 4;
            tavernBanner.anchoredPosition = Vector2.Lerp(new Vector2(0, 60), new Vector2(0, 69), percent);
            yield return null;
        }
    }

    void FillElementSkinColor(Color skinColor)
    {
        var blocks = generatedCharaterBlocks;
        foreach (var block in blocks)
        {
            var characterElements = block.baseCharacter.elements;
            var characterEyes = characterElements.eye;
            var characterMouth = characterElements.mouth;
            var leftWeapon = characterElements.leftWeapon;
            var rightWeapon = characterElements.rightWeapon;
            characterElements.rightLeg.color
                = characterElements.leftLeg.color
                = characterElements.rightArm.color
                = characterElements.leftArm.color
                = characterElements.body.color
                = characterMouth.color
                = characterEyes.color
                = characterElements.head.color
                = leftWeapon.color
                = rightWeapon.color
                = skinColor;
        }
        blocks = null;
    }

    void LerpElementSkinColor(Color from, Color to, float speed = 5f)
    {
        StartCoroutine(LerpingElementSkinColor(from, to, speed));
    }

    IEnumerator LerpingElementSkinColor(Color from, Color to, float speed = 5f)
    {
        var percent = 0f;
        var blocks = generatedCharaterBlocks;
        while (percent <= 1f)
        {
            percent += Time.deltaTime * speed;
            foreach (var block in blocks)
            {
                var characterElements = block.baseCharacter.elements;
                var characterEyes = characterElements.eye;
                var characterMouth = characterElements.mouth;
                var leftWeapon = characterElements.leftWeapon;
                var rightWeapon = characterElements.rightWeapon;
                characterElements.rightLeg.color
                    = characterElements.leftLeg.color
                    = characterElements.rightArm.color
                    = characterElements.leftArm.color
                    = characterElements.body.color
                    = characterMouth.color
                    = characterEyes.color
                    = characterElements.head.color
                    = leftWeapon.color
                    = rightWeapon.color
                    = Color.Lerp(from, to, percent);
            }
            yield return null;
        }
        blocks = null;
    }

    IEnumerator Generating()
    {
        LerpElementSkinColor(Color.white, Color.black, 10f);

        var percent = 0f;
        var val = 0f;
        var frogEyeRotatorSpeed = frogEyeLeft.speed;
        frogEyeLeft.speed = frogEyeRotatorSpeed + 250;
        frogEyeRight.speed = frogEyeRotatorSpeed + 250;
        var i = 0;
        // scale out character
        foreach (var block in generatedCharaterBlocks)
        {
            var baseCharacter = block.baseCharacter;
            var percentScaleOutCharacter = 0f;
            var baseCharacterTransform = baseCharacter.transform;
            var baseCharacterLocalPosition = baseCharacterTransform.localPosition;
            var baseCharacterLocalPositionLerp = new Vector3(baseCharacterLocalPosition.x - 100, baseCharacterLocalPosition.y, baseCharacterLocalPosition.z);
            while (percentScaleOutCharacter <= 1)
            {
                percentScaleOutCharacter += Time.deltaTime * 25;
                baseCharacterTransform.localScale = Vector3.Lerp(Vector3.one * 27, Vector3.one * 35, percentScaleOutCharacter);
                baseCharacterTransform.localPosition = Vector3.Lerp(baseCharacterLocalPosition, baseCharacterLocalPositionLerp, percentScaleOutCharacter);
                yield return null;
            }
            block.tendencyPoint.gameObject.SetActive(false);
            block.level.transform.parent.gameObject.SetActive(false);
            block.jobLabel.gameObject.SetActive(false);
            block.goldButton.gameObject.SetActive(false);
            block.diamondButton.gameObject.SetActive(false);
            block.name.gameObject.SetActive(false);
            block.classImage.gameObject.SetActive(false);
        }
        // generating
        var deltaSlowdown = 50f;
        var deltaSlowdownPercent = 1.5f;
        var isInSlowdown = false;
        while (percent <= 1)
        {
            percent += Time.deltaTime * deltaSlowdownPercent;
            val = Mathf.Lerp(0, 1, percent);
            if (percent >= .5f && percent < .75f && !isInSlowdown)
            {
                deltaSlowdown /= 2f;
                deltaSlowdownPercent /= 2f;
                isInSlowdown = true;
            }
            else if (percent >= .75f && percent < .9f)
            {
                if(isInSlowdown){
                    isInSlowdown = false;
                    deltaSlowdown /= 2f;
                    deltaSlowdownPercent /= 2f;
                }
            }
            else if (percent >= .9f && !isInSlowdown)
            {
                deltaSlowdown /= 2f;
                deltaSlowdownPercent /= 2f;
                isInSlowdown = true;
            }
            yield return new WaitForSeconds(val / deltaSlowdown);
            __Generate();
            ++i;
            yield return null;
        }
        frogEyeLeft.speed = frogEyeRotatorSpeed;
        frogEyeRight.speed = frogEyeRotatorSpeed;
        yield return new WaitForSeconds(.5f);
        // LerpElementSkinColor(Color.black, Color.white);
        foreach (var block in generatedCharaterBlocks)
        {
            var characterElements = block.baseCharacter.elements;
            var characterEyes = characterElements.eye;
            var characterMouth = characterElements.mouth;
            var leftWeapon = characterElements.leftWeapon;
            var rightWeapon = characterElements.rightWeapon;
            var appearedColorPercent = 0f;
            while (appearedColorPercent <= 1)
            {
                appearedColorPercent += Time.deltaTime;
                characterElements.rightLeg.color
                    = characterElements.leftLeg.color
                    = characterElements.rightArm.color
                    = characterElements.leftArm.color
                    = characterElements.body.color
                    = characterMouth.color
                    = characterEyes.color
                    = characterElements.head.color
                    = leftWeapon.color
                    = rightWeapon.color
                    = Color.Lerp(Color.black, Color.white, appearedColorPercent);
                yield return null;
            }
        }
        yield return new WaitForSeconds(.625f);
        // scale in character
        var percentScaleInCharacter = 0f;
        var originalPosition = generatedCharaterBlocks[0].baseCharacter.transform.localPosition;
        var originalPositionLerp = new Vector3(originalPosition.x + 100, originalPosition.y, originalPosition.z);
        var originalScale = generatedCharaterBlocks[0].baseCharacter.transform.localScale;
        while (percentScaleInCharacter <= 1)
        {
            percentScaleInCharacter += Time.deltaTime * 15;
            foreach (var block in generatedCharaterBlocks)
            {
                var baseCharacter = block.baseCharacter;
                var baseCharacterTransform = baseCharacter.transform;
                // var baseCharacterLocalPosition = baseCharacterTransform.localPosition;
                baseCharacterTransform.localScale = Vector3.Lerp(Vector3.one * 35, Vector3.one * 27, percentScaleInCharacter);
                baseCharacterTransform.localPosition = Vector3.Lerp(originalPosition, originalPositionLerp, percentScaleInCharacter);
            }
            yield return null;
        }
        var showingPercent = 0f;
        while (showingPercent <= 1)
        {
            showingPercent += Time.deltaTime * 5f;
            foreach (var block in generatedCharaterBlocks)
            {
                block.tendencyPoint.gameObject.SetActive(true);
                block.level.transform.parent.gameObject.SetActive(true);
                block.jobLabel.gameObject.SetActive(true);
                block.goldButton.gameObject.SetActive(true);
                block.diamondButton.gameObject.SetActive(true);
                block.name.gameObject.SetActive(true);
                block.classImage.gameObject.SetActive(true);
                block.tendencyPoint.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 30, showingPercent);
            }
            yield return null;
        }
        generatedButton.interactable = true;
    }

    void __Generate()
    {
        // var descFormat = "{0} ({1})\nLvl. {2}\nHP {3}/{4}";
        var blocks = generatedCharaterBlocks;
        foreach (var block in blocks)
        {
            var currentGeneratedBaseCharacter = block.baseCharacter;
            var tendencyPoint = block.tendencyPoint;
            var characterName = block.name;
            // var characterDescription = block.description;
            var classImage = block.classImage;
            var characterElements = currentGeneratedBaseCharacter.elements;

            // generating names
            var genName = nameGenerator.Generate();
            currentGeneratedBaseCharacter.name = genName;
            characterName.text = genName;

            // generating jobs
            var baseJob = currentGeneratedBaseCharacter.baseJob;
            baseJob.label = GenerateJob();

            // assign weapon by job
            characterElements.rightWeapon.sprite = null;

            AssignWeaponByJob(baseJob.label, characterElements.leftWeapon, characterElements.rightWeapon);

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
            var statDamage = stats.damage;
            var statSpeed = stats.speed;
            var statHp = stats.hp;
            statDamage.baseValue = Mathf.Round(baseClassPointPerLevel * tendencyPoint.damagePoint);
            statSpeed.baseValue = Mathf.Round(baseClassPointPerLevel * tendencyPoint.speedPoint);
            statHp.baseValue = baseClassPointPerLevel - (stats.damage.baseValue + stats.speed.baseValue); //Mathf.Round(basePointPerLevel * tendencyPoint.hpPoint);
            baseClass.level = baseLevel;
            stats.TransformValues();

            // presentation of description
            // var pHealth = Mathf.FloorToInt(stats.currentHealth);
            // var pMaxHealth = Mathf.FloorToInt(stats.maxHealth.GetValue());
            // var desc = string.Format(descFormat, baseJob.label, baseClass.label, baseLevel, pHealth, pMaxHealth);
            // characterDescription.text = desc;

            // presentation of info
            block.jobLabel.text = baseJob.label.ToString();
            classImage.sprite = GetClassSpriteByLabel(baseClass.label);
            block.level.text = baseLevel.ToString();

            // presentation of stats
            block.damage.text = string.Format("Damage ({0})", statDamage.GetValue());
            block.hp.text = string.Format("HP ({0})", statHp.GetValue());
            block.speed.text = string.Format("Speed ({0})", statSpeed.GetValue());

            // generate sprite of characters
            GenerateSpriteCharacters(currentGeneratedBaseCharacter, baseClass.label);

            // generate posture by job
            GeneratePostureByJob(currentGeneratedBaseCharacter, baseJob.label);

            block.goldText.text = GenerateGoldByClass(baseClass.label, baseGoldAmount).ToString();
            block.diamondText.text = GenerateDiamondByClass(baseClass.label, baseDiamondAmount).ToString();

            // release memory
            currentGeneratedBaseCharacter = null;
            tendencyPoint = null;
        }
        blocks = null;
    }

    public void Generate()
    {
        if (gold < amountOfCall)
        {
            Debug.Log("You have not enough amount for calling");
            return;
        }
        generatedButton.interactable = false;
        gold -= amountOfCall;
        StartCoroutine(Generating());
        amountOfCall = Mathf.FloorToInt(amountOfCall * baseAmountOnTime);
        amountForCallText.text = amountOfCall.ToString();
    }

    void GeneratePostureByJob(GeneratedBaseCharacter character, JobLabel jobLabel)
    {
        var idle = string.Format("{0}_Idle", jobLabel);
        character.animator.Play(idle);
    }

    void GenerateSpriteCharacters(GeneratedBaseCharacter character, ClassLabel classLabel)
    {
        var pattern = "{0} => {1}";

        var generatedAClassLabels = new[] { generatedCommonClassLabel, generatedAClassLabel };
        var generatedSClassLabels = new[] { generatedSClassLabel, generatedAClassLabel };

        var realHeadSpriteLoc = string.Empty;
        var realEyeSpriteLoc = string.Empty;
        var realMouthSpriteLoc = string.Empty;
        var realBodySpriteLoc = string.Empty;
        var realArmSpriteLoc = string.Empty;
        var realLegSpriteLoc = string.Empty;

        if (classLabel == ClassLabel.B || classLabel == ClassLabel.C)
        {
            realHeadSpriteLoc = string.Format(headSpriteLoc, generatedCommonClassLabel);
            realEyeSpriteLoc = string.Format(eyeSpriteLoc, generatedCommonClassLabel);
            realMouthSpriteLoc = string.Format(mouthSpriteLoc, generatedCommonClassLabel);
            realBodySpriteLoc = string.Format(bodySpriteLoc, generatedCommonClassLabel);
            realArmSpriteLoc = string.Format(armSpriteLoc, generatedCommonClassLabel);
            realLegSpriteLoc = string.Format(legSpriteLoc, generatedCommonClassLabel);
        }
        else if (classLabel == ClassLabel.A)
        {
            var rIndex = Random.Range(0, generatedAClassLabels.Length);
            realHeadSpriteLoc = string.Format(headSpriteLoc, generatedAClassLabels[rIndex]);
            rIndex = Random.Range(0, generatedAClassLabels.Length);
            realEyeSpriteLoc = string.Format(eyeSpriteLoc, generatedAClassLabels[rIndex]);
            rIndex = Random.Range(0, generatedAClassLabels.Length);
            realMouthSpriteLoc = string.Format(mouthSpriteLoc, generatedAClassLabels[rIndex]);
            rIndex = Random.Range(0, generatedAClassLabels.Length);
            realBodySpriteLoc = string.Format(bodySpriteLoc, generatedAClassLabels[rIndex]);
            realArmSpriteLoc = string.Format(armSpriteLoc, generatedAClassLabels[rIndex]);
            rIndex = Random.Range(0, generatedAClassLabels.Length);
            realLegSpriteLoc = string.Format(legSpriteLoc, generatedAClassLabels[rIndex]);
        }
        else
        {
            var rIndex = Random.Range(0, generatedSClassLabels.Length);
            realHeadSpriteLoc = string.Format(headSpriteLoc, generatedSClassLabels[rIndex]);
            rIndex = Random.Range(0, generatedSClassLabels.Length);
            realEyeSpriteLoc = string.Format(eyeSpriteLoc, generatedSClassLabels[rIndex]);
            rIndex = Random.Range(0, generatedSClassLabels.Length);
            realMouthSpriteLoc = string.Format(mouthSpriteLoc, generatedSClassLabels[rIndex]);
            rIndex = Random.Range(0, generatedSClassLabels.Length);
            realBodySpriteLoc = string.Format(bodySpriteLoc, generatedSClassLabels[rIndex]);
            realArmSpriteLoc = string.Format(armSpriteLoc, generatedSClassLabels[rIndex]);
            rIndex = Random.Range(0, generatedSClassLabels.Length);
            realLegSpriteLoc = string.Format(legSpriteLoc, generatedSClassLabels[rIndex]);
        }

        countOfHeadSprite = spriteHelper.Count(realHeadSpriteLoc);
        countOfEyeSprite = spriteHelper.Count(realEyeSpriteLoc);
        countOfMouthSprite = spriteHelper.Count(realMouthSpriteLoc);
        countOfBodySprite = spriteHelper.Count(realBodySpriteLoc);
        countOfLegSprite = spriteHelper.Count(realLegSpriteLoc) / 2;

        var headIndex = Random.Range(0, countOfHeadSprite);
        var eyeIndex = Random.Range(0, countOfEyeSprite);
        var mouthIndex = Random.Range(0, countOfMouthSprite);
        var bodyIndex = Random.Range(0, countOfBodySprite);
        var legIndex = Random.Range(0, countOfLegSprite);

        var headSprite = spriteHelper.Get(string.Format(pattern, realHeadSpriteLoc, headIndex));
        var eyeSprite = spriteHelper.Get(string.Format(pattern, realEyeSpriteLoc, eyeIndex));
        var mouthSprite = spriteHelper.Get(string.Format(pattern, realMouthSpriteLoc, mouthIndex));
        var bodySprite = spriteHelper.Get(string.Format(pattern, realBodySpriteLoc, bodyIndex));
        var leftArmSprite = spriteHelper.Get(string.Format(pattern, realArmSpriteLoc, "l_" + bodyIndex));
        var rightArmSprite = spriteHelper.Get(string.Format(pattern, realArmSpriteLoc, "r_" + bodyIndex));
        var leftLegSprite = spriteHelper.Get(string.Format(pattern, realLegSpriteLoc, "l_" + legIndex));
        var rightLegSprite = spriteHelper.Get(string.Format(pattern, realLegSpriteLoc, "r_" + legIndex));

        // var instanceOfBaseCharacter = Instantiate<GeneratedBaseCharacter>(generatedBaseCharacterPrefab, Vector3.zero, Quaternion.identity);
        // _generatedCharaters.Add(instanceOfBaseCharacter);

        var characterElements = character.elements;
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
    }

    void AssignWeaponByJob(JobLabel jobLabel, SpriteRenderer leftWeaponRenderer, SpriteRenderer rightWeaponRenderer)
    {
        var pattern = "{0} => {1}_{2}";
        Sprite sprite;
        switch (jobLabel)
        {
            default:
            case JobLabel.Swordman:
                sprite = spriteHelper.Get(string.Format(pattern, weaponSpriteLoc, "sword", 0));
                leftWeaponRenderer.sortingOrder = 1;
                break;
            case JobLabel.Archer:
                sprite = spriteHelper.Get(string.Format(pattern, weaponSpriteLoc, "bow", 0));
                leftWeaponRenderer.flipX = true;
                leftWeaponRenderer.sortingOrder = 1;
                break;
            case JobLabel.Mage:
                sprite = spriteHelper.Get(string.Format(pattern, weaponSpriteLoc, "staff", 0));
                leftWeaponRenderer.flipX = true;
                leftWeaponRenderer.sortingOrder = 1;
                break;
            case JobLabel.Healer:
                sprite = spriteHelper.Get(string.Format(pattern, weaponSpriteLoc, "book", 0));
                leftWeaponRenderer.sortingOrder = 5;
                break;
        }
        leftWeaponRenderer.sprite = sprite;
        rightWeaponRenderer.sprite = null;
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

    public Sprite GetClassSpriteByLabel(ClassLabel label)
    {
        var pattern = "{0} => {1}";
        switch (label)
        {
            case ClassLabel.A:
                return spriteHelper.Get(string.Format(pattern, recruitUILoc, "a-class"));
            case ClassLabel.B:
                return spriteHelper.Get(string.Format(pattern, recruitUILoc, "b-class"));
            case ClassLabel.C:
                return spriteHelper.Get(string.Format(pattern, recruitUILoc, "c-class"));
            case ClassLabel.S:
                return spriteHelper.Get(string.Format(pattern, recruitUILoc, "s-class"));
            default:
                break;
        }
        return null;
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
                point = (int)Random.Range(sPointPerLevel.min, sPointPerLevel.max + 1);
                break;
            case ClassLabel.A:
                point = (int)Random.Range(aPointPerLevel.min, aPointPerLevel.max + 1);
                break;
            case ClassLabel.B:
                point = (int)Random.Range(bPointPerLevel.min, bPointPerLevel.max + 1);
                break;
            case ClassLabel.C:
                point = (int)Random.Range(cPointPerLevel.min, cPointPerLevel.max + 1);
                break;
            default:
                break;
        }
        return point;
    }

    public int GenerateGoldByClass(ClassLabel classLabel, int baseAmount)
    {
        var gold = 0;
        switch (classLabel)
        {
            case ClassLabel.S:
                gold = baseAmount * Random.Range(5, 10);
                break;
            case ClassLabel.A:
                gold = baseAmount * Random.Range(2, 4);
                break;
            case ClassLabel.B:
                gold = baseAmount * Random.Range(1, 2);
                break;
            case ClassLabel.C:
                gold = baseAmount * Random.Range(1, 1);
                break;
            default:
                break;
        }
        return gold;
    }

    public int GenerateDiamondByClass(ClassLabel classLabel, int baseAmount)
    {
        var amount = 0;
        switch (classLabel)
        {
            case ClassLabel.S:
                amount = baseAmount * Random.Range(5, 10);
                break;
            case ClassLabel.A:
                amount = baseAmount * Random.Range(2, 4);
                break;
            case ClassLabel.B:
                amount = baseAmount * Random.Range(1, 2);
                break;
            case ClassLabel.C:
                amount = baseAmount * Random.Range(1, 1);
                break;
            default:
                break;
        }
        return amount;
    }
}