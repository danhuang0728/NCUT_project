using UnityEngine;

[CreateAssetMenu(fileName = "PotionData", menuName = "Inventory/Potion Data")]
public class PotionData : ScriptableObject
{
    [System.Serializable]
    public struct PotionInfo
    {
        public string potionName;
        public string potionCategory;
        public Sprite potionIcon;
        public float cooldownTime;
        public int potionId;
    }
    
    [System.Serializable]
    public struct ShopItemInfo
    {
        public int itemId;
         public string itemCategory;
        public int itemPrice;
        // 可加入其他屬性，例如效果等
    }


    public PotionInfo[] potions; // 藥水欄位資料
    public ShopItemInfo[] shopItems; // 商店物品資料
}