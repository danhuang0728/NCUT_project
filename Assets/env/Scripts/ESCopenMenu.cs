using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject menuPanel;    // 選項選單面板
    public GameObject submenuPanel;   // 子選單

    private bool isSubmenuOpen = false; // 子選單的狀態
    private bool isMenuOpen = false; // 選項選單的狀態


    void Update()
    {
        // 檢測 ESC 按鍵
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleESC();
        }
        // 檢測所有選單都關閉的情況
        CheckIfAllPanelsClosed();
    }

    // 處理 ESC 功能
    private void HandleESC()
    {
        if (isSubmenuOpen || isMenuOpen) 
        {
            // 若選項選單已開啟，關閉它
            CloseAllPanels();
        }
        else
        {
            // 若兩個選單都未開啟，打開選項選單
            ToggleMenu(true);
        }
    }

    // 控制選項選單的開關
    private void ToggleMenu(bool state)
    {
        isMenuOpen = state;
        menuPanel.SetActive(state);
        if (!state)
        {
            ToggleSubmenu(false);
        }
        if (state)
        {
            Time.timeScale = 0; // 暫停遊戲
        }
        else
        {
            Time.timeScale = 1; // 恢復遊戲
        }
    }

    private void ToggleSubmenu(bool state)
    {
        isSubmenuOpen = state;
        submenuPanel.SetActive(state);
    }

    // 檢查所有面板是否都關閉了
    private void CheckIfAllPanelsClosed()
    {
       if (!isSubmenuOpen && !isMenuOpen)
        {
            //Debug.Log("所有選單都關閉了!");
            // 在這裡寫你的額外邏輯
            // 例如：恢復遊戲的 UI 狀態, 恢復遊戲播放音效...等等
        }
    }
    // 關閉所有選單並恢復時間
     public void ResumeGame()
    {
        ToggleMenu(false); // 關閉選單
    }
   // 關閉所有選單
    public void CloseAllPanels()
    {
        ToggleMenu(false);
        ToggleSubmenu(false);
    }
}