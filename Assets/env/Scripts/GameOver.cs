using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;
using Unity.VisualScripting;
using Unity.Mathematics;

public class GameOver : MonoBehaviour
{
    private PlayerControl playerControl;
    private character_value_ingame Character_Value_Ingame;
    private float award;
    [SerializeField]private GameObject obj;
    public Character_Values_SETUP character_Values_SETUP;
    public TextMeshProUGUI gold_t;
    public TextMeshProUGUI kill_M_t;
    public TextMeshProUGUI endAward;
    
    void Start()
    {
        try
        {
            playerControl = GameObject.Find("player1").GetComponent<PlayerControl>();
            Character_Value_Ingame = GameObject.Find("player1").GetComponent<character_value_ingame>();
            
            if (obj != null)
            {
                obj.SetActive(false); 
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"GameOver.Start發生錯誤: {e.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu1");
    }
    public void save()
    {
        SaveCharacterData();
        SaveManager.Instance.SaveDataToPlayerPrefs_Tetr();
    }
    public IEnumerator dead()
    {
        // 首先保存數據
        if(SaveManager.Instance != null)
        {
            SaveCharacterData();
            SaveManager.Instance.SaveDataToPlayerPrefs_Tetr();
        }
        else
        {
            Debug.LogError("未正確儲存檔案");
        }
        
        // 等待2秒
        yield return new WaitForSeconds(2f);
        
        try
        {
            UIstate.isAnyPanelOpen = true;
            
            // 調用設置文本
            if (gold_t != null && kill_M_t != null && endAward != null)
            {
                setTEXT();
            }
            
            // 結算計算
            if (Character_Value_Ingame != null && character_Values_SETUP != null)
            {
                award = math.round((Character_Value_Ingame.gold + PlayerControl.kill_monster_count)/1000); 
                character_Values_SETUP.GIFT_Value = character_Values_SETUP.GIFT_Value + (int)award;
            }
            
            Time.timeScale = 0f; 
            
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"死亡處理過程中出錯: {e.Message}");
            Time.timeScale = 0f;
        }
    }
    
    // 將保存功能拆分為獨立方法
    private void SaveCharacterData()
    {
        try
        {
            if (SaveManager.Instance != null)
            {
                if (SaveManager.Instance.character_Values_SETUP != character_Values_SETUP)
                {
                    SaveManager.Instance.character_Values_SETUP = character_Values_SETUP;
                }
                
                SaveManager.Instance.SaveCharacterValues();
            }
            else
            {
                SaveDirectly();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"保存數據時出錯: {e.Message}");
            try { SaveDirectly(); } catch { }
        }
    }
    
    // 直接儲存到PlayerPrefs的備用方法
    private void SaveDirectly()
    {
        if (character_Values_SETUP == null) return;
        
        PlayerPrefs.SetFloat("damage_addition", character_Values_SETUP.damage_addition);
        PlayerPrefs.SetFloat("criticalDamage_addition", character_Values_SETUP.criticalDamage_addition);
        PlayerPrefs.SetFloat("criticalHitRate_addition", character_Values_SETUP.criticalHitRate_addition);
        PlayerPrefs.SetFloat("speed_addition", character_Values_SETUP.speed_addition);
        PlayerPrefs.SetFloat("health_addition", character_Values_SETUP.health_addition);
        PlayerPrefs.SetFloat("cooldown_addition", character_Values_SETUP.cooldown_addition);
        PlayerPrefs.SetFloat("lifeSteal_addition", character_Values_SETUP.lifeSteal_addition);
        PlayerPrefs.SetFloat("gold_addition", character_Values_SETUP.gold_addition);
        PlayerPrefs.SetInt("GIFT_Value", character_Values_SETUP.GIFT_Value);
        PlayerPrefs.Save();
    }
    
    void setTEXT()
    {
        if (Character_Value_Ingame == null || gold_t == null || kill_M_t == null || endAward == null) return;
        
        gold_t.text = Character_Value_Ingame.gold.ToString();
        kill_M_t.text = PlayerControl.kill_monster_count.ToString();  
        endAward.text = award.ToString();
    }
}
