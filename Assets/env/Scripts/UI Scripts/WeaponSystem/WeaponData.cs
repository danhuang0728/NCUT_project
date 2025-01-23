using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSkillData", menuName = "Inventory/Weapon Skill Data")]
public class WeaponSkillData : ScriptableObject
{
     [System.Serializable]
     public struct WeaponSkillInfo
     {
         public string skillName;
         public Sprite skillIcon;
         public float cooldownTime;
         public int skillId; //技能ID
         // 可加入技能效果的數值或其他屬性
     }
     
    public WeaponSkillInfo[] weaponSkills;
}