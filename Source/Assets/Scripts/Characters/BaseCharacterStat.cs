using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCharacterStat : MonoBehaviour
{
    const float baseHpValue = 100f;
    const float baseHpGrowthDelta = 1.07f;

    const float baseDamageValue = 10f;
    const float baseDamageGrowthDelta = 1.15f;

    [Header("Speed")]
    public Stat speed;
    [Header("Damage")]
    public Stat damage;
    public Stat realDamage;
    [Header("HP")]
    public Stat hp;
    public Stat maxHealth;
    public float currentHealth { get; protected set; }
    [Space]
    public Image healthBar;

    GeneratedBaseCharacter character;

    void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }

    void Start()
    {
        character = GetComponent<GeneratedBaseCharacter>();
    }

    public void TransformValues()
    {
        // health
        var hpPoint = hp.GetValue();
        maxHealth.baseValue = baseHpValue * Mathf.Pow(baseHpGrowthDelta, hpPoint);
        currentHealth = maxHealth.GetValue();
        // damage
        var damagePoint = damage.GetValue();
        realDamage.baseValue = baseDamageValue * Mathf.Pow(baseDamageGrowthDelta, damagePoint);
    }

    public void TakeDamage(float damage)
    {
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
