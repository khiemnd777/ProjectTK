using UnityEngine;

[System.Serializable]
public class AnimationPartEffect
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
    [Space]
    public GameObject effectObject;
    public float startFrame;
}