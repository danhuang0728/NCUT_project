using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust_evolution1_obj : MonoBehaviour
{
    public float damage = 1f;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Monster"))
        {
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            if(monster != null)
            {
                monster.HP -= damage;
            }
        }
    }
}
