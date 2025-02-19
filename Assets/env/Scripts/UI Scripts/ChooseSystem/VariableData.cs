using UnityEngine;
using System.Collections;

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
                _randomValue = Mathf.Round(Random.Range(minValue, maxValue + 1) * 10f) / 10f; //四捨五入到小數點後一位
                OnValidate();
                Debug.Log("randomValue:"+_randomValue);
            }
            return _randomValue;
            
        }
    }
    public string description;

    public PowerUpType powerUpType; // 新增能力值類型欄位
    public Rarity rarity; //新增稀有度
    // 可以加入更多不同類型的變數

    public void OnValidate()
    {
        _randomValue = Mathf.Round(Random.Range(minValue, maxValue + 1) * 10f) / 10f; //四捨五入
        description = _randomValue.ToString("F1"); // F1 表示顯示一位小數
    }

    public void ResetRandomValue()
    {
        _randomValue = 0;
        description = "";
    }
}