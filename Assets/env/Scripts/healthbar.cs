using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    public Slider slider;
    public float health;
    public character_value_ingame characterValuesIngame;
    public Character_Values_SETUP characterValues;
    
    private PlayerControl playerControl; //套用玩家的主角本讀取初始生命變數
    
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void sethealth(float health)
    {
        slider.value = health;
    }

    private void Start() 
    {
        SetMaxHealth(100); //初始血量值為100
        playerControl = FindObjectOfType<PlayerControl>(); //初始化讀取腳本
    }

    private void Update() 
    {
        if (playerControl != null)
        {
            if (playerControl.HP >= 0)
            {
                health = playerControl.HP;
                sethealth(health);
            }
        }
        UpdateMaxHealth();
    }

    private void UpdateMaxHealth()
    {
        slider.maxValue = 100 + playerControl.Calculating_Values_health;
    }
}
