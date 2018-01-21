using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    new public string name = "New Character";
    [Space]
    public Transform model;
    [Space]
    public Sprite icon;
    [Space]
    [Header("Animation")]
    public RuntimeAnimatorController animatorController;
    [Space]
    public int slot;
    public bool flip;
    public bool isDeath;
    public bool isEnemy;
    public bool isTurn;
    [Space]
    public List<Skill> skills = new List<Skill>();
    public List<Skill> learnedSkills = new List<Skill>();
    public List<Tactical> tactics = new List<Tactical>();
    public List<Ability> abilities = new List<Ability>();

    public delegate void OnAbilityHandled(Character character);
    public OnAbilityHandled onAbilityHandledCallback;

    Animator _animator;

    public Animator animator
    {
        get
        {
            return _animator ?? (_animator = model.GetComponentInChildren<Animator>());
        }
    }

    void Awake(){
        Flip();
    }

    public void AddSkill(Skill skill)
    {
        skills.Add(skill);
    }

    public void Setup()
    {
        // Clear all list before sets it up
        ClearAllLearnedSkills();
        ClearAllTactics();
        // Assigns animator controller
        if (!animator.IsNull())
        {
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
            yield return StartCoroutine(ExecuteAbility(singleAbility, marathonRunner));
            this.StopCoroutine("ExecuteAbility");
            isNonDefaultUsed = true;
        }

        if (!isNonDefaultUsed)
        {
            singleAbility = validAbilities.FirstOrDefault(x => x.isDefault);
            if (!singleAbility.IsNull())
            {
                yield return StartCoroutine(ExecuteAbility(singleAbility, marathonRunner));
                this.StopCoroutine("ExecuteAbility");
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

    IEnumerator ExecuteAbility(Ability singleAbility, MarathonRunner marathonRunner)
    {
        singleAbility.Setup();
        var tactics = singleAbility.tactics.OrderBy(x => x.displayOrder).Where(x => x.Define());
        var singleTactic = tactics.FirstOrDefault(x => !x.isDefault) ?? tactics.FirstOrDefault(x => x.isDefault);
        var characterRunner = marathonRunner.GetCharacterRunner(this);
        characterRunner.RunOnActionRoad(singleAbility.executedTime);
        yield return StartCoroutine(singleAbility.Use(new AbilityUsingParams
        {
            tactic = singleTactic,
            marathonRunner = marathonRunner
        }));
        singleAbility.StopCoroutine("Use");
        singleAbility.Exit();

        characterRunner = null;
        tactics = null;
        singleTactic = null;
    }

    void Flip()
    {
        if (flip)
        {
            var originScale = transform.localScale;
            originScale.x *= -1;
            transform.localScale = originScale;
        }
    }
}