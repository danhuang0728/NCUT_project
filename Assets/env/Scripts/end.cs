using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class End : MonoBehaviour
{
    public End end;
    public Image img;
    public string[] text;
    public TextMeshProUGUI textMeshProUGUI;
    public float typingSpeed = 0.05f; // 每個字符顯示的時間間隔
    public float readTime = 3f; // 閱讀時間
    public float fadeInTime = 1.5f; // 增加淡入時間
    public float fadeOutTime = 1.0f; // 淡出時間
    public GameObject endButton; // 新增按鈕物件引用
    
    void Awake()
    {
        img.color = new Color(0, 0, 0, 0);
        textMeshProUGUI.text = "";
        if (endButton != null)
        {
            endButton.SetActive(false);
        }
    }

    void Update()
    {
    }
    
    public IEnumerator startEndCutscene()
    {
        yield return new WaitForSecondsRealtime(3f);
        float fadeInElapsedTime = 0;
        while (fadeInElapsedTime < fadeInTime)
        {
            img.color = new Color(0, 0, 0, fadeInElapsedTime / fadeInTime);
            fadeInElapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        img.color = new Color(0, 0, 0, 1);
        yield return new WaitForSecondsRealtime(0.5f);
         UIstate.isAnyPanelOpen = true;
        // 顯示文字陣列中的每一段文字
        for (int i = 0; i < text.Length; i++)
        {
            // 重置文字
            textMeshProUGUI.text = "";
            textMeshProUGUI.color = new Color(1, 1, 1, 1); // 設置為完全可見
            
            // 逐字顯示文字，同時應用淡入效果
            string currentText = text[i];
            for (int charIndex = 0; charIndex < currentText.Length; charIndex++)
            {
                textMeshProUGUI.text = currentText.Substring(0, charIndex + 1);
                
                // 使用非線性曲線增強淡入效果
                float progress = (float)(charIndex + 1) / currentText.Length;
                float alpha = Mathf.Pow(progress, 0.6f);
                
                textMeshProUGUI.color = new Color(1, 1, 1, alpha);
                
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
            
            // 確保文字完全顯示
            textMeshProUGUI.text = currentText;
            textMeshProUGUI.color = new Color(1, 1, 1, 1);
            
            // 等待一段時間讓玩家閱讀
            yield return new WaitForSecondsRealtime(readTime);
            
            // 如果是最後一段文字，顯示按鈕並保持文字顯示
            if (i == text.Length - 1)
            {
                if (endButton != null)
                {
                    endButton.SetActive(true);
                }
            }
            else
            {
                // 如果不是最後一段文字，執行淡出效果
                float elapsedTime = 0;
                while (elapsedTime < fadeOutTime)
                {
                    textMeshProUGUI.color = new Color(1, 1, 1, 1 - (elapsedTime / fadeOutTime));
                    elapsedTime += Time.unscaledDeltaTime;
                    yield return null;
                }
                
                // 確保文字完全消失
                textMeshProUGUI.color = new Color(1, 1, 1, 0);
                yield return new WaitForSecondsRealtime(0.5f);
            }
        }
    }
    public void startEndCutscene_void()
    {
        StartCoroutine(startEndCutscene());
    }
}
