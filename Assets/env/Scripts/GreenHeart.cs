using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GreenHeart : MonoBehaviour
{
    private PlayerControl playerControl;
    private healthbar healthbar;
    void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>(); //初始化讀取腳本
        healthbar = FindObjectOfType<healthbar>();
    }

    
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerControl.HP += 20;
            if (playerControl.HP > healthbar.slider.maxValue)
            {
                playerControl.HP = healthbar.slider.maxValue;
            }
            Destroy(gameObject);
        }
    }
}
