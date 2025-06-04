using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GuideSystem : MonoBehaviour
{
    public static GuideSystem Instance;
    public GameObject chatPanel;
    public TextMeshProUGUI bubbletext;
    public List<string> dialogue = new List<string>();
    public float wordspeed;
    public float waitTime;
    public bool isTyping;
    private int index;
    private bool isPlaying = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        zeroText();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // ==================小助手發話(text為要講的內容文字)=============
    public void Guide(string text)
    {
        dialogue.Add(text);
        if (!isPlaying)
        {
            isPlaying = true;
            openChatPanel();
            StartCoroutine(autonextline());
        }
    }
    IEnumerator Typing()
    {
        if (bubbletext == null)
        {
            Debug.LogError("chatbubble: bubbletext 未設置！");
        }
        isTyping = true;
        if (index < dialogue.Count)
        {
            bubbletext.text = "";
            string currentDialogue = dialogue[index];
            
            int i = 0;
            while (i < currentDialogue.Length)
            {
                // 處理HTML標籤
                if (currentDialogue[i] == '<')
                {
                    int tagEnd = currentDialogue.IndexOf('>', i);
                    if (tagEnd == -1) tagEnd = currentDialogue.Length;
                    
                    // 一次性加入完整標籤
                    bubbletext.text += currentDialogue.Substring(i, tagEnd - i + 1);
                    i = tagEnd + 1;
                }
                else
                {
                    // 正常逐字顯示
                    bubbletext.text += currentDialogue[i];
                    i++;
                    yield return new WaitForSeconds(wordspeed);
                }
            }
        }
    }
    public void NextLine()
    {
        if (index < dialogue.Count)
        {
            StartCoroutine(Typing());
            index++;
        }
    }
    IEnumerator autonextline()
    {
        if (chatPanel == null)
        {
            Debug.LogError("chatbubble: chatPanel GameObject 未設置！");
        }
        
        NextLine();
        yield return new WaitForSeconds(waitTime);
        if (index < dialogue.Count)
        {
            StartCoroutine(autonextline());
        }
        else
        {
            zeroText();
            //清除對話過的list
            dialogue.Clear();
            closeChatPanel();
            yield return new WaitForSeconds(waitTime);
            isTyping = false;
            isPlaying = false;
        }
    }
    public void zeroText()
    {
        if (bubbletext != null)
        {
            bubbletext.text = "";
        }
        index = 0;
        if (chatPanel != null)
        {
            closeChatPanel();
        }
    }
    void openChatPanel()
    {
        chatPanel.SetActive(true);
    }
    void closeChatPanel()
    {
        chatPanel.SetActive(false);
    }
}
