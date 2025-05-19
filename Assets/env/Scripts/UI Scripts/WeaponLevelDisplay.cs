using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponLevelDisplay : MonoBehaviour
{
    [Header("等級圖片，依序放 Level0~LV5")]
    public Sprite[] levelSprites; // 0~5級的圖片
    public Image[] levelImages;   // 三個武器欄位的等級Image
    public Player_WeaponData playerWeaponData; // 玩家武器資料

    private void OnEnable()
    {
        // 订阅武器数据变化事件
        if (playerWeaponData != null)
        {
            playerWeaponData.OnWeaponDataChanged += UpdateWeaponLevels;
        }
    }

    private void OnDisable()
    {
        // 取消订阅事件
        if (playerWeaponData != null)
        {
            playerWeaponData.OnWeaponDataChanged -= UpdateWeaponLevels;
        }
    }

    void Start()
    {
        // 初始化时更新一次UI
        UpdateWeaponLevels();
    }

    private void UpdateWeaponLevels()
    {
        for (int i = 0; i < levelImages.Length; i++)
        {
            if (playerWeaponData.WeaponDataList.Count > i)
            {
                int weaponLevel = playerWeaponData.WeaponDataList[i].level;
                weaponLevel = Mathf.Clamp(weaponLevel, 0, 5);
                levelImages[i].sprite = levelSprites[weaponLevel];
            }
            else
            {
                // 沒有武器時顯示Level0
                levelImages[i].sprite = levelSprites[0];
            }
        }
    }
}
