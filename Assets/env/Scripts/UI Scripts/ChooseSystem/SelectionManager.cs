using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    public VariableDatabase variableDatabase; // 新增能力提升資料庫
    public GameObject[] optionPanels; // 三個選項 Panel
    public TextMeshProUGUI[] textFields; // 三個文字欄位
    public Image[] images;   // 三個圖片欄位
    public GameObject optionPanelParent; // 儲存選單的父物件
    private VariableData selectedOption; // 儲存選取的選項
    private bool isPanelOpen = false; // 確認選單是否打開


    void Start()
    {
        UpdateUI();
         if (optionPanelParent != null)
          {
            optionPanelParent.SetActive(false);
        }
    }

    public void OpenPanel()
    {
        UpdateUI();
        isPanelOpen = true;
         if (optionPanelParent != null)
          {
            optionPanelParent.SetActive(true);
        }
         PauseGame();

    }


      public void ClosePanel()
    {
          if (optionPanelParent != null)
          {
            optionPanelParent.SetActive(false);
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
      List<VariableData> selectedData = SelectRandomData(3);

      for(int i = 0; i < 3; i++){
        if(selectedData[i] == null)
        {
            Debug.LogError("A null data was detected.");
            return;
        }
        textFields[i].text = selectedData[i].variableName + "\n" +
                             selectedData[i].description;

        images[i].sprite = selectedData[i].image; // 設定圖片
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

        List<VariableData> availableData = new List<VariableData>(variableDatabase.variableDataList);

        for(int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availableData.Count);
            selection.Add(availableData[randomIndex]);
            availableData.RemoveAt(randomIndex);
        }
        return selection;
    }

      public void ButtonClicked(int panelIndex)
    {
        // 在這裡處理按鈕點擊事件，例如：
        Debug.Log("Panel " + (panelIndex+1) + " clicked!");
        selectedOption = GetSelectedDataByIndex(panelIndex);
         if(selectedOption != null)
            {
              IncreasePlayerPower(selectedOption);
             ClosePanel();
             }
         else{
            Debug.LogError("Selected option is null.");
           }

    }


     private VariableData GetSelectedDataByIndex(int index)
    {
        if(variableDatabase == null || variableDatabase.variableDataList == null || variableDatabase.variableDataList.Count <= index)
        {
          Debug.LogError("Data not found for index: " + index);
           return null;
        }
        List<VariableData> selectedData = SelectRandomData(3);
        return selectedData[index];
    }

     // 假設你已經有一個 PlayerManager 來管理玩家的能力值
    private void IncreasePlayerPower(VariableData selectedData)
    {
      // 在這裡，你可以呼叫你的 PlayerManager 來增加玩家的能力值
      // 這裡只是一個示例，你需要根據你的專案來實作
      Debug.Log("增加能力值: " + selectedData.powerIncreaseAmount);
      // 示例：呼叫 PlayerManager 的增加能力值函式
      switch(selectedData.powerUpType)
      {
          case VariableData.PowerUpType.Attack:
          //PlayerManager.Instance.IncreaseAttack(selectedData.powerIncreaseAmount);
          Debug.Log("Increase Attack:" + selectedData.powerIncreaseAmount);
          break;
          case VariableData.PowerUpType.Defense:
           // PlayerManager.Instance.IncreaseDefense(selectedData.powerIncreaseAmount);
             Debug.Log("Increase Defense:" + selectedData.powerIncreaseAmount);
          break;
           case VariableData.PowerUpType.Speed:
            //PlayerManager.Instance.IncreaseSpeed(selectedData.powerIncreaseAmount);
             Debug.Log("Increase Speed:" + selectedData.powerIncreaseAmount);
          break;
           case VariableData.PowerUpType.Health:
             //PlayerManager.Instance.IncreaseHealth(selectedData.powerIncreaseAmount);
            Debug.Log("Increase Health:" + selectedData.powerIncreaseAmount);
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
        Time.timeScale = 1;
      }
}