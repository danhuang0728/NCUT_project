using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private ExperienceSystem experienceSystem;
    public LevelData levelData;
    private Weapon_Manager weaponManager;

    void Start()
    {
        // 获取 ExperienceSystem 组件的引用
        experienceSystem = GetComponent<ExperienceSystem>();
        weaponManager = FindObjectOfType<Weapon_Manager>();
        
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
            experienceSystem.AddExperience(amount);
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

    public float GetCurrentSpeed()
    {
        if (levelData == null || experienceSystem == null) return 0f;

        int currentLevel = experienceSystem.currentLevel;
        // 因為陣列索引從0開始，而等級從1開始，所以要減1
        int arrayIndex = currentLevel - 1;
        
        if (arrayIndex >= 0 && arrayIndex < levelData.levels.Length)
        {
            return levelData.levels[arrayIndex].speed;
        }
        
        Debug.LogWarning($"找不到等級 {currentLevel} 的速度值");
        return 0f;
    }

    public float GetCurrentAttack()
    {
        if (levelData == null || experienceSystem == null) return 0f;

        int currentLevel = experienceSystem.currentLevel;
        // 因為陣列索引從0開始，而等級從1開始，所以要減1
        int arrayIndex = currentLevel - 1;
        
        if (arrayIndex >= 0 && arrayIndex < levelData.levels.Length)
        {
            return levelData.levels[arrayIndex].attack;
        }
        
        Debug.LogWarning($"找不到等級 {currentLevel} 的攻擊值");
        return 0f;
    }

    // 获取武器总等级
    public int GetTotalWeaponLevel()
    {
        if (weaponManager == null) return 0;

        int totalLevel = 0;
        
        // 累加所有武器等级
        totalLevel += weaponManager.circle ? weaponManager.circle_level : 0;
        totalLevel += weaponManager.Boomerang ? weaponManager.boomerang_level : 0;
        totalLevel += weaponManager.MagicBook ? weaponManager.magicbook_level : 0;
        totalLevel += weaponManager.thrust ? weaponManager.thrust_level : 0;
        totalLevel += weaponManager.Axe ? weaponManager.axe_level : 0;
        totalLevel += weaponManager.crossbow ? weaponManager.crossbow_level : 0;
        totalLevel += weaponManager.main_hand1 ? weaponManager.main_hand_level1 : 0;
        totalLevel += weaponManager.main_hand2 ? weaponManager.main_hand_level2 : 0;

        return totalLevel;
    }
}
