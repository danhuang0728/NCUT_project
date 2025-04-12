using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrDrops : MonoBehaviour
{
    public tetr_database tetr_database;
    private player_tetr_Manager player_tetr_Manager;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        player_tetr_Manager = FindObjectOfType<player_tetr_Manager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = tetr_database.tetr_sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")&&Input.GetKeyDown(KeyCode.E))
        {
            player_tetr_Manager.add_tetr_obj(tetr_database);
            Destroy(gameObject);
        }
    }
}
