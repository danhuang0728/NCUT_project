using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Manager : MonoBehaviour
{
    public Player_WeaponData player_weaponData;
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
    [Range(1, 5)]
    public int axe_level = 1;
    GameObject axe_weapon;

    public bool AxeSpawner = false;
    private AxeSpawner axeSpawner;
    private int previousAxeLevel = 1;


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
        GameObject player = GameObject.Find("player1");

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
        
        // 從player1獲取AxeSpawner組件
        if (player != null)
        {
            axeSpawner = player.GetComponent<AxeSpawner>();
            if (axeSpawner != null)
            {
                previousAxeLevel = axe_level;
                axeSpawner.level = axe_level;
                Debug.Log("成功獲取player1上的AxeSpawner組件");
            }
            else
            {
                Debug.LogWarning("未能在player1上找到AxeSpawner組件");
            }
        }
        else
        {
            Debug.LogError("未能找到player1物件");
        }

        initialAttackRange = playerControl.AttackRange;
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
        
        // 控制普通斧頭和飛斧的切換
        if (AxeSpawner)
        {
            // 如果啟動飛斧，關閉普通斧頭
            axe_weapon.SetActive(false);
        }
        else
        {
            // 如果關閉飛斧，則根據Axe的狀態控制普通斧頭
            setActiveWeapon(Axe, axe_weapon);
        }

        setActiveWeapon(crossbow, crossbow_weapon);
        setActiveWeapon(main_hand1, main_hand1_weapon);

        // 控制 AxeSpawner 腳本的開關
        if (axeSpawner != null)
        {
            axeSpawner.enabled = AxeSpawner;  // 直接控制腳本的啟用狀態
            
            // 如果腳本被啟用且等級發生變化，則更新等級
            if (AxeSpawner && axe_level != previousAxeLevel)
            {
                previousAxeLevel = axe_level;
                axeSpawner.level = axe_level;
                axeSpawner.OnLevelChanged();
            }
        }

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
    void setWeaponLevel(int level, WeaponData weaponData)
    {
        if(player_weaponData.WeaponDataList.Contains(weaponData))
        {
            weaponData.level = level;
        }
    }
    public void UpdateAllWeaponLevels()
    {
        if (player_weaponData == null || player_weaponData.WeaponDataList == null)
        {
            Debug.LogError("玩家武器資料庫為空或未正確設置");
            return;
        }

        foreach (WeaponData weaponData in player_weaponData.WeaponDataList)
        {
            if (weaponData != null)
            {
                setWeaponLevel(weaponData.level, weaponData);
                
                // 根據武器類型更新對應的武器等級變數
                switch (weaponData.weapontype)
                {
                    case WeaponData.Weapontype.gear:
                        circle_level = weaponData.level;
                        break;
                    case WeaponData.Weapontype.Boomerang:
                        boomerang_level = weaponData.level;
                        break;
                    case WeaponData.Weapontype.MagicBook:
                        magicbook_level = weaponData.level;
                        break;
                    case WeaponData.Weapontype.crossbow:
                        crossbow_level = weaponData.level;
                        break;
                    case WeaponData.Weapontype.Axe:
                        axe_level = weaponData.level;
                        break;
                    case WeaponData.Weapontype.thrust:
                        thrust_level = weaponData.level;
                        break;
                    default:
                        Debug.LogWarning("未知的武器類型: " + weaponData.weapontype);
                        break;
                }
                
                Debug.Log("更新武器等級: " + weaponData.skillName + " 等級: " + weaponData.level);
            }
        }
    }
    public void UpdateWeaponStatus()
    {
        if (player_weaponData == null || player_weaponData.WeaponDataList == null)
        {
            Debug.LogError("玩家武器資料庫為空或未正確設置");
            return;
        }
        // 歷遍玩家武器庫，依照對應武器類型將 bool 設為 true
        foreach (WeaponData weaponData in player_weaponData.WeaponDataList)
        {
            if (weaponData != null)
            {
                switch (weaponData.weapontype)
                {
                    case WeaponData.Weapontype.gear:
                        circle = true;
                        break;
                    case WeaponData.Weapontype.Boomerang:
                        Boomerang = true;
                        break;
                    case WeaponData.Weapontype.MagicBook:
                        MagicBook = true;
                        break;
                    case WeaponData.Weapontype.crossbow:
                        crossbow = true;
                        break;
                    case WeaponData.Weapontype.Axe:
                        Axe = true;
                        break;
                    case WeaponData.Weapontype.thrust:
                        thrust = true;
                        break;
                    default:
                        Debug.LogWarning("未知的武器類型: " + weaponData.weapontype);
                        break;
                }
                
                Debug.Log("武器狀態更新: " + weaponData.skillName + " 已啟用");
            }
        }
    }


}
