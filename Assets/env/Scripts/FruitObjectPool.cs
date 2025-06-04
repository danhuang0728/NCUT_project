using UnityEngine;
using System.Collections.Generic;

public class FruitObjectPool : MonoBehaviour
{
    public static FruitObjectPool Instance { get; private set; }
        
    [System.Serializable]
    public class FruitPoolInfo
    {
        public GameObject fruitPrefab;
        public FruitType fruitType;
        public int poolSize = 20;
    }

    [SerializeField] private List<FruitPoolInfo> fruitPoolInfos = new List<FruitPoolInfo>();
    private Dictionary<GameObject, Queue<GameObject>> fruitPools;
        
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
        fruitPools = new Dictionary<GameObject, Queue<GameObject>>();
        
        foreach (var poolInfo in fruitPoolInfos)
        {
            if (poolInfo.fruitPrefab == null) continue;
            
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < poolInfo.poolSize; i++)
            {
                CreateNewFruit(poolInfo.fruitPrefab, pool);
            }
            
            fruitPools[poolInfo.fruitPrefab] = pool;
        }
    }
        
    private void CreateNewFruit(GameObject prefab, Queue<GameObject> pool)
    {
        GameObject fruit = Instantiate(prefab);
        fruit.SetActive(false);
        pool.Enqueue(fruit);
    }
        
    public GameObject GetFruit(FruitType fruitType)
    {
        GameObject prefab = fruitPoolInfos.Find(info => info.fruitType == fruitType)?.fruitPrefab;
        if (prefab == null)
        {
            Debug.LogError($"找不到水果類型 {fruitType} 的預製體");
            return null;
        }
        if (!fruitPools.ContainsKey(prefab))
        {
            Debug.LogWarning($"找不到預製體 {prefab.name} 的物件池，創建新的物件池");
            fruitPools[prefab] = new Queue<GameObject>();
        }

        Queue<GameObject> pool = fruitPools[prefab];
        
        if (pool.Count == 0)
        {
            CreateNewFruit(prefab, pool);
        }
        
        GameObject fruit = pool.Dequeue();
        fruit.SetActive(true);
        return fruit;
    }
        
    public void ReturnFruit(GameObject fruit)
    {
        GameObject prefab = null;
        foreach (var poolInfo in fruitPoolInfos)
        {
            if (fruit.name.Contains(poolInfo.fruitPrefab.name))
            {
                prefab = poolInfo.fruitPrefab;
                break;
            }
        }
        
        if (prefab == null || !fruitPools.ContainsKey(prefab))
        {
            Debug.LogWarning($"找不到水果 {fruit.name} 對應的物件池，無法回收水果");
            Destroy(fruit);
            return;
        }

        fruit.SetActive(false);
        fruitPools[prefab].Enqueue(fruit);
    }
    public enum FruitType
    {
            // 維生素A類（視野）
        Mango,      // 芒果
        Papaya,     // 木瓜
        Tomato,     // 番茄
        Melon,      // 哈密瓜
        Orange,     // 橘子
        Peach,      // 桃子

        // 維生素B類（移動速度）
        Banana,     // 香蕉
        Grape,      // 葡萄
        Longan,     // 龍眼
        Coconut,    // 椰子
        PassionFruit, // 百香果

        // 維生素C類（血量）
        Kiwi,       // 奇異果
        Lemon,      // 檸檬
        Strawberry, // 草莓
        Guava,      // 芭樂
        Chili,      // 辣椒
        Apple,      // 蘋果
        Durian,     // 榴槤

        // 維生素D類（攻擊力）
        Watermelon,   // 西瓜
        Pineapple,    // 鳳梨
        StarFruit,    // 楊桃
        Blueberry,    // 藍莓
        SugarApple,   // 釋迦
        Tangerine     // 柳橙
    }
}