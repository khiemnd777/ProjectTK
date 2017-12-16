using UnityEngine;

public class EnemySkill : MonoBehaviour
{
    new public string name = "New Skill";
    public Sprite icon;
    public Enemy owner;
    
    public virtual void Use() 
    {
        Debug.Log("Enemy is using skill " + name);
    }
}