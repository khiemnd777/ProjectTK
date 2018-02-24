using UnityEngine;

[System.Serializable]
public class BaseClass
{
    public int level;
    public int basePointPerLevel;
    public int pointPerLevel;
    public ClassLabel label;

    public void AssignBasePointPerLevel()
    {
        var point = 0;
        switch (label)
        {
            case ClassLabel.S:
                point = 5;
                break;
            case ClassLabel.A:
                point = Random.Range(4, 6);
                break;
            case ClassLabel.B:
                point = Random.Range(3, 5);
                break;
            case ClassLabel.C:
                point = Random.Range(2, 4);
                break;
            default:
                break;
        }
        basePointPerLevel = point;
    }
}