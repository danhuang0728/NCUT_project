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
        for (int i = 0; i < weaponImages.Length; i++)
        {
            weaponImages[i].sprite = playerWeaponData.WeaponDataList[i].skillIcon;
        }
    }
}
