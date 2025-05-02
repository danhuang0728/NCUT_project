using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class gift : MonoBehaviour
{
    private Slider slider; // 滑桿物
    public float giftValue;
    public Character_Values_SETUP character_Values_SETUP; // 角色數值腳本
    private TextMeshProUGUI gift_point;
    #if UNITY_EDITOR
    [StringDropdown("damage", "criticalDamage", "criticalHitRate", "speed", "health", "cooldown", "lifeSteal")] // 根据实际关卡设计修改选项
    #endif
    [SerializeField] private string selectedOption = "damage"; // 设置默认值
    private float effect_addition; // 效果加成值
    void Start()
    {
        gift_point = GameObject.Find("point_Value").GetComponent<TextMeshProUGUI>();
        //抓取子物件slider
        slider = GetComponentInChildren<Slider>();
        if (selectedOption == "damage")
        {
            slider.value = character_Values_SETUP.damage_addition / 10 + 1;
        }
        else if (selectedOption == "criticalDamage")
        {
            slider.value = character_Values_SETUP.criticalDamage_addition / 10 + 1;
        }
        else if (selectedOption == "criticalHitRate")
        {
            slider.value = character_Values_SETUP.criticalHitRate_addition / 10 + 1;
        }
        else if (selectedOption == "speed")
        {
            slider.value = character_Values_SETUP.speed_addition / 10 + 1;
        }
        else if (selectedOption == "health")
        {
            slider.value = character_Values_SETUP.health_addition / 10 + 1;
        }
        else if (selectedOption == "cooldown")
        {
            slider.value = character_Values_SETUP.cooldown_addition / 10 + 1;
        }
        else if (selectedOption == "lifeSteal")
        {
            slider.value = character_Values_SETUP.lifeSteal_addition / 10 + 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gift_point.text = character_Values_SETUP.GIFT_Value.ToString();
        OnSliderValueChanged(slider.value, selectedOption);
    }
    public void OnSliderValueChanged(float value, string selectedOption)
    {
        // 更新效果加成值
        effect_addition = (value - 1) * 10; 

        if (selectedOption == "damage")
        {
            character_Values_SETUP.damage_addition = effect_addition;
        }
        else if (selectedOption == "criticalDamage")
        {
            character_Values_SETUP.criticalDamage_addition = effect_addition / 2; //每一點5%
        }
        else if (selectedOption == "criticalHitRate")
        {
            character_Values_SETUP.criticalHitRate_addition = effect_addition / 2 ; //每一點5%
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
            character_Values_SETUP.cooldown_addition = effect_addition / 2; //每一點5%
        }
        else if (selectedOption == "lifeSteal")
        {
            character_Values_SETUP.lifeSteal_addition = effect_addition;
        }
    }

    public void add_buttonclick()
    {
        if(slider.value < slider.maxValue && character_Values_SETUP.GIFT_Value > 0)
        {
            // 獲取滑桿的當前值
            float sliderValue = slider.value;
            slider.value ++;
            character_Values_SETUP.GIFT_Value --;
        }
    }
    public void minus_buttonclick()
    {
        if(slider.value > slider.minValue)
        {
            // 獲取滑桿的當前值
            float sliderValue = slider.value;
            slider.value --;
            character_Values_SETUP.GIFT_Value ++;
        }
    }
}
