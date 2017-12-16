using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Character))]
public class CharacterStats : MonoBehaviour
{
    public Stat maxHealth;
    public float currentHealth { get; protected set; }
    public Stat dexterity;
    public Stat damage;
    public Stat armor;
    [Space]
    public Image healthBar;

    Character character;

    void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }

    void Start()
    {
        character = GetComponent<Character>();
    }

    public void TakeDamage(float damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;

        healthBar.fillAmount = currentHealth / maxHealth.GetValue();

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
