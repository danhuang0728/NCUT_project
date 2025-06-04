using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FruitItem : MonoBehaviour
{
    public FruitType fruitType;
    private bool isCollected = false;
    private VitaminManager vitaminManager;
    public static bool vitaminSys_Introduced = false;
    private static Dictionary<FruitType, bool> fruitIntroduced = new Dictionary<FruitType, bool>();

    private void OnEnable()
    {
        ResetFruit();
    }

    private void ResetFruit()
    {
        isCollected = false;
    }

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
                FruitObjectPool.Instance.ReturnFruit(gameObject);
                return;
            }

            // 根据水果类型补充对应的维生素
            VitaminType vitaminType = GetVitaminType(fruitType);
            vitaminManager.AddVitamin(vitaminType, 40f);
            introduceFruit(fruitType);
            AudioManager.Instance.PlaySFX("collect_fruit");
            
            FruitObjectPool.Instance.ReturnFruit(gameObject);
        }
    }
    IEnumerator introduceFruitReset(FruitType fruitType)
    {
        yield return new WaitForSeconds(60f);
        if (fruitIntroduced.ContainsKey(fruitType))
        {
            fruitIntroduced[fruitType] = false;
        }
    }

    // =======================維生素類型設定========================
    private VitaminType GetVitaminType(FruitType fruitType)
    {
        switch (fruitType)
        {
            // 維生素A：芒果、木瓜、番茄、哈密瓜、橘子
            case FruitType.Mango:
            case FruitType.Papaya:
            case FruitType.Tomato:
            case FruitType.Melon:
            case FruitType.Orange:
            case FruitType.Peach:
                return VitaminType.A;

            // 維生素B：香蕉、葡萄、龍眼、椰子、百香果
            case FruitType.Banana:
            case FruitType.Grape:
            case FruitType.Longan:
            case FruitType.Coconut:
            case FruitType.PassionFruit:
                return VitaminType.B;

            // 維生素C：奇異果、檸檬、草莓、芭樂、辣椒
            case FruitType.Kiwi:
            case FruitType.Lemon:
            case FruitType.Strawberry:
            case FruitType.Guava:
            case FruitType.Chili:
            case FruitType.Apple:
            case FruitType.Durian:
                return VitaminType.C;

            // 維生素D：西瓜、鳳梨、楊桃、藍莓、釋迦、柳橙
            case FruitType.Watermelon:
            case FruitType.Pineapple:
            case FruitType.StarFruit:
            case FruitType.Blueberry:
            case FruitType.SugarApple:
            case FruitType.Tangerine:
                return VitaminType.D;

            default:
                Debug.LogError($"未知的水果类型: {fruitType}");
                return VitaminType.B;
        }
    }
     // =======================小助手介紹水果========================
     private void introduceFruit(FruitType fruitType)
    {
        switch (fruitType)
        {
            case FruitType.Banana:
                if(!fruitIntroduced.ContainsKey(FruitType.Banana) || !fruitIntroduced[FruitType.Banana])
                {
                    GuideSystem.Instance.Guide("<color=yellow>香蕉</color>:富含鉀離子與熱量");
                    GuideSystem.Instance.Guide("在運動時補充確實可以幫助預防抽筋");
                    GuideSystem.Instance.Guide("定期攝取香蕉可以解除掉<color=red>緩速</color>效果");
                    fruitIntroduced[FruitType.Banana] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Banana));
                }
                break;
            case FruitType.Lemon:
                if(!fruitIntroduced.ContainsKey(FruitType.Lemon) || !fruitIntroduced[FruitType.Lemon])
                {
                    GuideSystem.Instance.Guide("");
                    fruitIntroduced[FruitType.Lemon] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Lemon));
                }
                break;
            case FruitType.PassionFruit:
                break;
            case FruitType.Watermelon:
                break;
            case FruitType.Kiwi:
                break;
            case FruitType.Apple:
                break;
            case FruitType.Guava:
                break;  
            case FruitType.Orange:
                break;
            case FruitType.SugarApple:
                break;
            case FruitType.Coconut:
                break;
            case FruitType.Grape:
                break;
            case FruitType.Blueberry:
                break;
            case FruitType.Mango:
                break;
            case FruitType.Pineapple:
                break;
            case FruitType.Tomato:
                break;
        }
    }
}