using UnityEngine;

[CreateAssetMenu(fileName = "VariableData", menuName = "Custom/Variable Data")]
public class VariableData : ScriptableObject
{
    public enum PowerUpType
    {
        None,
        Damage,
        Critical_Damage,
        Critical_Hit_Rate,
        Speed,
        Health,
        Cooldown,
        Life_Steal,
        Gold,
        // 可以加入更多能力值類型
    }
    //新增稀有度
    public enum Rarity
    {
        Common,
        Uncommon,
        Epic,
        Legendary
    }
    public string variableName;
    public string stringValue;
    //最大最小值
    public float minValue;
    public float maxValue;
    public Sprite image;
    private float _randomValue;
    public float powerIncreaseAmount
    {
        get
        {
            if (_randomValue == 0)
            {
                _randomValue = Random.Range(minValue, maxValue + 1);
            }
            return _randomValue;
        }
    }
    public string description;
    public PowerUpType powerUpType; // 新增能力值類型欄位
    public Rarity rarity; //新增稀有度
    // 可以加入更多不同類型的變數
}