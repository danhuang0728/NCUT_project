using UnityEngine;

public class Debuff : MonoBehaviour
{
    private character_value_ingame characterValue;

    void Start()
    {
        // 获取character_value_ingame组件
        characterValue = GetComponent<character_value_ingame>();
        if (characterValue == null)
        {
            Debug.LogError("未找到character_value_ingame组件！");
        }
        DecreaseDamage(90f);
    }

    /// <summary>
    /// 降低伤害值
    /// </summary>
    /// <param name="decreaseAmount">降低的数值</param>
    public void DecreaseDamage(float decreaseAmount)
    {
        if (characterValue != null)
        {
            characterValue.damage -= decreaseAmount;
            // 确保伤害不会低于0
            characterValue.damage = Mathf.Max(0, characterValue.damage);
        }
    }

    /// <summary>
    /// 降低生命值
    /// </summary>
    /// <param name="decreaseAmount">降低的数值</param>
    public void DecreaseHP(float decreaseAmount)
    {
        if (characterValue != null)
        {
            characterValue.health -= decreaseAmount;
            // 确保生命值不会低于0
            characterValue.health = Mathf.Max(0, characterValue.health);
        }
    }

    /// <summary>
    /// 降低速度值
    /// </summary>
    /// <param name="decreaseAmount">降低的数值</param>
    public void DecreaseSpeed(float decreaseAmount)
    {
        if (characterValue != null)
        {
            characterValue.speed -= decreaseAmount;
            // 确保速度不会低于0
            characterValue.speed = Mathf.Max(0, characterValue.speed);
        }
    }
} 