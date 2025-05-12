using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossbow_Prb : MonoBehaviour
{
    public GameObject bulletPrefab; // 子彈 Prefab
    public float fireRate = 1f; // 發射頻率 (每秒發射多少顆子彈)
    public float bulletSpeed = 5f; // 子彈速度
    private float fireTimer = 0f; // 發射計時器
    private float timer = 0f;
    private crossbow crossbow_script;
    void Start()

    {
        crossbow_script = GameObject.Find("crossbow").GetComponent<crossbow>();
        GameObject nearestMonster = FindNearestMonster();
        if (nearestMonster != null)
        {
            // 计算目标方向
            Vector2 direction = (nearestMonster.transform.position - transform.position).normalized;

            // 计算子弹的发射角度
            float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 45f; // 轉向-45度
            transform.rotation = Quaternion.Euler(0, 0, fireAngle);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 每当timer达到1秒时发射
        if (timer >= 1f / fireRate)
        {
            FireBullet();
            timer = 0f; // 重置计时器
        }

    }

    void FireBullet()
    {
        GameObject nearestMonster = FindNearestMonster();
        if (nearestMonster == null)
        {
            Debug.LogWarning("沒有找到最近的 Monster 目標，無法發射子彈！");
            return;
        }

        // 計算與怪物的距離
        float distanceToMonster = Vector3.Distance(transform.position, nearestMonster.transform.position);
        
        // 設定最大射程範圍
        float maxFireRange = 12f; // 可以根據需要調整射程
        
        // 檢查怪物是否在射程範圍內
        if (distanceToMonster > maxFireRange)
        {
            // 怪物超出射程範圍，不發射子彈
            return;
        }
        //發射音效
        AudioManager.Instance.PlaySFX("crossbow_shoot");

        // 計算子彈起始位置
        Vector3 bulletPosition = transform.position;

        if(crossbow_script.is_levelUP == true){
            float randomAngle = Random.Range(-15f, 15f);
            Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * (nearestMonster.transform.position - transform.position).normalized;
            float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 45f; // 轉向-45度
            transform.rotation = Quaternion.Euler(0, 0, fireAngle);

            // 產生子彈
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
            bullet.SetActive(true);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = direction * bulletSpeed;
            bullet.transform.rotation = Quaternion.Euler(0, 0, fireAngle);
        }
        else
        {
            // 計算目標方向
            Vector2 direction = (nearestMonster.transform.position - transform.position).normalized;
            // 計算子彈的發射角度
            float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 45f; // 轉向-45度
            transform.rotation = Quaternion.Euler(0, 0, fireAngle);

            // 產生子彈
            GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
            bullet.SetActive(true);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = direction * bulletSpeed;
            bullet.transform.rotation = Quaternion.Euler(0, 0, fireAngle);
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
