## SpecStory

## 重置UpdateUI方法中的_randomValue (2025/2/20 上午1:14:02)

_**User**_

幫我在@SelectionManager.cs 的UpdateUI方法裡 將randomValue設為0

---

_**Assistant**_

在 `SelectionManager.cs` 文件的 `UpdateUI` 方法中，您希望将 `_randomValue` 的值设为 0。以下是更新后的代码片段：

```csharp:Assets/env/Scripts/UI Scripts/ChooseSystem/SelectionManager.cs
public void UpdateUI()
{
    if (variableDatabase == null || variableDatabase.variableDataList.Count < 3)
    {
        Debug.LogError("Variable Database is not properly set up or doesn't have enough data.");
        return;
    }
    // 將 _randomValue 的值設為 0
    foreach (var data in variableDatabase.variableDataList)
    {
        data.ResetRandomValue();
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
```

同时，您需要在 `VariableData.cs` 文件中添加一个方法来重置 `_randomValue`：

```csharp:Assets/env/Scripts/UI Scripts/ChooseSystem/VariableData.cs
public void ResetRandomValue()
{
    _randomValue = 0;
}
```

这样，每次调用 `UpdateUI` 方法时，所有 `VariableData` 实例的 `_randomValue` 都会被重置为 0。

---



---

