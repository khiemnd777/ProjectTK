﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroSkillHandler : BaseSkillHandler
{
    public BaseSkill[] baseSkills;

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
    BaseCharacter currentOpponent;
    BaseSkill currentSkill;
    GeneratedBaseCharacter _baseCharacter;
    Vector3 _originalPosition;

    void Start()
    {
        _baseCharacter = GetComponent<GeneratedBaseCharacter>();
    }

    public override ActionInfo DoAction()
    {
        _opponents = null;
        if (!baseSkills.Any())
            return new ActionInfo
            {
                time = 0f
            };
        var skill = currentSkill = baseSkills.FirstOrDefault();
        if (skill == null || skill is Object && skill.Equals(null))
            return new ActionInfo
            {
                time = 0f
            };
        _opponents = skill.DetermineOpponents();
        // Execute the skill
        skill.Execute(animator, _baseCharacter);
        return new ActionInfo
        {
            time = skill.GetLength()
        };
    }

    #region Animation Events
    public override void EventMoveToOpponent(AnimationEvent animEvent)
    {
        Event_MoveToOpponent(animEvent);
    }

    public override void Event_MoveToOpponent(AnimationEvent animEvent)
    {
        _originalPosition = transform.position;
        if (!_opponents.Any())
            return;
        currentOpponent = _opponents[animEvent.intParameter];
        if (currentOpponent == null || currentOpponent is Object && currentOpponent.Equals(null))
            return;
        var frameRate = animEvent.animatorClipInfo.clip.frameRate;
        var length = CalculatorUtility.TimeByFrame(animEvent.floatParameter, frameRate);
        StartCoroutine(TransformUtility.MoveToTarget(transform, _originalPosition, currentOpponent.impactPoint.transform.position, length));
    }

    public override void EventMoveBack(AnimationEvent animEvent)
    {
        Event_MoveBack(animEvent);
    }

    public override void Event_MoveBack(AnimationEvent animEvent)
    {
        if (currentOpponent == null || currentOpponent is Object && currentOpponent.Equals(null))
            return;
        var frameRate = animEvent.animatorClipInfo.clip.frameRate;
        var length = CalculatorUtility.TimeByFrame(animEvent.floatParameter, frameRate);
        StartCoroutine(TransformUtility.MoveToTarget(transform, currentOpponent.impactPoint.transform.position, _originalPosition, length));
    }

    public override void Event_ActivateEffect(AnimationEvent animEvent)
    {
        if(currentSkill == null || currentSkill is Object && currentSkill.Equals(null))
            return;
        currentSkill.ActivateEffect(animEvent.stringParameter, _baseCharacter);
    }
    #endregion
}