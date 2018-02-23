using UnityEngine;

public enum ClassLabel
{
    S, A, B, C
}

public enum JobLabel
{
    Swordman, Archer, Mage
}

[System.Serializable]
public class BaseCharacterElement
{
    public SpriteRenderer head;
    public SpriteRenderer eye;
    public SpriteRenderer mouth;
    public SpriteRenderer body;
    public SpriteRenderer leftArm;
    public SpriteRenderer rightArm;
    public SpriteRenderer leftLeg;
    public SpriteRenderer rightLeg;
}

public class GeneratedBaseCharacter : MonoBehaviour
{
    public BaseCharacterElement elements;

    [System.NonSerialized]
    public int id;

    public JobLabel jobLabel;
    public ClassLabel classLabel;

    ClassLabel[] _cachedClassLabels;
    JobLabel[] _cachedJobLabels;

    void Awake()
    {
        id = transform.GetInstanceID();
        GenerateClass();
        jobLabel = JobLabel.Swordman;
    }

    public void GenerateClass(float sPercent = .01f, float aPercent = .05f, float bPercent = .6f, float cPercent = .34f)
    {
        if(_cachedClassLabels == null || _cachedClassLabels.Length == 0){
            var classLabels = new[] { ClassLabel.S, ClassLabel.A, ClassLabel.B, ClassLabel.C };
            var percents = new[] { sPercent * 100, aPercent * 100, bPercent * 100, cPercent * 100 };
            _cachedClassLabels = Probability.Initialize(classLabels, percents);
        }
        classLabel = Probability.GetValueInProbability(_cachedClassLabels);
    }

    public void GenerateJob(float swordmanPercent = 1/3f, float archerPercent = 1/3f, float magePercent = 1/3f)
    {
        if(_cachedJobLabels == null || _cachedJobLabels.Length == 0){
            var jobLabels = new[] { JobLabel.Swordman, JobLabel.Archer, JobLabel.Mage };
            var percents = new[] { swordmanPercent * 100, archerPercent * 100, magePercent * 100 };
            _cachedJobLabels = Probability.Initialize(jobLabels, percents);
        }
        jobLabel = Probability.GetValueInProbability(_cachedJobLabels);
    }
}