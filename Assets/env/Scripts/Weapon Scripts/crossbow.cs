using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crossbow : MonoBehaviour
{
    public GameObject Crossbow_prb;  //弩的預置物
    private Transform player;
    public float spawnInterval = 3f; // 每n秒執行一次
    private float spawnTimer;
    public bool Is_in_range = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.position;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            if (Is_in_range == true)
            {
                SpawnCrossbow();
                spawnTimer = spawnInterval; // 只在范围内重置计时器
            }
        }
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
}
