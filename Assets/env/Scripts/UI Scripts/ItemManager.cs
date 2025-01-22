using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct ItemData
{
    public string itemName;
    public Sprite itemSprite;
    public Sprite notAcquiredSprite;
    [TextArea] public string itemDescription;
    public bool isAcquired;
}

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;
}

public class ItemManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;
    public GameObject itemButtonPrefab;
    public Transform gridLayoutTransform;
    public Image detailImage;
    public TextMeshProUGUI detailTitleText;
    public TextMeshProUGUI detailDescriptionText;
    public Sprite defaultNotAcquiredImage;

    private List<GameObject> itemButtons = new List<GameObject>();

    void Start()
    {
        InitializeItems();
        UpdateUI();
    }

    void InitializeItems()
    {
        if (itemDatabase == null || gridLayoutTransform == null)
        {
            Debug.LogError("Item Database or Grid Layout Transform is not assigned.");
            return;
        }

        foreach (ItemData itemData in itemDatabase.items)
        {
            GameObject newItemButton = Instantiate(itemButtonPrefab, gridLayoutTransform);
            var uiItem = newItemButton.GetComponent<UIInventoryItem>();
            if (uiItem == null)
            {
                Debug.LogError("The itemButton Prefab does not have the UIInventoryItem component");
                continue;
            }
            uiItem.normalSprite = itemData.itemSprite;
            uiItem.notAcquiredSprite = itemData.notAcquiredSprite == null ? defaultNotAcquiredImage : itemData.notAcquiredSprite;
            uiItem.SetIsAcquired(itemData.isAcquired);
            itemButtons.Add(newItemButton);
             Debug.Log("InitializeItems: Item " + itemData.itemName + ", isAcquired = " + itemData.isAcquired);
        }
    }
   public void UpdateUI()
    {
         if (itemButtons.Count != itemDatabase.items.Count)
        {
            Debug.LogError("Item button and database count mismatch!");
            return;
        }
         for (int i = 0; i < itemDatabase.items.Count; i++)
         {
            var itemData = itemDatabase.items[i];
            var itemButton = itemButtons[i].GetComponent<UIInventoryItem>();
            if(itemButton == null)
                continue;
              itemButton.SetIsAcquired(itemData.isAcquired);
             Debug.Log("UpdateUI: Item " + itemData.itemName + ", isAcquired = " + itemData.isAcquired);
        }
    }

    public void OnItemClicked(ItemData itemData)
    {
          if (itemData.isAcquired)
        {
           detailImage.sprite = itemData.itemSprite;
           detailTitleText.text = itemData.itemName;
           detailDescriptionText.text = itemData.itemDescription;
            Debug.Log("OnItemClicked: Item " + itemData.itemName + ", itemDescription = " + itemData.itemDescription + ", itemSprite = " + itemData.itemSprite.name);
        }
        else
        {
            detailImage.sprite = itemData.notAcquiredSprite != null ? itemData.notAcquiredSprite : defaultNotAcquiredImage;
             detailTitleText.text = "尚未取得";
             detailDescriptionText.text = "";
            Debug.Log("OnItemClicked: Item Not Acquired");
        }
    }


     public void AcquireItem(string itemName)
    {
      int index = itemDatabase.items.FindIndex(item => item.itemName == itemName);
      if(index > -1){
          itemDatabase.items[index] = new ItemData{
            itemName = itemDatabase.items[index].itemName,
            itemSprite = itemDatabase.items[index].itemSprite,
            notAcquiredSprite = itemDatabase.items[index].notAcquiredSprite,
            itemDescription = itemDatabase.items[index].itemDescription,
            isAcquired = true
          };
        UpdateUI();
      }
    }
}