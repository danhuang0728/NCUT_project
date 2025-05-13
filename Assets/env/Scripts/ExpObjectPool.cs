using UnityEngine;
using System.Collections.Generic;

public class ExpObjectPool : MonoBehaviour
{
    public static ExpObjectPool Instance { get; private set; }
        
    [System.Serializable]
    public class ExpPoolInfo
    {
        public GameObject expPrefab;
        public ExpType expType;
        public int poolSize = 20;
    }

    [SerializeField] private List<ExpPoolInfo> expPoolInfos = new List<ExpPoolInfo>();
    private Dictionary<GameObject, Queue<GameObject>> expPools;
        
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
        expPools = new Dictionary<GameObject, Queue<GameObject>>();
        
        foreach (var poolInfo in expPoolInfos)
        {
            if (poolInfo.expPrefab == null) continue;
            
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < poolInfo.poolSize; i++)
            {
                CreateNewExp(poolInfo.expPrefab, pool);
            }
            
            expPools[poolInfo.expPrefab] = pool;
        }
    }
        
    private void CreateNewExp(GameObject prefab, Queue<GameObject> pool)
    {
        GameObject exp = Instantiate(prefab);
        exp.SetActive(false);
        pool.Enqueue(exp);
    }
        
    public GameObject GetExp(ExpType expType)
    {
        GameObject prefab = expPoolInfos.Find(info => info.expType == expType)?.expPrefab;
        if (prefab == null)
        {
            Debug.LogError($"找不到經驗值類型 {expType} 的預製體");
            return null;
        }
        if (!expPools.ContainsKey(prefab))
        {
            Debug.LogWarning($"找不到預製體 {prefab.name} 的物件池，創建新的物件池");
            expPools[prefab] = new Queue<GameObject>();
        }

        Queue<GameObject> pool = expPools[prefab];
        
        if (pool.Count == 0)
        {
            CreateNewExp(prefab, pool);
        }
        
        GameObject exp = pool.Dequeue();
        exp.SetActive(true);
        return exp;
    }
        
    public void ReturnExp(GameObject exp)
    {
        // 尋找經驗值對應的預製體類型
        GameObject prefab = null;
        foreach (var poolInfo in expPoolInfos)
        {
            if (exp.name.Contains(poolInfo.expPrefab.name))
            {
                prefab = poolInfo.expPrefab;
                break;
            }
        }
        
        if (prefab == null || !expPools.ContainsKey(prefab))
        {
            Debug.LogWarning($"找不到經驗值 {exp.name} 對應的物件池，無法回收經驗值");
            return;
        }

        exp.SetActive(false);
        expPools[prefab].Enqueue(exp);
    }

    public enum ExpType
    {
        Small,
        Medium,
        Large
    }
} 