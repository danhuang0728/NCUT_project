using UnityEngine;
using System.Collections.Generic;

public class MonsterObjectPool : MonoBehaviour
{
    public static MonsterObjectPool Instance { get; private set; }
        
    [System.Serializable]
    public class MonsterPoolInfo
    {
        public GameObject monsterPrefab;
        public MonsterType monsterType;
        public int poolSize = 20;
    }

    [SerializeField] private List<MonsterPoolInfo> monsterPoolInfos = new List<MonsterPoolInfo>();
    private Dictionary<GameObject, Queue<GameObject>> monsterPools;
        
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        InitializePools();
    }
        
    private void InitializePools()
    {
        monsterPools = new Dictionary<GameObject, Queue<GameObject>>();
        
        foreach (var poolInfo in monsterPoolInfos)
        {
            if (poolInfo.monsterPrefab == null) continue;
            
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < poolInfo.poolSize; i++)
            {
                CreateNewMonster(poolInfo.monsterPrefab, pool);
            }
            
            monsterPools[poolInfo.monsterPrefab] = pool;
        }
    }
        
    private void CreateNewMonster(GameObject prefab, Queue<GameObject> pool)
    {
        GameObject monster = Instantiate(prefab);
        monster.SetActive(false);
        pool.Enqueue(monster);
    }
        
    public GameObject GetMonster(MonsterType monsterType)
    {
        GameObject prefab = monsterPoolInfos.Find(info => info.monsterType == monsterType)?.monsterPrefab;
        if (prefab == null)
        {
            Debug.LogError($"找不到怪物類型 {monsterType} 的預製體");
            return null;
        }
        if (!monsterPools.ContainsKey(prefab))
        {
            Debug.LogWarning($"找不到預製體 {prefab.name} 的物件池，創建新的物件池");
            monsterPools[prefab] = new Queue<GameObject>();
        }

        Queue<GameObject> pool = monsterPools[prefab];
        
        if (pool.Count == 0)
        {
            CreateNewMonster(prefab, pool);
        }
        
        GameObject monster = pool.Dequeue();
        monster.SetActive(true);
        return monster;
    }
        
    public void ReturnMonster(GameObject monster)
    {
        // 尋找怪物對應的預製體類型
        GameObject prefab = null;
        foreach (var poolInfo in monsterPoolInfos)
        {
            if (monster.name.Contains(poolInfo.monsterPrefab.name))
            {
                prefab = poolInfo.monsterPrefab;
                break;
            }
        }
        
        if (prefab == null || !monsterPools.ContainsKey(prefab))
        {
            Debug.LogWarning($"找不到怪物 {monster.name} 對應的物件池，無法回收怪物");
            return;
        }

        monster.SetActive(false);
        monsterPools[prefab].Enqueue(monster);
    }
    public enum MonsterType
    {
        thief_0,
        magic1_0,
        undead_zombie,
        lemonSlime,
        skeleton_0,
        undead_SK102,
        forest_Tree_402,
        Cave_eye_601,
        forest_Flower_401,
        forest_Snake_403,
        forest_Wolf_404,
        undead_ghost,
        undead_SK_king,
        undead_zombie3,
        undead_SK103,
        bee,
        bell_2,
        bell,
        bell_1,
        lychee,
        watermelon,
        apple_wolf,
        Slime_1,
        spider_0,
        peach,
        banana_monster1
    }
} 