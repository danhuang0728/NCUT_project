using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_healthbar : MonoBehaviour
{
    public Slider slider;
    public float health;
    public BossFloat bossFloat;
    
    private PlayerControl playerControl; //套用玩家的主角本讀取初始生命變數
    
    public void SetMaxHealth(float health)
    {
        slider.maxValue = 20;
        slider.value = 20;
    }

    public void sethealth(float health)
    {
        slider.value = health;
    }

    private void Start() 
    {
        SetMaxHealth(20); //初始血量值為100
    }

    private void Update() 
    {
        if (bossFloat != null)
        {
            if (bossFloat.mainHp >= 0)
            {
                health = bossFloat.mainHp;
                sethealth(health);
            }
        }
    }


}
