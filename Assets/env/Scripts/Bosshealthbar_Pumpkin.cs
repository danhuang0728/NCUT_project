using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bosshealthbar_Pumpkin : MonoBehaviour
{
    public Slider slider;
    private float health;
    public PumpkinBoss_main pumpkinBoss_Main;
    
    private PlayerControl playerControl; //套用玩家的主角本讀取初始生命變數
    
    public void SetMaxHealth()
    {
        slider.maxValue = pumpkinBoss_Main.HP;
    }

    public void sethealth(float health)
    {
        slider.value = health;
    }

    private void Start()
    {

    }

    private void Update()
    {
        pumpkinBoss_Main = FindObjectOfType<PumpkinBoss_main>();
        if (pumpkinBoss_Main != null)
        {
            if (slider.maxValue <= 70000)
            {
                SetMaxHealth();
            }
            if (pumpkinBoss_Main.HP >= 0)
            {
                health = pumpkinBoss_Main.HP;
                sethealth(health);
            }
        }
    }


}
