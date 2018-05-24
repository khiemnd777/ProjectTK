using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillHandler : MonoBehaviour
{
    public virtual ActionInfo DoAction()
    {
        return default(ActionInfo);
    }

    [System.Obsolete("using Event_MoveToOpponent instead")]
    public virtual void EventMoveToOpponent(AnimationEvent animEvent)
    {
        Event_MoveToOpponent(animEvent);
    }

    public virtual void Event_MoveToOpponent(AnimationEvent animEvent)
    {
        
    }

    [System.Obsolete("using Event_MoveBack instead")]
    public virtual void EventMoveBack(AnimationEvent animEvent)
    {
        Event_MoveBack(animEvent);
    }

    public virtual void Event_MoveBack(AnimationEvent animEvent)
    {

    }

    public virtual void Event_ActivateEffect(AnimationEvent animEvent)
    {
        Event_ActivateFx(animEvent);
    }

    public virtual void Event_ActivateFx(AnimationEvent animEvent)
    {

    }

    public virtual void Event_MoveFxToOpponent(AnimationEvent animEvent)
    {

    }
}
