using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ExperienceSystem : MonoBehaviour
{
    public LevelData levelData;
    public int currentLevel = 1;
    public int currentExperience = 0;

    public Image experienceBarImage; // 使用 Image 而非 Slider
    public TMPro.TextMeshProUGUI expText; // 顯示當前經驗值的 Text

    private List<LevelData.LevelInfo> levelInfos;

    void Start()
    {
        if (levelData == null)
        {
            Debug.LogError("Error: LevelData has not been assigned!");
            return;
        }

        levelInfos = new List<LevelData.LevelInfo>(levelData.levels);
        UpdateUI();
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        CheckLevelUp();
        UpdateUI();
    }

    void CheckLevelUp()
    {
        while (currentExperience >= GetRequiredExperience())
        {
            currentExperience -= GetRequiredExperience();
            currentLevel++;
            Debug.Log("Level Up! Now level: " + currentLevel);
        }
    }

    public int GetRequiredExperience()
    {
         LevelData.LevelInfo currentLevelInfo = levelInfos.Find(levelInfo => levelInfo.level == currentLevel);

         if (currentLevelInfo.level == 0)
        {
            Debug.LogError("找不到等級: " + currentLevel + " 的相關資訊!");
            return 0;
        }

        return currentLevelInfo.requiredExperience;
    }

    void UpdateUI()
    {
        if (experienceBarImage != null)
        {
            // 計算經驗條的填滿比例
             float fillAmount = (float)currentExperience / GetRequiredExperience();
            experienceBarImage.fillAmount = fillAmount;
        }


        if (expText != null)
        {
           expText.text = currentExperience + "/" + GetRequiredExperience();
        }
    }

    public void IncreaseExperience(int amount) 
    {
        AddExperience(amount);
    }
}