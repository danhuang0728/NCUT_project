using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossbow : MonoBehaviour
{
    public GameObject Crossbow_prb;  //弩的預置物
    private Transform player;
    public float spawnInterval = 3f; // 每n秒執行一次
    private character_value_ingame characterValuesIngame;
    public Character_Values_SETUP characterValues;
    private float spawnTimer;
    public bool Is_in_range = false;
    private Arrow arrow;
    private crossbow_Prb crossbow_Prb;
    [Range(1, 5)]
    public int level = 1;
    public bool is_levelUP = false;




    void Start()
    {
        characterValuesIngame = GameObject.Find("player1").GetComponent<character_value_ingame>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        arrow = transform.Find("arrow").GetComponent<Arrow>();
        crossbow_Prb = transform.Find("crossbow_body").GetComponent<crossbow_Prb>();
        transform.position = player.position;
    }



    void Update()
    {
        spawnInterval = 3f - (3f * (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage));
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            if (Is_in_range == true)
            {
                SpawnCrossbow();
                spawnTimer = spawnInterval; // 只在范围内重置计时器
            }
        }
        ProcessLevel(level,is_levelUP);
    }


    void SpawnCrossbow()
    {
        GameObject nearestMonster = FindNearestMonster();
        if (nearestMonster == null)
        {
            Debug.LogWarning("没有找到最近的 Monster 目标，无法发射子弹！");
            return;
        }

        // 计算目标方向
        Vector2 direction = (nearestMonster.transform.position - transform.position).normalized;
        GameObject crossbowInstance = Instantiate(Crossbow_prb, player.position, Quaternion.identity);
        crossbowInstance.SetActive(true);
        Destroy(crossbowInstance, 5f); // 5秒後銷毀

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
    public void ProcessLevel(int level,bool is_levelUP)
    {
        if(level == 1)
        {
            if(is_levelUP == false)
            {
                crossbow_Prb.fireRate = 1.5f;
                arrow.damage = 8;

            }
            else
            {
                crossbow_Prb.fireRate = 3f;
                arrow.damage = 10;
            }
        }
        else if(level == 2)
        {
            if(is_levelUP == false)
            {
                crossbow_Prb.fireRate = 2f;

                arrow.damage = 15;
            }
            else
            {
                crossbow_Prb.fireRate = 6f;
                arrow.damage = 10;
            }

        }

        else if(level == 3)
        {
            if(is_levelUP == false)
            {
                crossbow_Prb.fireRate = 4;
                arrow.damage = 25;
            }

            else
            {
                crossbow_Prb.fireRate = 9f;
                arrow.damage = 40;
            }



        }
        else if(level == 4)
        {
            if(is_levelUP == false)
            {
                crossbow_Prb.fireRate = 6;

                arrow.damage = 30;
            }
            else
            {
                crossbow_Prb.fireRate = 12f;
                arrow.damage = 50;
            }


        }
        else if(level == 5)
        {
            if(is_levelUP == false)
            {
                crossbow_Prb.fireRate = 8;

                arrow.damage = 50;
            }
            else
            {
                crossbow_Prb.fireRate = 15f;
                arrow.damage = 60;
            }
        }

    }
}
