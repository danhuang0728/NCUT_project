using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBook_Prb : MonoBehaviour
{
    
    public float damage = 1f;
    public float knockbackForce = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            Debug.Log("monstertest:"+monster);
            if (monster != null)
            {
                // 造成傷害
                monster.HP -= damage;

                // 擊退效果
                Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                Rigidbody2D monsterRb = other.GetComponent<Rigidbody2D>();
                if (monsterRb != null)
                {
                    monsterRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                }
            }
            Destroy(gameObject);
        }
        if (other.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}
