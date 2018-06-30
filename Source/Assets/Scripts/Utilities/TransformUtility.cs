using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUtility
{
    public static IEnumerator MoveToTarget(Transform transform, Vector3 start, Vector3 end, float runningTime, System.Action callback = null)
    {
        var percent = 0f;
        while (percent <= 1f)
        {
            percent += Time.deltaTime / runningTime;
            transform.position = Vector3.Lerp(start, end, percent);
            yield return null;
        }
        if(callback != null){
            callback();
        }
    }
}