using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Timers;
using System.Linq;
using UnityEditor;
using Unity.VisualScripting;
public class LevelTrigger : MonoBehaviour
{
    public Collider2D collider2d;
    public LayerMask player;
    public LayerMask monsterlayer;
    public int levelminTime = 180;
    public int levelmaxTime = 180;
    public static int levelFinish = 0;
    [SerializeField] private Transform playerTransform; 


    [SerializeField] private GameObject canvas;  // 連結 Canvas 物件
    private Timer timerScript;

    private int levelTime = 0;
    private TrapControll[] trapControlls; // 一次抓取全部linktrap的TrapControll腳本
    private bool isLevelActive = false; 

    private spawn[] spawns;
    private spawn[] spawns_self;
    private spawn targetSpawn; // 目標 Spawn 腳本

    private spawn targetSpawn2; // 抓SK Spawn 腳本

    private spawn targetSpawn3; // 抓Spider Spawn 腳本

    private bool isplaymusic = false;
    private MainGPS playerGPS;

#if UNITY_EDITOR
    [StringDropdown("(1_2)", "(1_3)", "(1_4)", "(1_5)", "(1_6)", "(1_7)", "(1_8)", "(1_9)", "(1_10)")] // 根据实际关卡设计修改选项
#endif
    [SerializeField] private string selectedOption = "(1_1)"; // 设置默认值

    [SerializeField] private GameObject[] shopItemPrefabs; // 在Inspector中设置可能出现的物品预制体

    [SerializeField] private Transform[] spawnPoints; // 在Inspector中设置4个生成点
    private bool itemsSpawned = false;

    private bool isInRoom = false;

    void Awake()
    {
        playerGPS = GameObject.Find("GPS_icon").GetComponent<MainGPS>();
    }
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        playerTransform = GameObject.Find("player1").transform;

        // 取得 Timer 腳本
        timerScript = canvas.GetComponent<Timer>();
        
        collider2d = GetComponent<Collider2D>();
        trapControlls = FindObjectsOfType<TrapControll>(); // 一次抓取全部linktrap的物件
        spawns = FindObjectsOfType<spawn>();

        // 過濾符合條件的 spawn 並放入 spawns_self
        spawns_self = spawns.Where(repOb => repOb.gameObject.name.Contains(selectedOption)).ToArray();
        
        foreach (spawn repOb in spawns_self) // 關閉帶有spawn 腳本的物件
        {
            repOb.gameObject.SetActive(false);
        }
        Debug.Log($"{spawns_self} ");
    }

    void Update()
    {
        if (collider2d.IsTouchingLayers(player)) // 進入房間的觸發器 判斷是否進入房間
        {
            LevelTrigger.levelFinish++; // 計算通關進度
            //GPS關閉
            playerGPS.closeGPS();
            //進入房間
            if (collider2d != null) // 讓觸發器只能觸發一次
            {
                if (!isplaymusic)
                {
                    isplaymusic = true;
                    //AudioManager.Instance.PlayNextBattleMusic();
                }
                foreach (spawn repOb in spawns) // 關閉帶有spawn 腳本的物件
                {
                    //Debug.Log($"{repOb.gameObject.name} ");
                } 
                collider2d.enabled = false;  
            }
            levelTime = UnityEngine.Random.Range(levelminTime, levelmaxTime); // 關卡的隨機時間設定
            StartCoroutine(levelstart(levelTime));

            foreach (TrapControll trap in trapControlls) // 修改全部trap物件裡的的bool為true
            {
                trap.close = true; //尖刺伸出來
            }
        }
    }


    IEnumerator levelstart(int leveltime)
    {
        AudioManager.Instance.PlayNextBattleMusic(selectedOption);
        //Debug.Log($"關卡開始，持續時間：{leveltime} 秒");
        foreach (spawn repOb in spawns) // 修改全部重生點為開啟狀態
        {
            if (repOb.gameObject.name.Contains(selectedOption))
            {
                repOb.gameObject.SetActive(true);
                timerScript.remainingTime= leveltime;
                //Debug.Log(timerScript);
            }
            
        }
        // 設定timerScript的maxTime
        timerScript.maxTime = levelmaxTime;
        timerScript.remainingTime = leveltime;
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
                //Debug.Log($"剩餘時間：{remainingTime:F2} 秒，Slime新生成間隔：{newInterval:F2} 秒");
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
        StartCoroutine(MoveExpObjectsTowardsPlayerCoroutine());
        SpawnShopItems();

        foreach (spawn repOb in spawns) // 修改全部重生點為關閉狀態
        {
            repOb.gameObject.SetActive(false);
        }
        ClearAllClones();  //刪除剩餘怪物
        AudioManager.Instance.PlayRestMusic(); // 播放休息音樂
        
        foreach (TrapControll trap in trapControlls) // 修改全部trap物件裡的的bool為true
        {
            Animator trapAni = trap.GetComponent<Animator>();
            trapAni.Play("TarpAni", 0, 0f);
            trapAni.Update(0);
            trapAni.Play("TarpAni_single", 0, 0f);
            trapAni.Update(0);
            trap.close = false; //尖刺縮回去
        }
        
    }

    public void ClearAllClones() // 清除複製物件
    {
        // 獲取所有場景中的物件
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();



        // 遍歷所有物件，查找名稱包含 "Clone" 的物件
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Monster")) // 替換為適當的名稱檢查
            {
                Destroy(obj);
            }
        }
        foreach (GameObject obj2 in allObjects) // 修改全部重生點為關閉狀態
        {
            if (obj2.CompareTag("XX"))
            {
                obj2.gameObject.SetActive(false);
            }
        }
    }
    private IEnumerator MoveExpObjectsTowardsPlayerCoroutine()
    {
        GameObject[] expObjects = GameObject.FindGameObjectsWithTag("exp"); // 獲取所有tag為exp的物件
        Transform playerPosition = playerTransform; // 獲取玩家的位置
        //GPS開啟
        playerGPS.openGPS();

        while (true) // 持續移動物件
        {
            for (int i = expObjects.Length - 1; i >= 0; i--) // 反向遍歷以避免清除物件時影響迴圈
            {
                GameObject expObject = expObjects[i];
                if (expObject != null) // 確保物件存在
                {
                    // 計算物件到玩家的方向
                    Vector3 direction = (playerPosition.position - expObject.transform.position).normalized;
                    // 移動物件
                    expObject.transform.position += direction * 10f * Time.deltaTime; // 根據速度移動物件
                }
                else
                {
                    // 如果物件已被清除，則從陣列中移除
                    expObjects[i] = expObjects[expObjects.Length - 1]; // 將最後一個物件移到當前位置
                    System.Array.Resize(ref expObjects, expObjects.Length - 1); // 縮小陣列大小
                }
            }
            yield return null; // 等待下一幀
        }
    }
    
    private void SpawnShopItems()
    {
        if (itemsSpawned) return;
        itemsSpawned = true;

        // 确保有足够的生成点
        if (spawnPoints.Length < 4)
        {
            Debug.LogError("需要设置4个生成点!");
            return;
        }

        // 確保有足夠的物品預製體
        if (shopItemPrefabs.Length < 4)
        {
            Debug.LogError("需要至少4個不同的物品預製體!");
            return;
        }

        // 創建一個列表來追蹤已使用的物品索引
        List<int> usedIndices = new List<int>();

        // 在4个位置生成随机物品
        for (int i = 0; i < 4; i++)
        {
            int randomIndex;
            do
            {
                // 隨機選擇一個未使用過的物品索引
                randomIndex = Random.Range(0, shopItemPrefabs.Length);
            } while (usedIndices.Contains(randomIndex));

            // 將選中的索引加入已使用列表
            usedIndices.Add(randomIndex);

            // 在指定位置生成物品
            GameObject itemPrefab = shopItemPrefabs[randomIndex];
            Instantiate(itemPrefab, spawnPoints[i].position, Quaternion.identity);
        }
    }
}
