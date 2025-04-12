using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponPanel_manager : MonoBehaviour
{
    [SerializeField] private Image[] weaponImages;
    [SerializeField] private Player_WeaponData playerWeaponData;
    void Start()
    {
    }

    void Update()
    {
        if (playerWeaponData.WeaponDataList.Count > 0)
        {
            foreach (var weapon in playerWeaponData.WeaponDataList)
            {
                int index = playerWeaponData.WeaponDataList.IndexOf(weapon);
                weaponImages[index].sprite = weapon.skillIcon;
            }
        }
    }
}
