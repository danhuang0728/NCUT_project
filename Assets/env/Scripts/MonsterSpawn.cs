using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class spawn : MonoBehaviour
{
    
    public GameObject monsterPrefab;   // 引用怪物
    public Transform[] spawnPoints;    // 生成點的陣列
    
    public Transform[] spawnCircle;    //圓形生成點的陣列
    
    public float spawnInterval = 5f;   // 生成間隔時間
    public float limit;
    private float timer;

    private int roomtime;
    private int monster;

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
}
