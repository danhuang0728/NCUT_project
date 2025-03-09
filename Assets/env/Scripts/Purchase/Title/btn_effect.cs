using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;          // 新增命名空間
using UnityEngine.EventSystems; // 新增命名空間
using TMPro;
public class btn_effect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite normalSprite;  // 正常狀態圖片
    public Sprite hoverSprite;   // 滑鼠懸停圖片
    public TextMeshProUGUI buttonText;         // 新增文字組件引用
    public float textOffset = 2f;   // 文字下移量
    
    private Image buttonImage;   // 按鈕的Image組件
    private Vector2 originalTextPosition; // 儲存原始文字位置

    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<Image>();
        // 只在沒有設定normalSprite時使用預設圖片
        if(normalSprite == null) 
        {
            normalSprite = buttonImage.sprite;
        }
        
        // 獲取文字初始位置
        if(buttonText != null)
        {
            originalTextPosition = buttonText.rectTransform.anchoredPosition;
        }
    }

    // 實現滑鼠進入事件
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = hoverSprite;
        // 移動文字位置
        if(buttonText != null)
        {
            Vector2 newPosition = originalTextPosition;
            newPosition.y -= textOffset;
            buttonText.rectTransform.anchoredPosition = newPosition;
        }
    }

    // 實現滑鼠離開事件
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = normalSprite;
        // 恢復文字位置
        if(buttonText != null)
        {
            buttonText.rectTransform.anchoredPosition = originalTextPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
