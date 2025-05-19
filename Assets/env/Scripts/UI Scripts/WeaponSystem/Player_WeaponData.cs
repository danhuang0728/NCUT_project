using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "Player_WeaponData", menuName = "Custom/Player_WeaponData")]
public class Player_WeaponData : ScriptableObject
{
    [SerializeField]
    private List<WeaponData> weaponDataList = new List<WeaponData>(3);

    // 添加事件
    public event Action OnWeaponDataChanged;

    public List<WeaponData> WeaponDataList
    {
        get => weaponDataList;
        set
        {
            if (value.Count <= 3)
            {
                weaponDataList = value;
                OnWeaponDataChanged?.Invoke();
            }
            else
            {
                Debug.LogWarning("武器清單不能超過3個");
                weaponDataList = value.GetRange(0, 3);
                OnWeaponDataChanged?.Invoke();
            }
        }
    }

    // 添加方法来更新武器等级并触发事件
    public void UpdateWeaponLevel(WeaponData weapon, int newLevel)
    {
        if (weaponDataList.Contains(weapon))
        {
            weapon.level = Mathf.Clamp(newLevel, 0, 5);
            OnWeaponDataChanged?.Invoke();
        }
    }

    // 添加重置方法
    public void ResetWeaponData()
    {
        // 清空武器列表
        weaponDataList.Clear();
        // 触发事件通知UI更新
        OnWeaponDataChanged?.Invoke();
    }

    // 添加重置单个武器等级的方法
    public void ResetWeaponLevel(WeaponData weapon)
    {
        if (weaponDataList.Contains(weapon))
        {
            weapon.level = 0;
            OnWeaponDataChanged?.Invoke();
        }
    }
}