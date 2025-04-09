using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Player_WeaponData", menuName = "Custom/Player_WeaponData")]
public class Player_WeaponData : ScriptableObject
{
    [SerializeField]
    private List<WeaponData> weaponDataList = new List<WeaponData>(3);

    public List<WeaponData> WeaponDataList
    {
        get => weaponDataList;
        set
        {
            if (value.Count <= 3)
            {
                weaponDataList = value;
            }
            else
            {
                Debug.LogWarning("武器清單不能超過3個");
                weaponDataList = value.GetRange(0, 3);
            }
        }
    }
}