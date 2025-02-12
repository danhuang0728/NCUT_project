using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Manager : MonoBehaviour
{
    // 圓形武器：包含啟用開關與等級設定
    [Header("圓環武器")]
    public bool circle = false;
    [Range(1, 5)]
    public int circle_level = 1;
    [Tooltip("無限時間")]  public bool is_circle_levelUP_1 = false;
    GameObject circle_weapon;
    GearWeaponSystem circle_weapon_system;
    


    // 回旋標武器：包含啟用開關與等級設定
    [Header("回旋標武器")]
    public bool Boomerang = false;
    [Range(1, 5)]
    public int boomerang_level = 1;
    [Tooltip("無限彈射")]  public bool is_boomerang_levelUP_1 = false;
    GameObject boomerang_weapon;
    BoomerangController boomerang_controller;


    // 魔法書武器：包含啟用開關與等級設定
    [Header("魔法書武器")]
    public bool MagicBook = false;
    [Range(1, 5)]
    public int magicbook_level = 1;
    [Tooltip("火球追蹤")]  public bool is_magicbook_levelUP_1 = false;
    [Tooltip("巨大火球")]  public bool is_magicbook_levelUP_2 = false;
    GameObject magicbook_weapon;
    MagicBook magicbook_script;



    // 西洋劍武器：包含啟用開關與等級設定
    [Header("西洋劍武器")]
    public bool thrust = false;
    [Range(1, 7)]
    public int thrust_level = 1;
    GameObject thrust_weapon;



    // 斧頭武器：包含啟用開關與等級設定
    [Header("斧頭武器")]
    public bool Axe = false;
    [Tooltip("等級系統尚未完成")]
    [Range(1, 5)]
    public int axe_level = 1;
    GameObject axe_weapon;


    // 弩箭砲台武器：包含啟用開關與等級設定
    [Header("弩箭砲台武器")]
    public bool crossbow = false;
    [Range(1, 5)]
    public int crossbow_level = 1;
    [Tooltip("加速連射")]  public bool is_crossbow_levelUP_1 = false;
    GameObject crossbow_weapon;
    crossbow crossbow_script;
    // 主手武器：包含啟用開關與等級設定
    [Header("主手進化武器")]
    [Tooltip("亂砍")]public bool main_hand1 = false;
    GameObject main_hand1_weapon; // 亂砍武器
    [Tooltip("居合")]public bool main_hand2 = false;
    GameObject main_hand2_weapon; // 居合武器
    [Range(1, 5)]
    public int main_hand_level1 = 1;
    public int main_hand_level2 = 1;

    private bool hasSpawnedCircleGears = false;
    private float initialAttackRange;
    private PlayerControl playerControl;

    void Start()
    {
        playerControl = GameObject.Find("player1").GetComponent<PlayerControl>();

        circle_weapon = transform.Find("circle").gameObject;
        boomerang_weapon = transform.Find("Boomerang").gameObject;
        magicbook_weapon = transform.Find("MagicBook").gameObject;
        thrust_weapon = transform.Find("thrust").gameObject;
        axe_weapon = transform.Find("Axe_Slashh_0").gameObject;
        crossbow_weapon = transform.Find("crossbow").gameObject;
        main_hand1_weapon = transform.Find("sword_attack").gameObject;
        main_hand2_weapon = transform.Find("Tameshigiri").gameObject;
        circle_weapon_system = circle_weapon.GetComponent<GearWeaponSystem>();
        boomerang_controller = boomerang_weapon.GetComponent<BoomerangController>();
        magicbook_script = magicbook_weapon.GetComponent<MagicBook>();
        crossbow_script = crossbow_weapon.GetComponent<crossbow>();

        initialAttackRange = playerControl.AttackRange; // 儲存改變前的攻擊範圍
    }



    void Update()
    {
        if(circle == false)  // 處理圓環武器開關問題
        {
            circle_weapon_system.DestroyGears();
            hasSpawnedCircleGears = false;
        }
        else
        {
            if (!hasSpawnedCircleGears) // 確保只執行一次
            {
                circle_weapon_system.SpawnGears();
                hasSpawnedCircleGears = true;
            }
        }
        setActiveWeapon(circle, circle_weapon);
        setActiveWeapon(Boomerang, boomerang_weapon);
        setActiveWeapon(MagicBook, magicbook_weapon);
        setActiveWeapon(thrust, thrust_weapon);
        setActiveWeapon(Axe, axe_weapon);
        setActiveWeapon(crossbow, crossbow_weapon);
        setActiveWeapon(main_hand1, main_hand1_weapon);
        if(main_hand1 == true) // 亂砍武器開啟時，攻擊範圍為0 改為使用亂砍的判定大小
        {
            playerControl.AttackRange = 0f;
        }
        else
        {
            playerControl.AttackRange = initialAttackRange;
        }

        setActiveWeapon(main_hand2, main_hand2_weapon);


        circle_weapon_system.level = circle_level;
        circle_weapon_system.is_levelUP = is_circle_levelUP_1; // 圓環武器進化(無限時間)

        boomerang_controller.level = boomerang_level;
        boomerang_controller.is_levelUP = is_boomerang_levelUP_1; // 回旋標武器進化(無限彈射)
        
        magicbook_script.level = magicbook_level;
        magicbook_script.is_levelUP1 = is_magicbook_levelUP_1; // 魔法書武器進化(火球追蹤)
        magicbook_script.is_levelUP2 = is_magicbook_levelUP_2; // 魔法書武器進化(巨大火球)

        crossbow_script.level = crossbow_level;
        crossbow_script.is_levelUP = is_crossbow_levelUP_1; // 弩箭砲台武器進化(加速連社)

    }
    void setActiveWeapon(bool weapon, GameObject weapon_object)
    {
        if(weapon)
        {
            weapon_object.SetActive(true);
        }
        else
        {
            weapon_object.SetActive(false);
        }
    }
}
