using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class spawn : MonoBehaviour
{
    
    public GameObject monsterPrefab;   // 引用怪物
    public Transform[] spawnPoints;    // 生成點的陣列
    
    public Transform[] spawnCircle;    //圓形生成點的陣列

    public Transform[] spawnCircle2;    //圓形生成點的陣列
    
    public float spawnInterval = 5f;   // 生成間隔時間
    public float limit;
    private float timer;

    private int roomtime;
    private int monster;
    private float circleTimer = 10f; // 圓形生成計時器
    private float currentCircleTime = 0f; // 當前圓形計時
    private float circleRadius = 1f; // 生成圓的半徑
    private int circleMonsterCount = 30; // 圓周上生成的怪物數量

    void Start()
    {
        timer = spawnInterval;
        monster = 1; 
        //roomtime = 180;
    }

    void Update()
    {   
        //roomtime = roomtime-1;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (monster <= limit)
            {
                SpawnMonster();
                timer = spawnInterval;
            }
        }
            // 新增：處理圓形生成計時
        currentCircleTime += Time.deltaTime;
        if (currentCircleTime >= circleTimer)
            {
            //SpawnCircleAroundPlayer();
            currentCircleTime = 0f;
            }
        
        /*
        if (roomtime == 170 || roomtime == 60 || roomtime == 40 || roomtime == 15)
        {
            Debug.Log("Circle");
            Circle();    // 執行Circle怪物重生
        }
        */

        
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
        
        if (monster == 10 || monster == 25 || monster == 35 || monster == 40 || monster == 50 || monster == 60 || monster == 70 )
        {
            Circle();    // 執行Circle怪物重生
            Debug.Log("生成Circle");
        }
        
        if (monster == 25 || monster == 50 || monster == 60 || monster == 70 )
        {
            Circle2();    // 執行Circle怪物重生
            Debug.Log("生成Circle2");
        }
    }

    void Circle()
    {
        // 遍歷所有生成點
        int spawnIndex2 = spawnCircle.Length;
        foreach (Transform spawnPoint2 in spawnCircle)
        {
            // 在每個生成點生成怪物
            GameObject copyMonster = Instantiate(monsterPrefab, spawnPoint2.position, Quaternion.identity);
            copyMonster.SetActive(true);
        }
    }

    void Circle2()
    {
        // 遍歷所有生成點
        int spawnIndex2 = spawnCircle2.Length;
        foreach (Transform spawnPoint3 in spawnCircle2)
        {
            // 在每個生成點生成怪物
            GameObject copyMonster = Instantiate(monsterPrefab, spawnPoint3.position, Quaternion.identity);
            copyMonster.SetActive(true);
        }
    }



    // 新增：在玩家周圍生成圓形怪物的方法
private void SpawnCircleAroundPlayer()
{
    if (monster >= limit) return;
    
    // 獲取玩家位置（假設玩家標籤為"Player"）
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    if (player == null) return;
    
    Vector3 playerPos = player.transform.position;
    
    // 在圓周上均勻生成怪物
    for (int i = 0; i < circleMonsterCount; i++)
    {
        if (monster >= limit) break;
        
        // 計算圓周上的位置
        float angle = i * (360f / circleMonsterCount);
        float radian = angle * Mathf.Deg2Rad;
        
        Vector3 spawnPos = playerPos + new Vector3(
            Mathf.Cos(radian) * circleRadius,
            0,
            Mathf.Sin(radian) * circleRadius
        );
        
        // 生成怪物
        Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
        monster++;
    }
}
}


