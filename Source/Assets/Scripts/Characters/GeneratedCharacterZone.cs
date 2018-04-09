using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GeneratedCharacterBlock
{
    public GeneratedBaseCharacter baseCharacter;
    public TendencyPoint tendencyPoint;
    public Text name;
    public Text description;
    public Text level;
    public Text jobLabel;
    public Image classImage;
    public TextMesh damage;
    public TextMesh hp;
    public TextMesh speed;
    public Button goldButton;
    public Text goldText;
    public Button diamondButton;
    public Text diamondText;
    public bool clickedOnGoldButton;
    public bool clickedOnDiamondButton;
    public Transform recruitedMark;
    public ParticleSystem effectStarBurst;
}