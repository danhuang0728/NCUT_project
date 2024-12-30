using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCopenMenu : MonoBehaviour
{
    public GameObject menuPanel; // 選單的 UI 物件
    private bool isMenuOpen = false; // 選單是否開啟的狀態

    void Update()
    {
        // 檢測 ESC 或 Tab 鍵按下
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu(); // 切換選單狀態
        }
    }

    // 切換選單狀態
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen; // 切換狀態
        menuPanel.SetActive(isMenuOpen); // 顯示或隱藏選單
    }
}
