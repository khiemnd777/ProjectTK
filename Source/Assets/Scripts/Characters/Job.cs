
public enum ClassLabel
{
    S, A, B, C
}

public class Job
{
    public ClassLabel classLabel;

    public Job()
    {
        Init();
    }

    void Init()
    {
        var classLabels = new[] { ClassLabel.S, ClassLabel.A, ClassLabel.B, ClassLabel.C };
        var percents = new[] { .01f, .05f, .6f, .34f };
        var probabilities = Probability.Initialize(classLabels, percents);
        classLabel = Probability.GetValueInProbability(probabilities);
    }
}