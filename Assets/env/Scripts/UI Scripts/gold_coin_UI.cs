using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class gold_coin : MonoBehaviour
{
    public character_value_ingame character_value_ingame;
    public TextMeshProUGUI gold_coin_text;
    void Start()
    {
        character_value_ingame = FindObjectOfType<character_value_ingame>();
        gold_coin_text.text = Mathf.Round(character_value_ingame.gold).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        gold_coin_text.text = Mathf.Round(character_value_ingame.gold).ToString();
    }
}
