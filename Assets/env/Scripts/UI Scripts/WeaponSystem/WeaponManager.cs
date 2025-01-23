using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    public WeaponSkillData weaponSkillDatabase;
    public List<Image> skillImages; // 技能圖片列表，對應 UI 中的 Image 物件
    public List<Image> cooldownMasks; // 冷卻遮罩列表，對應 UI 中的 Image 物件
    public List<float> cooldownTimers; // 各技能的冷卻計時器
    public List<bool> inCooldowns; // 各技能是否在冷卻的狀態

     private Dictionary<int, int> skillIndexMap = new Dictionary<int, int>(); // 技能ID對應的索引

    void Start()
    {
        InitializeSkills();
    }

    void InitializeSkills()
    {
         if (weaponSkillDatabase == null)
         {
             Debug.LogError("Weapon Skill Database 未設定！");
             return;
         }

         if (weaponSkillDatabase.weaponSkills.Length == 0)
         {
             Debug.LogWarning("Weapon Skill Database 中沒有武器技能資料！");
             return;
         }
        // 初始化技能欄位的資訊與計時器
          if(weaponSkillDatabase.weaponSkills.Length != skillImages.Count || weaponSkillDatabase.weaponSkills.Length != cooldownMasks.Count) {
            Debug.LogError("武器技能資料數量與UI數量不匹配");
            return;
         }
        cooldownTimers = new List<float>(new float[weaponSkillDatabase.weaponSkills.Length]);
        inCooldowns = new List<bool>(new bool[weaponSkillDatabase.weaponSkills.Length]);

        for (int i = 0; i < weaponSkillDatabase.weaponSkills.Length; i++)
        {
            // 設定技能圖片
            skillImages[i].sprite = weaponSkillDatabase.weaponSkills[i].skillIcon;
            // 初始化冷卻遮罩
            cooldownMasks[i].fillAmount = 0;
            cooldownMasks[i].enabled = false;
            inCooldowns[i] = false;
            skillIndexMap[weaponSkillDatabase.weaponSkills[i].skillId] = i;
        }
    }

    void Update()
    {
        UpdateCooldown();
        ActivatePassiveSkills();
    }

    void UpdateCooldown()
    {
        for (int i = 0; i < weaponSkillDatabase.weaponSkills.Length; i++)
        {
            if (inCooldowns[i])
            {
                cooldownTimers[i] += Time.deltaTime;
                cooldownMasks[i].fillAmount = 1 - (cooldownTimers[i] / weaponSkillDatabase.weaponSkills[i].cooldownTime);
                if (cooldownTimers[i] >= weaponSkillDatabase.weaponSkills[i].cooldownTime)
                {
                    cooldownTimers[i] = 0;
                    cooldownMasks[i].fillAmount = 0;
                    cooldownMasks[i].enabled = false;
                    inCooldowns[i] = false;
                }
            }
        }
    }
    
     void ActivatePassiveSkills()
     {
         for (int i = 0; i < weaponSkillDatabase.weaponSkills.Length; i++)
         {
             if (!inCooldowns[i])
             {
                // 在這邊實作被動技能的邏輯，這裡範例只會顯示技能啟用
                Debug.Log("啟動武器技能: " + weaponSkillDatabase.weaponSkills[i].skillName);

                 // 啟動技能後，立刻進入冷卻時間
                 inCooldowns[i] = true;
                 cooldownTimers[i] = 0;
                 cooldownMasks[i].enabled = true;
             }
         }
    }
}