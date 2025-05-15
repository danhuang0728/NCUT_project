using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TetrisBlockTooltip : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI blockNameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI attributesText;
    public TextMeshProUGUI specialAbilityText;

    private Canvas tooltipCanvas;

    void Start()
    {
        tooltipCanvas = GetComponent<Canvas>();
        
        // 确保提示框初始状态为隐藏
        HideTooltip();
        
        // 设置提示框样式
        SetupTooltipStyle();
    }

    private void SetupTooltipStyle()
    {
        // 设置文本样式
        if (blockNameText != null)
        {
            blockNameText.fontSize = 24;
            blockNameText.color = Color.white;
            blockNameText.fontStyle = FontStyles.Bold;
        }

        if (descriptionText != null)
        {
            descriptionText.fontSize = 18;
            descriptionText.color = new Color(0.8f, 0.8f, 0.8f);
        }

        if (attributesText != null)
        {
            attributesText.fontSize = 16;
            attributesText.color = new Color(0.7f, 1f, 0.7f);  // 浅绿色
        }

        if (specialAbilityText != null)
        {
            specialAbilityText.fontSize = 16;
            specialAbilityText.color = new Color(1f, 0.8f, 0.2f);  // 金色
        }
    }

    public void ShowTooltip(TetrisBlockData blockData)
    {
        if (blockData == null) return;

        // 更新提示框内容
        blockNameText.text = blockData.blockName;
        descriptionText.text = blockData.description;
        
        // 更新属性文本
        string attributesString = "";
        foreach (var attribute in blockData.attributes)
        {
            if (attribute.Value != 0)  // 只显示非零属性
            {
                string sign = attribute.Value > 0 ? "+" : "";
                attributesString += $"{attribute.Key}: {sign}{attribute.Value}\n";
            }
        }
        attributesText.text = attributesString;
        
        // 更新特殊能力文本
        specialAbilityText.text = string.IsNullOrEmpty(blockData.specialAbility) ? 
            "" : $"特殊能力：{blockData.specialAbility}";
        
        // 显示提示框
        tooltipPanel.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}

[System.Serializable]
public class TetrisBlockData
{
    public string blockName;
    public string description;
    public Dictionary<string, float> attributes = new Dictionary<string, float>();
    public string specialAbility;
}