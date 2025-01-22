using UnityEngine;

public class InventoryToggle : MonoBehaviour
{
    public GameObject inventoryPanel; // 你的圖鑑面板
    private bool isInventoryOpen = false;

    void Start()
    {
        // 預設情況下，隱藏圖鑑面板
        if(inventoryPanel != null)
            inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
         if(inventoryPanel != null)
            inventoryPanel.SetActive(isInventoryOpen);

        // 根據圖鑑是否開啟來設定時間暫停或恢復
        if (isInventoryOpen)
        {
            Time.timeScale = 0f; // 暫停遊戲
             Debug.Log("Pause Game");
        }
        else
        {
            Time.timeScale = 1f; // 恢復遊戲
             Debug.Log("Resume Game");
        }
    }
}