using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [Header("引用")]
    public Tetris_ability_Manager abilityManager;
    public TextMeshProUGUI statsText;
    
    [Header("设置")]
    public float updateInterval = 1; // 更新间隔，避免每帧更新
    public bool showPercentages = true; // 是否显示百分比值
    
    private float timer;
    
    void Start()
    {
        // 如果没有手动指定，自动查找
        if (abilityManager == null)
            abilityManager = FindObjectOfType<Tetris_ability_Manager>();
            
        if (statsText == null)
            statsText = GetComponent<TextMeshProUGUI>();
            
        UpdateStatsDisplay();
    }
    
    void Update()
    {
        {
            UpdateStatsDisplay();
        }
    }
    
    void UpdateStatsDisplay()
    {
        if (abilityManager == null || statsText == null)
            return;
            
        // 构建显示文本
        string displayText = "\n\n";
        
        // 添加各项属性
        displayText += FormatStat("攻擊", abilityManager.damage, "%");
        displayText += FormatStat("生命", abilityManager.health, "");
        displayText += FormatStat("爆擊傷害", abilityManager.criticalDamage, "%");
        displayText += FormatStat("爆擊機率", abilityManager.criticalHitRate, "%");
        displayText += FormatStat("速度", abilityManager.speed, "%");
        displayText += FormatStat("冷卻時間", abilityManager.cooldown, "%");
        displayText += FormatStat("生命偷取", abilityManager.lifeSteal, "%");
        displayText += FormatStat("承受傷害加成", abilityManager.damage_taken_addtion, "%");
        
        // 更新UI文本
        statsText.text = displayText;
    }
    
    string FormatStat(string name, float value, string percentage)
    {
        if (showPercentages && percentage != "")
            return $"{name}: {value} <color=#00FFFF>{percentage}</color>\n";
        else
            return $"{name}: {value}\n";
    }
}