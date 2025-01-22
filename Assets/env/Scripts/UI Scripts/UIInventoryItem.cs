using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
    public Image targetImage; // 要切換圖片的 Image
    public Sprite normalSprite;  // 正常狀態的圖片 (已取得)
    public Sprite clickedSprite; // 點擊狀態的圖片 (已取得)
    public Sprite notAcquiredSprite; // 未取得的圖片
    public bool isAcquired = false; // 是否取得的狀態
    public Sprite defaultNotAcquiredSprite;
    public Sprite defaultNormalSprite;
     public GameObject borderObject;  // 新增一個用於引用邊框的變數
    private bool isClicked = false;


    void Awake()
    {
       if (notAcquiredSprite == null)
        {
            notAcquiredSprite = defaultNotAcquiredSprite;
        }

         if (normalSprite == null)
        {
            normalSprite = defaultNormalSprite;
        }
    }
  
    void OnEnable()
    {
        isClicked = false; // 確保初始狀態是未點擊
        UpdateImage();
        if(borderObject != null)
             borderObject.SetActive(false); // 初始化時隱藏邊框
    }

    // 根據 isAcquired 的狀態來設定圖片
    void UpdateImage()
    {
        if(targetImage == null) return;
        if (isAcquired)
        {
           targetImage.sprite = normalSprite;
        }
        else
        {
          targetImage.sprite = notAcquiredSprite;
        }
    }

    public void SetIsAcquired(bool acquired)
    {
       isAcquired = acquired;
        UpdateImage();
    }

    public void OnClicked()
    {
        if (isAcquired && borderObject != null)
        {
            isClicked = !isClicked;
             borderObject.SetActive(isClicked); // 點擊時顯示或隱藏邊框
        }
    }
}