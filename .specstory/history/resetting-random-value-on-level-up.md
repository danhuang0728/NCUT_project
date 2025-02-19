## SpecStory

## Resetting Random Value on Level Up (2025/2/20 上午12:50:58)

_**User**_

@EXPController.cs 我想要在升級時將@VariableData.cs中的 _randomValue值設為0 

---

_**Assistant**_

要在升级时将 `VariableData` 中的 `_randomValue` 值设为 0，你需要在 `EXPController` 中找到合适的地方调用 `VariableData` 的方法或直接访问其字段。假设你有一个 `VariableData` 的实例，你可以在 `CheckLevelUp` 方法中进行修改。

首先，确保 `EXPController` 中有对 `VariableData` 的引用。然后在升级时重置 `_randomValue`。以下是如何修改代码的示例：

### 修改 `EXPController.cs`

```csharp:Assets/env/Scripts/UI Scripts/EXPSystem/EXPController.cs
public class ExperienceSystem : MonoBehaviour
{
    // 其他代码...

    public VariableData variableData; // 添加对 VariableData 的引用

    void CheckLevelUp()
    {
        while (currentExperience >= GetRequiredExperience())
        {
            currentExperience -= GetRequiredExperience();
            currentLevel++; // 升级
            FindObjectOfType<SelectionManager>().OpenPanel();
            Debug.Log("Level Up! Now level: " + currentLevel);

            // 在这里重置 VariableData 中的 _randomValue
            if (variableData != null)
            {
                variableData.ResetRandomValue();
            }
        }
    }

    // 其他代码...
}
```

### 修改 `VariableData.cs`

在 `VariableData` 中添加一个方法来重置 `_randomValue`：

```csharp:Assets/env/Scripts/UI Scripts/ChooseSystem/VariableData.cs
public class VariableData : ScriptableObject
{
    // 其他代码...

    public void ResetRandomValue()
    {
        _randomValue = 0;
        Debug.Log("Random value reset to 0");
    }

    // 其他代码...
}
```

通过以上修改，当 `ExperienceSystem` 检测到升级时，它会调用 `VariableData` 的 `ResetRandomValue` 方法，将 `_randomValue` 重置为 0。确保在 Unity 编辑器中将 `VariableData` 的实例分配给 `ExperienceSystem` 的 `variableData` 字段。

---



---

