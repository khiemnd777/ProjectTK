using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroSkillHandler : BaseSkillHandler
{
    // Animator component
    Animator _animator;
    public Animator animator
    {
        get
        {
            return _animator ?? (_animator = GetComponent<Animator>());
        }
    }

    BaseCharacter[] _opponents;

	public override ActionInfo DoAction()
    {
        _opponents = null;
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
        _opponents = skill.DetermineOpponents();
        // Execute the skill
        skill.Execute(animator);
        return new ActionInfo
        {
            time = skill.GetLength()
        };
    }

    public override void MoveToOpponentEvent(int index, float length)
    {
        if(!_opponents.Any())
            return;
        var opponent = _opponents[index];
        if(opponent == null || opponent is Object && opponent.Equals(null))
            return;
        TransformUtility.MoveToTarget(transform, transform.position, opponent.transform.position, length);
    }

    public override void MoveBackEvent()
    {

    }
}
