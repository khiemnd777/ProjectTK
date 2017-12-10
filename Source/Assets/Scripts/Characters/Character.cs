using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    new public string name = "New Character";
    public float health;
    public float maxHealth;
    [Space]
    public Transform model;
    [Space]
    public Sprite icon;
    [Space]
    [Header("Stats")]
    public float dexterity;
    [Space]
    [Header("Animation")]
    public RuntimeAnimatorController animatorController;
    [Space]
    public int slot;
    public bool isDeath;
    public bool isEnemy;
    public bool isTurn;
    public List<Skill> skills = new List<Skill>();
    public List<Skill> learnedSkills = new List<Skill>();
    public List<Tactical> tactics = new List<Tactical>();
    public List<Ability> abilities = new List<Ability>();

    public delegate void OnAbilityHandled(Character character);
    public OnAbilityHandled onAbilityHandledCallback;

    public void AddSkill(Skill skill)
    {
        skills.Add(skill);
    }

    public void Setup(){
        // Clear all list before sets it up
        ClearAllLearnedSkills();
        ClearAllTactics();
        // Assigns animator controller
        var animator = model.GetComponentInChildren<Animator>();
        if(!animator.IsNull()){
            animator.runtimeAnimatorController = animatorController;
        }
    }

    public Skill LearnSkill(Skill skill)
    {
        var learnedSkill = Instantiate<Skill>(skill, Vector3.zero, Quaternion.identity, transform);
        // default tactic is always in tactic list
        var defaultTactic = Instantiate<Tactical>(learnedSkill.defaultTacticPrefab, Vector3.zero, Quaternion.identity);
        learnedSkill.character = this;
        // add default tactic into list
        learnedSkill.AddTactic(defaultTactic);
        learnedSkills.Add(learnedSkill);
        return learnedSkill;
    }

    public void ClearAllLearnedSkills()
    {
        learnedSkills.Clear();
    }

    public void AddTactic(Tactical tactic)
    {
        tactics.Add(tactic);
    }

    public void ClearAllTactics()
    {
        tactics.Clear();
    }

    public void AddAbility(Ability ability)
    {
        abilities.Add(ability);
    }

    public void ClearAllAbilities()
    {
        abilities.Clear();
    }

    public IEnumerator HandleAbilities(MarathonRunner marathonRunner)
    {
        var owner = this;
        var validAbilities = owner.abilities
            .OrderBy(x => x.displayOrder)
            .Where(x =>
                x.tactics
                    .OrderBy(x1 => x1.displayOrder)
                    .Any(x1 => x1.Define()))
            .ToArray();

        var isNonDefaultUsed = false;
        var singleAbility = validAbilities.FirstOrDefault(x => !x.isDefault);
        if (!singleAbility.IsNull())
        {
            var tactics = singleAbility.tactics.OrderBy(x => x.displayOrder).Where(x => x.Define());
            var singleTactic = tactics.FirstOrDefault(x => !x.isDefault) ?? tactics.FirstOrDefault(x => x.isDefault);
            var characterRunner = marathonRunner.GetCharacterRunner(this);
            characterRunner.RunOnActionRoad(singleAbility.deltaWaitingTime);
            yield return StartCoroutine(singleAbility.Use(new AbilityUsingParams
            {
                tactic = singleTactic,
                marathonRunner = marathonRunner
            }));
            singleAbility.StopCoroutine("Use");
            isNonDefaultUsed = true;
            characterRunner = null;
        }

        if (!isNonDefaultUsed)
        {
            singleAbility = validAbilities.FirstOrDefault(x => x.isDefault);
            if (!singleAbility.IsNull())
            {
                var tactics = singleAbility.tactics.OrderBy(x => x.displayOrder).Where(x => x.Define());
                var singleTactic = tactics.FirstOrDefault(x => !x.isDefault) ?? tactics.FirstOrDefault(x => x.isDefault);
                var characterRunner = marathonRunner.GetCharacterRunner(this);
                characterRunner.RunOnActionRoad(singleAbility.deltaWaitingTime);
                yield return StartCoroutine(singleAbility.Use(new AbilityUsingParams
                {
                    tactic = singleTactic,
                    marathonRunner = marathonRunner
                }));
                singleAbility.StopCoroutine("Use");
                characterRunner = null;
            }
        }

        if (owner.onAbilityHandledCallback != null)
            owner.onAbilityHandledCallback.Invoke(owner);

        marathonRunner.StartSingleRunner(owner);

        validAbilities = null;
        singleAbility = null;

        owner.StopCoroutine("OnAbilityHandling");
        owner = null;
    }
}