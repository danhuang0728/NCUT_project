using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class healthbar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI HPtext;
    public float health;
    public character_value_ingame characterValuesIngame;
    public Character_Values_SETUP characterValues;
    
    private PlayerControl playerControl; //套用玩家的主角本讀取初始生命變數
    private float lastHealth;
    
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
        playerControl = FindObjectOfType<PlayerControl>(); //初始化讀取腳本
        float baseMaxHealth = 100;
        float initialMaxHealth = baseMaxHealth + playerControl.Calculating_Values_health;
        SetMaxHealth(initialMaxHealth);
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
            // 更新最大生命值
            float baseMaxHealth = 100;
            float newMaxHealth = baseMaxHealth + playerControl.Calculating_Values_health;
            if (slider.maxValue != newMaxHealth)
            {
                slider.maxValue = newMaxHealth;
            }
            HPtext.text = playerControl.HP.ToString("F0") + "/" + slider.maxValue.ToString("F0");
        }
    }
}
