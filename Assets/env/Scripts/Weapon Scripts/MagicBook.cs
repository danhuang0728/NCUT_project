using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBook : MonoBehaviour
{
    public GameObject bulletPrefab; // 子彈 Prefab
    public float fireRate = 1f; // 發射頻率 (每秒發射多少顆子彈)
    public float bulletSpeed = 5f; // 子彈速度
    private float fireTimer = 0f; // 發射計時器
    private float timer = 0f;



    void Start()
    {
        
       
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 每当timer达到1秒时发射
        if (timer >= 1f / fireRate)
        {
            Debug.Log(timer);
            FireBullet();
            timer = 0f; // 重置计时器
        }
    }

    void FireBullet()
    {
        GameObject nearestMonster = FindNearestMonster();
        if (nearestMonster == null)
        {
            Debug.LogWarning("没有找到最近的 Monster 目标，无法发射子弹！");
            return;
        }

        // 计算子弹起始位置 (在BOSS附近)
        Vector3 bulletPosition = transform.position;

        // 计算目标方向
        Vector2 direction = (nearestMonster.transform.position - transform.position).normalized;

        // 计算子弹的发射角度
        float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 产生子弹
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // 设置子弹速度
        bulletRb.velocity = direction * bulletSpeed;

        // 设置子弹的旋转角度
        bullet.transform.rotation = Quaternion.Euler(0, 0, fireAngle);
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
