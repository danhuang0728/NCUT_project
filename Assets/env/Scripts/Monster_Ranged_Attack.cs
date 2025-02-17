using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Ranged_Attack : MonoBehaviour
{
    public GameObject bullet_prefab;
    private NormalMonster_setting normalMonster;
    public float bulletSpeed = 6f;
    private bool is_in_range = false;
    private float timer = 0f;
    public float fireRate = 1f;
    public float attackRange = 5f; // 攻擊範圍半徑
    public float angle_Offset = 0f; //偏移角度補償
    private float init_speed;
    void Start()
    {
        normalMonster = GetComponent<NormalMonster_setting>();
        init_speed = normalMonster.movespeed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // 每当timer达到1秒时发射
        if (timer >= 1f / fireRate)
        {
            if (is_in_range == true)
            {
                FireBullet();
            }
            // 重置计时器
            timer = 0f;
        }
        // 檢查玩家是否在攻擊範圍內
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= attackRange)
            {
                normalMonster.movespeed = 0;
                is_in_range = true;
            }
            else
            {
                normalMonster.movespeed = init_speed;
                is_in_range = false;
            }
        }
    }
    void FireBullet()
    {
        GameObject nearestPlayer = FindNearestPlayer();
        if (nearestPlayer == null)
        {
            Debug.LogWarning("没有找到最近的 Player 目标，无法发射子弹！");
            return;
        }

        // 计算子弹起始位置 
        Vector3 bulletPosition = transform.position;

        // 计算目标方向
        Vector2 direction = (nearestPlayer.transform.position - transform.position).normalized;

        // 计算子弹的发射角度
        float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 产生子弹
        GameObject bullet = Instantiate(bullet_prefab, bulletPosition, Quaternion.identity);
        bullet.SetActive(true);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // 设置子弹速度
        bulletRb.velocity = direction * bulletSpeed;

        // 设置子弹的旋转角度
        bullet.transform.rotation = Quaternion.Euler(0, 0, fireAngle + angle_Offset);
    }
    GameObject FindNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player;
            }
        }

        return nearestPlayer;
    }
    


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
