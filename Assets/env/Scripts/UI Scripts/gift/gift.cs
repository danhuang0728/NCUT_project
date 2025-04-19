using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gift : MonoBehaviour
{
    public Slider slider; // 滑桿物件
    public Character_Values_SETUP character_Values_SETUP; // 角色數值腳本
    #if UNITY_EDITOR
    [StringDropdown("damage", "criticalDamage", "criticalHitRate", "speed", "health", "cooldown", "lifeSteal")] // 根据实际关卡设计修改选项
    #endif
    [SerializeField] private string selectedOption = "damage"; // 设置默认值
    public float effect_addition; // 效果加成值
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnSliderValueChanged(float value, string selectedOption)
    {
        // 更新效果加成值
        effect_addition = value; 

        if (selectedOption == "damage")
        {
            character_Values_SETUP.damage_addition = effect_addition;
        }
        else if (selectedOption == "criticalDamage")
        {
            character_Values_SETUP.criticalDamage_addition = effect_addition;
        }
        else if (selectedOption == "criticalHitRate")
        {
            character_Values_SETUP.criticalHitRate_addition = effect_addition;
        }
        else if (selectedOption == "speed")
        {
            character_Values_SETUP.speed_addition = effect_addition;
        }
        else if (selectedOption == "health")
        {
            character_Values_SETUP.health_addition = effect_addition;
        }
        else if (selectedOption == "cooldown")
        {
            character_Values_SETUP.cooldown_addition = effect_addition;
        }
        else if (selectedOption == "lifeSteal")
        {
            character_Values_SETUP.lifeSteal_addition = effect_addition;
        }
    }

    public void add_buttonclick()
    {
        // 獲取滑桿的當前值
        float sliderValue = slider.value;
        slider.value ++;
    }
    public void minus_buttonclick()
    {
        // 獲取滑桿的當前值
        float sliderValue = slider.value;
        slider.value --;
    }
}
