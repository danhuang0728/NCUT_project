using UnityEngine;
using System.Collections.Generic;

public class Debuff : MonoBehaviour
{
    private character_value_ingame characterValue;
    private PlayerControl playerControl;
    private healthbar Healthbar;
    private Dictionary<FruitType, float> activeDebuffs = new Dictionary<FruitType, float>();
    
    // Debuff 效果的數值
    private const float SPEED_DEBUFF_PERCENTAGE = 0.5f; // 降低50%速度
    private const float DAMAGE_DEBUFF_AMOUNT = 5f;
    private const float HP_DEBUFF_PERCENTAGE = 0.2f; // 降低20%生命值

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
            case FruitType.Banana:
            case FruitType.Lemon:
                Debug.Log($"應用速度降低Debuff，當前速度: {playerControl.speed}");
                float originalSpeed = playerControl.speed;
                playerControl.speed *= (1 - SPEED_DEBUFF_PERCENTAGE);
                activeDebuffs.Add(fruitType, originalSpeed);
                Debug.Log($"速度降低後: {playerControl.speed}");
                break;

            case FruitType.Kiwi:
            case FruitType.Apple:
            case FruitType.Guava:
                Debug.Log($"應用生命值降低Debuff，當前生命: {playerControl.HP}");
                float decreaseAmount = playerControl.HP * HP_DEBUFF_PERCENTAGE;
                playerControl.HP -= decreaseAmount;
                activeDebuffs.Add(fruitType, decreaseAmount);
                Debug.Log($"生命值降低後: {playerControl.HP}");
                break;

            case FruitType.Orange:
            case FruitType.SugarApple:
                Debug.Log($"應用攻擊力降低Debuff，當前攻擊力: {characterValue.damage}");
                DecreaseDamage(DAMAGE_DEBUFF_AMOUNT);
                activeDebuffs.Add(fruitType, DAMAGE_DEBUFF_AMOUNT);
                Debug.Log($"攻擊力降低後: {characterValue.damage}");
                break;
        }

        if (fruitType == FruitType.Lemon)
        {
            Debug.Log("檸檬特殊效果：額外降低攻擊力");
            DecreaseDamage(DAMAGE_DEBUFF_AMOUNT);
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
            case FruitType.Banana:
            case FruitType.Lemon:
                Debug.Log($"恢復速度，當前速度: {playerControl.speed}");
                playerControl.speed = originalValue;
                Debug.Log($"速度恢復後: {playerControl.speed}");
                break;

            case FruitType.Kiwi:
            case FruitType.Apple:
            case FruitType.Guava:
                Debug.Log($"恢復生命值，當前生命值: {playerControl.HP}");
                // 恢復20%當前最大生命值
                float healAmount = Healthbar.slider.maxValue * HP_DEBUFF_PERCENTAGE;
                playerControl.HP = Mathf.Min(playerControl.HP + healAmount, Healthbar.slider.maxValue);
                Debug.Log($"生命值恢復後: {playerControl.HP}");
                break;

            case FruitType.Orange:
            case FruitType.SugarApple:
                Debug.Log($"恢復攻擊力，當前攻擊力: {characterValue.damage}");
                IncreaseDamage(originalValue);
                Debug.Log($"攻擊力恢復後: {characterValue.damage}");
                break;
        }

        if (fruitType == FruitType.Lemon)
        {
            Debug.Log("移除檸檬的額外攻擊力降低效果");
            IncreaseDamage(DAMAGE_DEBUFF_AMOUNT);
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