using UnityEngine;
using System.Collections.Generic;

public class DebuffManager : MonoBehaviour
{
    private static DebuffManager _instance;
    public static DebuffManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DebuffManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("DebuffManager");
                    _instance = go.AddComponent<DebuffManager>();
                    Debug.Log("創建了新的 DebuffManager");
                }
            }
            return _instance;
        }
    }

    private Debuff playerDebuff;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            Debug.Log("銷毀重複的 DebuffManager");
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("DebuffManager 初始化完成");
    }

    private void Start()
    {
        var player = FindObjectOfType<PlayerControl>();
        if (player == null)
        {
            Debug.LogError("找不到 PlayerControl！");
            return;
        }

        playerDebuff = player.gameObject.GetComponent<Debuff>();
        if (playerDebuff == null)
        {
            Debug.LogError("玩家物件上找不到 Debuff 組件，正在嘗試添加...");
            playerDebuff = player.gameObject.AddComponent<Debuff>();
        }
        
        Debug.Log("DebuffManager 啟動完成，玩家Debuff組件狀態: " + (playerDebuff != null));
    }

    public void ApplyDebuff(FruitType fruitType)
    {
        if (playerDebuff == null)
        {
            Debug.LogError("嘗試應用Debuff時發現 playerDebuff 為空！");
            return;
        }

        Debug.Log($"正在嘗試給予玩家 {fruitType} 的Debuff效果");
        playerDebuff.ApplyDebuff(fruitType);
        Debug.Log($"已給予玩家 {fruitType} 的Debuff效果");
    }

    public void RemoveDebuff(FruitType fruitType)
    {
        if (playerDebuff == null)
        {
            Debug.LogError("嘗試移除Debuff時發現 playerDebuff 為空！");
            return;
        }

        Debug.Log($"正在嘗試移除玩家的 {fruitType} Debuff效果");
        playerDebuff.RemoveDebuff(fruitType);
        Debug.Log($"已移除玩家的 {fruitType} Debuff效果");
    }

    public bool HasDebuff(FruitType fruitType)
    {
        if (playerDebuff == null)
        {
            Debug.LogError("檢查Debuff時發現 playerDebuff 為空！");
            return false;
        }
        return playerDebuff.HasDebuff(fruitType);
    }
} 