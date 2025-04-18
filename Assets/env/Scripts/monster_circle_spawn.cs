using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterspawn : MonoBehaviour
{
    [Header("生成設定")]
    public GameObject monsterPrefab; // 怪物預製體
    public GameObject warningIconPrefab; // 警告圖示預製體
    public float spawnRadius = 5f; // 生成半徑
    public int monsterCount = 5; // 要生成的怪物數量
    public float spawnInterval = 2f; // 生成間隔時間
    public float warningDuration = 2f; // 警告圖示顯示時間

    private PlayerControl player; // 玩家控制腳本
    private bool isSpawning = false; // 是否正在生成
    private LevelTrigger levelTrigger; // LevelTrigger腳本引用
    private TrapControll[] trapControlls; // 陷阱控制腳本陣列

    void Start()
    {
        // 檢查警告圖示預製體
        if (warningIconPrefab == null)
        {
            Debug.LogError("警告圖示預製體未設定！請在Inspector中設定。");
            return;
        }
        
        // 尋找玩家物件
        player = FindObjectOfType<PlayerControl>();
        if (player == null)
        {
            Debug.LogError("找不到玩家物件！");
            return;
        }
        
        // 檢查怪物預製體
        if (monsterPrefab == null)
        {
            Debug.LogError("怪物預製體未設定！請在Inspector中設定。");
            return;
        }
        
        // 尋找LevelTrigger腳本
        levelTrigger = FindObjectOfType<LevelTrigger>();
        if (levelTrigger == null)
        {
            Debug.LogError("找不到LevelTrigger腳本！");
            return;
        }
        
        // 獲取所有陷阱控制腳本
        trapControlls = FindObjectsOfType<TrapControll>();
        if (trapControlls.Length == 0)
        {
            Debug.LogError("找不到任何陷阱控制腳本！");
            return;
        }
    }

    void Update()
    {
        // 檢查所有陷阱的狀態
        bool anyTrapActive = false;
        foreach (TrapControll trap in trapControlls)
        {
            if (trap.close)
            {
                anyTrapActive = true;
                break;
            }
        }

        // 根據陷阱狀態控制生成
        if (anyTrapActive && !isSpawning)
        {
            StartSpawning();
        }
        else if (!anyTrapActive && isSpawning)
        {
            StopSpawning();
        }
    }

    // 開始生成怪物
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnMonsters());
        }
    }

    // 停止生成怪物
    public void StopSpawning()
    {
        isSpawning = false;
    }

    // 生成怪物的協程
    private IEnumerator SpawnMonsters()
    {
        while (isSpawning)
        {
            List<GameObject> warningIcons = new List<GameObject>();
            List<Vector3> spawnPositions = new List<Vector3>();

            // 首先计算所有生成位置并显示警告图标
            for (int i = 0; i < monsterCount; i++)
            {
                float angle = i * (360f / monsterCount);
                float radian = angle * Mathf.Deg2Rad;
                
                Vector3 spawnPosition = player.transform.position + new Vector3(
                    Mathf.Cos(radian) * spawnRadius,
                    Mathf.Sin(radian) * spawnRadius,
                    0f
                );
                
                spawnPositions.Add(spawnPosition);
                
                // 显示警告图标
                GameObject warningIcon = Instantiate(warningIconPrefab, spawnPosition, Quaternion.identity);
                warningIcon.SetActive(true);
                warningIcons.Add(warningIcon);
            }

            // 等待警告时间
            yield return new WaitForSeconds(warningDuration);

            // 同时生成所有怪物
            for (int i = 0; i < monsterCount; i++)
            {
                GameObject monster = Instantiate(monsterPrefab, spawnPositions[i], Quaternion.identity);
                monster.SetActive(true);
                
                // 销毁对应的警告图标
                Destroy(warningIcons[i]);
            }

            // 等待间隔时间后进行下一轮生成
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 在Scene視圖中繪製生成範圍
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(player.transform.position, spawnRadius);
        }
    }
}
