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
    private AudioController audioController;

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

    public GameObject[] shopItemPrefabs; // 在Inspector中设置可能出现的物品预制体

    [SerializeField] private Transform[] spawnPoints; // 在Inspector中设置4个生成点
    private bool itemsSpawned = false;


    void Awake()
    {
        playerGPS = GameObject.Find("GPS_icon").GetComponent<MainGPS>();
        audioController = FindAnyObjectByType<AudioController>();
    }
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        playerTransform = GameObject.Find("player1").transform;
        
        //重製關卡
        levelFinish = 0;

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
        
        // 查找并设置对应关卡的RandomSpawn组件的levelId
        RandomSpawn[] randomSpawns = FindObjectsOfType<RandomSpawn>();
        foreach (RandomSpawn randomSpawn in randomSpawns)
        {
            // 特别处理第四关和第五关
            if ((selectedOption == "(1_7)" || selectedOption == "(1_10)") && IsNearby(randomSpawn.transform, this.transform, 20f))
            {
                Debug.Log($"检测到{selectedOption}关附近的RandomSpawn: {randomSpawn.gameObject.name}");
                randomSpawn.SetLevelId(selectedOption);
            }
            else if (randomSpawn.gameObject.name.Contains(selectedOption))
            {
                // 设置每个RandomSpawn的levelId与selectedOption一致
                randomSpawn.SetLevelId(selectedOption);
            }
        }
    }

    void Update()
    {
        if (collider2d.IsTouchingLayers(player)) // 進入房間的觸發器 判斷是否進入房間
        {
            LevelTrigger.levelFinish++; // 計算通關進度
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
            levelTime = Random.Range(levelminTime, levelmaxTime); // 關卡的隨機時間設定
            StartCoroutine(levelstart(levelTime));

            foreach (TrapControll trap in trapControlls) // 修改全部trap物件裡的的bool為true
            {
                trap.close = true; //尖刺伸出來
            }
        }
    }


    IEnumerator levelstart(int leveltime)
    {
        //房間音樂設置
        AudioManager.Instance.PlayNextBattleMusic(selectedOption);
        audioController.MusicVolume(); 
        
        TimerPanel.isfighting = true; //設定關卡進行狀態(timer用)
        RandomSpawn.SetLevelFightingState(selectedOption, true); // 設置當前關卡的戰鬥狀態
        
        // 添加调试日志
        Debug.Log($"关卡激活: selectedOption={selectedOption}, isfighting={TimerPanel.isfighting}");
        
        // 直接在所有RandomSpawn中查找带有当前关卡名称的组件并激活
        RandomSpawn[] allRandomSpawns = FindObjectsOfType<RandomSpawn>();
        Debug.Log($"找到 {allRandomSpawns.Length} 个RandomSpawn组件");
        
        bool foundMatchingRandomSpawn = false;
        foreach (RandomSpawn rs in allRandomSpawns)
        {
            Debug.Log($"检查RandomSpawn: {rs.gameObject.name}, levelId={rs.GetLevelId()}");
            
            // 检查RandomSpawn对象的名称是否包含当前关卡的标识
            if (rs.gameObject.name.Contains(selectedOption))
            {
                foundMatchingRandomSpawn = true;
                // 设置此RandomSpawn的levelId并直接开始生成
                rs.SetLevelId(selectedOption);
                Debug.Log($"添加新关卡战斗状态: levelId={selectedOption}, state=True");
                RandomSpawn.SetLevelFightingState(selectedOption, true);
                
                // 强制开始生成
                rs.StartSpawnDirectly();
            }
        }
        
        // 对于第四关或第五关，尝试通过位置查找
        if (selectedOption == "(1_7)" || selectedOption == "(1_10)")
        {
            Debug.Log($"尝试通过位置查找{selectedOption}关的RandomSpawn");
            foreach (RandomSpawn rs in allRandomSpawns)
            {
                if (IsNearby(rs.transform, this.transform, 20f))
                {
                    Debug.Log($"通过位置找到{selectedOption}关的RandomSpawn: {rs.gameObject.name}");
                    rs.SetLevelId(selectedOption);
                    RandomSpawn.SetLevelFightingState(selectedOption, true);
                    rs.StartSpawnDirectly();
                    foundMatchingRandomSpawn = true;
                    break;
                }
            }
        }
        
        // 如果没有找到匹配的RandomSpawn，输出警告
        if (!foundMatchingRandomSpawn)
        {
            Debug.LogWarning($"警告：没有找到包含 {selectedOption} 的RandomSpawn组件！");
            
            // 如果仍然没有找到，直接设置状态
            RandomSpawn.SetLevelFightingState(selectedOption, true);
            Debug.Log($"直接设置关卡状态: {selectedOption}=True");
        }

        // 如果是第四关或第五关且仍然没有找到匹配的RandomSpawn，强制所有RandomSpawn生成怪物
        if ((selectedOption == "(1_7)" || selectedOption == "(1_10)") && !foundMatchingRandomSpawn)
        {
            Debug.LogWarning($"特殊处理：{selectedOption}关未找到匹配的RandomSpawn，尝试激活所有RandomSpawn");
            
            // 强制所有RandomSpawn使用当前关ID并开始生成
            foreach (RandomSpawn rs in allRandomSpawns)
            {
                rs.SetLevelId(selectedOption);
                RandomSpawn.SetLevelFightingState(selectedOption, true);
                rs.StartSpawnDirectly();
                Debug.Log($"强制激活RandomSpawn: {rs.gameObject.name} 用于{selectedOption}关");
            }
        }

        foreach (spawn repOb in spawns_self) 
        {
            repOb.gameObject.SetActive(true);
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
        TimerPanel.isfighting = false; //設定關卡進行狀態(timer用)
        RandomSpawn.SetLevelFightingState(selectedOption, false); // 重置當前關卡的戰鬥狀態
        Debug.Log($"关卡结束: selectedOption={selectedOption}");

        // 查找所有RandomSpawn并停止生成
        RandomSpawn[] endRandomSpawns = FindObjectsOfType<RandomSpawn>();
        foreach (RandomSpawn randomSpawn in endRandomSpawns)
        {
            if (randomSpawn.gameObject.name.Contains(selectedOption))
            {
                Debug.Log($"强制停止当前关卡的RandomSpawn: {randomSpawn.gameObject.name}");
                // RandomSpawn将在自己的Update方法中检测到TimerPanel.isfighting为false并停止生成
            }
        }

        foreach (spawn repOb in spawns_self) 
        {
            repOb.gameObject.SetActive(false);
        }
        ClearAllClones();  //刪除剩餘怪物
        AudioManager.Instance.PlayRestMusic(); // 播放休息音樂
    }
    public void CloseTrap()
    {
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
        MainGPS.Instance.openGPS();

        float duration = 10f; // 設定持續時間為10秒
        float elapsedTime = 0f; // 已經過的時間

        while (elapsedTime < duration) // 只在10秒內移動物件
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
            elapsedTime += Time.deltaTime; // 更新已經過的時間
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

    // 检查两个Transform是否接近
    private bool IsNearby(Transform t1, Transform t2, float maxDistance)
    {
        float distance = Vector3.Distance(t1.position, t2.position);
        return distance <= maxDistance;
    }
}
