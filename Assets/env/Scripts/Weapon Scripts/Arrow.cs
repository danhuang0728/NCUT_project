using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
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
        if (other.CompareTag("Monster"))
        {
            Renderer renderer = other.GetComponent<Renderer>();
            Material material = renderer.material;
            material.color = Color.red;
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            //Debug.Log("monstertest:"+monster);
            BossFlower bossFlower = other.GetComponent<BossFlower>();
            if (bossFlower != null)

            {
                Renderer renderer_flower = bossFlower.GetComponent<Renderer>();
                bossFlower.HP -= 1;
                playerController.SetBoolWithDelay_void(renderer_flower.material, renderer_flower);
            }
            if (monster != null)

            {
                    
                // 造成傷害
                monster.HP -= damage;
                //閃白效果
                playerController.SetBoolWithDelay_void(material, renderer);
                // 擊退效果
                Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                Rigidbody2D monsterRb = other.GetComponent<Rigidbody2D>();
                if (monsterRb != null)
                {
                    monsterRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                }
                // 檢查是否有燃燒效果
                PlayerControl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
                if (player != null && player.hasBurnEffect)
                {
                    monster.burn_monster_start(player.burnDuration);
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
