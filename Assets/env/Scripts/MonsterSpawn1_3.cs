using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn1_3 : MonoBehaviour
{
    
    public GameObject monsterPrefab;   // 引用怪物
    public Transform[] spawnPoints;    // 生成點的陣列
    public float spawnInterval = 5f;   // 生成間隔時間
    public float limit;
    private float timer;

    private int monster;

    void Start()
    {
        timer = spawnInterval;
        monster = 1; 
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (monster <= limit)
            {
                SpawnMonster();
                timer = spawnInterval;
            }
        }
    }

    void SpawnMonster()
    {
        // 從生成點中隨機選擇一個點
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // 生成怪物
        GameObject copyMonster = Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
        copyMonster.SetActive(true);
        monster++;
    }
}
