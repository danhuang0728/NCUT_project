using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Inventory/WeaponData")]
public class WeaponData : ScriptableObject
{
    public enum Weapontype
    {
        gear,
        Axe,
        Boomerang,
        crossbow,
        MagicBook,
        thrust,

    }
    public string skillName;
    public string weapon_inturoduction;
    public int level;
    public Sprite skillIcon;
    public float cooldownTime;
    public int skillId; //技能ID
    // 可加入技能效果的數值或其他屬性
    public Weapontype weapontype;
}