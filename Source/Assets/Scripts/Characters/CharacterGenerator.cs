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

    bool isCalling;

    List<string> _listOfCharacterName;
    Transform _goldRes;
    Transform _diamondRes;
    ReduceAmountMotion _reduceAmountMotionRes;

    void Awake()
    {
        spriteHelper = SpriteHelper.instance;
        nameGenerator = CharacterNameGenerator.instance;
        // Init resources of gold and diamond
        _goldRes = Resources.Load<Transform>("Prefabs/Shared/Gold");
        _diamondRes = Resources.Load<Transform>("Prefabs/Shared/Diamond");
        _reduceAmountMotionRes = Resources.Load<ReduceAmountMotion>("Prefabs/Shared/Reduce Amount Motion");
    }

    void Start()
    {
        StartCoroutine(LoadResourcesAtFirstTime());
        amountForCallText.text = amountOfCall.ToString();
        FillElementSkinColor(Color.black);
        StartCoroutine(MainPanelShowing());
        tavernBanner.anchoredPosition = new Vector2(tavernBanner.anchoredPosition.x, 193f);
        // Register Gold & Diamond Buttons
        RegisterGoldButtons();
        RegisterDiamondButtons();
    }

    void Update()
    {
        // goldText.text = gold.ToString();
        diamondText.text = diamond.ToString();
        CheckInteractableButtons();
    }

    IEnumerator LoadResourcesAtFirstTime()
    {
        var pattern = "{0} => {1}";
        spriteHelper.Get(string.Format(pattern, string.Format(headSpriteLoc, "A"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(eyeSpriteLoc, "A"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(mouthSpriteLoc, "A"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(bodySpriteLoc, "A"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(armSpriteLoc, "A"), "l_" + 0));
        spriteHelper.Get(string.Format(pattern, string.Format(armSpriteLoc, "A"), "r_" + 0));
        spriteHelper.Get(string.Format(pattern, string.Format(legSpriteLoc, "A"), "l_" + 0));
        spriteHelper.Get(string.Format(pattern, string.Format(legSpriteLoc, "A"), "r_" + 0));
        
        spriteHelper.Get(string.Format(pattern, string.Format(headSpriteLoc, "S"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(eyeSpriteLoc, "S"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(mouthSpriteLoc, "S"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(bodySpriteLoc, "S"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(armSpriteLoc, "S"), "l_" + 0));
        spriteHelper.Get(string.Format(pattern, string.Format(armSpriteLoc, "S"), "r_" + 0));
        spriteHelper.Get(string.Format(pattern, string.Format(legSpriteLoc, "S"), "l_" + 0));
        spriteHelper.Get(string.Format(pattern, string.Format(legSpriteLoc, "S"), "r_" + 0));

        spriteHelper.Get(string.Format(pattern, string.Format(headSpriteLoc, "Common"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(eyeSpriteLoc, "Common"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(mouthSpriteLoc, "Common"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(bodySpriteLoc, "Common"), 0));
        spriteHelper.Get(string.Format(pattern, string.Format(armSpriteLoc, "Common"), "l_" + 0));
        spriteHelper.Get(string.Format(pattern, string.Format(armSpriteLoc, "Common"), "r_" + 0));
        spriteHelper.Get(string.Format(pattern, string.Format(legSpriteLoc, "Common"), "l_" + 0));
        spriteHelper.Get(string.Format(pattern, string.Format(legSpriteLoc, "Common"), "r_" + 0));
        yield return null;
    }

    #region Check Interactable Buttons
    void CheckInteractableButtons()
    {
        // Updating gold and diamond's amount
        CheckInteractableGoldButton(generatedCharaterBlocks[0]);
        CheckInteractableGoldButton(generatedCharaterBlocks[1]);
        CheckInteractableGoldButton(generatedCharaterBlocks[2]);

        CheckInteractableDiamondButton(generatedCharaterBlocks[0]);
        CheckInteractableDiamondButton(generatedCharaterBlocks[1]);
        CheckInteractableDiamondButton(generatedCharaterBlocks[2]);

        generatedButton.interactable = !isCalling && gold >= amountOfCall;
    }

    void CheckInteractableGoldButton(GeneratedCharacterBlock block)
    {
        var gold = System.Convert.ToInt32(block.goldText.text);
        if(this.gold >= gold)
            block.goldButton.interactable = !block.clickedOnGoldButton;
        else
            block.goldButton.interactable = false;
    }

    void CheckInteractableDiamondButton(GeneratedCharacterBlock block)
    {
        var diamond = System.Convert.ToInt32(block.diamondText.text);
        if(this.diamond >= diamond)
            block.diamondButton.interactable = !block.clickedOnDiamondButton;
        else
            block.diamondButton.interactable = false;
    }
    #endregion

    #region Register Gold & Diamond Buttons
    void RegisterGoldButtons()
    {
        var blocks = generatedCharaterBlocks;
        blocks[0].goldButton.onClick.AddListener(() => {
            RegisterGoldButton(generatedCharaterBlocks[0]);
        });

        blocks[1].goldButton.onClick.AddListener(() => {
            RegisterGoldButton(generatedCharaterBlocks[1]);
        });

        blocks[2].goldButton.onClick.AddListener(() => {
            RegisterGoldButton(generatedCharaterBlocks[2]);
        });
        blocks = null;
    }

    void RegisterGoldButton(GeneratedCharacterBlock block)
    {
        var gold = System.Convert.ToInt32(block.goldText.text);
        if(this.gold < gold){
            block.goldButton.interactable = false;
            return;
        }
        this.gold -= gold;
        StartCoroutine(RunAmountToDestination(goldText, this.gold));
        StartCoroutine(SelectCharacter(block.baseCharacter));
        // StartCoroutine(SelectiveEffect(block.baseCharacter));
        StartCoroutine(EffectReduceGolds(goldText.transform, block.baseCharacter.transform, gold, block));
        block.goldButton.interactable = false;
        block.clickedOnGoldButton = block.clickedOnDiamondButton = true;
        CharacterList.instance.AddCharacter(block.baseCharacter);
    }

    void RegisterDiamondButtons()
    {
        var blocks = generatedCharaterBlocks;
        blocks[0].diamondButton.onClick.AddListener(() => {
            RegisterDiamondButton(generatedCharaterBlocks[0]);
        });

        blocks[1].diamondButton.onClick.AddListener(() => {
            RegisterDiamondButton(generatedCharaterBlocks[1]);
        });

        blocks[2].diamondButton.onClick.AddListener(() => {
            RegisterDiamondButton(generatedCharaterBlocks[2]);
        });
        blocks = null;
    }

    void RegisterDiamondButton(GeneratedCharacterBlock block)
    {
        var diamond = System.Convert.ToInt32(block.diamondText.text);
        if(this.diamond < diamond){
            block.diamondButton.interactable = false;
            return;
        }
        this.diamond -= diamond;
        StartCoroutine(RunAmountToDestination(diamondText, this.diamond));
        StartCoroutine(SelectCharacter(block.baseCharacter));
        // StartCoroutine(SelectiveEffect(block.baseCharacter));
        StartCoroutine(EffectReduceDiamonds(diamondText.transform, block.baseCharacter.transform, diamond, block));
        block.diamondButton.interactable = false;
        block.clickedOnGoldButton = block.clickedOnDiamondButton = true;
        CharacterList.instance.AddCharacter(block.baseCharacter);
    }

    IEnumerator SelectCharacter(GeneratedBaseCharacter character)
    {
        var percent = 0f;
        var characterTransform = character.transform;
        while(percent <= 1f){
            percent += Time.deltaTime * 15f;
            characterTransform.localScale = Vector3.Lerp(Vector3.one * 27, Vector3.one * 38, percent);
            yield return null;
        }
        percent = 0f;
        while(percent <= 1f)
        {
            percent += Time.deltaTime * 25f;
            characterTransform.localScale = Vector3.Lerp(Vector3.one * 38, Vector3.one * 27, percent);
            yield return null;
        }
    }

    IEnumerator SelectiveEffect(GeneratedBaseCharacter character)
    {
        StartCoroutine(SelectiveEffects(character));
        yield return new WaitForSeconds(.125f);
        StartCoroutine(SelectiveEffects(character));
    }

    IEnumerator SelectiveEffects(GeneratedBaseCharacter character)
    {
        var percent = 0f;
        var characterTransform = character.transform;
        var clone = Instantiate<GeneratedBaseCharacter>(character, characterTransform.position, characterTransform.rotation, characterTransform.parent);
        var cloneTransform = clone.transform.Find("Body");
        var characterElements = clone.elements;
        var characterEyes = characterElements.eye;
        var characterMouth = characterElements.mouth;
        var leftWeapon = characterElements.leftWeapon;
        var rightWeapon = characterElements.rightWeapon;
        cloneTransform.localScale = Vector3.one;
        cloneTransform.SetSiblingIndex(character.transform.GetSiblingIndex() - 1);
        cloneTransform.localScale = characterTransform.localScale;
        while(percent <= 1f)
        {
            percent += Time.deltaTime * 4.5f;
            cloneTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.375f, percent);
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
                = new Color32(255, 255, 255, (byte)Mathf.Lerp(255, 50, percent));
            yield return null;
        }
        Destroy(clone.gameObject);
    }
    #endregion
    
    #region Effect Reducing Gold & Diamond
    IEnumerator EffectReduceGolds(Transform pocket, Transform target, int amountGold, GeneratedCharacterBlock block)
    {
        var amount = Mathf.Min(amountGold / 10, 40);
        amount = amount < 1 ? 1 : amount;
        // init array of gold and position
        var golds = new Transform[amount];
        var positions = new Vector2[amount];
        for(var i = 0; i < amount; i++)
        {
            var gold = Instantiate<Transform>(_goldRes, pocket.position, Quaternion.identity);
            gold.transform.localScale = Vector2.one * .625f;
            golds[i] = gold;
        }
        for(var i = 0; i < golds.Length; i++){
            positions[i] = Random.insideUnitCircle * Random.Range(1, 2) + new Vector2(pocket.position.x, pocket.position.y);
        }
        // jump around target, then moving to destination
        for(var i = 0; i < golds.Length; i++)
        {
            StartCoroutine(JumpToDestination(golds[i], pocket.position, positions[i]
                , false, MoveToDestination(golds[i], target, true
                    , SelectiveEffects(block.baseCharacter)
                    , SealRecruitedMark(block, i == golds.Length - 1))));
            yield return new WaitForSeconds(.02f);
        }
        golds = null;
        positions = null;
    }

    IEnumerator EffectReduceDiamonds(Transform pocket, Transform target, int amountDiamond, GeneratedCharacterBlock block)
    {
        var amount = amountDiamond / 1;
        amount = amount < 1 ? 1 : amount;
        // init array of gold and position
        var golds = new Transform[amount];
        var positions = new Vector2[amount];
        for(var i = 0; i < amount; i++)
        {
            var gold = Instantiate<Transform>(_diamondRes, pocket.position, Quaternion.identity);
            gold.transform.localScale = Vector2.one * .625f;
            golds[i] = gold;
        }
        for(var i = 0; i < golds.Length; i++){
            positions[i] = Random.insideUnitCircle * Random.Range(1, 2) + new Vector2(pocket.position.x, pocket.position.y);
        }
        // jump around target, then moving to destination
        for(var i = 0; i < golds.Length; i++)
        {
            StartCoroutine(JumpToDestination(golds[i], pocket.position, positions[i]
                , false
                    , MoveToDestination(golds[i], target, true
                        , SelectiveEffects(block.baseCharacter)
                        , SealRecruitedMark(block, i == golds.Length - 1))));
            yield return new WaitForSeconds(.02f);
        }
        golds = null;
        positions = null;
    }
    #endregion

    #region Transform Utility
    IEnumerator JumpToDestination(Transform target, Vector3 source, Vector3 destination, bool destroy, params IEnumerator[] nextActions)
    {
        var percent = .0f;
        while(percent <= 1f){
            percent += Time.deltaTime * 2;
            var currentPos = Vector2.Lerp(source, destination, percent);
            var trajectoryHeight = Random.Range(.2f, .6f);
            currentPos.y += trajectoryHeight * Mathf.Sin(Mathf.Clamp01(percent) * Mathf.PI);
            target.position = currentPos;
            yield return null;
        }
        for(var i = 0; i < nextActions.Length; i++)
        {
            yield return StartCoroutine(nextActions[i]);
        }
        if(destroy)
            Destroy(target.gameObject);
    }

    IEnumerator MoveToDestination(Transform target, Transform destination, bool destroy, params IEnumerator[] nextActions)
    {
        var percent = .0f;
        while(percent <= 1f){
            percent += Time.deltaTime * 2;
            target.position = Vector2.MoveTowards(target.position, destination.position, percent);
            yield return null;
        }
        for(var i = 0; i < nextActions.Length; i++)
        {
            yield return StartCoroutine(nextActions[i]);
        }
        if(destroy)
            Destroy(target.gameObject);
    }
    #endregion

    IEnumerator SealRecruitedMark(GeneratedCharacterBlock block, bool executed)
    {
        if(!executed)
            yield break;
        block.recruitedMark.gameObject.SetActive(true);
        var percent = 0f;
        while(percent <= 1f)
        {
            percent += Time.deltaTime * 10f;
            block.recruitedMark.localScale = Vector3.Lerp(Vector3.one * 45, Vector3.one * 27, percent);
            yield return null;
        }
    }

    IEnumerator RunAmountToDestination(Text text, int destination, params IEnumerator[] nextActions)
    {
        var percent = 0f;
        while(percent <= 1f)
        {
            percent += Time.deltaTime * .75f;
            var source = System.Convert.ToInt32(text.text);
            source = (int) Mathf.Lerp(source, destination, percent);
            text.text = source.ToString();
            yield return null;
        }
        for(var i = 0; i < nextActions.Length; i++)
        {
            yield return StartCoroutine(nextActions[i]);
        }
        text.text = destination.ToString();
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

        // Showing tavern panel
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
        StartCoroutine(LerpingElementSkinColor(from , to, speed));
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
        isCalling = true;
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
            block.recruitedMark.gameObject.SetActive(false);
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
            block.effectStarBurst.Play();
            StartCoroutine(PlayParticleSystem(block.effectStarBurst, 1.2f));
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
        isCalling = false;

        var blocks = generatedCharaterBlocks;
        blocks[0].clickedOnGoldButton = 
        blocks[0].clickedOnDiamondButton = 
        blocks[1].clickedOnGoldButton = 
        blocks[1].clickedOnDiamondButton = 
        blocks[2].clickedOnGoldButton = 
        blocks[2].clickedOnDiamondButton = false;
        blocks = null;
    }

    IEnumerator PlayParticleSystem(ParticleSystem particleSystem, float seconds)
    {
        particleSystem.Play();
        yield return new WaitForSeconds(seconds);
        particleSystem.Stop();
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
            currentGeneratedBaseCharacter.characterName = genName;
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
            generatedButton.interactable = false; 
            return;
        }
        generatedButton.interactable = false;
        gold -= amountOfCall;

        var reduceAmountMotion = Instantiate<ReduceAmountMotion>(_reduceAmountMotionRes, goldText.transform.position, Quaternion.identity, goldText.transform);
        StartCoroutine(reduceAmountMotion.Reduce(goldText.transform, amountOfCall));

        StartCoroutine(RunAmountToDestination(goldText, gold));
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