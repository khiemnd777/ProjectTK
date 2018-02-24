using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCharacterStat : MonoBehaviour
{
    public Stat maxHealth;
    public float currentHealth { get; protected set; }
    public Stat speed;
    public Stat damage;
    public Stat hp;
    public Stat realDamage;
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
        maxHealth.ClearModifiers();
        var baseHpValue = 100f;
        var baseHpDelta = 1.07f;
        var baseHpIncreasement = baseHpDelta;
        for(var i = 0; i < hpPoint - 1; i++)
        {
            baseHpIncreasement *= baseHpDelta;
        }
        maxHealth.baseValue = (float)(baseHpValue * baseHpIncreasement);

        // damage
        var damagePoint = damage.GetValue();
        realDamage.ClearModifiers();
        var baseDamageValue = 10f;
        var baseDamageDelta = 1.15f;
        var baseDamageIncreasement = baseDamageDelta;
        for(var i = 0; i < damagePoint - 1; i++)
        {
            baseDamageIncreasement *= baseDamageDelta;
        }
        realDamage.baseValue = (float)(baseDamageValue * baseDamageIncreasement);
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
