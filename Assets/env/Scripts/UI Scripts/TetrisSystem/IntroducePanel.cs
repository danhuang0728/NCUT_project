using UnityEngine;
using UnityEngine.UI;

public class ValueDisplayPanel : InventoryPanel
{
    public Text valueText;

    public void UpdateValue(int value)
    {
        valueText.text = "Current Value: " + value.ToString();
    }
}