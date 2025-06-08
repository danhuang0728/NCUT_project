using UnityEngine;
using System.Collections.Generic;
using System;

public class FruitObjectPool : MonoBehaviour
{
    public static FruitObjectPool Instance { get; private set; }
    
    private Dictionary<FruitType, GameObject> fruitPrefabs;
    private Dictionary<FruitType, Queue<GameObject>> fruitPools;
    [SerializeField] private int defaultPoolSize = 20;
    
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
        
        LoadFruitPrefabs();
        InitializePools();
    }
    
    private void LoadFruitPrefabs()
    {
        fruitPrefabs = new Dictionary<FruitType, GameObject>();
        
        // 遍历所有水果类型
        foreach (FruitType fruitType in Enum.GetValues(typeof(FruitType)))
        {
            // 从Resources文件夹加载预制体
            string prefabPath = $"Fruits/{fruitType}";
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            
            if (prefab != null)
            {
                fruitPrefabs.Add(fruitType, prefab);
            }
            else
            {
                Debug.LogWarning($"无法加载水果预制体: {prefabPath}");
            }
        }
    }
    
    private void InitializePools()
    {
        fruitPools = new Dictionary<FruitType, Queue<GameObject>>();
        
        foreach (var kvp in fruitPrefabs)
        {
            FruitType fruitType = kvp.Key;
            GameObject prefab = kvp.Value;
            
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int i = 0; i < defaultPoolSize; i++)
            {
                CreateNewFruit(prefab, pool);
            }
            
            fruitPools[fruitType] = pool;
        }
    }
    
    private void CreateNewFruit(GameObject prefab, Queue<GameObject> pool)
    {
        GameObject fruit = Instantiate(prefab);
        fruit.SetActive(false);
        
        // 设置水果的 Sorting Layer 为 "wall"
        SpriteRenderer spriteRenderer = fruit.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "wall";
        }
        
        pool.Enqueue(fruit);
    }
    
    public GameObject GetFruit(FruitType fruitType)
    {
        if (!fruitPools.ContainsKey(fruitType))
        {
            Debug.LogError($"找不到水果類型 {fruitType} 的物件池");
            return null;
        }

        Queue<GameObject> pool = fruitPools[fruitType];
        
        if (pool.Count == 0)
        {
            CreateNewFruit(fruitPrefabs[fruitType], pool);
        }
        
        GameObject fruit = pool.Dequeue();
        fruit.SetActive(true);
        return fruit;
    }
    
    public void ReturnFruit(GameObject fruit)
    {
        foreach (var kvp in fruitPrefabs)
        {
            if (fruit.name.Contains(kvp.Value.name))
            {
                fruit.SetActive(false);
                fruitPools[kvp.Key].Enqueue(fruit);
                return;
            }
        }
        
        Debug.LogWarning($"找不到水果 {fruit.name} 對應的物件池，無法回收水果");
        Destroy(fruit);
    }
}