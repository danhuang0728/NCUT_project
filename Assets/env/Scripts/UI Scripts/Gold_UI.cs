using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Gold_UI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public character_value_ingame Character_value_ingame;
    // Start is called before the first frame update
    void Start()
    {
        Character_value_ingame = GameObject.Find("player1").GetComponent<character_value_ingame>();
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = Character_value_ingame.gold.ToString();
    }
}
