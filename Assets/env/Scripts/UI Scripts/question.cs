using System.Collections; // 引入集合類庫
using System.Collections.Generic; // 引入泛型集合類庫
using UnityEditor; // 引入Unity編輯器類庫
using UnityEngine; // 引入Unity核心類庫
using UnityEngine.UI; // 引入Unity UI類庫
using TMPro;
using UnityEngine.InputSystem; // 引入TextMesh Pro類庫

public class Question : MonoBehaviour // 定義NPC對話類，繼承自MonoBehaviour
{
    public GameObject dialoguePanel; // 對話面板GameObject
    public TMPro.TextMeshProUGUI dialogueText; // 對話文字UI元素
    [SerializeField] private List<string> dialogue; // 可序列化的對話內容數組
    private int index; // 當前對話索引
    public GameObject ans1_btn; // 繼續按鈕GameObject
    public TextMeshProUGUI ans1_text;
    public string ans1_str; // 繼續按鈕GameObject  
    public GameObject ans2_btn; // 繼續按鈕GameObject
    public TextMeshProUGUI ans2_text;
    public string ans2_str; // 繼續按鈕GameObject
    public GameObject ans3_btn; // 繼續按鈕GameObject
    public TextMeshProUGUI ans3_text;
    public string ans3_str; // 繼續按鈕GameObject
    public GameObject end_btn; // 結束對話按鈕GameObject
    public int answer; // 答案
    public float wordspeed; // 文字顯示速度
    public bool playerIsClose; // 玩家是否靠近NPC
    public bool isDialogueActive; // 對話是否處於活動狀態
    private bool chatStart = false;
    private bool isTyping = false; // 是否正在打字效果中
    private bool isAnswered = false; // 是否回答過
    private bool isIntro = false; // 是否介紹過
    private LevelTrigger levelTrigger;
    void Start() // 初始化函數
    {   
        dialoguePanel.SetActive(false); // 初始時禁用對話面板
        ClearButtonListeners();
        levelTrigger = FindObjectOfType<LevelTrigger>();
    }

    void Update() // 每幀更新函數
    {
        if (!IsValidReference()) return; // 檢查引用是否有效，無效則返回
        isDialogueActive = dialoguePanel.activeInHierarchy; // 更新對話活動狀態

        if (Input.GetKeyDown(KeyCode.E) && playerIsClose && isAnswered == false) // 當玩家按下E鍵且靠近NPC時
        {
            chatStart = true;
            if (dialoguePanel.activeInHierarchy) // 如果對話面板已經顯示
            {
                zeroText(); // 重置對話
            }
            else // 如果對話面板未顯示
            {
                UIstate.isAnyPanelOpen = true;
                dialoguePanel.SetActive(true); // 激活對話面板
                ans1_btn.SetActive(true);
                ans2_btn.SetActive(true);
                ans3_btn.SetActive(true);
                ans1_btn.GetComponent<Button>().onClick.AddListener(ans1);
                ans2_btn.GetComponent<Button>().onClick.AddListener(ans2);
                ans3_btn.GetComponent<Button>().onClick.AddListener(ans3);
                end_btn.GetComponent<Button>().onClick.AddListener(end);
                ans1_text.text = ans1_str;
                ans2_text.text = ans2_str;
                ans3_text.text = ans3_str;
                StartCoroutine(Typing()); // 啟動打字效果協程
            }
            
        }

    }

    public void zeroText() // 重置對話文本的方法
    {
        if (dialogueText != null) // 如果對話文本UI有效
        {
            chatStart = false;
            dialogueText.text = ""; // 清空文本內容
        }
        index = 0; // 重置對話索引
        
        if (dialoguePanel != null) // 如果對話面板有效
        {
            UIstate.isAnyPanelOpen = false;
            dialoguePanel.SetActive(false); // 禁用對話面板
        }
    }
    public void ClearButtonListeners()
    {
        if (ans1_btn != null)
        {
            ans1_btn.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        if (ans2_btn != null)
        {
            ans2_btn.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        if (ans3_btn != null)
        {
            ans3_btn.GetComponent<Button>().onClick.RemoveAllListeners();
        }
        if (end_btn != null)
        {
            end_btn.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    IEnumerator Typing() // 文字打字效果協程
    {
        isTyping = true;
        if (dialogueText == null) yield break; // 如果關鍵引用無效，結束協程
        
        if (index < dialogue.Count) // 如果索引在有效範圍內
        {
            dialogueText.text = ""; // 清空當前文本
            string currentDialogue = dialogue[index];
            int i = 0;
            float lastUpdateTime = Time.realtimeSinceStartup;
            
            while (i < currentDialogue.Length)
            {
                float currentTime = Time.realtimeSinceStartup;
                if (currentTime - lastUpdateTime >= wordspeed)
                {
                    // 處理HTML標籤
                    if (currentDialogue[i] == '<')
                    {
                        int tagEnd = currentDialogue.IndexOf('>', i);
                        if (tagEnd == -1) tagEnd = currentDialogue.Length;
                        
                        // 一次性加入完整標籤
                        dialogueText.text += currentDialogue.Substring(i, tagEnd - i + 1);
                        i = tagEnd + 1;
                    }
                    else
                    {
                        // 正常逐字顯示
                        dialogueText.text += currentDialogue[i];
                        i++;
                    }
                    
                    lastUpdateTime = currentTime;
                }
                
                if (dialogueText == null) yield break; // 安全檢查
                yield return null;
            }
        }
        
        isTyping = false; // 設置打字狀態為結束
    }

    public void NextLine() // 切換到下一行對話的方法
    {
        if(chatStart == true)
        {
            if (!IsValidReference()) return; // 檢查引用是否有效
            
            if (index >= dialogue.Count - 1) // 如果已經是最後一行對話
            {
                zeroText(); // 重置對話
                return; // 結束方法
            }

            index++; // 增加對話索引
            StartCoroutine(Typing()); // 啟動打字效果協程
        }
    }
    public void ans1()
    {
        if(chatStart == true && isTyping == false)
        {
            if (!IsValidReference()) return; // 檢查引用是否有效
            
            if (TimerPanel.isfighting == true)
            {
                dialogueText.text = "現在正在戰鬥中，無法回答問題...";
                ans1_btn.SetActive(false);
                ans2_btn.SetActive(false);
                ans3_btn.SetActive(false);
                return;
            }
            
            if (index >= dialogue.Count - 1) // 如果已經是最後一行對話
            {
                zeroText(); // 重置對話
                return; // 結束方法
            }

            index = 1; // 增加對話索引
            StartCoroutine(Typing()); // 啟動打字效果協程
            ans1_btn.SetActive(false);
            ans2_btn.SetActive(false);
            ans3_btn.SetActive(false);
            if(answer == 1)
            {
                levelTrigger.CloseTrap();
                isAnswered = true;
            }
        }
    }
    public void ans2()
    {
        if(chatStart == true && isTyping == false)
        {
            if (!IsValidReference()) return; // 檢查引用是否有效
            
            if (TimerPanel.isfighting == true)
            {
                dialogueText.text = "現在正在戰鬥中，無法回答問題...";
                ans1_btn.SetActive(false);
                ans2_btn.SetActive(false);
                ans3_btn.SetActive(false);
                return;
            }
            
            if (index >= dialogue.Count - 1) // 如果已經是最後一行對話
            {
                zeroText(); // 重置對話
                return; // 結束方法
            }

            index = 2; // 增加對話索引
            StartCoroutine(Typing()); // 啟動打字效果協程
            ans1_btn.SetActive(false);
            ans2_btn.SetActive(false);
            ans3_btn.SetActive(false);
            if(answer == 2)
            {
                levelTrigger.CloseTrap();
                isAnswered = true;
            }
        } 
    }
    public void ans3()
    {
        if(chatStart == true && isTyping == false)
        {
            if (!IsValidReference()) return; // 檢查引用是否有效
            
            if (TimerPanel.isfighting == true)
            {
                dialogueText.text = "現在正在戰鬥中，無法回答問題...";
                ans1_btn.SetActive(false);
                ans2_btn.SetActive(false);
                ans3_btn.SetActive(false);
                return;
            }
            
            if (index >= dialogue.Count - 1) // 如果已經是最後一行對話
            {
                zeroText(); // 重置對話
                return; // 結束方法
            }

            index = 3; // 增加對話索引
            StartCoroutine(Typing()); // 啟動打字效果協程
            ans1_btn.SetActive(false);
            ans2_btn.SetActive(false);
            ans3_btn.SetActive(false);
            if(answer == 3)
            {
                levelTrigger.CloseTrap();
                isAnswered = true;
            }
        }
    }
    public void end()
    {
        if(chatStart == true && isTyping == false)
        {
            zeroText();
        }
    }
    public void click(InputAction.CallbackContext context) //inputSystem觸發用
    {
        if(context.started)
        {   
            NextLine();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) // 觸發器進入事件
    {
        if (other.CompareTag("Player")) // 如果觸發者是玩家
        {
            playerIsClose = true; // 設置玩家靠近狀態為真
            if(TimerPanel.isfighting == false && isAnswered == false && isIntro == false)
            {   
                GuideSystem.Instance.Guide("看起來這裡是通往下一間的方向");
                GuideSystem.Instance.Guide("但陷阱還沒關閉，看來要先解決陷阱");
                GuideSystem.Instance.Guide("上面的提示似乎就是關閉陷阱的方法");
                GuideSystem.Instance.Guide("<color=red>(選擇正確答案)</color>");
                isIntro = true;
            }
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


    private bool IsValidReference() // 檢查關鍵引用是否有效的方法
    {
        return dialoguePanel != null && // 檢查對話面板引用
               dialogueText != null; // 檢查對話文本引用
    }
}