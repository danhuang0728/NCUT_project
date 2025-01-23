using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PotionManager : MonoBehaviour
{
    public PotionData potionDatabase;

    public List<Image> potionImages;
    public List<Image> cooldownMasks;
    public List<float> cooldownTimers;
    public List<bool> inCooldowns;

    private Dictionary<int, int> keyToPotionIndex = new Dictionary<int, int>();

    void Start()
    {
        InitializePotions();
    }

    public void InitializePotions()
    {
        if (potionDatabase == null)
        {
            Debug.LogError("Potion Database 未設定！");
            return;
        }

        if (potionDatabase.potions.Length == 0)
        {
            Debug.LogWarning("Potion Database 中沒有藥水資料！");
            return;
        }

        if (potionDatabase.potions.Length != potionImages.Count || potionDatabase.potions.Length != cooldownMasks.Count)
        {
            Debug.LogError("藥水資料數量與UI數量不匹配");
            return;
        }

        cooldownTimers = new List<float>(new float[potionDatabase.potions.Length]);
        inCooldowns = new List<bool>(new bool[potionDatabase.potions.Length]);

        for (int i = 0; i < potionDatabase.potions.Length; i++)
        {
            potionImages[i].sprite = potionDatabase.potions[i].potionIcon;
            cooldownMasks[i].fillAmount = 0;
            cooldownMasks[i].enabled = false;
            inCooldowns[i] = false;
            keyToPotionIndex[potionDatabase.potions[i].potionId] = i;
        }
    }

    void Update()
    {
        HandleInput();
        UpdateCooldown();
    }

    void HandleInput()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (keyToPotionIndex.ContainsKey(i + 1))
                {
                    UsePotion(keyToPotionIndex[i + 1]);
                }
            }
        }
    }

    void UpdateCooldown()
    {
        for (int i = 0; i < potionDatabase.potions.Length; i++)
        {
            if (inCooldowns[i])
            {
                cooldownTimers[i] += Time.deltaTime;
                cooldownMasks[i].fillAmount = 1 - (cooldownTimers[i] / potionDatabase.potions[i].cooldownTime);

                if (cooldownTimers[i] >= potionDatabase.potions[i].cooldownTime)
                {
                    cooldownTimers[i] = 0;
                    cooldownMasks[i].fillAmount = 0;
                    cooldownMasks[i].enabled = false;
                    inCooldowns[i] = false;
                }
            }
        }
    }

    public void UsePotion(int potionIndex)
    {
        if (potionIndex >= potionDatabase.potions.Length)
        {
            Debug.LogWarning("超過可使用的藥水欄位");
            return;
        }

        if (!inCooldowns[potionIndex])
        {
            inCooldowns[potionIndex] = true;
            cooldownTimers[potionIndex] = 0;
            cooldownMasks[potionIndex].enabled = true;
            Debug.Log("使用藥水: " + potionDatabase.potions[potionIndex].potionName);
            // 這裡可以加上藥水效果邏輯
        }
        else
        {
            Debug.Log("藥水還在冷卻");
        }
    }
}