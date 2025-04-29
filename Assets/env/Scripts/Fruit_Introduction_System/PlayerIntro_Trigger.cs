using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerIntro_Trigger : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 5f; // 檢測範圍半徑
    [SerializeField] private LayerMask monsterLayer; // Monster 的 Layer
    public FruitDataList fruitDataList;

    // 對話系統相關變數
    [SerializeField] private TextMeshProUGUI dialogueText; // 對話文本UI
    [SerializeField] private GameObject dialoguePanel; // 對話面板
    [SerializeField] private GameObject ContinueButton; // 繼續按鈕
    [SerializeField] private float wordspeed = 0.05f; // 打字速度
    private int index = 0; // 當前對話索引
    private bool isTyping = false; // 是否正在打字
    private string[] dialogue; // 對話內容

    // Start is called before the first frame update
    void Start()
    {
        // 初始化對話系統
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 檢測範圍內的 Monster
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, monsterLayer);
        //Debug.Log(""+colliders);
        if (colliders.Length > 0)
        {
            // 當檢測到 Monster 時執行的邏輯
            foreach (Collider2D monster in colliders)
            {
                NormalMonster_setting monster_Setting = monster.GetComponent<NormalMonster_setting>();
                // 檢查怪物是否有水果資料
                if (monster_Setting != null && monster_Setting.fruitData != null)
                {
                    // 尋找fruitDataList中與當前怪物相同TYPE的水果資料
                    foreach (FruitData fruitData in fruitDataList.fruitDatas)
                    {
                        // 比對水果怪物類型是否相同
                        if (fruitData.fruitMonsterTYPE == monster_Setting.fruitData.fruitMonsterTYPE)
                        {
                            // 檢查該水果怪物是否已經被介紹過
                            if (!fruitData.isIntroduced)
                                {
                                // 更新怪物的水果資料為找到的資料
                                monster_Setting.fruitData = fruitData;
                                
                                // 觸發對話介紹
                                StartIntroduction(monster_Setting);
                                
                                // 標記為已介紹
                                monster_Setting.fruitData.isIntroduced = true;
                                
                                // 找到並處理完畢，跳出循環
                                break;
                            }
                        }                      
                    }
                }
            }
        }
    }

    private void StartIntroduction(NormalMonster_setting monster)
    {
        // 設置對話內容
        dialogue = monster.fruitData.introduce;
        ContinueButton.GetComponent<Button>().onClick.RemoveAllListeners(); // 移除繼續按鈕的所有監聽器
        ContinueButton.GetComponent<Button>().onClick.AddListener(() => NextLine()); // 添加新的點擊監聽器以切換到下一行對話
        
        // 顯示對話面板
        if (dialoguePanel != null)
        {
            UIstate.isAnyPanelOpen = true;
            dialoguePanel.SetActive(true);

        }
        
        // 開始對話
        index = 0;
        StartCoroutine(Typing());
    }

    private bool IsValidReference()
    {
        return dialogueText != null && ContinueButton != null && dialoguePanel != null;
    }

    // 在 Scene 視圖中繪製檢測範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    public void zeroText() // 重置對話文本的方法
    {
        if (dialogueText != null) // 如果對話文本UI有效
        {
            dialogueText.text = ""; // 清空文本內容
        }
        index = 0; // 重置對話索引
        
        if (dialoguePanel != null) // 如果對話面板有效
        {
            dialoguePanel.SetActive(false); // 禁用對話面板
        }
    }

    IEnumerator Typing() // 文字打字效果協程
    {
        if (dialogueText == null || ContinueButton == null) yield break; // 如果關鍵引用無效，結束協程
        
        if (index < dialogue.Length) // 如果索引在有效範圍內
        {
            dialogueText.text = ""; // 清空當前文本
            foreach (char letter in dialogue[index].ToCharArray()) // 遍歷當前對話字符
            {
                if (dialogueText == null) yield break; // 安全檢查
                dialogueText.text += letter; // 逐字添加到文本
                yield return new WaitForSecondsRealtime(wordspeed); // 使用不受遊戲時間影響的等待
            }
        }
        
        isTyping = false; // 設置打字狀態為結束
    }

    public void NextLine() // 切換到下一行對話的方法
    {
        if (!IsValidReference()) return; // 檢查引用是否有效
        
        if (index >= dialogue.Length - 1) // 如果已經是最後一行對話
        {
            zeroText(); // 重置對話
            UIstate.isAnyPanelOpen = false;
            return; // 結束方法
        }

        index++; // 增加對話索引
        StartCoroutine(Typing()); // 啟動打字效果協程
    }
}
