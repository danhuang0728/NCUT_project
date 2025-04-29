using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
public class ValueDisplay : MonoBehaviour
{
    private character_value_ingame Character_Value_Ingame;
    private Tetris_ability_Manager abilityManager;
    private GameObject panel;
    private float totaldamgae;
    private float totalCriticalDamage;
    private bool C_damage_isRestricted;
    private float totalCriticalHitRate;
    private float totalSpeed;
    private bool Speed_isRestricted;
    private float totalHealth;
    private float totalCooldown;
    private float totalLifeSteal;
    public Character_Values_SETUP character_Values_SETUP;
    public TextMeshProUGUI damage_value;
    public TextMeshProUGUI C_damage_value;
    public TextMeshProUGUI C_rate_value;
    public TextMeshProUGUI speed_value;
    public TextMeshProUGUI health_value;
    public TextMeshProUGUI cooldown_value;
    public TextMeshProUGUI lifeSteal_value;


    void Start()
    {
        Character_Value_Ingame = FindObjectOfType<character_value_ingame>();
        abilityManager = FindObjectOfType<Tetris_ability_Manager>();
        panel = transform.Find("Panel")?.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("UI開啟狀態:"+UIstate.isAnyPanelOpen);
        Update_Data();
        C_damageLimit();
        C_speedLimit();
        panelUpdate();
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            setUIstate();
            if(panel.activeSelf && UIstate.isAnyPanelOpen)
            {
                closePanel();
                setUIstate();
            }
            else
            {
                openPanel();
                setUIstate();
            }
        }
    }

    void Update_Data()
    {
        totaldamgae = Character_Value_Ingame.damage + 
                     abilityManager.damage + 
                     character_Values_SETUP.damage_addition;

        totalCriticalDamage = Character_Value_Ingame.criticalDamage + 
                            abilityManager.criticalDamage + 
                            character_Values_SETUP.criticalDamage_addition;

        totalCriticalHitRate = Character_Value_Ingame.criticalHitRate + 
                             abilityManager.criticalHitRate + 
                             character_Values_SETUP.criticalHitRate_addition;

        totalSpeed = Character_Value_Ingame.speed + 
                    abilityManager.speed + 
                    character_Values_SETUP.speed_addition;

        totalHealth = Character_Value_Ingame.health + 
                     abilityManager.health + 
                     character_Values_SETUP.health_addition;

        totalCooldown = Character_Value_Ingame.cooldown + 
                       abilityManager.cooldown + 
                       character_Values_SETUP.cooldown_addition;

        totalLifeSteal = Character_Value_Ingame.lifeSteal + 
                        abilityManager.lifeSteal + 
                        character_Values_SETUP.lifeSteal_addition;
    }
    void C_damageLimit()
    {
       if(totalCriticalDamage > 50)
       {
            totalCriticalDamage = 50;
            C_damage_isRestricted = true;
       } 
       else {C_damage_isRestricted = false;}
    }
    void C_speedLimit()
    {
       if(totalSpeed > 20)
       {
            totalCriticalDamage = 20;
            Speed_isRestricted = true;
       } 
       else {Speed_isRestricted = false;}
    }
    void panelUpdate()
    {
        // 傷害值顯示
        damage_value.text = totaldamgae.ToString("F1") + " " + 
                           "(" + Character_Value_Ingame.damage.ToString("F1") + "%)" + 
                           "<color=#ADD8E6>(" + abilityManager.damage.ToString() + "%)</color>" + 
                           "<color=#90EE90>(" + character_Values_SETUP.damage_addition.ToString() + "%)</color>";

        // 暴擊傷害顯示
        C_damage_value.text = totalCriticalDamage.ToString("F1") + " " + 
                             "(" + Character_Value_Ingame.criticalDamage.ToString("F1") + "%)" + 
                             "<color=#ADD8E6>(" + abilityManager.criticalDamage.ToString() + "%)</color>" + 
                             "<color=#90EE90>(" + character_Values_SETUP.criticalDamage_addition.ToString() + "%)</color>";
        
        if(C_damage_isRestricted)
        {
            C_damage_value.text = "<color=#FFB6C1>" + 
                                 totalCriticalDamage.ToString("F1") + " " + 
                                 "(" + Character_Value_Ingame.criticalDamage.ToString("F1") + "%)" + 
                                 "<color=#ADD8E6>(" + abilityManager.criticalDamage.ToString() + "%)</color>" + 
                                 "<color=#90EE90>(" + character_Values_SETUP.criticalDamage_addition.ToString() + "%)</color>" + 
                                 "</color>";
        }

        // 暴擊率顯示
        C_rate_value.text = totalCriticalHitRate.ToString("F1") + " " + 
                           "(" + Character_Value_Ingame.criticalHitRate.ToString("F1") + "%)" + 
                           "<color=#ADD8E6>(" + abilityManager.criticalHitRate.ToString() + "%)</color>" + 
                           "<color=#90EE90>(" + character_Values_SETUP.criticalHitRate_addition.ToString() + "%)</color>";

        // 速度顯示
        speed_value.text = totalSpeed.ToString("F1") + " " + 
                          "(" + Character_Value_Ingame.speed.ToString("F1") + "%)" + 
                          "<color=#ADD8E6>(" + abilityManager.speed.ToString() + "%)</color>" + 
                          "<color=#90EE90>(" + character_Values_SETUP.speed_addition.ToString() + "%)</color>";
        
        if(Speed_isRestricted)
        {
            speed_value.text = "<color=#FFB6C1>" + 
                              totalSpeed.ToString("F1") + " " + 
                              "(" + Character_Value_Ingame.speed.ToString("F1") + "%)" + 
                              "<color=#ADD8E6>(" + abilityManager.speed.ToString() + "%)</color>" + 
                              "<color=#90EE90>(" + character_Values_SETUP.speed_addition.ToString() + "%)</color>" + 
                              "</color>";
        }

        // 生命值顯示
        health_value.text = totalHealth.ToString("F1") + " " + 
                           "(" + Character_Value_Ingame.health.ToString("F1") + ")" + 
                           "<color=#ADD8E6>(" + abilityManager.health.ToString() + ")</color>" + 
                           "<color=#90EE90>(" + character_Values_SETUP.health_addition.ToString() + ")</color>";

        // 冷卻時間顯示
        cooldown_value.text = totalCooldown.ToString("F1") + " " + 
                             "(" + Character_Value_Ingame.cooldown.ToString("F1") + "%)" + 
                             "<color=#ADD8E6>(" + abilityManager.cooldown.ToString() + "%)</color>" + 
                             "<color=#90EE90>(" + character_Values_SETUP.cooldown_addition.ToString() + "%)</color>";

        // 生命偷取顯示
        lifeSteal_value.text = totalLifeSteal.ToString("F1") + " " + 
                              "(" + Character_Value_Ingame.lifeSteal.ToString("F1") + "%)" + 
                              "<color=#ADD8E6>(" + abilityManager.lifeSteal.ToString() + "%)</color>" + 
                              "<color=#90EE90>(" + character_Values_SETUP.lifeSteal_addition.ToString() + "%)</color>";
    }
    void setUIstate()  //防止複數版面衝突
    {
        if(panel.activeSelf)
        {
            UIstate.isAnyPanelOpen = true;
        }
        else
        {
            UIstate.isAnyPanelOpen = false;
        }
    }
    public void openPanel()
    {
        panel.SetActive(true);
        setUIstate();
        Time.timeScale = 0; // 暫停遊戲
    }
    public void closePanel()
    {
        panel.SetActive(false);
        setUIstate();
        Time.timeScale = PlayerControl.N; // 恢復遊戲
    }
}
