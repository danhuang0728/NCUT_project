using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public struct LevelInfo
    {
        public int level;
        public int requiredExperience;
        public float speed;

        public float attack;

        // 你可以新增其他屬性
    }

    public LevelInfo[] levels;

    /// <summary>
    /// 初始化等級資料，根據指定的等級範圍計算升級需求經驗值
    /// </summary>
    /// <param name="maxLevel">最大等級</param>
    public void InitializeLevelData(int maxLevel)
    {
        levels = new LevelInfo[maxLevel];
        for (int i = 0; i < maxLevel; i++)
        {
            int level = i + 1; // 等級從 1 開始
            int requiredExperience = Mathf.RoundToInt(100 * Mathf.Pow(level, 1.5f) + 50); // 使用公式計算經驗需求
            
            float speed = CalculateSpeed(level); 
            float attack = CalculateAttack(level); // 添加攻擊力計算

            levels[i] = new LevelInfo
            {
                level = level,
                requiredExperience = requiredExperience,
                speed = speed,
                attack = attack // 設置攻擊力
            };
        } 
    }

    public static float CalculateSpeed(int level)  //照等級速度加成函數
    {
        float minSpeed = 5.0f;
        float maxSpeed = 10.0f;
        int minLevel = 1;
        int maxLevel = 100;

        // 使用公式計算速度
        float speed = minSpeed + (level - minLevel) * (maxSpeed - minSpeed) / (maxLevel - minLevel);
        return speed;
    }

    public static float CalculateAttack(int level)  // 照等級攻擊力加成函數
    {
        float minAttack = 3.0f;
        float maxAttack = 100.0f;
        int minLevel = 1;
        int maxLevel = 100;

        // 使用公式計算攻擊力
        float attack = minAttack + (level - minLevel) * (maxAttack - minAttack) / (maxLevel - minLevel);
        return attack;
    }
    
}