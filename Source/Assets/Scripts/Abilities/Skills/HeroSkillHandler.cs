using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroSkillHandler : BaseSkillHandler
{
	public BaseSkill[] baseSkills;

	public override ActionInfo DoAction()
    {
        if (!baseSkills.Any())
            return new ActionInfo
            {
                time = 0f
            };
        var skill = baseSkills.FirstOrDefault();
        if (skill == null || skill is Object && skill.Equals(null))
            return new ActionInfo
            {
                time = 0f
            };
        // Execute the skill
        skill.Execute();
        return new ActionInfo
        {
            time = skill.GetLength()
        };
    }
}
