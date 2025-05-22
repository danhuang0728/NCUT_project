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
        if (Vector2.Distance(transform.position, player.transform.position) < 1f && Input.GetKeyDown(KeyCode.E))
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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
