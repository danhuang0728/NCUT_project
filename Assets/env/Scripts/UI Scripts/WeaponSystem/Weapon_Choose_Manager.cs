using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Weapon_Choose_Manager : MonoBehaviour
{
    public WeaponDatabase weaponDatabase; // 新增武器資料庫
    public Player_WeaponData playerWeaponData; // 新增玩家武器資料庫
    [HideInInspector]public Weapon_Manager weaponManager; // 新增武器管理器
    public GameObject[] optionPanels; // 三個選項 Panel
    public TextMeshProUGUI[] choose_TITLE; // 三個文字欄位
    public TextMeshProUGUI[] textFields; // 三個文字欄位
    public Image[] images;   // 三個圖片欄位
    public GameObject optionPanelParent; // 儲存選單的父物件
    public GameObject background; // 背景
    private List<WeaponData> selectedData; // 儲存選取的選項
    private bool isPanelOpen = false; // 確認選單是否打

    void Start()
    {
       // 重置玩家武器数据
       playerWeaponData.ResetWeaponData();
       
       // 重置所有武器的等级为0
       foreach (WeaponData weapon in weaponDatabase.WeaponDataList)
       {
           weapon.level = 0;
       }
       
       weaponManager = FindObjectOfType<Weapon_Manager>();
       
       // 确保武器管理器也更新状态
       if (weaponManager != null)
       {
           weaponManager.UpdateWeaponStatus();
           weaponManager.UpdateAllWeaponLevels();
       }
    }

    // Update is called once per frame
    void Update()
    {
      weaponManager.UpdateWeaponStatus();
      weaponManager.UpdateAllWeaponLevels();
        // 按數字8可開啟武器選擇面板
        if (Input.GetKeyDown(KeyCode.Alpha8) && UIstate.isAnyPanelOpen == false)
        {
            if (!isPanelOpen)
            {
                OpenPanel();
            }
            else
            {
                ClosePanel();
            }
        }
        // 新增使用数字键 1、2、3 选择武器的功能
        if (isPanelOpen)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ButtonClicked(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ButtonClicked(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ButtonClicked(2);
            }
        }
    }
    public void OpenPanel()
    {
      UpdateUI();
      isPanelOpen = true;
      AudioManager.Instance.PlaySFX("open_box");
      if (optionPanelParent != null && UIstate.isAnyPanelOpen == false)
      {
        UIstate.isAnyPanelOpen = true;
        background.SetActive(true);
        optionPanelParent.SetActive(true);
      }
      PauseGame();

    }
    public void ClosePanel()
    {
      isPanelOpen = false;
      if (optionPanelParent != null)
        {
          UIstate.isAnyPanelOpen = false;
          optionPanelParent.SetActive(false);
          background.SetActive(false);
        }
        ResumeGame();
    }

    private void PauseGame()
    {
    Time.timeScale = 0;
    }


    private void ResumeGame()
    {
    Time.timeScale = PlayerControl.N;
    }
     public void UpdateUI()
    {
      if (weaponDatabase == null || weaponDatabase.WeaponDataList.Count < 3)
        {
          Debug.LogError("Weapon Database is not properly set up or doesn't have enough data.");
          return;
        }
      if(optionPanels.Length != 3 || textFields.Length != 3 || images.Length != 3){
        Debug.LogError("Panels or textFields or images not properly set up.");
        return;
      }
      // 隨機選取三個不同的資料
      selectedData = SelectRandomData(3);

      for(int i = 0; i < 3; i++){
        if(selectedData[i] == null)
        {
            Debug.LogError("A null data was detected.");
            return;
        }
        
        // 設定文字內容
        choose_TITLE[i].text = selectedData[i].skillName;
        textFields[i].text = selectedData[i].weapon_inturoduction;

        images[i].sprite = selectedData[i].skillIcon;
      }
    }
    private List<WeaponData> SelectRandomData(int count)
    {
      List<WeaponData> selection = new List<WeaponData>();
      if(weaponDatabase == null || weaponDatabase.WeaponDataList == null || weaponDatabase.WeaponDataList.Count == 0)
      {
          Debug.LogError("No data found");
          return selection;
      }
      if (weaponDatabase.WeaponDataList.Count < count)
      {
        Debug.LogWarning("Not enough data in database for selection.");
        return weaponDatabase.WeaponDataList; // return all data if not enough
      }
      
      // 創建一個新的列表來存儲可用的資料,避免直接修改原始資料庫
      List<WeaponData> availableData = new List<WeaponData>(weaponDatabase.WeaponDataList);
      
      // 獲取玩家已有的武器
      if (playerWeaponData != null && playerWeaponData.WeaponDataList != null && playerWeaponData.WeaponDataList.Count == 3)
      {
        // 如果玩家已有三個武器，直接返回這三個武器
        return new List<WeaponData>(playerWeaponData.WeaponDataList);
      }
      
      // 如果玩家武器少於三個，則隨機選擇武器填滿選項
      for (int i = 0; i < count && availableData.Count > 0; i++)
      {
        int randomIndex = Random.Range(0, availableData.Count);
        selection.Add(availableData[randomIndex]);
        availableData.RemoveAt(randomIndex);
      }
      
      return selection;
    }
     public void ButtonClicked(int panelIndex)
    {
      ClosePanel();
      IncreasePlayerPower(selectedData[panelIndex]);
    }


    private void IncreasePlayerPower(WeaponData selectedData) // 增加玩家能力值
    {
      if(playerWeaponData.WeaponDataList.Contains(selectedData))
      {
        if(selectedData.level < 5)
        {
          playerWeaponData.UpdateWeaponLevel(selectedData, selectedData.level + 1);
        }
        else
        {
          //顯示武器滿等提示字樣
        }
      }
      else
      {
        // 如果玩家武器清單中沒有該武器，則添加到清單中
        if (playerWeaponData.WeaponDataList.Count < 3)
        {
          // 先设置武器等级为1
          selectedData.level = 1;
          // 然后添加到列表中
          var newList = new List<WeaponData>(playerWeaponData.WeaponDataList);
          newList.Add(selectedData);
          playerWeaponData.WeaponDataList = newList;
          Debug.Log("已將新武器 " + selectedData.skillName + " 添加到玩家武器清單中，初始等級為1");
        }
        else
        {
          Debug.LogWarning("玩家武器清單已滿，無法添加新武器");
        }
      }
    }

}
