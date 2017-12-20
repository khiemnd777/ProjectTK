using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public AnimationPart[] animations;

    void Start()
    {
        
    }

    void Update(){
        if(animations.Length > 0){
            Debug.Log((1f/animations[0].animation.frameRate) * 30);
        }
    }

    public void Play()
    {

    }

    public void Stop()
    {

    }
}