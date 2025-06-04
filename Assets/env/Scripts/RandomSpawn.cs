using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 難度枚舉
public enum DifficultyLevel
{
    Easy,    // 簡單
    Normal,  // 普通
    Hard     // 困難
}

[System.Serializable]
public class DifficultySettings
{
    public float monsterHealthMultiplier = 1f;    // 怪物血量倍率
    public float monsterDamageMultiplier = 1f;    // 怪物傷害倍率
    public float monsterSpeedMultiplier = 1f;     // 怪物速度倍率
    public float spawnIntervalMultiplier = 1f;    // 生成間隔倍率
    public float eliteMonsterHealthMultiplier = 1f; // 菁英怪血量倍率
}

[System.Serializable]
public class RandomSpawnPhase
{
    public int monsterCount = 5; // 該階段要生成的怪物數量
    public float spawnInterval = 2f; // 該階段的生成間隔時間
    public float warningDuration = 2f; // 該階段的警告圖示顯示時間
    public MonsterObjectPool.MonsterType[] monsterTypes = new MonsterObjectPool.MonsterType[0]; // 該階段可使用的怪物類型陣列
    public float[] monsterWeights = new float[0]; // 怪物出現的權重（機率）
}

public class RandomSpawn : MonoBehaviour
{
    [Header("關卡設定")]
    [SerializeField] private string levelId; // 關卡ID，用於識別不同關卡

    // 靜態字典，用於追蹤每個關卡的戰鬥狀態
    private static Dictionary<string, bool> levelFightingStates = new Dictionary<string, bool>();

    // 靜態難度管理器
    private static DifficultyLevel globalDifficulty = DifficultyLevel.Normal;
    public static event System.Action<DifficultyLevel> OnDifficultyChanged;

    [Header("難度設定")]
    [SerializeField]
    private DifficultyLevel currentDifficulty = DifficultyLevel.Normal;

    private DifficultySettings easySettings = new DifficultySettings 
    { 
        monsterHealthMultiplier = 1f,
        monsterDamageMultiplier = 1f,
        monsterSpeedMultiplier = 1f,
        spawnIntervalMultiplier = 1f,
        eliteMonsterHealthMultiplier = 1f
    };
    
    private DifficultySettings normalSettings = new DifficultySettings 
    { 
        monsterHealthMultiplier = 1.25f,
        monsterDamageMultiplier = 1.25f,
        monsterSpeedMultiplier = 1.25f,
        spawnIntervalMultiplier = 1f,
        eliteMonsterHealthMultiplier = 1.25f
    };
    
    private DifficultySettings hardSettings = new DifficultySettings 
    { 
        monsterHealthMultiplier = 3f,
        monsterDamageMultiplier = 1.3f,
        monsterSpeedMultiplier = 2f,
        spawnIntervalMultiplier = 0.9f,
        eliteMonsterHealthMultiplier = 1.2f
    };

    [Header("房間設定")]
    public Transform roomCorner1; // 房間角落1
    public Transform roomCorner2; // 房間角落2
    public float playerSafeRadius = 3f; // 玩家安全半徑

    [Header("基本設定")]
    public GameObject warningIconPrefab; // 警告圖示預製體

    [Header("生怪階段設定")]
    [SerializeField]
    private RandomSpawnPhase[] spawnPhases = new RandomSpawnPhase[5]; // 五個生怪階段
    public float phaseTransitionDelay = 1f; // 階段轉換延遲時間

    [Header("菁英怪設定")]
    public List<GameObject> eliteMonsterPrefabs = new List<GameObject>(); // 菁英怪預製體列表
    private int currentEliteIndex = 0; // 當前菁英怪索引
    private bool[] eliteSpawned = new bool[5]; // 記錄每個時間點的菁英怪是否已生成

    [Header("階段狀態")]
    public int currentPhase = 0; // 當前生怪階段
    private float phaseCheckTimer = 0f; // 階段檢查計時器
    private const float PHASE_CHECK_INTERVAL = 1f; // 階段檢查間隔（秒）
    
    private PlayerControl player; // 玩家控制腳本
    private bool isSpawning = false; // 是否正在生成
    private LevelTrigger levelTrigger; // LevelTrigger腳本引用
    private TrapControll[] trapControlls; // 陷阱控制腳本陣列
    private Vector2 roomMin; // 房間最小座標
    private Vector2 roomMax; // 房間最大座標
    private Timer timerScript; // Timer腳本引用

    // 獲取當前難度
    public static DifficultyLevel GlobalDifficulty
    {
        get { return globalDifficulty; }
        set 
        { 
            globalDifficulty = value;
            // 通知所有 RandomSpawn 實例難度已更改
            OnDifficultyChanged?.Invoke(globalDifficulty);
        }
    }

    void Start()
    {
        // 確保關卡ID已設置
        if (string.IsNullOrEmpty(levelId))
        {
            Debug.LogError("關卡ID未設置！請在Inspector中設定。");
            return;
        }

        // 初始化該關卡的戰鬥狀態
        if (!levelFightingStates.ContainsKey(levelId))
        {
            levelFightingStates[levelId] = false;
        }

        // 設定當前難度為全局難度
        HandleDifficultyChanged(globalDifficulty);
        Debug.Log("難度已設成" + globalDifficulty);
        
        // 檢查警告圖示預製體
        if (warningIconPrefab == null)
        {
            Debug.LogError("警告圖示預製體未設定！請在Inspector中設定。");
            return;
        }
        
        // 檢查房間角落物件
        if (roomCorner1 == null || roomCorner2 == null)
        {
            Debug.LogError("房間角落物件未設定！請在Inspector中設定。");
            return;
        }
        
        // 初始化生怪階段
        for (int i = 0; i < spawnPhases.Length; i++)
        {
            if (spawnPhases[i] == null)
            {
                spawnPhases[i] = new RandomSpawnPhase();
            }
            // 初始化怪物預製體陣列
            if (spawnPhases[i].monsterTypes == null || spawnPhases[i].monsterTypes.Length == 0)
            {
                Debug.LogError($"第 {i + 1} 階段沒有設定怪物預製體！");
                return;
            }
            // 初始化權重陣列
            if (spawnPhases[i].monsterWeights == null || spawnPhases[i].monsterWeights.Length != spawnPhases[i].monsterTypes.Length)
            {
                spawnPhases[i].monsterWeights = new float[spawnPhases[i].monsterTypes.Length];
                // 預設權重均等
                for (int j = 0; j < spawnPhases[i].monsterWeights.Length; j++)
                {
                    spawnPhases[i].monsterWeights[j] = 1f;
                }
            }
        }
        
        // 計算房間範圍
        roomMin = new Vector2(
            Mathf.Min(roomCorner1.position.x, roomCorner2.position.x),
            Mathf.Min(roomCorner1.position.y, roomCorner2.position.y)
        );
        roomMax = new Vector2(
            Mathf.Max(roomCorner1.position.x, roomCorner2.position.x),
            Mathf.Max(roomCorner1.position.y, roomCorner2.position.y)
        );
        
        // 尋找玩家物件
        player = FindObjectOfType<PlayerControl>();
        if (player == null)
        {
            Debug.LogError("找不到玩家物件！");
            return;
        }
        
        // 尋找LevelTrigger腳本
        levelTrigger = FindObjectOfType<LevelTrigger>();
        if (levelTrigger == null)
        {
            Debug.LogError("找不到LevelTrigger腳本！");
            return;
        }

        // 尋找Timer腳本
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas != null)
        {
            timerScript = canvas.GetComponent<Timer>();
            if (timerScript == null)
            {
                Debug.LogError("找不到Timer腳本！");
                return;
            }
        }
        
        // 獲取所有陷阱控制腳本
        trapControlls = FindObjectsOfType<TrapControll>();
        if (trapControlls.Length == 0)
        {
            Debug.LogError("找不到任何陷阱控制腳本！");
            return;
        }

        // 初始化菁英怪生成記錄
        eliteSpawned = new bool[5];
        for (int i = 0; i < eliteSpawned.Length; i++)
        {
            eliteSpawned[i] = false;
        }

        // 訂閱難度變化事件
        OnDifficultyChanged += HandleDifficultyChanged;
    }

    // 在 OnDestroy 中取消訂閱
    void OnDestroy()
    {
        OnDifficultyChanged -= HandleDifficultyChanged;
    }

    // 處理難度變化
    private void HandleDifficultyChanged(DifficultyLevel newDifficulty)
    {
        currentDifficulty = newDifficulty;
        ApplyDifficultySettings(gameObject);
    }

    void Update()
    {
        // 檢查當前關卡的戰鬥狀態
        bool currentLevelFighting = TimerPanel.isfighting && levelFightingStates[levelId];
        
        if (currentLevelFighting && !isSpawning)
        {
            StartSpawning();
        }
        else if (!currentLevelFighting && isSpawning)
        {
            StopSpawning();
        }

        // 每秒更新一次階段
        if (isSpawning && timerScript != null)
        {
            phaseCheckTimer += Time.deltaTime;
            if (phaseCheckTimer >= PHASE_CHECK_INTERVAL)
            {
                UpdatePhaseBasedOnTime();
                phaseCheckTimer = 0f;
            }

            // 檢查是否到達菁英怪生成時間點
            float remainingTime = timerScript.remainingTime;
            
            // 檢查3分30秒（210秒）
            if (remainingTime <= 211f && remainingTime >= 209f && !eliteSpawned[0])
            {
                SpawnEliteMonster();
                eliteSpawned[0] = true;
            }
            // 檢查2分30秒（150秒）
            else if (remainingTime <= 151f && remainingTime >= 149f && !eliteSpawned[1])
            {
                SpawnEliteMonster();
                eliteSpawned[1] = true;
            }
            // 檢查1分30秒（90秒）
            else if (remainingTime <= 91f && remainingTime >= 89f && !eliteSpawned[2])
            {
                SpawnEliteMonster();
                eliteSpawned[2] = true;
            }
            // 檢查45秒
            else if (remainingTime <= 46f && remainingTime >= 44f && !eliteSpawned[3])
            {
                SpawnEliteMonster();
                eliteSpawned[3] = true;
            }
            // 檢查15秒
            else if (remainingTime <= 16f && remainingTime >= 14f && !eliteSpawned[4])
            {
                SpawnEliteMonster();
                eliteSpawned[4] = true;
            }
        }
    }

    // 檢查玩家是否在房間範圍內
    private bool IsPlayerInRoom()
    {
        if (player == null) return false;

        Vector2 playerPos = player.transform.position;
        return playerPos.x >= roomMin.x && playerPos.x <= roomMax.x &&
               playerPos.y >= roomMin.y && playerPos.y <= roomMax.y;
    }

    // 根據時間更新階段
    private void UpdatePhaseBasedOnTime()
    {
        float remainingTime = timerScript.remainingTime;
        float totalTime = 240f; // 4分鐘 = 240秒
        float elapsedTime = totalTime - remainingTime;
        int newPhase;

        // 根據剩餘時間決定階段
        if (remainingTime > 240f) // 超過4分鐘
        {
            newPhase = 0;
        }
        else if (remainingTime > 180f) // 3-4分鐘
        {
            newPhase = 0;
        }
        else if (remainingTime > 120f) // 2-3分鐘
        {
            newPhase = 1;
        }
        else if (remainingTime > 60f) // 1-2分鐘
        {
            newPhase = 2;
        }
        else if (remainingTime > 30f) // 30秒-1分鐘
        {
            newPhase = 3;
        }
        else // 0-30秒
        {
            newPhase = 4;
        }

        // 如果階段改變，更新並通知
        if (newPhase != currentPhase)
        {
            currentPhase = newPhase;
            Debug.Log($"進入第 {currentPhase + 1} 階段 (剩餘時間: {remainingTime:F1} 秒)");
        }
    }

    // 開始生成怪物
    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            currentPhase = 0; // 重置階段
            phaseCheckTimer = 0f; // 重置計時器
            
            // 重置菁英怪生成記錄
            for (int i = 0; i < eliteSpawned.Length; i++)
            {
                eliteSpawned[i] = false;
            }
            
            StartCoroutine(SpawnMonsters());
        }
    }

    // 停止生成怪物
    public void StopSpawning()
    {
        isSpawning = false;
        currentPhase = 0; // 重置階段
        phaseCheckTimer = 0f; // 重置計時器
    }

    // 根據權重隨機選擇怪物預製體
    private MonsterObjectPool.MonsterType GetRandomMonsterType(RandomSpawnPhase phase)
    {
        float totalWeight = 0f;
        foreach (float weight in phase.monsterWeights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        for (int i = 0; i < phase.monsterTypes.Length; i++)
        {
            currentWeight += phase.monsterWeights[i];
            if (randomValue <= currentWeight)
            {
                return phase.monsterTypes[i];
            }
        }

        return phase.monsterTypes[0]; // 如果出現意外情況，返回第一個類型
    }

    // 獲取當前難度的設定
    private DifficultySettings GetCurrentDifficultySettings()
    {
        // 使用全局難度而不是實例難度
        switch (globalDifficulty)
        {
            case DifficultyLevel.Easy:
                return easySettings;
            case DifficultyLevel.Hard:
                return hardSettings;
            default:
                return normalSettings;
        }
    }

    // 修改生成怪物的協程
    private IEnumerator SpawnMonsters()
    {
        while (isSpawning)
        {
            // 檢查是否還有下一個階段
            if (currentPhase >= spawnPhases.Length)
            {
                isSpawning = false;
                yield break;
            }

            RandomSpawnPhase currentPhaseSettings = spawnPhases[currentPhase];
            List<GameObject> warningIcons = new List<GameObject>();
            List<Vector3> spawnPositions = new List<Vector3>();

            // 生成指定數量的怪物
            for (int i = 0; i < currentPhaseSettings.monsterCount; i++)
            {
                Vector3 spawnPosition;
                bool validPosition;
                int attempts = 0;
                const int maxAttempts = 10;

                // 嘗試找到有效的生成位置
                do
                {
                    // 在房間範圍內隨機生成
                    float randomX = Random.Range(roomMin.x, roomMax.x);
                    float randomY = Random.Range(roomMin.y, roomMax.y);
                    spawnPosition = new Vector3(randomX, randomY, 0f);

                    // 檢查是否在玩家安全範圍外
                    float distanceToPlayer = Vector3.Distance(spawnPosition, player.transform.position);
                    validPosition = distanceToPlayer > playerSafeRadius;

                    // 檢查是否與其他已生成的怪物位置太近
                    if (validPosition)
                    {
                        foreach (Vector3 existingPosition in spawnPositions)
                        {
                            if (Vector3.Distance(spawnPosition, existingPosition) < 2f)
                            {
                                validPosition = false;
                                break;
                            }
                        }
                    }

                    attempts++;
                } while (!validPosition && attempts < maxAttempts);

                if (validPosition)
                {
                    spawnPositions.Add(spawnPosition);
                    
                    // 顯示警告圖示
                    GameObject warningIcon = Instantiate(warningIconPrefab, spawnPosition, Quaternion.identity);
                    warningIcon.SetActive(true);
                    warningIcons.Add(warningIcon);
                }
            }

            // 等待警告時間
            yield return new WaitForSeconds(currentPhaseSettings.warningDuration);

            // 生成所有怪物
            for (int i = 0; i < spawnPositions.Count; i++)
            {
                MonsterObjectPool.MonsterType monsterType = GetRandomMonsterType(currentPhaseSettings);
                GameObject monster = MonsterObjectPool.Instance.GetMonster(monsterType);
                monster.GetComponent<NormalMonster_setting>().ResetMonsterHealth();
                monster.transform.position = spawnPositions[i];
                // 應用難度設定
                ApplyDifficultySettings(monster);
                
                monster.SetActive(true);
                Destroy(warningIcons[i]);
            }

            // 根據難度調整生成間隔
            float adjustedInterval = currentPhaseSettings.spawnInterval * GetCurrentDifficultySettings().spawnIntervalMultiplier;
            yield return new WaitForSeconds(adjustedInterval);
        }
    }

    // 應用難度設定到怪物
    private void ApplyDifficultySettings(GameObject monster)
    {
        DifficultySettings settings = GetCurrentDifficultySettings();
        
        // 獲取怪物的 NormalMonster_setting 組件
        NormalMonster_setting monsterSettings = monster.GetComponent<NormalMonster_setting>();
        
        if (monsterSettings != null)
        {
            // 調整怪物屬性
            // 檢查是否為菁英怪（根據 monster_type 判斷）
            bool isElite = monsterSettings.monster_type == 2; // 假設 monster_type 2 是菁英怪
            float healthMultiplier = isElite ? 
                settings.eliteMonsterHealthMultiplier : 
                settings.monsterHealthMultiplier;
            
            // 調整血量
            monsterSettings.HP *= healthMultiplier;
            
            // 調整移動速度
            monsterSettings.maxspeed *= settings.monsterSpeedMultiplier;
            monsterSettings.minspeed *= settings.monsterSpeedMultiplier;
        }
    }

    // 修改菁英怪生成方法
    private void SpawnEliteMonster()
    {
        if (eliteMonsterPrefabs == null || eliteMonsterPrefabs.Count == 0)
        {
            Debug.LogWarning("沒有設定菁英怪預製體！");
            return;
        }

        // 檢查是否還有菁英怪可以生成
        if (currentEliteIndex >= eliteMonsterPrefabs.Count)
        {
            Debug.Log("所有菁英怪都已生成完畢！");
            return;
        }

        // 在房間範圍內隨機生成位置
        Vector3 spawnPosition = new Vector3(
            Random.Range(roomMin.x, roomMax.x),
            Random.Range(roomMin.y, roomMax.y),
            0f
        );

        // 生成菁英怪
        GameObject eliteMonster = Instantiate(eliteMonsterPrefabs[currentEliteIndex], spawnPosition, Quaternion.identity);
        ApplyDifficultySettings(eliteMonster);
        eliteMonster.SetActive(true);
        
        // 更新菁英怪索引
        currentEliteIndex++;

        Debug.Log($"生成菁英怪：{eliteMonster.name}");
    }

    // 在Scene視圖中繪製房間範圍和玩家安全區域
    private void OnDrawGizmos()
    {
        if (roomCorner1 != null && roomCorner2 != null)
        {
            // 繪製房間範圍
            Gizmos.color = Color.blue;
            Vector3 size = new Vector3(
                Mathf.Abs(roomCorner2.position.x - roomCorner1.position.x),
                Mathf.Abs(roomCorner2.position.y - roomCorner1.position.y),
                0.1f
            );
            Vector3 center = (roomCorner1.position + roomCorner2.position) / 2f;
            Gizmos.DrawWireCube(center, size);

            // 繪製玩家安全區域
            if (player != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(player.transform.position, playerSafeRadius);
            }
        }
    }

    // 提供一個方法來設置特定關卡的戰鬥狀態
    public static void SetLevelFightingState(string levelId, bool state)
    {
        if (levelFightingStates.ContainsKey(levelId))
        {
            levelFightingStates[levelId] = state;
        }
    }
}
