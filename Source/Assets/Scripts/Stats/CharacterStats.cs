using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    public int currentHealth { get; protected set; }

    public Stat damage;
    public Stat armor;

    Character character;

    void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }

    void Start()
    {
        character = GetComponent<Character>();
    }

    public void TakeDamage(int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(character.name + " takes " + damage + " damage");

        // If he hits 0, he dies
        if (currentHealth <= 0)
        {
            character.isDeath = true;
        }
    }

    public void Heal(int amount)
    {
		currentHealth += amount;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
    }
}
