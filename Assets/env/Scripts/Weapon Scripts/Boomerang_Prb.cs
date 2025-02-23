using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang_Prb : MonoBehaviour
{
    public float damage = 10;
    public float knockbackForce = 10;
    public BoomerangController boomerang;
    public PlayerControl playerControl;
    public int MaxBounce = 3;
    private int BounceCount = 0;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            BossFlower bossFlower = other.GetComponent<BossFlower>();
            Renderer renderer = other.GetComponent<Renderer>();
            
            if (bossFlower != null)
            {
                Renderer renderer_flower = bossFlower.GetComponent<Renderer>();
                bossFlower.HP -= damage;
                playerControl.SetBoolWithDelay_void(renderer_flower.material, renderer_flower);
                Destroy(gameObject);
            }

            if (monster != null)
            {
                // 造成傷害
                monster.HP -= damage;
                AudioManager.Instance.PlaySFX("Boomerrang_hit");

                // 擊退效果
                Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                Rigidbody2D monsterRb = other.GetComponent<Rigidbody2D>();
                Material Mat = renderer.GetComponent<Material>();
                playerControl.SetBoolWithDelay_void(Mat, renderer);

                if (monsterRb != null)
                {
                    monsterRb.AddForce(knockbackDir * knockbackForce * 10, ForceMode2D.Impulse);
                }
                if (BounceCount <= MaxBounce)
                {
                    // 找最近的Monster目標
                    GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                    GameObject nearestMonster = null;
                    float shortestDistance = Mathf.Infinity;

                    foreach (GameObject m in monsters)
                    {
                        // 排除當前已擊中的怪物
                        if (m != other.gameObject)
                        {
                            float distance = Vector3.Distance(transform.position, m.transform.position);
                            if (distance < shortestDistance)
                            {
                                shortestDistance = distance;
                                nearestMonster = m;
                            }
                        }
                    }

                    // 如果找到新目標，設定新的移動方向
                    if (nearestMonster != null)
                    {
                        Vector2 newDirection = (nearestMonster.transform.position - transform.position).normalized;
                        GetComponent<Rigidbody2D>().velocity = newDirection * boomerang.bulletSpeed;
                        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg);
                    }
                    else
                    {
                        // 如果沒有新目標，觸發子彈結束邏輯
                        Destroy(gameObject);
                    }
                    BounceCount++;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        
        if (other.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}
