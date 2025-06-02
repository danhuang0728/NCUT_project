using UnityEngine;
using System.Collections;

public class FruitItem : MonoBehaviour
{
    public FruitType fruitType;
    private bool isCollected = false;
    private VitaminManager vitaminManager;
    public static bool vitaminSys_Introduced = false;

    private void Start()
    {
        Debug.Log($"水果 {fruitType} 已生成");
        vitaminManager = VitaminManager.Instance;
        if (vitaminManager == null)
        {
            Debug.LogError("找不到 VitaminManager！");
        }

        if(!vitaminSys_Introduced)
        {
            Debug.Log("介绍维生素系统");
            vitaminSys_Introduced = true;
            GuideSystem.Instance.Guide("看到掉落的<color=#009100>水果</color>了嗎?"); 
            GuideSystem.Instance.Guide("維生素會隨著時間減少");
            GuideSystem.Instance.Guide("當維生素低於一定程度時會受到<color=red>負面效果</color>的影響");
            GuideSystem.Instance.Guide("記得及時補充<color=#009100>水果</color>補充維生素");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            Debug.Log($"玩家收集到水果 {fruitType}");
            isCollected = true;
            
            if (vitaminManager == null)
            {
                Debug.LogError("碰触水果时 VitaminManager 为空！");
                Destroy(gameObject);
                return;
            }

            // 根据水果类型补充对应的维生素
            VitaminType vitaminType = GetVitaminType(fruitType);
            vitaminManager.AddVitamin(vitaminType, 20f);
            AudioManager.Instance.PlaySFX("collect_fruit");
            
            Destroy(gameObject);
        }
    }

    private VitaminType GetVitaminType(FruitType fruitType)
    {
        switch (fruitType)
        {
            case FruitType.Blueberry:
            case FruitType.Mango:
            case FruitType.Pineapple:
            case FruitType.Tomato:
                return VitaminType.A;

            case FruitType.Banana:
            case FruitType.Lemon:
            case FruitType.PassionFruit:
            case FruitType.Watermelon:
                return VitaminType.B;

            case FruitType.Apple:
            case FruitType.Kiwi:
            case FruitType.Guava:
                return VitaminType.C;

            case FruitType.Orange:
            case FruitType.SugarApple:
            case FruitType.Coconut:
            case FruitType.Grape:
                return VitaminType.D;

            default:
                Debug.LogError($"未知的水果类型: {fruitType}");
                return VitaminType.B;
        }
    }
} 