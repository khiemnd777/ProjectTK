using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	new public string name = "New Enemy";
    public float health;
    public Sprite icon;

	public List<EnemySkill> skills = new List<EnemySkill>();
	public List<EnemySkill> learnedSkills = new List<EnemySkill>();
	
	public void AddSkill(EnemySkill skill){
		skill.owner = this;
		skills.Add(skill);
	}
}