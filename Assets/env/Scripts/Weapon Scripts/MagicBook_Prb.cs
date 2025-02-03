using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBook_Prb : MonoBehaviour
{
    
    public float damage = 1f;
    public float knockbackForce = 1f;
    private PlayerControl playerController;
    public MagicBook magicBook;
    [Range(1f,100f)]
    public float attractionForce = 10f; // 可根据需要调整力的大小


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
        if (magicBook.level == 6)
        {
            GameObject nearestMonster = FindNearestMonster();
            if (nearestMonster != null)
            {
                // 计算指向最近怪物的方向
                Vector2 direction = (nearestMonster.transform.position - transform.position).normalized;
                
                // 给物体施加一个恒定的力，使其向最近的怪物方向运动
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(direction * attractionForce);
                }

                if (rb != null && rb.velocity.sqrMagnitude > 0.001f)
                {
                    // 根據 Rigidbody2D 的速度方向計算目標角度（單位：度）
                    float targetAngle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                    // 獲取當前物體的 Z 軸旋轉角度
                    float currentAngle = transform.eulerAngles.z;
                    // 使用線性插值平滑轉向目標角度
                    float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, 5 * Time.deltaTime);
                    // 更新物體的旋轉
                    transform.rotation = Quaternion.Euler(0, 0, newAngle);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Renderer renderer = other.GetComponent<Renderer>();
            Material material = renderer.material;
            material.color = Color.red;
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            Debug.Log("monstertest:"+monster);
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
            }
            Destroy(gameObject);
        }
        if (other.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
    GameObject FindNearestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestMonster = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float distanceToMonster = Vector3.Distance(transform.position, monster.transform.position);
            if (distanceToMonster < shortestDistance)
            {
                shortestDistance = distanceToMonster;
                nearestMonster = monster;
            }
        }

        return nearestMonster;
    }
}
