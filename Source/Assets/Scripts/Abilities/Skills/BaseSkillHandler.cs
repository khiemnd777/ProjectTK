using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkillHandler : MonoBehaviour
{
	public virtual ActionInfo DoAction()
    {
        return default(ActionInfo);
    }
}
