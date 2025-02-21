using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    public GameObject bulletPrefab; // 子彈 Prefab
    public Boomerang_Prb boomerang_Prb;
    private character_value_ingame characterValuesIngame;
    public Character_Values_SETUP characterValues;
    private float fireRate = 1f; // 發射頻率 (每秒發射多少顆子彈)
    public float bulletSpeed = 15f; // 子彈速度
    private float timer = 0f;
    public bool Is_in_range = false;
    [Range(1, 6)]
    public int level; // 武器等級 (6為無限)
    public bool is_levelUP = false;
    private int count = 1;
    private Transform player_t;



    void Start()
    {
        player_t = GameObject.Find("player1").transform;
        characterValuesIngame = GameObject.Find("player1").GetComponent<character_value_ingame>();
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
                StartCoroutine(Fire());
            }
            // 重置计时器
            timer = 0f;



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

        // 计算子弹起始位置 (在BOSS附近)
        Vector3 bulletPosition = transform.position;

        // 计算目标方向
        Vector2 direction = (nearestMonster.transform.position - transform.position).normalized;

        // 计算子弹的发射角度
        float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 产生子弹
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
        bullet.SetActive(true);
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
    public void ProcessLevel(int level)
    {
        if(level == 1)
        {
            fireRate = 0.33f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            boomerang_Prb.damage = 5;
            boomerang_Prb.MaxBounce = 3;
            count = 1;
        }
        else if(level == 2)
        {
            fireRate = 0.5f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            boomerang_Prb.damage = 10;
            boomerang_Prb.MaxBounce = 3;
            count = 1;
        }


        else if(level == 3)
        {
            fireRate = 1f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            boomerang_Prb.damage = 20;
            boomerang_Prb.MaxBounce = 5;
            count = 1;
        }

        else if(level == 4)
        {
            fireRate = 1f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            boomerang_Prb.damage = 30;
            boomerang_Prb.MaxBounce = 7;
            count = 2;
        }
        else if(level == 5)
        {
            fireRate = 1f + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
            boomerang_Prb.damage = 50;
            boomerang_Prb.MaxBounce = 7;
            count = 3;
        }
        if(is_levelUP == true)
        {
            boomerang_Prb.MaxBounce = 1000;
        }

    }
    IEnumerator Fire()

    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.15f);
            FireBullet();
        }
    }


}
