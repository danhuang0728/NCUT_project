using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Custom/WeaponDatabase")]
public class WeaponDatabase : ScriptableObject
{
    public List<WeaponData> WeaponDataList;
}