using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatorUtility
{
    public static void ActiveLayer(Animator animator, string layerName)
    {
        for (var i = 0; i < animator.layerCount; i++)
        {
            animator.SetLayerWeight(i, 0);
        }
        animator.SetLayerWeight(animator.GetLayerIndex(layerName), 1);
    }
}