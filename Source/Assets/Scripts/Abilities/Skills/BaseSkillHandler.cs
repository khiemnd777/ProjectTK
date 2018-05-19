using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillHandler : MonoBehaviour
{
    public BaseSkill[] baseSkills;

    public virtual ActionInfo DoAction()
    {
        return default(ActionInfo);
    }

    public virtual void MoveToOpponentEvent(int index, float length)
    {
        
    }

    public virtual void MoveBackEvent()
    {

    }
}
