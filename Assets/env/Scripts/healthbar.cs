using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{

    public Slider slider;
    public int health;
    
    private PlayerControl playerControl; //套用玩家的主角本讀取初始生命變數
    public void SetMaxHealth(int health)
    {
        slider.maxValue=health;
        slider.value=health;
    }
    
    public void sethealth(int health)
    {
        slider.value = health;
    }
    private void Start() {
        
        SetMaxHealth(100); //初始血量值為100
        playerControl = FindObjectOfType<PlayerControl>(); //初始化讀取腳本
    }
    private void Update() {
        if (playerControl != null){
            if (playerControl.HP >= 0){
                health = playerControl.HP;
                sethealth(health);
            }
        }
    }


}