using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Cutscenes : MonoBehaviour
{
    public Image img;
    public string[] text;
    public TextMeshProUGUI textMeshProUGUI;
    public float typingSpeed = 0.05f; // 每個字符顯示的時間間隔
    public float readTime = 3f; // 閱讀時間
    public float fadeInTime = 1.5f; // 增加淡入時間
    public float fadeOutTime = 1.0f; // 淡出時間
    
    void Awake()
    {
        img.color = new Color(0, 0, 0, 1);
        textMeshProUGUI.text = "";
        StartCoroutine(startCutscene());
    }

    void Update()
    {
        
    }
    
    IEnumerator startCutscene()
    {
        yield return new WaitForSeconds(1f);
        img.color = new Color(0, 0, 0, 1);
        
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
                float alpha = Mathf.Pow(progress, 0.6f); // 非線性曲線，使開始時較快結束時較慢
                
                textMeshProUGUI.color = new Color(1, 1, 1, alpha);
                
                yield return new WaitForSeconds(typingSpeed);
            }
            
            // 確保文字完全顯示
            textMeshProUGUI.text = currentText;
            textMeshProUGUI.color = new Color(1, 1, 1, 1);
            
            // 等待一段時間讓玩家閱讀
            yield return new WaitForSeconds(readTime);
            
            // 文字淡出
            float elapsedTime = 0;
            while (elapsedTime < fadeOutTime)
            {
                textMeshProUGUI.color = new Color(1, 1, 1, 1 - (elapsedTime / fadeOutTime));
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            // 確保文字完全消失
            textMeshProUGUI.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.5f);
        }
        
        // 圖片淡出
        float fadeElapsedTime = 0;
        float imageFadeOutTime = 1.5f;
        while (fadeElapsedTime < imageFadeOutTime)
        {
            img.color = new Color(0, 0, 0, 1 - (fadeElapsedTime / imageFadeOutTime));
            fadeElapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // 確保圖片完全消失
        img.color = new Color(0, 0, 0, 0);
    }
}
