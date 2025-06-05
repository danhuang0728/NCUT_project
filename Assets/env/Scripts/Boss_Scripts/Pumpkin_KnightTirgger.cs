using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin_KnightTirgger : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Pumpkin_Knight_spawn.instance.PumpkinKnight_spawn();
        }
    }
}
