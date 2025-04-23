using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class gold_coin : MonoBehaviour
{
    public character_value_ingame character_value_ingame;
    public TextMeshProUGUI gold_coin_text;
    private float lastGoldValue = 0f;

    void Start()
    {
        character_value_ingame = FindObjectOfType<character_value_ingame>();
        lastGoldValue = character_value_ingame.gold;
        gold_coin_text.text = Mathf.Round(lastGoldValue).ToString();
    }

    void Update()
    {
        float currentGold = character_value_ingame.gold;
        // 只有在金幣值改變時才更新UI
        if (currentGold != lastGoldValue)
        {
            lastGoldValue = currentGold;
            gold_coin_text.text = Mathf.Round(currentGold).ToString();
        }
    }
}
