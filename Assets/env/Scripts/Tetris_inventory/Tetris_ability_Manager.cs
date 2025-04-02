using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris_ability_Manager : MonoBehaviour
{
    public Weapon_Manager weapon_manager;
    public GameObject EquippedGridContainer;
    public float damage; 
    [HideInInspector]public float damage_percentage;
    public float criticalDamage; 
    [HideInInspector]public float criticalDamage_percentage;
    public float criticalHitRate; 
    public float speed;   
    [HideInInspector]public float speed_percentage;
    public float health; 
    public float cooldown; 
    [HideInInspector]public float cooldown_percentage;
    public float lifeSteal; 
    [HideInInspector]public float lifeSteal_percentage;
    public float damage_taken_addtion;
    [HideInInspector]public float damage_taken_addtion_percentage;
    private int childCountLastFrame;
    public PlayerControl playerControl;
    void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>();
    }
    void Update()
    {
        damage_percentage = damage/100;
        criticalDamage_percentage = criticalDamage/100;
        lifeSteal_percentage = lifeSteal/100;
        damage_taken_addtion_percentage = damage_taken_addtion/100;
        //計算總共的加成用變數
        float damage_accumulation = 0;
        float health_accumulation = 0;
        float criticalDamage_accumulation = 0;
        float criticalHitRate_accumulation = 0;
        float speed_accumulation = 0;
        float cooldown_accumulation = 0;
        float lifeSteal_accumulation = 0;
        float damage_taken_addtion_accumulation = 0;
        //判斷持有方塊
        int currentChildCount = EquippedGridContainer.transform.childCount;
        if (currentChildCount != childCountLastFrame)
        {
            //在循環開始前先重置所有武器狀態
            playerControl.isCollision_skill_damage = false;
            weapon_manager.main_hand2 = false;
            weapon_manager.main_hand1 = false;
            weapon_manager.is_circle_levelUP_1 = false;
            weapon_manager.is_magicbook_levelUP_1 = false;
            weapon_manager.is_magicbook_levelUP_2 = false;
            weapon_manager.is_boomerang_levelUP_1 = false;
            foreach (Transform child in EquippedGridContainer.transform)
            {
                //-----------基礎數值方塊(粉)-----------
                //判定是否有Pink_1
                if (child.gameObject.name.Contains("Pink_1"))
                {
                    damage_accumulation += 10;
                }
                if (child.gameObject.name.Contains("Pink_2"))
                {
                    damage_accumulation += 3;
                    criticalDamage_accumulation += 12;
                }
                if (child.gameObject.name.Contains("Pink_3"))
                {
                    criticalHitRate_accumulation += 10;
                }
                if (child.gameObject.name.Contains("Pink_5"))
                {
                    health_accumulation += 25;
                    damage_accumulation += 25;
                }

                //-----------基礎數值方塊(綠)-----------
                if (child.gameObject.name.Contains("Green_1"))
                {
                    health_accumulation += 12;
                    damage_accumulation += 10;
                    damage_taken_addtion_accumulation += 30;
                }
                if (child.gameObject.name.Contains("Green_2"))
                {
                    health_accumulation += 15;
                    damage_accumulation += 15;
                    damage_taken_addtion_accumulation += 50;
                }
                if (child.gameObject.name.Contains("Green_3"))
                {
                    health_accumulation += 15;
                    damage_accumulation += 15;
                    damage_taken_addtion_accumulation += 50;
                }

                //-----------特殊能力方塊(粉)-----------
                if (child.gameObject.name.Contains("Pink_6"))
                {
                    playerControl.isCollision_skill_damage = true;
                }
                
                //---------------------武器進化方塊-------------------------
                //判定是否持有居合 
                if (child.gameObject.name.Contains("(S_1)"))
                {
                    weapon_manager.main_hand2 = true;
                }

                //判定是否持有亂砍
                if (child.gameObject.name.Contains("(S_2)"))
                {
                    weapon_manager.main_hand1 = true;
                }

                //判定是否有無限圓環
                if (child.gameObject.name.Contains("(C_1)"))
                {
                    weapon_manager.is_circle_levelUP_1 = true;
                }
                
                //判定是否有追蹤火球
                if(child.gameObject.name.Contains("(F_1)"))
                {
                    weapon_manager.is_magicbook_levelUP_1 = true;
                }
                
                //判定是否有巨大火球
                if(child.gameObject.name.Contains("(F_2)"))
                {
                    weapon_manager.is_magicbook_levelUP_2 = true;
                }
                
                //判定是否有無限彈射
                if(child.gameObject.name.Contains("(B_1)"))
                {
                    weapon_manager.is_boomerang_levelUP_1 = true;
                }
            }
            //總值賦值
            damage = damage_accumulation; 
            health = health_accumulation;
            criticalDamage = criticalDamage_accumulation;
            criticalHitRate = criticalHitRate_accumulation;
            speed = speed_accumulation;
            cooldown = cooldown_accumulation;
            lifeSteal = lifeSteal_accumulation;
            damage_taken_addtion = damage_taken_addtion_accumulation;
            //更新上一幀的子物件數量
            childCountLastFrame = currentChildCount; 
        }
        
    }
}
