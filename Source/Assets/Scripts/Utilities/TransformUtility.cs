using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUtility
{
    public static IEnumerator MoveToTarget(Transform transform, Vector3 start, Vector3 end, float runningTime)
    {
        var percent = 0f;
        while (percent <= 1f)
        {
            transform.position = Mathfx.Sinerp(start, end, percent);
            percent += Time.deltaTime / runningTime;
            yield return null;
        }
    }
}