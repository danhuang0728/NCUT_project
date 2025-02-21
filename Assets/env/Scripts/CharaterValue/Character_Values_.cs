using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Character_Values_SETUP", menuName = "Custom/Character_Values_SETUP")]

public class Character_Values_SETUP : ScriptableObject
{   
    public float damage_addition = 0;
    public float damage_addition_percentage => damage_addition / 100f;
    public float criticalDamage_addition = 0;
    public float criticalDamage_addition_percentage => criticalDamage_addition / 100f;
    public float criticalHitRate_addition = 0;
    public float speed_addition = 0;
    public float speed_addition_percentage => speed_addition / 100f;
    public float health_addition = 0;
    public float health_addition_percentage => health_addition / 100f;
    public float cooldown_addition = 0;
    public float cooldown_addition_percentage => cooldown_addition / 100f;
    public float lifeSteal_addition = 0;
    public float lifeSteal_addition_percentage => lifeSteal_addition / 100f;
    public float gold_addition = 0;
    public float gold_addition_percentage => gold_addition / 100f;
}


