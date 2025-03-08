using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public enum CostType
    {
        Coins,
        Health
    }

    public enum ItemType
    {
        SpeedBoost,
        MaxHealth,
        IncreaseDamage,
        IncreaseCriticalDamage,
        IncreaseCriticalHitRate,
        IncreaseSpeed,
        IncreaseCooldown,
        IncreaseLifeSteal,
        burn_effect,
        
    }

    public string itemName;
    public int cost;
    public CostType costType;
    public ItemType itemType;
    public float effectValue; // 效果數值（速度增加量或治療量）
    
    // 添加燃燒效果的持續時間參數
    public int burnDuration = 5; // 預設燃燒持續5秒
    
    private bool playerInRange = false;
    private GameObject promptText; // 用于显示提示文本
    
    private void Start()
    {
        // 创建提示文本
        CreatePromptText();
        // 初始时隐藏提示
        if (promptText != null)
            promptText.SetActive(false);
    }

    private void CreatePromptText()
    {
        GameObject textObj = new GameObject("PromptText");
        textObj.transform.SetParent(transform);
        textObj.transform.localPosition = new Vector3(0, 1f, 0); // 在物品上方顯示

        TextMesh textMesh = textObj.AddComponent<TextMesh>();
        textMesh.text = $"{itemName}\n{cost} {(costType == CostType.Coins ? "金幣" : "生命")}";
        textMesh.characterSize = 0.15f; // 文字大小
        textMesh.fontSize = 48; // 添加字體大小設置
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.color = Color.white; // 設置文字顏色確保可見性

        promptText = textObj;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptText != null)
                promptText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptText != null)
                promptText.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryPurchase();
        }
    }

    private void TryPurchase()
    {
        PlayerControl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        character_value_ingame playerValues = GameObject.FindGameObjectWithTag("Player").GetComponent<character_value_ingame>();
        
        if (player != null && playerValues != null)
        {
            bool canPurchase = false;
            
            // 檢查是否有足夠的資源購買
            if (costType == CostType.Health)
            {
                if (player.HP > cost) // 確保玩家有足夠的血量
                {
                    player.TakeDamage(cost);
                    canPurchase = true;
                }
            }
            else if (costType == CostType.Coins)
            {
                if (playerValues.gold >= cost) // 檢查是否有足夠的金幣
                {
                    playerValues.gold -= cost;
                    canPurchase = true;
                }
            }
            
            if (canPurchase)
            {
                ApplyItemEffect(player, playerValues);
                Destroy(gameObject);
            }
        }
    }

    private void ApplyItemEffect(PlayerControl player, character_value_ingame playerValues)
    {
        switch (itemType)
        {
            case ItemType.SpeedBoost:
                player.Legend_speed = true;
                playerValues.speed += effectValue;
                break;
            case ItemType.MaxHealth:
                // 直接增加 character_value_ingame 中的 health 值
                playerValues.health += effectValue;
                // 增加當前血量
                player.HP += effectValue;
                break;
            case ItemType.IncreaseDamage:
                playerValues.damage += effectValue;
                break;
            case ItemType.burn_effect:
                player.hasBurnEffect = true;
                player.burnDuration = burnDuration;
                break;
            case ItemType.IncreaseCooldown:
                playerValues.cooldown += effectValue;
                break;
            case ItemType.IncreaseLifeSteal:
                playerValues.lifeSteal += effectValue;
                break;
        }
    }
} 