using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class TradingSystem : MonoBehaviour
{
    public PotionData potionDatabase;

    public Image selectedItemImage;
    public int currentMoney;
    public List<RectTransform> shopItemContainers;
    public List<Image> shopItemImages;
    public List<TextMeshProUGUI> shopItemNames;
    public Image shopItemSelectedBorder;
 
     public Button refreshButton; // 新增刷新按鈕欄位
    
    private int selectedShopItemIndex = 0;
    private int purchaseState = 0;
    private PotionData.ShopItemInfo selectedItem;
    private Dictionary<int, int> itemIndexMap = new Dictionary<int, int>();
    private PotionManager potionManager;

    void Start()
    {
        InitializeShop();
        potionManager = FindObjectOfType<PotionManager>();
         gameObject.SetActive(false);
        refreshButton.onClick.AddListener(RefreshShop); // 綁定刷新按鈕事件
    }
    public void CloseShop()
    {
         gameObject.SetActive(false);
    }


    void InitializeShop()
    {
        if (potionDatabase == null)
        {
            Debug.LogError("Potion Database 未設定！");
            return;
        }

        if (potionDatabase.shopItems.Length == 0)
        {
            Debug.LogWarning("Potion Database 中沒有物品資料！");
            return;
        }

        if (potionDatabase.shopItems.Length != shopItemContainers.Count ||
            potionDatabase.shopItems.Length != shopItemImages.Count ||
            potionDatabase.shopItems.Length != shopItemNames.Count)
        {
            Debug.LogError("商店資料數量與UI數量不匹配");
            return;
        }

        for (int i = 0; i < potionDatabase.shopItems.Length; i++)
        {
            itemIndexMap[potionDatabase.shopItems[i].itemId] = i;

            if (potionDatabase.potions.Any(p => p.potionId == potionDatabase.shopItems[i].itemId))
            {
                var potionInfo = potionDatabase.potions.First(p => p.potionId == potionDatabase.shopItems[i].itemId);
                shopItemImages[i].sprite = potionInfo.potionIcon;
                shopItemNames[i].text = potionInfo.potionName;
            }
        }
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeShopItem(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeShopItem(1);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(shopItemContainers[selectedShopItemIndex], Input.mousePosition))
            {
                PurchaseItem();
            }
        }
    }

    public void ChangeShopItem(int change)
    {
        selectedShopItemIndex += change;
        if (selectedShopItemIndex < 0)
        {
            selectedShopItemIndex = shopItemContainers.Count - 1;
        }
        else if (selectedShopItemIndex >= shopItemContainers.Count)
        {
            selectedShopItemIndex = 0;
        }

        UpdateSelectedItemBorder();
    }

    public void UpdateSelectedItemBorder()
    {
        shopItemSelectedBorder.enabled = true;
        shopItemSelectedBorder.rectTransform.position = shopItemContainers[selectedShopItemIndex].position;
        selectedItem = potionDatabase.shopItems[selectedShopItemIndex];
    }

    public void PurchaseItem()
    {
        if (purchaseState == 0)
        {
            if (potionDatabase.potions.Any(p => p.potionId == selectedItem.itemId))
            {
                var potionInfo = potionDatabase.potions.First(p => p.potionId == selectedItem.itemId);
                selectedItemImage.sprite = potionInfo.potionIcon;
            }
            purchaseState = 1;
        }
        else if (purchaseState == 1)
        {
            if (currentMoney >= selectedItem.itemPrice)
            {
                currentMoney -= selectedItem.itemPrice;
                 if (potionManager != null)
                 {
                     if (itemIndexMap.ContainsKey(selectedItem.itemId))
                     {
                             int potionIndex = itemIndexMap[selectedItem.itemId];
                             if (potionManager.potionDatabase.potions.Length > potionIndex)
                             {
                                    if (potionDatabase.potions.Any(p => p.potionId == selectedItem.itemId))
                                     {
                                             var potionInfo = potionDatabase.potions.First(p => p.potionId == selectedItem.itemId);
                                              potionManager.potionDatabase.potions[potionIndex] = new PotionData.PotionInfo()
                                             {
                                                 potionId = selectedItem.itemId,
                                                 potionName = potionInfo.potionName,
                                                 potionIcon = potionInfo.potionIcon,
                                                 potionCategory = potionInfo.potionCategory,
                                                 cooldownTime = 5
                                             };
                                     }
                                     potionManager.InitializePotions();
                             }
                         }
                     }

                purchaseState = 0;
                Debug.Log("購買成功: " + (potionDatabase.potions.Any(p => p.potionId == selectedItem.itemId) ? potionDatabase.potions.First(p => p.potionId == selectedItem.itemId).potionName : "未知藥水") + ", 剩餘金幣: " + currentMoney);

            }
            else
            {
                Debug.Log("金幣不足!");
            }
        }
    }

   public void RefreshShop()
     {
        InitializeShop();
     }
}