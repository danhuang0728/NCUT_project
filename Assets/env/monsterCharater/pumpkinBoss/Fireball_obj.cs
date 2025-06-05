using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_obj : MonoBehaviour
{
    private float damage = 10;
    public PlayerControl playerControl;
    void Start()
    {
        playerControl = GameObject.Find("player1").GetComponent<PlayerControl>();
    }

    
    void Update()
    {
        //最大生命20%傷害
        damage = (100+playerControl.Calculating_Values_damage) * 0.05f;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerControl.TakeDamage(damage);
        }
        if(other.gameObject.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}
