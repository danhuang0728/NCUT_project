using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_arrow : MonoBehaviour
{
    public float damage = 1f;
    public float knockbackForce = 1f;
    private PlayerControl playerController;


    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerControl>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            playerController.TakeDamage(damage);
            Destroy(gameObject);
        }
        if (other.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}

