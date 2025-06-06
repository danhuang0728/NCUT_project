using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBook : MonoBehaviour
{
    public GameObject bulletPrefab; // 子彈 Prefab
    public MagicBook_Prb magicBook_Prb;
    public float fireRate = 1f; // 發射頻率 (每秒發射多少顆子彈)
    public float bulletSpeed = 15f; // 子彈速度
    public Character_Values_SETUP characterValues;
    private character_value_ingame characterValuesIngame;
    private float fireTimer = 0f; // 發射計時器
    private float timer = 0f;
    public bool Is_in_range = false;
    [Range(1,6)]
    public int level = 1; // 武器等級
    public bool is_levelUP1 = false;
    public bool is_levelUP2 = false;
    private Transform player_t;
    public int spreadBulletCount = 5; // 在90度范围内发射的子弹数量


    void Start()
    {
        characterValuesIngame = GameObject.Find("player1").GetComponent<character_value_ingame>();
        player_t = GameObject.Find("player1").transform;
    }

    void Update()
    {
        transform.position = player_t.transform.position;
        timer += Time.deltaTime;

        // 每当timer达到1秒时发射
        if (timer >= 1f / fireRate)
        {
            if (Is_in_range == true)
            {
                FireBullet();
            }
            timer = 0f; // 重置计时器
        }
        ProcessLevel(level);
        
    }

    void FireBullet()
    {
        GameObject nearestMonster = FindNearestMonster();
        if (nearestMonster == null)
        {
            Debug.LogWarning("没有找到最近的 Monster 目标，无法发射子弹！");
            return;
        }

        // 计算基础方向（保持原逻辑）
        Vector2 baseDirection = (nearestMonster.transform.position - transform.position).normalized;
        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
        AudioManager.Instance.PlaySFX("fireball");

        // 处理子弹数量为1的特殊情况
        if (spreadBulletCount == 1)
        {
            // 直接发射一颗正对目标的子弹
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.SetActive(true);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = baseDirection * bulletSpeed;
            bullet.transform.rotation = Quaternion.Euler(0, 0, baseAngle);
            return;
        }

        // 生成扇形子弹（数量≥2时）
        float totalSpread = 90f; // 总扩散角度
        float angleStep = totalSpread / (spreadBulletCount - 1); // 使用安全的分母
        float startAngle = baseAngle - totalSpread / 2; // 起始角度（左边）

        for (int i = 0; i < spreadBulletCount; i++)
        {
            // 计算当前子弹角度
            float currentAngle = startAngle + angleStep * i;
            
            // 将角度转换为方向
            Vector2 dir = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            ).normalized;

            // 生成子弹并设定子弹速度
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.SetActive(true);
            bullet.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                bulletRb.velocity = dir * bulletSpeed;
            }
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
    public void ProcessLevel(int level)
    {
        if(level == 1)
        {
            fireRate = 1f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            magicBook_Prb.damage = 10 ;
            spreadBulletCount = 1;
   
        }
        else if(level == 2)
        {
            fireRate = 2f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            magicBook_Prb.damage = 20 ;
            spreadBulletCount = 1;
           
        }
        else if(level == 3)
        {
            fireRate = 1.5f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            magicBook_Prb.damage = 30 ;
            spreadBulletCount = 3;
           
        }
        else if(level == 4)
        {
            fireRate = 1.75f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            magicBook_Prb.damage = 30 ;
            spreadBulletCount = 5;
         
        }
        else if(level == 5)
        {
            fireRate = 2f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            magicBook_Prb.damage = 30 ;
            spreadBulletCount = 7;
        }
        if(is_levelUP1 == true)
        {
            bulletSpeed = 5f;
        }
        else
        {
            bulletSpeed = 15f;
        }
        if(is_levelUP2 == true)
        {
            fireRate = 0.5f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            spreadBulletCount = 1;
        }
    }


}
