using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class spawn : MonoBehaviour
{
    
    public GameObject monsterPrefab;   // 引用怪物
    private GameObject monster_warningIcon;   // 生怪icon
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
    private float initialSpawnInterval = 5f;  // 初始生成間隔
    private float roomTime = 180f;           // 房間初始時間

    void Start()
    {
        timer = spawnInterval;
        monster = 1; 
        initialSpawnInterval = spawnInterval; // 保存初始生成間隔
        roomTime = 180f;
        monster_warningIcon = GameObject.Find("XX");
    
    }

    void Update()
    {   
        // 更新房間時間
        roomTime -= Time.deltaTime;
        
        // 根據剩餘時間調整生成間隔
        UpdateSpawnInterval();
        
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
        StartCoroutine(SpawnMonsterWithWarning());
    }

    IEnumerator SpawnMonsterWithWarning()
    {
        // 從生成點中隨機選擇一個點
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];

        // 先生成警告圖示
        GameObject warning = Instantiate(monster_warningIcon, spawnPoint.position, Quaternion.identity);
        warning.SetActive(true);
        
        // 等待2秒
        yield return new WaitForSeconds(2f);
        
        // 刪除警告圖示
        Destroy(warning);

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
        StartCoroutine(CircleWithWarning());
    }

    IEnumerator CircleWithWarning()
    {
        List<GameObject> warnings = new List<GameObject>();
        
        // 先生成所有警告圖示
        foreach (Transform spawnPoint2 in spawnCircle)
        {
            GameObject warning = Instantiate(monster_warningIcon, spawnPoint2.position, Quaternion.identity);
            warnings.Add(warning);
        }

        // 等待2秒
        yield return new WaitForSeconds(2f);

        // 刪除所有警告圖示並生成怪物
        for (int i = 0; i < spawnCircle.Length; i++)
        {
            Destroy(warnings[i]);
            GameObject copyMonster = Instantiate(monsterPrefab, spawnCircle[i].position, Quaternion.identity);
            copyMonster.SetActive(true);
        }
    }

    void Circle2()
    {
        StartCoroutine(Circle2WithWarning());
    }

    IEnumerator Circle2WithWarning()
    {
        List<GameObject> warnings = new List<GameObject>();
        
        // 先生成所有警告圖示
        foreach (Transform spawnPoint3 in spawnCircle2)
        {
            GameObject warning = Instantiate(monster_warningIcon, spawnPoint3.position, Quaternion.identity);
            warnings.Add(warning);
        }

        // 等待2秒
        yield return new WaitForSeconds(2f);

        // 刪除所有警告圖示並生成怪物
        for (int i = 0; i < spawnCircle2.Length; i++)
        {
            Destroy(warnings[i]);
            GameObject copyMonster = Instantiate(monsterPrefab, spawnCircle2[i].position, Quaternion.identity);
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

    // 新增：根據剩餘時間調整生成間隔的方法
    void UpdateSpawnInterval()
    {
        // 當房間時間少於150秒開始加快生成
        if (roomTime <= 150f)
        {
            // 計算新的生成間隔
            // 從初始間隔(5秒)逐漸減少到最短間隔(1秒)
            float timeRatio = roomTime / 150f;  // 時間比例
            spawnInterval = Mathf.Max(0.5f, initialSpawnInterval * timeRatio);
        }
    }
}


