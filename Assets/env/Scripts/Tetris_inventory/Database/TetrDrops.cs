using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrDrops : MonoBehaviour
{
    public tetr_database tetr_database;
    public int gold_cost; // 花費金幣
    private player_tetr_Manager player_tetr_Manager;
    private character_value_ingame Character_value_ingame;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        player_tetr_Manager = FindObjectOfType<player_tetr_Manager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = tetr_database.tetr_sprite;
        Character_value_ingame = FindObjectOfType<character_value_ingame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")&&Input.GetKeyDown(KeyCode.E))
        {
            if (Character_value_ingame.gold >= gold_cost)
            {
                player_tetr_Manager.add_tetr_obj(tetr_database);
                Character_value_ingame.gold -= gold_cost;
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("金幣不足");
            }
        }
    }
}
