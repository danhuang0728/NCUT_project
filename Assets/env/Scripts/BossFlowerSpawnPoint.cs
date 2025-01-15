using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlowerSpawnPoint : MonoBehaviour
{
    public GameObject monsterPrefab;   // 引用怪物
    public Transform[] spawnPoints;    // 生成點的陣列
    public float spawnInterval = 10f;   // 生成間隔時間
    private BossFlower[] flower; // 用來存放抓取花的list
    public float limit;
    private float timer;

    private int monster;

    void Start()
    {
        SpawnMonster();  //一次生兩朵花
        SpawnMonster();
        timer = spawnInterval;
        monster = 1; 
        
    }

    void Update()
    {    
        BossFlower[] flower = FindObjectsOfType<BossFlower>(); //抓取已生出來的花
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (monster <= limit &&  flower.Length < 1)  // 場上還有花或是花已達總生成量就不會再生
            {
                SpawnMonster();  //一次生兩朵花
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
