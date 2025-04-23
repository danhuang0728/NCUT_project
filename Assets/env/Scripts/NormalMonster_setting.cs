using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class NormalMonster_setting : MonoBehaviour
{
    // Start is called before the first frame update
    private float previousXPosition; // 用來儲存物件的上一幀位置
    private PlayerControl playerControl;
    private ExperienceSystem experienceSystem;
    private Weapon_Manager weaponManager;
    private Transform player1; // 修正：新增變數
    public healthbar Healthbar;
    public int monster_type; //區分水果or一般怪物 1==水果 0==一般
    public int exp_type; //區分經驗值類別 初級,中級,高級
    public GameObject LowExpPrefab;    // 低級經驗值預製體
    public GameObject MediumExpPrefab;  // 中級經驗值預製體
    public GameObject HighExpPrefab;    // 高級經驗值預製體
    public GameObject boxPrefab; // 武器箱預製體

    [Header("武器箱怪物移動速度設定")]
    public float maxspeed = 5f; // 最大速度
    public float minspeed = 1f; // 最小速度

    private GameObject burn_effect;  // 燃燒效果
    [SerializeField] private damage_effect damageEffect;
    [SerializeField] private critical_effect critical_effect;
    bool isFlip = false;
    private bool isBurning = false; // 新增：標記是否正在燃燒
    public float HP = 100f;  // 直接使用HP作为基础值

    void Start()
    {
        Healthbar = GameObject.Find("healthbar").GetComponent<healthbar>();
        experienceSystem = FindObjectOfType<ExperienceSystem>();
        weaponManager = FindObjectOfType<Weapon_Manager>();
        burn_effect = GameObject.Find("fire_0");
        playerControl = FindObjectOfType<PlayerControl>();
        player1 = playerControl.GetComponent<Transform>();
        damageEffect = GameObject.Find("damage_effect").GetComponent<damage_effect>();
        critical_effect = GameObject.Find("critical_effect").GetComponent<critical_effect>();

        // 根据等级和武器计算HP
        if (experienceSystem != null && weaponManager != null)
        {
            // 计算武器等级加成
            int weaponBonus = 0;
            if (weaponManager.circle) weaponBonus += weaponManager.circle_level;
            if (weaponManager.Boomerang) weaponBonus += weaponManager.boomerang_level;
            if (weaponManager.MagicBook) weaponBonus += weaponManager.magicbook_level;
            if (weaponManager.thrust) weaponBonus += weaponManager.thrust_level;
            if (weaponManager.Axe) weaponBonus += weaponManager.axe_level;
            if (weaponManager.crossbow) weaponBonus += weaponManager.crossbow_level;
            if (weaponManager.main_hand1) weaponBonus += weaponManager.main_hand_level1;
            if (weaponManager.main_hand2) weaponBonus += weaponManager.main_hand_level2;

            // 简化计算，减少浮点运算次数
            float totalMultiplier = 1 + (Mathf.Pow(experienceSystem.currentLevel, 0.8f) + weaponBonus) / 33.33f;
            HP *= totalMultiplier;

            #if UNITY_EDITOR
            Debug.Log($"怪物血量更新 - 基础HP: {HP:F0}, 总倍率: {totalMultiplier:F2}");
            #endif
        }
        Previous_health = HP;
    }


    // Update is called once per frame
    public GameObject monster;
    public float movespeed;
    private float Previous_health;
    private float Getting_damage;
    private float Getting_damage_first;
    private float critical_damage;
    void Update()
    {
        speed_controll(minspeed, maxspeed); // 設定怪物移動速度

        if (Previous_health != HP) //偵測怪物血量是否改變
        {
            Getting_damage_first = Previous_health - HP; //計算怪物受到的傷害值
            float random_critical = Random.Range(0, 100); // 條回0,100
            if(playerControl.Calculating_Values_criticalHitRate > 50) //如果暴擊率大於50%，則暴擊率為50%
            {
                playerControl.Calculating_Values_criticalHitRate = 50;
            }
            if (playerControl.Calculating_Values_criticalHitRate > random_critical) // if暴擊
            {
                Debug.Log("暴擊!!");
                float addition_damage = Getting_damage_first * playerControl.Calculating_Values_damage;
                critical_damage = (Getting_damage_first + addition_damage) * (1+playerControl.Calculating_Values_criticalDamage);
            }
            else                                                                     //else 不暴擊
            {
                critical_damage = Getting_damage_first * playerControl.Calculating_Values_damage;
            }       
            HP -= critical_damage; //計算怪物受到的傷害值，並把加成傷害%數計算進去
            Getting_damage = Previous_health - HP;   //計算怪物因加成傷害%數所受到的傷害值
            
            //最后伤害计算 
            // 生成傷害數字
            Getting_damage = Mathf.Round(Getting_damage); // 取整
            if (playerControl.Calculating_Values_criticalHitRate > random_critical) // 如果爆擊
            {
                if (critical_effect != null)
                {
                    critical_effect.damageEffect_Pop_up(Getting_damage, transform);
                }
            }
            else // 如果沒爆擊
            {
                if (damageEffect != null)
                {
                    Getting_damage = Mathf.Round(Getting_damage);
                    damageEffect.damageEffect_Pop_up(Getting_damage, transform);
                }
            }

            //-------------------------------------------------------------
            //吸血效果
            if (playerControl.HP < Healthbar.slider.maxValue)  //如果玩家血量小於最大血量，則吸血
            {
                playerControl.HP = playerControl.HP + critical_damage * (playerControl.Calculating_Values_lifeSteal/100); //吸血效果
            }
            else
            {
                playerControl.HP = Healthbar.slider.maxValue; //如果玩家血量大於最大血量，則血量為最大血量
            }

            Previous_health = HP;
        }

        if (HP <= 0)
        {
            MonsterDead(monster);
        }
        previousXPosition = transform.position.x; //previousXPosition 為移動前位置
        
        Vector2 direction = player1.position - transform.position;
        direction.Normalize();
        transform.position = Vector2.MoveTowards(transform.position, player1.position, movespeed * Time.deltaTime );

    

        float currentXPosition = transform.position.x;  //currentXPosition 為移動後的位置
        if (currentXPosition < previousXPosition) //x變大往左移動
        {
            if (isFlip == false) 
            {
                isFlip = true;
                transform.Rotate(0.0f,180.0f,0.0f);
            }
            else{}
        }
        if (currentXPosition > previousXPosition) //x變大往右移動
        {
            if (isFlip == true)
            {
                isFlip = false;
                transform.Rotate(0.0f,180.0f,0.0f);
            }
            else{}
        }
        

    }
    public IEnumerator burn_monster(int burn_time)
    {
        isBurning = true;
        
        GameObject burnEffectClone = Instantiate(burn_effect, transform.position, transform.rotation);
        Material material = this.GetComponent<Renderer>().material;
        burnEffectClone.transform.SetParent(transform);
        burnEffectClone.transform.localPosition = new Vector3(0f, -0.01f, 0f);
        
        for(int i = 0; i < burn_time; i++)
        {
            HP -= 1 + playerControl.attack_damage * 0.2f; // 每秒減少的血量
            playerControl.SetBoolWithDelay_void(material, this.GetComponent<Renderer>());
            yield return new WaitForSeconds(1f);
        }

        Destroy(burnEffectClone);
        isBurning = false; // 燃燒結束後重置標記
    }

    public void burn_monster_start(int burn_time)
    {
        // 如果已經在燃燒中，就不重複觸發
        if (!isBurning)
        {
            StartCoroutine(burn_monster(burn_time));
        }
    }






    public void MonsterDead(GameObject monster)

    {
        PlayerControl.kill_monster_count++;
        
        monster.SetActive(false);

        // 判斷怪物類型和經驗值等級
        if (monster_type==0)  // 假設 monster_type = 0 代表一般怪物
        {
            switch (exp_type)
            {
                case 1:  // 低級經驗值
                    // 在這裡生成15經驗值的掉落物
                    if(LowExpPrefab != null)
                    {
                        GameObject expObject = Instantiate(LowExpPrefab, transform.position, Quaternion.identity);
                        expObject.SetActive(true);
                        AudioManager.Instance.PlaySFX("drop_exp");
                        break;
                    }
                    break;

                case 2:  // 中級經驗值
                    if(MediumExpPrefab != null)
                    {
                        // 在這裡生成40經驗值的掉落物
                        GameObject expObject2 = Instantiate(MediumExpPrefab, transform.position, Quaternion.identity);
                        expObject2.SetActive(true);
                        AudioManager.Instance.PlaySFX("drop_exp");
                        break;
                    }  
                    break;

                case 3:  // 高級經驗值
                    if(HighExpPrefab != null)
                    {
                        // 在這裡生成100經驗值的掉落物
                        GameObject expObject3 = Instantiate(HighExpPrefab, transform.position, Quaternion.identity);
                        expObject3.SetActive(true);
                        AudioManager.Instance.PlaySFX("drop_exp");
                        break;
                    }
                    break;

            }
        }
        else if (monster_type==1)  // monster_type = 1 代表水果怪
        {
            switch (exp_type)
            {
                case 1:  // 低級經驗值
                    // 在這裡生成水果怪的15經驗值掉落物
                    // Instantiate(fruitLowExpPrefab, transform.position, Quaternion.identity);
                    AudioManager.Instance.PlaySFX("drop_exp");
                    break;
                case 2:  // 中級經驗值
                    // 在這裡生成水果怪的40經驗值掉落物
                    // Instantiate(fruitMediumExpPrefab, transform.position, Quaternion.identity);
                    AudioManager.Instance.PlaySFX("drop_exp");
                    break;
                case 3:  // 高級經驗值
                    // 在這裡生成水果怪的100經驗值掉落物
                    // Instantiate(fruitHighExpPrefab, transform.position, Quaternion.identity);
                    AudioManager.Instance.PlaySFX("drop_exp");
                    break;
            }
        }
        else if(monster_type == 2) //武器箱怪物
        {
            if(boxPrefab != null)
            {
                GameObject box = Instantiate(boxPrefab, transform.position, Quaternion.identity); //生成武器箱物品
                box.SetActive(true);
            }
        }
    }
    void speed_controll(float Minspeed , float Maxspeed)
    {
        if(monster_type == 2)
        {
            float distanceToPlayer = Vector2.Distance(transform .position,player1.position);
            movespeed = Mathf.Clamp(distanceToPlayer / 2, Minspeed, Maxspeed); // 距離越近速度越慢，距離越遠速度越快，限制速度範圍在Minspeed到Maxspeed之間
        }
    }

}
