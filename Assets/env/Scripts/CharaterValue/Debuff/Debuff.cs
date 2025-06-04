using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class Debuff : MonoBehaviour
{
    private character_value_ingame characterValue;
    private PlayerControl playerControl;
    private healthbar Healthbar;
    private Light2D playerLight;
    private Dictionary<FruitType, float> activeDebuffs = new Dictionary<FruitType, float>();
    
    // Debuff 效果的數值
    private const float SPEED_DEBUFF_PERCENTAGE = 0.2f; // 降低20%速度
    private const float DAMAGE_DEBUFF_AMOUNT = 5f;
    private const float HP_DEBUFF_PERCENTAGE = 0.2f; // 降低20%生命值
    private const float BLINDNESS_LIGHT_INTENSITY = 0.4f; // 失明時的光照強度

    // 新增速度修正因子
    public float speedModifier = 1f;

    void Start()
    {
        characterValue = GetComponent<character_value_ingame>();
        if (characterValue == null)
        {
            Debug.LogError("未找到character_value_ingame組件！");
        }

        playerControl = GetComponent<PlayerControl>();
        if (playerControl == null)
        {
            Debug.LogError("未找到PlayerControl組件！");
        }

        Healthbar = GameObject.Find("healthbar").GetComponent<healthbar>();
        if (Healthbar == null)
        {
            Debug.LogError("未找到healthbar組件！");
        }

        playerLight = GetComponent<Light2D>();
        if (playerLight == null)
        {
            Debug.LogError("未找到Light2D組件！");
        }

        Debug.Log("Debuff組件初始化完成");
    }

    public void ApplyDebuff(FruitType fruitType)
    {
        Debug.Log($"正在嘗試應用 {fruitType} 的Debuff效果");
        
        if (playerControl == null)
        {
            Debug.LogError("必要組件為空，無法應用Debuff！");
            return;
        }

        if (activeDebuffs.ContainsKey(fruitType))
        {
            Debug.Log($"已經存在 {fruitType} 的Debuff效果");
            return;
        }

        switch (fruitType)
        {
            // 降低移動速度的水果
            case FruitType.Banana:
            case FruitType.Lemon:
            case FruitType.PassionFruit:
            case FruitType.Watermelon:
                Debug.Log($"應用速度降低Debuff，當前速度修正: {speedModifier}");
                speedModifier *= (1 - SPEED_DEBUFF_PERCENTAGE);
                activeDebuffs.Add(fruitType, SPEED_DEBUFF_PERCENTAGE);
                if (BuffGroup_manager.instance != null)
                {
                    BuffGroup_manager.instance.setOpenIcon(BuffGroup_manager.BuffType.speed_down);
                }
                
                Debug.Log($"速度修正降低後: {speedModifier}");
                break;

            // 降低生命值的水果
            case FruitType.Kiwi:
            case FruitType.Apple:
            case FruitType.Guava:
                Debug.Log($"應用生命值降低Debuff，當前生命值: {playerControl.HP}");
                float decreaseAmount = playerControl.HP * HP_DEBUFF_PERCENTAGE; // 计算要扣除的生命值（20%）
                playerControl.HP -= decreaseAmount; // 扣除生命值
                activeDebuffs.Add(fruitType, decreaseAmount); // 记录扣除的生命值数量
                if (BuffGroup_manager.instance != null)
                {
                    BuffGroup_manager.instance.setOpenIcon(BuffGroup_manager.BuffType.health_down);
                }

                Debug.Log($"生命值降低{decreaseAmount}點後: {playerControl.HP}");
                break;

            // 降低攻擊力的水果
            case FruitType.Orange:
            case FruitType.SugarApple:
            case FruitType.Coconut:
            case FruitType.Grape:
                Debug.Log($"應用攻擊力降低Debuff，當前攻擊力: {characterValue.damage}");
                DecreaseDamage(DAMAGE_DEBUFF_AMOUNT);
                activeDebuffs.Add(fruitType, DAMAGE_DEBUFF_AMOUNT);
                Debug.Log($"攻擊力降低後: {characterValue.damage}");
                if (BuffGroup_manager.instance != null)
                {
                    BuffGroup_manager.instance.setOpenIcon(BuffGroup_manager.BuffType.damage_down);
                }

                break;

            // 失明效果的水果
            case FruitType.Blueberry:
            case FruitType.Mango:
            case FruitType.Pineapple:
            case FruitType.Tomato:
                if (playerLight != null)
                {
                    Debug.Log($"應用失明Debuff，當前光照強度: {playerLight.intensity}");
                    float originalIntensity = playerLight.intensity;
                    playerLight.intensity = BLINDNESS_LIGHT_INTENSITY;
                    activeDebuffs.Add(fruitType, originalIntensity);
                    Debug.Log($"光照強度降低後: {playerLight.intensity}");
                    if (BuffGroup_manager.instance != null)
                    {
                        BuffGroup_manager.instance.setOpenIcon(BuffGroup_manager.BuffType.blindness);
                    }
                }
                break;
        }
    }

    public void RemoveDebuff(FruitType fruitType)
    {
        Debug.Log($"正在嘗試移除 {fruitType} 的Debuff效果");
        
        if (playerControl == null)
        {
            Debug.LogError("必要組件為空，無法移除Debuff！");
            return;
        }

        if (!activeDebuffs.ContainsKey(fruitType))
        {
            Debug.Log($"未找到 {fruitType} 的Debuff效果");
            return;
        }

        float originalValue = activeDebuffs[fruitType];

        switch (fruitType)
        {
            // 恢復移動速度
            case FruitType.Banana:
            case FruitType.Lemon:
            case FruitType.PassionFruit:
            case FruitType.Watermelon:
                Debug.Log($"恢復速度，當前速度修正: {speedModifier}");
                speedModifier /= (1 - SPEED_DEBUFF_PERCENTAGE);
                if (BuffGroup_manager.instance != null)
                {
                    BuffGroup_manager.instance.setCloseIcon(BuffGroup_manager.BuffType.speed_down);
                }
                Debug.Log($"速度修正恢復後: {speedModifier}");
                break;

            // 恢復生命值
            case FruitType.Kiwi:
            case FruitType.Apple:
            case FruitType.Guava:
                Debug.Log($"恢復生命值，當前生命值: {playerControl.HP}");
                float recoveryAmount = activeDebuffs[fruitType]; // 获取之前扣除的生命值数量
                playerControl.HP = Mathf.Min(playerControl.HP + recoveryAmount, Healthbar.slider.maxValue); // 恢复生命值，但不超过最大值
                if (BuffGroup_manager.instance != null)
                {
                    BuffGroup_manager.instance.setCloseIcon(BuffGroup_manager.BuffType.health_down);
                }
                Debug.Log($"恢復{recoveryAmount}點生命值後: {playerControl.HP}");
                break;

            // 恢復攻擊力
            case FruitType.Orange:
            case FruitType.SugarApple:
            case FruitType.Coconut:
            case FruitType.Grape:
                Debug.Log($"恢復攻擊力，當前攻擊力: {characterValue.damage}");
                IncreaseDamage(originalValue);
                Debug.Log($"攻擊力恢復後: {characterValue.damage}");
                if (BuffGroup_manager.instance != null)
                {
                    BuffGroup_manager.instance.setCloseIcon(BuffGroup_manager.BuffType.damage_down);
                }
                break;

            // 恢復視野
            case FruitType.Blueberry:
            case FruitType.Mango:
            case FruitType.Pineapple:
            case FruitType.Tomato:
                if (playerLight != null)
                {
                    Debug.Log($"移除失明Debuff，當前光照強度: {playerLight.intensity}");
                    playerLight.intensity = originalValue;
                    Debug.Log($"光照強度恢復後: {playerLight.intensity}");
                    if (BuffGroup_manager.instance != null)
                    {
                        BuffGroup_manager.instance.setCloseIcon(BuffGroup_manager.BuffType.blindness);
                    }
                }
                break;
        }

        activeDebuffs.Remove(fruitType);
    }

    public bool HasDebuff(FruitType fruitType)
    {
        return activeDebuffs.ContainsKey(fruitType);
    }

    private void DecreaseDamage(float decreaseAmount)
    {
        if (characterValue != null)
        {
            characterValue.damage -= decreaseAmount;
            characterValue.damage = Mathf.Max(0, characterValue.damage);
        }
    }

    private void IncreaseDamage(float increaseAmount)
    {
        if (characterValue != null)
        {
            characterValue.damage += increaseAmount;
        }
    }

   
} 