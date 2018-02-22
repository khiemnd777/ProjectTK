using UnityEngine;

public class CalculatorUtility
{
    public static float TimeByFrame(float frameNumber, float frameRate)
    {
        return (1f / frameRate) * frameNumber;
    }

    public static float FrameByTime(float time, float frameRate)
    {
        return time * frameRate;
    }

    public static Vector3 LerpByDistance(Vector3 a, Vector3 b, float x)
    {
        var p = x * Vector3.Normalize(b - a) + a;
        return p;
    }

    public static float Snap(float value, float snapDelta)
    {
        var valApartSnap = value / snapDelta;
        var valRound = value < 0 ? Mathf.Ceil(valApartSnap) : Mathf.Floor(valApartSnap);
        return valRound * snapDelta + Mathf.Round((value % snapDelta) / snapDelta) * snapDelta;
    }
}