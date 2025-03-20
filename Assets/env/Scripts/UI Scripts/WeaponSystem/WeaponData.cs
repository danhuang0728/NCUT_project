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
        Sword,
        Sword_attack1,
        Sword_attack2

    }
    public string skillName;
    public int level;
    public Sprite skillIcon;
    public float cooldownTime;
    public int skillId; //技能ID
    // 可加入技能效果的數值或其他屬性
    public Weapontype weapontype;
}