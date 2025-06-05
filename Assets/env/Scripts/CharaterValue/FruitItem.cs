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
            GuideSystem.Instance.Guide("記得吃<color=#009100>水果</color>補充維生素");
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
            // 补充其他维生素各10点
            foreach (VitaminType type in System.Enum.GetValues(typeof(VitaminType)))
            {
                if (type != vitaminType)
                {
                    vitaminManager.AddVitamin(type, 10f);
                }
            }
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
            case FruitType.Mango: //v
            case FruitType.Papaya://v
            case FruitType.Tomato://v
            case FruitType.Melon://v
            case FruitType.Orange://v
            case FruitType.Peach://v
                return VitaminType.A;

            // 維生素B：香蕉、葡萄、龍眼、椰子、百香果
            case FruitType.Banana://v
            case FruitType.Grape://v
            case FruitType.Longan://v
            case FruitType.Coconut://v
            case FruitType.PassionFruit://v
                return VitaminType.B;

            // 維生素C：奇異果、檸檬、草莓、芭樂、辣椒
            case FruitType.Kiwi://v
            case FruitType.Lemon://v
            case FruitType.Strawberry://v
            case FruitType.Guava://v
            case FruitType.Chili://v
            case FruitType.Apple://v
            case FruitType.Durian://v
                return VitaminType.C;

            // 維生素D：西瓜、鳳梨、楊桃、藍莓、釋迦、柳橙
            case FruitType.Watermelon://v
            case FruitType.Pineapple://v
            case FruitType.StarFruit://v
            case FruitType.Blueberry://v
            case FruitType.SugarApple://v
            case FruitType.Tangerine:
            case FruitType.Pumpkin://v
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
                    GuideSystem.Instance.Guide("<color=yellow>香蕉</color>:富含維生素A、E、B、鉀、 鎂 、銅");
                    GuideSystem.Instance.Guide("在運動時補充可以幫助預防抽筋");
                    GuideSystem.Instance.Guide("攝取香蕉可以解除掉<color=red>緩速</color>效果");
                    fruitIntroduced[FruitType.Banana] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Banana));
                }
                break;
            case FruitType.Lemon:
                if(!fruitIntroduced.ContainsKey(FruitType.Lemon) || !fruitIntroduced[FruitType.Lemon])
                {
                    GuideSystem.Instance.Guide("<color=#00ff00>檸檬</color>:富含檸檬酸、維生素B、C、E以及鉀、鈣、鎂");
                    GuideSystem.Instance.Guide("維生素C可以增強免疫力");
                    GuideSystem.Instance.Guide("攝取檸檬可以解除掉<color=red>缺血</color>效果");
                    fruitIntroduced[FruitType.Lemon] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Lemon));
                }
                break;
            case FruitType.PassionFruit:
                if(!fruitIntroduced.ContainsKey(FruitType.PassionFruit) || !fruitIntroduced[FruitType.PassionFruit])
                {
                    GuideSystem.Instance.Guide("<color=#8b008b>百香果</color>:富含膳食纖維、鉀、維生素Ｃ、維生素Ａ、鐵質");
                    GuideSystem.Instance.Guide("可以促進腸道健康，預防便祕");
                    GuideSystem.Instance.Guide("攝取百香果可以解除掉<color=red>緩速</color>效果");
                    fruitIntroduced[FruitType.PassionFruit] = true;
                    StartCoroutine(introduceFruitReset(FruitType.PassionFruit));
                }
                break;
            case FruitType.Watermelon:
                if(!fruitIntroduced.ContainsKey(FruitType.Watermelon) || !fruitIntroduced[FruitType.Watermelon])
                {
                    GuideSystem.Instance.Guide("<color=#228b22>西瓜</color>:富含維生素A、C、鉀、鈣、鎂");
                    GuideSystem.Instance.Guide("可以促進運動表現、抗發炎效果且為低卡高纖的理想選擇");
                    GuideSystem.Instance.Guide("攝取西瓜可以解除掉<color=red>攻擊力降低</color>效果");
                    fruitIntroduced[FruitType.Watermelon] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Watermelon));
                }
                break;
            case FruitType.Kiwi:
                if(!fruitIntroduced.ContainsKey(FruitType.Kiwi) || !fruitIntroduced[FruitType.Kiwi])
                {
                    GuideSystem.Instance.Guide("<color=#7cfc00>奇異果</color>:富含維生素C、維生素E、葉黃素、β-胡蘿蔔素以及膳食纖維");
                    GuideSystem.Instance.Guide("可以促進心血管健康，降低高血壓風險");
                    GuideSystem.Instance.Guide("攝取奇異果可以解除掉<color=red>缺血</color>效果");
                    fruitIntroduced[FruitType.Kiwi] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Kiwi));
                }
                break;
            case FruitType.Apple:
                if(!fruitIntroduced.ContainsKey(FruitType.Apple) || !fruitIntroduced[FruitType.Apple])
                {
                    GuideSystem.Instance.Guide("<color=red>蘋果</color>:富含果糖、維生素A、B、C以及鋅、鈣、鎂等抗氧化物質");
                    GuideSystem.Instance.Guide("可以減少三酸甘油脂和控血糖");
                    GuideSystem.Instance.Guide("攝取蘋果可以解除掉<color=red>缺血</color>效果");
                    fruitIntroduced[FruitType.Apple] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Apple));
                }
                break;
            case FruitType.Guava:
                if(!fruitIntroduced.ContainsKey(FruitType.Guava) || !fruitIntroduced[FruitType.Guava])
                {
                    GuideSystem.Instance.Guide("<color=#adff2f>芭樂</color>:富含維生素C、A、E、鉀以及葉酸");
                    GuideSystem.Instance.Guide("可以幫助消水腫以及美白");
                    GuideSystem.Instance.Guide("攝取芭樂可以解除掉<color=red>缺血</color>效果");
                    fruitIntroduced[FruitType.Guava] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Guava));
                }
                break;  
            case FruitType.Orange:
                if(!fruitIntroduced.ContainsKey(FruitType.Orange) || !fruitIntroduced[FruitType.Orange])
                {
                    GuideSystem.Instance.Guide("<color=#ffa500>橘子</color>:富含維生素C、β-胡蘿蔔素、膳食纖維等");
                    GuideSystem.Instance.Guide("可以防感冒、幫助膠原蛋白合成");
                    GuideSystem.Instance.Guide("攝取橘子可以解除掉<color=red>失明</color>效果");
                    fruitIntroduced[FruitType.Orange] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Orange));
                }
                break;
            case FruitType.SugarApple:
                if(!fruitIntroduced.ContainsKey(FruitType.SugarApple) || !fruitIntroduced[FruitType.SugarApple])
                {
                    GuideSystem.Instance.Guide("<color=#7fff00>釋迦</color>:富含蛋白質、碳水化合物、膳食纖維、維生素C、鉀、鈣、磷及鎂");
                    GuideSystem.Instance.Guide("可以維持心血管健康、促進腸胃蠕動、增加飽足感");
                    GuideSystem.Instance.Guide("攝取釋迦可以解除掉<color=red>攻擊力將低</color>效果");
                    fruitIntroduced[FruitType.SugarApple] = true;
                    StartCoroutine(introduceFruitReset(FruitType.SugarApple));
                }
                break;
            case FruitType.Coconut:
                if(!fruitIntroduced.ContainsKey(FruitType.Coconut) || !fruitIntroduced[FruitType.Coconut])
                {
                    GuideSystem.Instance.Guide("<color=#a0522d>椰子</color>:富含維生素 B1 、維生素 B2 、維生素 C 以及鈣、磷、鐵等微量元素及礦物質");
                    GuideSystem.Instance.Guide("可以止咳化痰、改善口腔潰瘍");
                    GuideSystem.Instance.Guide("攝取椰子可以解除掉<color=red>緩速</color>效果");
                    fruitIntroduced[FruitType.Coconut] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Coconut));
                }
                break;
            case FruitType.Grape:
                if(!fruitIntroduced.ContainsKey(FruitType.Grape) || !fruitIntroduced[FruitType.Grape])
                {
                    GuideSystem.Instance.Guide("<color=#800080>葡萄</color>:富含維生素C、A、B群、鈣、鉀、鐵質");
                    GuideSystem.Instance.Guide("可以維持血壓、防癌");
                    GuideSystem.Instance.Guide("攝取葡萄可以解除掉<color=red>緩速</color>效果");
                    fruitIntroduced[FruitType.Grape] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Grape));
                }
                break;
            case FruitType.Blueberry:
                if(!fruitIntroduced.ContainsKey(FruitType.Blueberry) || !fruitIntroduced[FruitType.Blueberry])
                {
                    GuideSystem.Instance.Guide("<color=#4169e1>藍莓</color>:富含膳食纖維、維生素C、E、K、葉酸、鈣、鐵、鎂、磷、鉀、鈉、鋅、花青素以及類黃酮");
                    GuideSystem.Instance.Guide("可以助注意力集中、養顏美容");
                    GuideSystem.Instance.Guide("攝取藍莓可以解除掉<color=red>攻擊力降低</color>效果");
                    fruitIntroduced[FruitType.Blueberry] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Blueberry));
                }
                break;
            case FruitType.Mango:
                if(!fruitIntroduced.ContainsKey(FruitType.Mango) || !fruitIntroduced[FruitType.Mango])
                {
                    GuideSystem.Instance.Guide("<color=#ffd700>芒果</color>:富含維生素A、C、ß-胡蘿蔔素、葉黃素、鎂、鉀以及芒果苷");
                    GuideSystem.Instance.Guide("可以抗癌、維持眼睛健康及改善視力");
                    GuideSystem.Instance.Guide("攝取芒果可以解除掉<color=red>失明</color>效果");
                    fruitIntroduced[FruitType.Mango] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Mango));
                }
                break;
            case FruitType.Pineapple:
                if(!fruitIntroduced.ContainsKey(FruitType.Pineapple) || !fruitIntroduced[FruitType.Pineapple])
                {
                    GuideSystem.Instance.Guide("<color=#EEEE00>鳳梨</color>:富含膳食纖維、維生素C、鉀、鈣、鎂、鋅、類黃酮及鳳梨酵素");
                    GuideSystem.Instance.Guide("可以減少運動後肌肉發炎及幫助維持體態");
                    GuideSystem.Instance.Guide("攝取鳳梨可以解除掉<color=red>攻擊力降低</color>效果");
                    fruitIntroduced[FruitType.Pineapple] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Pineapple));
                }
                break;
            case FruitType.Tomato:
                if(!fruitIntroduced.ContainsKey(FruitType.Tomato) || !fruitIntroduced[FruitType.Tomato])
                {
                    GuideSystem.Instance.Guide("<color=#dc143c>番茄</color>:富含維生素C、K、鉀、茄紅素、β-胡蘿蔔素、綠原酸及柚皮素");
                    GuideSystem.Instance.Guide("可以抗氧化、抗老化、保護心臟及有益口腔健康");
                    GuideSystem.Instance.Guide("攝取番茄可以解除掉<color=red>攻擊力降低</color>效果");
                    fruitIntroduced[FruitType.Tomato] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Tomato));
                }
                break;
            case FruitType.Papaya:
                if(!fruitIntroduced.ContainsKey(FruitType.Papaya) || !fruitIntroduced[FruitType.Papaya])
                {
                    GuideSystem.Instance.Guide("<color=#ff8c00>木瓜</color>:富含維生素A、β-胡蘿蔔素、鉀、茄紅素、維生素C");
                    GuideSystem.Instance.Guide("可以增進心臟健康、促進鈣質吸收顧骨本");
                    GuideSystem.Instance.Guide("攝取木瓜可以解除掉<color=red>失明</color>效果");
                    fruitIntroduced[FruitType.Papaya] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Papaya));
                }
                break;
            case FruitType.Melon:
                if(!fruitIntroduced.ContainsKey(FruitType.Melon) || !fruitIntroduced[FruitType.Melon])
                {
                    GuideSystem.Instance.Guide("<color=#00ff7f>哈密瓜</color>:富含維生素A、C、膳食纖維、β-胡蘿蔔素、鉀");
                    GuideSystem.Instance.Guide("可以消水腫、美容養顏、補水及防中暑");
                    GuideSystem.Instance.Guide("攝取哈密瓜可以解除掉<color=red>失明</color>效果");
                    fruitIntroduced[FruitType.Melon] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Melon));
                }
                break;
            case FruitType.Peach:
                if(!fruitIntroduced.ContainsKey(FruitType.Peach) || !fruitIntroduced[FruitType.Peach])
                {
                    GuideSystem.Instance.Guide("<color=#ff69b4>水蜜桃</color>:富含維他命B、C、E、膳食纖維、果膠、鐵等");
                    GuideSystem.Instance.Guide("可以補充水分、促進消化、、改善便秘");
                    GuideSystem.Instance.Guide("攝取水蜜桃可以解除掉<color=red>失明</color>效果");
                    fruitIntroduced[FruitType.Peach] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Peach));
                }
                break;      
            case FruitType.Longan:
                if(!fruitIntroduced.ContainsKey(FruitType.Longan) || !fruitIntroduced[FruitType.Longan])
                {
                    GuideSystem.Instance.Guide("<color=#d2b48c>龍眼</color>:富含維生素B、C、E、鈣、鎂、鐵、磷、鉀、銅、錳等礦物質");
                    GuideSystem.Instance.Guide("可以預防記憶力下降、保養肌膚與傷口癒合");
                    GuideSystem.Instance.Guide("攝取龍眼可以解除掉<color=red>緩速</color>效果");
                    fruitIntroduced[FruitType.Longan] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Longan));
                }
                break;
            case FruitType.Strawberry:
                if(!fruitIntroduced.ContainsKey(FruitType.Strawberry) || !fruitIntroduced[FruitType.Strawberry])
                {
                    GuideSystem.Instance.Guide("<color=#ff4500>草莓</color>:富含維生素C、鉀、鎂、鈣、葉酸、膳食纖維");
                    GuideSystem.Instance.Guide("可以預防心臟病、中風、癌症、高血壓及便秘");
                    GuideSystem.Instance.Guide("攝取草莓可以解除掉<color=red>缺血</color>效果");
                    fruitIntroduced[FruitType.Strawberry] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Strawberry));
                }
                break;      
            case FruitType.Chili:
                if(!fruitIntroduced.ContainsKey(FruitType.Chili) || !fruitIntroduced[FruitType.Chili])
                {
                    GuideSystem.Instance.Guide("<color=#dc143c>辣椒</color>:富含維生素A、E、K、C、B群、檸檬酸、菸酸、鉀、纖維鐵、鎂等");
                    GuideSystem.Instance.Guide("可以維護心臟健康、提高代謝率及瘦身減重");
                    GuideSystem.Instance.Guide("攝取辣椒可以解除掉<color=red>缺血</color>效果");
                    fruitIntroduced[FruitType.Chili] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Chili));
                }
                break;
            case FruitType.Durian:
                if(!fruitIntroduced.ContainsKey(FruitType.Durian) || !fruitIntroduced[FruitType.Durian])
                {
                    GuideSystem.Instance.Guide("<color=#f0e68c>榴槤</color>:富含維生素C、鉀、鎂、葉酸、亞麻油酸");
                    GuideSystem.Instance.Guide("可以使心情愉悅、助眠、維持血壓及抗發炎");
                    GuideSystem.Instance.Guide("攝取榴槤可以解除掉<color=red>缺血</color>效果");
                    fruitIntroduced[FruitType.Durian] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Durian));
                }
                break;
            case FruitType.StarFruit:
                if(!fruitIntroduced.ContainsKey(FruitType.StarFruit) || !fruitIntroduced[FruitType.StarFruit])
                {
                    GuideSystem.Instance.Guide("<color=#ffd700>楊桃</color>:富含維生素C、A、鉀、納、鐵、膳食纖維");
                    GuideSystem.Instance.Guide("可以降低血脂、生津止渴、促進消化及止咳化痰");
                    GuideSystem.Instance.Guide("攝取楊桃可以解除掉<color=red>攻擊力降低</color>效果");
                    fruitIntroduced[FruitType.StarFruit] = true;
                    StartCoroutine(introduceFruitReset(FruitType.StarFruit));
                }
                break;
            case FruitType.Pumpkin:
                if(!fruitIntroduced.ContainsKey(FruitType.Pumpkin) || !fruitIntroduced[FruitType.Pumpkin])
                {
                    GuideSystem.Instance.Guide("<color=#ff8c00>南瓜</color>:富含維生素A、C、E、膳食纖維、β-胡蘿蔔素及礦物質等");
                    GuideSystem.Instance.Guide("可以有益心臟健康、熱量低可以助減肥及護眼");
                    GuideSystem.Instance.Guide("攝取南瓜可以解除掉<color=red>攻擊力降低</color>效果");
                    fruitIntroduced[FruitType.Pumpkin] = true;
                    StartCoroutine(introduceFruitReset(FruitType.Pumpkin));
                }
                break;
        }
    }
}