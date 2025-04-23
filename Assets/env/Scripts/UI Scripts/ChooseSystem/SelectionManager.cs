using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;


public class SelectionManager : MonoBehaviour
{
    public VariableDatabase variableDatabase; // 新增能力提升資料庫
    public GameObject[] optionPanels; // 三個選項 Panel
    public GameObject[] optionBorders; // 三個選項 border
    public TextMeshProUGUI[] choose_TITLE; // 三個文字欄位
    public TextMeshProUGUI[] textFields; // 三個文字欄位
    public Image[] images;   // 三個圖片欄位
    public GameObject optionPanelParent; // 儲存選單的父物件
    public GameObject background; // 背景
    public Character_Values_SETUP characterValues; // 新增能力提升資料庫
    public character_value_ingame characterValuesIngame; // 新增能力提升資料庫
    private List<VariableData> selectedData; // 儲存選取的選項
    private bool isPanelOpen = false; // 確認選單是否打開



    void Start()
    {
      characterValuesIngame = GameObject.Find("player1").GetComponent<character_value_ingame>();

      UpdateUI();
      if (optionPanelParent != null)
        {
          background.SetActive(false);
          optionPanelParent.SetActive(false);
        }
    }
    void Update()
    {
      if(isPanelOpen == true)
      {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
          ButtonClicked(0);
        }

        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
          ButtonClicked(1);
        }

        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
          ButtonClicked(2);
        }
      }
    }

    public void OpenPanel()
    {
      UpdateUI();
      AudioManager.Instance.PlaySFX("Level_up");
      isPanelOpen = true;
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
      if (optionPanelParent != null)
        {
          UIstate.isAnyPanelOpen = false;
          optionPanelParent.SetActive(false);
          background.SetActive(false);
        }
        isPanelOpen = false;
        ResumeGame();
    }

     public void UpdateUI()
    {
      if (variableDatabase == null || variableDatabase.variableDataList.Count < 3)
        {
          Debug.LogError("Variable Database is not properly set up or doesn't have enough data.");
          return;
        }
      if(optionPanels.Length != 3 || textFields.Length != 3 || images.Length != 3){
        Debug.LogError("Panels or textFields or images not properly set up.");
        return;
      }
      // 隨機選取三個不同的資料
      selectedData = SelectRandomData(3);

      for(int i = 0; i < 3; i++){
        selectedData[i].OnValidate();
        if(selectedData[i] == null)
        {
            Debug.LogError("A null data was detected.");
            return;
        }
        
        // 設定文字內容
        choose_TITLE[i].text = selectedData[i].variableName;
        choose_TITLE[i].GetComponent<Graphic>().color = GetRarityColor(selectedData[i].rarity);
        if (selectedData[i].powerUpType == VariableData.PowerUpType.Gold || selectedData[i].powerUpType == VariableData.PowerUpType.Health)
        {
          textFields[i].text = selectedData[i].stringValue + "\n" + 
                               "+"+selectedData[i].description;
        }
        else
        {
          textFields[i].text = selectedData[i]. stringValue + "\n" + 
                               "+"+selectedData[i].description+"%";
        }
        // 根據稀有度設定顏色 ▼▼▼
        //textFields[i].color = GetRarityColor(selectedData[i].rarity); //文字顏色  
        optionPanels[i].GetComponent<Image>().color = GetRarityColor(selectedData[i].rarity); //選項背景顏色
        optionBorders[i].GetComponent<Image>().color = GetRarityColor(selectedData[i].rarity); //選項邊框顏色
        // 顏色設定結束 ▲▲▲

        images[i].sprite = selectedData[i].image;
      }
    }

    private List<VariableData> SelectRandomData(int count)
    {
      
      List<VariableData> selection = new List<VariableData>();
      if(variableDatabase == null || variableDatabase.variableDataList == null || variableDatabase.variableDataList.Count == 0)
        {
            Debug.LogError("No data found");
            return selection;
        }
      if (variableDatabase.variableDataList.Count < count)
      {
        Debug.LogWarning("Not enough data in database for selection.");
        return variableDatabase.variableDataList; // return all data if not enough
      }

      // 創建一個新的列表來存儲可用的資料,避免直接修改原始資料庫
      List<VariableData> availableData = new List<VariableData>(variableDatabase.variableDataList);

      for(int i = 0; i < count; i++)
      {
        //決定稀有度判斷
        int roll = Random.Range(1, 101);
        VariableData.Rarity rarity = VariableData.Rarity.Common;
        if (roll <= 1) //傳說稀有度 1%
        {
          rarity = VariableData.Rarity.Legendary;
        }
        if (roll > 1 && roll <= 11) //史詩稀有度10%
        {
          rarity = VariableData.Rarity.Epic;
        }
        if (roll > 11 && roll <= 36) //稀有稀有度25%
        {
          rarity = VariableData.Rarity.Uncommon;
        }
        if(roll > 36 ) //普通稀有度64% 
        {
          rarity = VariableData.Rarity.Common;
        }
        int randomIndex = Random.Range(0, availableData.Count);
        //判定是否為抽到的稀有度
        while(availableData[randomIndex].rarity != rarity)
        {
          randomIndex = Random.Range(0, availableData.Count);
        }
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


    private void IncreasePlayerPower(VariableData selectedData) // 增加玩家能力值
    {
      Debug.Log("增加能力值: " + selectedData.powerUpType);
      switch(selectedData.powerUpType)
      {
        case VariableData.PowerUpType.Damage:
        characterValuesIngame.damage += selectedData.powerIncreaseAmount;
        Debug.Log("Increase Damage:" + selectedData.powerIncreaseAmount);
        break;
        case VariableData.PowerUpType.Critical_Damage:
        characterValuesIngame.criticalDamage += selectedData.powerIncreaseAmount;
            Debug.Log("Increase Critical Damage:" + selectedData.powerIncreaseAmount);
        break;
        case VariableData.PowerUpType.Critical_Hit_Rate:
        characterValuesIngame.criticalHitRate += selectedData.powerIncreaseAmount;
          Debug.Log("Increase Critical Hit Rate:" + selectedData.powerIncreaseAmount);
        break;
        case VariableData.PowerUpType.Health:
        characterValuesIngame.health += selectedData.powerIncreaseAmount;
        Debug.Log("Increase Health:" + selectedData.powerIncreaseAmount);
        break;
        case VariableData.PowerUpType.Speed:
        characterValuesIngame.speed += selectedData.powerIncreaseAmount;
        Debug.Log("Increase Speed:" + selectedData.powerIncreaseAmount);
        break;
        case VariableData.PowerUpType.Cooldown:
        characterValuesIngame.cooldown += selectedData.powerIncreaseAmount;
        Debug.Log("Increase Cooldown:" + selectedData.powerIncreaseAmount);
        break;
        case VariableData.PowerUpType.Life_Steal:
        characterValuesIngame.lifeSteal += selectedData.powerIncreaseAmount;
        Debug.Log("Increase Life Steal:" + selectedData.powerIncreaseAmount);
        break;
        case VariableData.PowerUpType.Gold:
        characterValuesIngame.gold += selectedData.powerIncreaseAmount;
        Debug.Log("Increase Gold:" + selectedData.powerIncreaseAmount);
        break;
        default:
          Debug.LogWarning("Unknown power up type");
        break;
      }
    }

      private void PauseGame()
     {
        Time.timeScale = 0;
      }


      private void ResumeGame()
      {
        Time.timeScale = PlayerControl.N;
      }

    private Color32 GetRarityColor(VariableData.Rarity rarity)
    {
        switch(rarity)
        {
            case VariableData.Rarity.Common:
                return new Color32(255, 255, 255, 255);
            case VariableData.Rarity.Uncommon:
                return new Color32(0, 255, 0, 255);
            case VariableData.Rarity.Epic:
                return new Color32(128, 0, 128, 255);
            case VariableData.Rarity.Legendary:
                return new Color32(255, 215, 0, 255);
            default:
                return new Color32(255, 255, 255, 255);
        }
    }
}