using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelTrigger : MonoBehaviour
{
    public Collider2D collider2d;
    public LayerMask player;
    public LayerMask monsterlayer;
    public int levelminTime = 180;
    public int levelmaxTime = 180;

    public GameObject canvas;  // 連結 Canvas 物件
    private Timer timerScript;

    public string block = "";
    private int levelTime = 0;
    private TrapControll[] trapControlls; // 一次抓取全部linktrap的TrapControll腳本

    

    
    private spawn[] spawns;

    private spawn targetSpawn; // 目標 Spawn 腳本

    private spawn targetSpawn2; // 抓SK Spawn 腳本

    private spawn targetSpawn3; // 抓Spider Spawn 腳本

 



    void Start()
    {
        // 獲取指定名稱的物件並取得其 Spawn 腳本
        Transform targetTransform = transform.Find("MonsterSpawnPoint (Slime)" + block);
        Transform targetTransform2 = transform.Find("MonsterSpawnPoint (SK)" + block); 
        Transform targetTransform3 = transform.Find("MonsterSpawnPoint (spider)" + block);

        // 取得 Timer 腳本
        timerScript = canvas.GetComponent<Timer>();
        

        if (targetTransform != null)
        {
            targetSpawn = targetTransform.GetComponent<spawn>();
            if (targetSpawn != null)
            {
                Debug.Log($"找到 Spawn 腳本: {targetSpawn.name}");
            }
            else
            {
                Debug.LogError("目標物件沒有 Spawn 腳本");
            }
        }
        else
        {
            Debug.LogError("找不到 MonsterSpawnPoint (Slime) (1_2) 物件");
        }
        if (targetTransform2 != null)
        {
            targetSpawn2 = targetTransform2.GetComponent<spawn>();   //targetTransform2 
            if (targetSpawn2 != null)
            {
                Debug.Log($"找到 Spawn 腳本: {targetSpawn2.name}");
            }
            else
            {
                Debug.LogError("目標物件沒有 Spawn 腳本");
            }
        }
        else
        {
            Debug.LogError("找不到 MonsterSpawnPoint (SK) (1_2) 物件");
        }

        if (targetTransform3 != null)
        {
            targetSpawn3 = targetTransform3.GetComponent<spawn>();    //targetTransform3 
            if (targetSpawn3 != null)
            {
                Debug.Log($"找到 Spawn 腳本: {targetSpawn3.name}");
            }
            else
            {
                Debug.LogError("目標物件沒有 Spawn 腳本");
            }
        }
        else
        {
            Debug.LogError("MonsterSpawnPoint (spider) (1_2)");
        }
        

        collider2d = GetComponent<Collider2D>();
        trapControlls = FindObjectsOfType<TrapControll>(); // 一次抓取全部linktrap的物件
        spawns = FindObjectsOfType<spawn>();
        foreach (spawn repOb in spawns) // 避免玩家還沒進入區域就開始生怪
        {
            repOb.enabled = false;
        }
    }

    void Update()
    {
        if (collider2d.IsTouchingLayers(player)) // 進入房間的觸發器
        {
            if (collider2d != null) // 讓觸發器只能觸發一次
            {
                collider2d.enabled = false;
            }
            levelTime = UnityEngine.Random.Range(levelminTime, levelmaxTime); // 關卡的隨機時間設定

            StartCoroutine(levelstart(levelTime));

            foreach (TrapControll trap in trapControlls) // 修改全部trap物件裡的的bool為true
            {
                trap.close = true;
            }
        }
    }

    IEnumerator levelstart(int leveltime)
    {
        Debug.Log($"關卡開始，持續時間：{leveltime} 秒");

        foreach (spawn repOb in spawns) // 修改全部重生點為開啟狀態
        {
            if (repOb.gameObject.name.Contains(block))
            {
                repOb.enabled = true;
                timerScript.remainingTime= 180;
                Debug.Log($"{repOb.gameObject.name} 的生成腳本已啟動");
            }
        }

        float remainingTime = leveltime;
        float maxInterval = 6f; // 最大生成間隔
        float minInterval = 0.15f; // 最小生成間隔

        // 隨著時間流逝調整 spawnInterval
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            // 動態計算新的生成間隔
            float newInterval = Mathf.Lerp(maxInterval, minInterval, 1 - (remainingTime / leveltime));
            float newInterval2 = Mathf.Lerp(maxInterval, minInterval, 1 - (remainingTime / leveltime * 0.65f)); // 加快進度  
            float newInterval3 = Mathf.Lerp(maxInterval, minInterval, 1 - (remainingTime / leveltime * 1.1f)); // 減緩進度

            // 更新目標 spawn 的生成間隔
            if (targetSpawn != null)
            {
                targetSpawn.spawnInterval = newInterval;
                Debug.Log($"剩餘時間：{remainingTime:F2} 秒，Slime新生成間隔：{newInterval:F2} 秒");
            }

             if (targetSpawn2 != null)
            {
                targetSpawn2.spawnInterval = newInterval2;
                //Debug.Log($"剩餘時間：{remainingTime:F2} 秒，SK新生成間隔：{newInterval2:F2} 秒");
            }

             if (targetSpawn3 != null)
             {
                targetSpawn3.spawnInterval = newInterval3;
                //Debug.Log($"剩餘時間：{remainingTime:F2} 秒，Spider新生成間隔：{newInterval3:F2} 秒");
            }

            yield return null;
        }

        // 關卡結束
        ClearAllClones();

        foreach (spawn repOb in spawns) // 修改全部重生點為關閉狀態
        {
            repOb.enabled = false;
        }
        foreach (TrapControll trap in trapControlls) // 修改全部trap物件裡的的bool為true
        {
            Animator trapAni = trap.GetComponent<Animator>();
            trapAni.Play("TarpAni", 0, 0f);
            trapAni.Update(0);
            trapAni.Play("TarpAni_single", 0, 0f);
            trapAni.Update(0);
            trap.close = false;
        }
    }

    public void ClearAllClones() // 清除複製物件
    {
        // 獲取所有場景中的物件
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // 遍歷所有物件，查找名稱包含 "Clone" 的物件
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Clone")) // 替換為適當的名稱檢查
            {
                Destroy(obj);
            }
        }
    }
}
