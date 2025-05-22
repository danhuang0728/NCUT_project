using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrDrops : MonoBehaviour
{
    public tetr_database tetr_database;
    public int gold_cost; // 花費金幣
    private player_tetr_Manager player_tetr_Manager;
    private GameObject player;
    private character_value_ingame Character_value_ingame;
    private SpriteRenderer spriteRenderer;
    private bool isPlayerInRange = false; // 新增：跟踪玩家是否在范围内

    void Start()
    {
        player_tetr_Manager = FindObjectOfType<player_tetr_Manager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = tetr_database.tetr_sprite;
        Character_value_ingame = FindObjectOfType<character_value_ingame>();
        player = FindObjectOfType<PlayerControl>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryPurchaseItem();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    } 
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void TryPurchaseItem()
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
