using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSkillData", menuName = "Inventory/Weapon Skill Data")]
public class WeaponSkillData : ScriptableObject
{
    [System.Serializable]
    public struct WeaponSkillInfo
    {
        public string skillName;
        public Sprite skillIcon;
        public int weaponID;
    }
     
    public WeaponSkillInfo[] weaponSkills;
}