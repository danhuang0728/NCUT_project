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
            float speed = level * 1.01f; // 假設每等級的血量獎勵為 1.1 倍

            levels[i] = new LevelInfo
            {
                level = level,
                requiredExperience = requiredExperience,
                speed = speed
            };
        } 
    }
}