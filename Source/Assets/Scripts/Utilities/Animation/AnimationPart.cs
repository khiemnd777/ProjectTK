using UnityEngine;

[System.Serializable]
public class AnimationPart
{
    public AnimationClip animation;
    public float startFrame;
    public float endFrame;
    public AnimationPartEvent[] events;
}