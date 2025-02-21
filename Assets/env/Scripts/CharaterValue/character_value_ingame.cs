using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_value_ingame : MonoBehaviour
{
    public float damage; //已完成
    public float damage_percentage;
    public float criticalDamage; //已完成
    public float criticalDamage_percentage;
    public float criticalHitRate; //已完成
    public float speed;   //已完成
    public float speed_percentage;
    public float health; //已完成
    public float cooldown; //圓環完成、魔法書完成、弩炮完成、迴力鏢完成、斧頭完成、西洋劍完成
    public float cooldown_percentage;
    public float lifeSteal; //已完成
    public float lifeSteal_percentage;
    public float gold;
    void Start()
    {
        damage = 0;
        criticalDamage = 0;
        criticalHitRate = 0;
        speed = 0;
        health = 0;
        cooldown = 0;
        lifeSteal = 0;
        gold = 0;
    }
    void Update()
    {
        damage_percentage = damage/100;
        criticalDamage_percentage = criticalDamage/100;
        speed_percentage = speed/100;
        cooldown_percentage = cooldown/100;
        lifeSteal_percentage = lifeSteal/100;
    }
}
