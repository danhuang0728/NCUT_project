using UnityEngine;

[CreateAssetMenu(fileName = "VariableData", menuName = "Custom/Variable Data")]
public class VariableData : ScriptableObject
{
    public enum PowerUpType
    {
        None,
        Attack,
        Defense,
        Speed,
        Health
        // 可以加入更多能力值類型
    }

    public string variableName;
    public int intValue;
    public float floatValue;
    public string stringValue;
    public Sprite image;
    public int powerIncreaseAmount;
    public string description;
    public PowerUpType powerUpType; // 新增能力值類型欄位
    // 可以加入更多不同類型的變數
}