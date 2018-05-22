using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillHandler : MonoBehaviour
{
    public virtual ActionInfo DoAction()
    {
        return default(ActionInfo);
    }

    [System.Obsolete]
    public virtual void EventMoveToOpponent(AnimationEvent animationEvent)
    {
        
    }

    public virtual void Event_MoveToOpponent(AnimationEvent animationEvent)
    {
        
    }

    [System.Obsolete]
    public virtual void EventMoveBack(AnimationEvent animEvent)
    {

    }

    public virtual void Event_MoveBack(AnimationEvent animEvent)
    {

    }
}
