using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 創建一個可序列化的類來包裝數據
[System.Serializable]
public class TetrDatabaseWrapper
{
    public List<tetr_database> tetrList = new List<tetr_database>();
}

public class SaveManager : MonoBehaviour
{
    // 單例模式，確保只有一個SaveManager實例
    public static SaveManager Instance { get; private set; }

    // Character_Values_SETUP的引用
    public Character_Values_SETUP character_Values_SETUP;
    public player_tetr_database database;
    
    // 用於記錄上一個場景
    private string previousScene;

    private void Awake()
    {
        // 實現單例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 添加場景切換監聽
            SceneManager.sceneLoaded += OnSceneLoaded;
            previousScene = SceneManager.GetActiveScene().name;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // 場景加載時的處理
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        previousScene = scene.name;
        
        // 確保Character_Values_SETUP引用仍然有效
        if (character_Values_SETUP == null)
        {
            // 可以在這裡嘗試重新查找引用
        }
    }

    private void Start()
    {
        
    }
    
    private void OnDestroy()
    {
        // 移除場景切換監聽
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 儲存角色數值到PlayerPrefs
    public void SaveCharacterValues()
    {
        if (character_Values_SETUP == null)
        {
            Debug.LogError("無法儲存：Character_Values_SETUP引用為空");
            return;
        }

        try
        {
            // 儲存所有角色數值
            PlayerPrefs.SetFloat("damage_addition", character_Values_SETUP.damage_addition);
            PlayerPrefs.SetFloat("criticalDamage_addition", character_Values_SETUP.criticalDamage_addition);
            PlayerPrefs.SetFloat("criticalHitRate_addition", character_Values_SETUP.criticalHitRate_addition);
            PlayerPrefs.SetFloat("speed_addition", character_Values_SETUP.speed_addition);
            PlayerPrefs.SetFloat("health_addition", character_Values_SETUP.health_addition);
            PlayerPrefs.SetFloat("cooldown_addition", character_Values_SETUP.cooldown_addition);
            PlayerPrefs.SetFloat("lifeSteal_addition", character_Values_SETUP.lifeSteal_addition);
            PlayerPrefs.SetFloat("gold_addition", character_Values_SETUP.gold_addition);
            PlayerPrefs.SetInt("GIFT_Value", character_Values_SETUP.GIFT_Value);
            
            // 儲存變更
            PlayerPrefs.Save();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"儲存數值時發生錯誤: {e.Message}");
        }
    }

    // 從PlayerPrefs載入角色數值
    public void LoadCharacterValues()
    {
        if (character_Values_SETUP == null)
        {
            Debug.LogError("無法載入：Character_Values_SETUP引用為空");
            return;
        }

        try
        {
            // 檢查是否有儲存的數據
            if (PlayerPrefs.HasKey("damage_addition"))
            {
                character_Values_SETUP.damage_addition = PlayerPrefs.GetFloat("damage_addition");
                character_Values_SETUP.criticalDamage_addition = PlayerPrefs.GetFloat("criticalDamage_addition");
                character_Values_SETUP.criticalHitRate_addition = PlayerPrefs.GetFloat("criticalHitRate_addition");
                character_Values_SETUP.speed_addition = PlayerPrefs.GetFloat("speed_addition");
                character_Values_SETUP.health_addition = PlayerPrefs.GetFloat("health_addition");
                character_Values_SETUP.cooldown_addition = PlayerPrefs.GetFloat("cooldown_addition");
                character_Values_SETUP.lifeSteal_addition = PlayerPrefs.GetFloat("lifeSteal_addition");
                character_Values_SETUP.gold_addition = PlayerPrefs.GetFloat("gold_addition");
                character_Values_SETUP.GIFT_Value = PlayerPrefs.GetInt("GIFT_Value");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"載入數值時發生錯誤: {e.Message}");
        }
    }

    // 清除所有儲存的數據
    public void ClearSavedData()
    {
        PlayerPrefs.DeleteKey("damage_addition");
        PlayerPrefs.DeleteKey("criticalDamage_addition");
        PlayerPrefs.DeleteKey("criticalHitRate_addition");
        PlayerPrefs.DeleteKey("speed_addition");
        PlayerPrefs.DeleteKey("health_addition");
        PlayerPrefs.DeleteKey("cooldown_addition");
        PlayerPrefs.DeleteKey("lifeSteal_addition");
        PlayerPrefs.DeleteKey("gold_addition");
        PlayerPrefs.DeleteKey("GIFT_Value");
        PlayerPrefs.Save();
    }
    
    // 儲存tetramino數據
    public void SaveDataToPlayerPrefs_Tetr()
    {
        if (database == null || database.tetr_database == null)
        {
            Debug.LogError("無法儲存：tetr_database引用為空");
            return;
        }
        
        try
        {
            // 使用包裝類序列化數據
            TetrDatabaseWrapper wrapper = new TetrDatabaseWrapper();
            wrapper.tetrList = database.tetr_database;
            
            string json = JsonUtility.ToJson(wrapper);
            PlayerPrefs.SetString("TetrDatabase", json);
            PlayerPrefs.Save();
            Debug.Log("Tetr數據已儲存，數據大小: " + json.Length);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"儲存Tetr數據時發生錯誤: {e.Message}");
        }
    }
    
    // 載入tetramino數據
    public void LoadDataFromPlayerPrefs_Tetr()
    {
        if (database == null)
        {
            Debug.LogError("無法載入：database引用為空");
            return;
        }
        
        try
        {
            string json = PlayerPrefs.GetString("TetrDatabase", "");
            if (!string.IsNullOrEmpty(json))
            {
                TetrDatabaseWrapper wrapper = JsonUtility.FromJson<TetrDatabaseWrapper>(json);
                if (wrapper != null && wrapper.tetrList != null)
                {
                    database.tetr_database = wrapper.tetrList;
                    Debug.Log("成功載入Tetr數據，項目數: " + wrapper.tetrList.Count);
                }
                else
                {
                    Debug.LogWarning("載入的Tetr數據為空");
                }
            }
            else
            {
                Debug.LogWarning("未找到已儲存的Tetr數據");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"載入Tetr數據時發生錯誤: {e.Message}");
        }
    }
} 