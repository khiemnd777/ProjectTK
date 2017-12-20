
[System.Serializable]
public class AnimationPartEvent
{
    public string eventName;
    public float startFrame;
    public System.Action<float> action;
}