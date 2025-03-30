using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris_ability_Manager : MonoBehaviour
{
    public Weapon_Manager weapon_manager;
    public GameObject EquippedGridContainer;
    public float damage; 
    public float damage_percentage;
    public float criticalDamage; 
    public float criticalDamage_percentage;
    public float criticalHitRate; 
    public float speed;   
    public float speed_percentage;
    public float health; 
    public float cooldown; 
    public float cooldown_percentage;
    public float lifeSteal; 
    public float lifeSteal_percentage;
    void Start()
    {
        damage = 0;
        criticalDamage = 0;
        criticalHitRate = 0;
        speed = 0;
        health = 0;
        cooldown = 0;
        lifeSteal = 0;
    }
    void Update()
    {
        damage_percentage =  damage/100;
        criticalDamage_percentage =  criticalDamage/100;
        speed_percentage = speed/100;
        cooldown_percentage = cooldown/100;
        lifeSteal_percentage =  lifeSteal/100;

        //判斷持有方塊
        foreach (Transform child in EquippedGridContainer.transform)
        {
            //判定是否持有居合 
            if (child.gameObject.name.Contains("(S_1)"))
            {
                weapon_manager.main_hand2 = true;
                break;
            }
            else
            {
                weapon_manager.main_hand2 = false;
            }
        }
        
    }
}
