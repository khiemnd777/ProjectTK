using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    #region Singleton
    static CharacterInfoUI _instance;

    public static CharacterInfoUI instance
    {
        get
        {
            if (!_instance) 
            {
                _instance = FindObjectOfType<CharacterInfoUI>();
                if (!_instance)
                {
                    Debug.LogError("There needs to be one active SelectionCharacterUI script on a GameObject in your scene.");
                }
                else
                {

                }
            }
            return _instance;
        }
    }
    #endregion

	const string iconJobUILocation = "Sprites/UI/selection-ui";

	public Transform characterPerformancePodium;
	public Image jobIcon;
	public Text characterName;
	public Text characterClass;
	public Text characterLevel;
	public Text characterAttackNumber;
	public Text characterSpeedNumber;
	public Text healthbarNumber;

	GeneratedBaseCharacter clonedCharacter;
	SpriteHelper _spriteHelper;

	void Start()
	{
		_spriteHelper = SpriteHelper.instance;
	}

	public void PerformCharacter(GeneratedBaseCharacter character)
	{
		// destroy cloned character before an instantiating
		if(clonedCharacter != null && clonedCharacter is Object && !clonedCharacter.Equals(null))
			DestroyImmediate(clonedCharacter.gameObject);
		// Perform on the podium
		clonedCharacter = Instantiate<GeneratedBaseCharacter>(character, Vector3.zero, Quaternion.identity, characterPerformancePodium);
		clonedCharacter.transform.localPosition = new Vector3(0f, 80f, 0);
		clonedCharacter.transform.localScale = Vector3.one * 23f;
		clonedCharacter.gameObject.SetActive(true);
		// Display character's informations
		jobIcon.sprite = GetJobIconSprite(clonedCharacter.baseJob.label);
		characterName.text = clonedCharacter.characterName;
		characterClass.text = clonedCharacter.baseClass.label.ToString();
		characterLevel.text = clonedCharacter.baseClass.level.ToString();
		characterAttackNumber.text = clonedCharacter.stats.damage.GetValue().ToString();
		characterSpeedNumber.text = clonedCharacter.stats.speed.GetValue().ToString();
		healthbarNumber.text  = Mathf.RoundToInt(clonedCharacter.stats.currentHealth) + "/" + Mathf.RoundToInt(clonedCharacter.stats.maxHealth.GetValue());
	}

	Sprite GetJobIconSprite(JobLabel label)
    {
        var pattern = "{0} => {1}";
        switch (label)
        {
            case JobLabel.Swordman:
                return _spriteHelper.Get(string.Format(pattern, iconJobUILocation, "icon_swordman"));
            case JobLabel.Archer:
                return _spriteHelper.Get(string.Format(pattern, iconJobUILocation, "icon_archer"));
            case JobLabel.Healer:
                return _spriteHelper.Get(string.Format(pattern, iconJobUILocation, "icon_healer"));
            case JobLabel.Mage:
                return _spriteHelper.Get(string.Format(pattern, iconJobUILocation, "icon_mage"));
            default:
                break;
        }
        return null;
    }
}