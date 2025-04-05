using System.Collections; // 引入集合類庫
using System.Collections.Generic; // 引入泛型集合類庫
using UnityEditor; // 引入Unity編輯器類庫
using UnityEngine; // 引入Unity核心類庫
using UnityEngine.UI; // 引入Unity UI類庫
using TMPro; // 引入TextMesh Pro類庫

public class NPCChat : MonoBehaviour // 定義NPC對話類，繼承自MonoBehaviour
{
    public GameObject dialoguePanel; // 對話面板GameObject
    public TMPro.TextMeshProUGUI dialogueText; // 對話文字UI元素
    [SerializeField] private string[] dialogue; // 可序列化的對話內容數組
    private int index; // 當前對話索引
    public float wordspeed; // 文字顯示速度
    public bool playerIsClose; // 玩家是否靠近NPC
    public bool isDialogueActive; // 對話是否處於活動狀態
    public GameObject ContinueButton; // 繼續按鈕GameObject
    private bool isTyping = false; // 是否正在打字效果中
    void Start() // 初始化函數
    {   
        dialoguePanel.SetActive(false); // 初始時禁用對話面板
    }

    void Update() // 每幀更新函數
    {
        if (!IsValidReference()) return; // 檢查引用是否有效，無效則返回
        isDialogueActive = dialoguePanel.activeInHierarchy; // 更新對話活動狀態

        if (Input.GetKeyDown(KeyCode.E) && playerIsClose) // 當玩家按下E鍵且靠近NPC時
        {
            if (dialoguePanel.activeInHierarchy) // 如果對話面板已經顯示
            {
                zeroText(); // 重置對話
            }
            else // 如果對話面板未顯示
            {
                dialoguePanel.SetActive(true); // 激活對話面板
                ContinueButton.GetComponent<Button>().onClick.RemoveAllListeners(); // 移除繼續按鈕的所有監聽器
                ContinueButton.GetComponent<Button>().onClick.AddListener(() => NextLine()); // 添加新的點擊監聽器以切換到下一行對話
                StartCoroutine(Typing()); // 啟動打字效果協程
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Space) && isDialogueActive && !isTyping) // 當玩家按空格鍵且對話處於活動狀態且不在打字過程中
        {
            if (ContinueButton != null && ContinueButton.activeSelf) // 如果繼續按鈕存在且處於活動狀態
            {
                Button btn = ContinueButton.GetComponent<Button>(); // 獲取按鈕組件
                if (btn != null && btn.onClick != null) // 如果按鈕和點擊事件有效
                {
                    btn.onClick.Invoke(); // 觸發點擊事件
                }
                else // 如果按鈕或點擊事件無效
                {
                    Debug.LogWarning("按鈕元件缺失"); // 輸出警告信息
                }
            }
        }
        if (dialogueText.text == dialogue[index]) // 如果當前文本已完全顯示
        {
            ContinueButton.SetActive(true); // 激活繼續按鈕
        }
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
                yield return new WaitForSeconds(wordspeed); // 等待指定時間
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
            return; // 結束方法
        }

        ContinueButton.SetActive(false); // 禁用繼續按鈕
        index++; // 增加對話索引
        StartCoroutine(Typing()); // 啟動打字效果協程
    }

    private void OnTriggerEnter2D(Collider2D other) // 觸發器進入事件
    {
        if (other.CompareTag("Player")) // 如果觸發者是玩家
        {
            playerIsClose = true; // 設置玩家靠近狀態為真
        }
    }

    private void OnTriggerExit2D(Collider2D other) // 觸發器退出事件
    {
        if (other.CompareTag("Player")) // 如果觸發者是玩家
        {
            playerIsClose = false; // 設置玩家靠近狀態為假
            
            if (dialoguePanel != null && dialoguePanel.activeSelf) // 如果對話面板處於活動狀態
            {
                zeroText(); // 重置對話
            }
        }
    }

    private void OnDestroy() // 銷毀時調用的方法
    {
        if (ContinueButton != null) // 如果繼續按鈕有效
        {
            ContinueButton.GetComponent<Button>().onClick.RemoveAllListeners(); // 移除按鈕的所有監聽器
        }
    }

    private bool IsValidReference() // 檢查關鍵引用是否有效的方法
    {
        return dialoguePanel != null && // 檢查對話面板引用
               dialogueText != null && // 檢查對話文本引用
               ContinueButton != null; // 檢查繼續按鈕引用
    }
}