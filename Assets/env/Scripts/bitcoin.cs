using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bitcoin : MonoBehaviour
{
    private PlayerControl playerControl;
    private character_value_ingame CharacterValuesIngame;
    void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>(); //初始化讀取腳本
        CharacterValuesIngame = FindObjectOfType<character_value_ingame>();
    }

    
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterValuesIngame.gold += 200; //增加金幣數量
            Destroy(gameObject);
        }
    }
}
