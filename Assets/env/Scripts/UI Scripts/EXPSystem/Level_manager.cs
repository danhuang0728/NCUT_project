using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private ExperienceSystem experienceSystem;

    void Start()
    {
        // 获取 ExperienceSystem 组件的引用
        experienceSystem = GetComponent<ExperienceSystem>();
        
        // 如果 ExperienceSystem 不在同一个游戏对象上，可以使用 Find 或其他方式查找
        if (experienceSystem == null)
        {
            experienceSystem = FindObjectOfType<ExperienceSystem>();
        }

        if (experienceSystem == null)
        {
            Debug.LogError("Cannot find ExperienceSystem!");
            return;
        }
    }

    // 获取当前等级
    public int GetCurrentLevel()
    {
        if (experienceSystem != null)
        {
            return experienceSystem.currentLevel;
        }
        return 0;
    }

    // 检查是否达到指定等级
    public bool IsLevelReached(int targetLevel)
    {
        return GetCurrentLevel() >= targetLevel;
    }

    // 增加经验值
    public void AddExperience(int amount)
    {
        if (experienceSystem != null)
        {
            experienceSystem.IncreaseExperience(amount);
        }
    }

    // 示例：获取到下一级所需经验值
    public int GetExperienceToNextLevel()
    {
        if (experienceSystem != null)
        {
            return experienceSystem.GetRequiredExperience() - experienceSystem.currentExperience;
        }
        return 0;
    }
}