using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;


public class PlayerControl : MonoBehaviour
{
    public LevelManager levelManager;
    public character_value_ingame characterValuesIngame;
    public Character_Values_SETUP characterValues;
    [Header("移動速度")]
    public float speed = 5f;
    [Header("普攻冷卻時間")]
    public float attackCooldown = 0.5f; // 攻击冷却时间
    //無敵狀態
    [Header("無敵狀態")]
    public bool isInvincible = false;
    [Header("碰撞怪物傷害特性")]
    public bool isCollision_skill_damage = false;
    public Transform AttackPoint;
    public LayerMask MonsterLayer;
    public LayerMask BossMonsterLayer;
    public LayerMask award_box_layer;
    public float AttackRange;
    public float Knockback_strength;
    public float HP;

    public static int kill_monster_count = 0;

    public AudioSource audioSource;
    public float attack_damage;
    public AudioClip audioClip;
    public healthbar Healthbar;

    


    //--------------------打擊特效開關的bool-----------------------------
    public string boolPropertyName = "_hitBool";
    //--------------------------------------------------------
    public bool isDead = false;
    private monsterMove slime_Scripts;
    private BossFlower bossFlower;
    private NormalMonster_setting normalMonster_setting; 
    private float InputX;
    private float InputY;
    private bool isFlip = false;
    private Rigidbody2D rig;
    private Animator ani;
    private GameObject axeSlash;
    private GameOver gameOver;
    public bool Legend_speed = false;
    public bool hasBurnEffect = false; // 是否具有燃燒效果
    public int burnDuration = 5; // 燃燒持續時間
    public Tetris_ability_Manager tetris_ability_manager;
    [Header("時間倍率")]
    public float timeScale = 1;
    public static float N = 1;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private Coroutine attackCoroutine;
    private ElectricAuraEffect electricAuraEffect; // 添加电气效果引用

    public void Awake()
    {
        gameOver = FindObjectOfType<GameOver>();
    }
    private void Start() 
    {
        rig = GetComponent<Rigidbody2D>();    
        ani = GetComponent<Animator>();
        electricAuraEffect = GetComponent<ElectricAuraEffect>(); // 获取电气效果组件
        //重設UI及時間狀態
        UIstate.isAnyPanelOpen = false;
        N = timeScale;
        Time.timeScale = N; 

        Debug.Log("Time.timeScale = " + Time.timeScale);
        axeSlash = GameObject.Find("Axe_Slashh_0");
        StartCoroutine(hurtDelay());  //啟動傷害判定的延遲迴圈

    }
    [HideInInspector]public float Calculating_Values_damage; //計算加成傷害%數(給怪物腳本用)
    [HideInInspector]public float Calculating_Values_lifeSteal; //計算加成吸血%數(給怪物腳本用)
    [HideInInspector]public float Calculating_Values_criticalDamage; //計算加成暴擊傷害%數(給怪物腳本用)
    [HideInInspector]public float Calculating_Values_criticalHitRate; //計算加成暴擊率%數(給怪物腳本用)
    [HideInInspector]public float Calculating_Values_health; //計算加成血量%數(給怪物腳本用)
    [HideInInspector]public float Calculating_Values_attackCooldown; //計算加成攻擊冷卻時間%數(給怪物腳本用)
    private void Update() 
    {
        
        if (Time.time % 7 < Time.deltaTime) //每過7秒顯示HP
        {
            //Debug.Log("當前HP: " + HP);
            Debug.Log("當前擊殺怪物數: " + kill_monster_count);
        }

        //檢查是否死亡
        bool isgameOverStart = false; //檢查死亡協成是否執行過
        if (HP <= 0  && isInvincible == false)
        {
            isDead = true;
            ani.SetBool("isdead",true);
            //停止所有動作
            InputX = 0;
            InputY = 0;
            rig.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            rig.velocity = Vector2.zero; // 清除殘留速度
            //受傷特效強制關閉
            Material mat = GetComponent<Renderer>().material;
            mat.SetInt(boolPropertyName, 0);

            //確定死亡協成只會執行一次
            if(isgameOverStart == false)
            {
                isgameOverStart = true;
                StartCoroutine(gameOver.dead()); //呼叫GameOver腳本的死亡方法
            }
            ///
        }
        

        // 定義碰撞傷害的執行間隔時間（秒）
        float collisionDamageInterval = 0.5f;
        // 記錄上次執行碰撞傷害的時間
        float lastCollisionDamageTime = 0f;
        // 檢查是否可以執行碰撞傷害
        if(isCollision_skill_damage == true)
        {
            if (Time.time - lastCollisionDamageTime >= collisionDamageInterval)
            {
                Collision_skill_damage();
                lastCollisionDamageTime = Time.time;
                // 启用电气效果
                if (electricAuraEffect != null)
                {
                    electricAuraEffect.SetActive(true);
                }
            }
        }
        else
        {
            // 禁用电气效果
            if (electricAuraEffect != null)
            {
                electricAuraEffect.SetActive(false);
            }
        }

        attack_damage = levelManager.GetCurrentAttack();
        Calculating_Values_damage = characterValuesIngame.damage_percentage + characterValues.damage_addition_percentage + tetris_ability_manager.damage_percentage;
        Calculating_Values_lifeSteal = characterValuesIngame.lifeSteal_percentage + characterValues.lifeSteal_addition_percentage + tetris_ability_manager.lifeSteal_percentage;
        Calculating_Values_criticalDamage = characterValuesIngame.criticalDamage_percentage + characterValues.criticalDamage_addition_percentage + tetris_ability_manager.criticalDamage_percentage;
        Calculating_Values_criticalHitRate = characterValuesIngame.criticalHitRate + characterValues.criticalHitRate_addition + tetris_ability_manager.criticalHitRate;
        speed = levelManager.GetCurrentSpeed() * 
        (1 + characterValuesIngame.speed_percentage + characterValues.speed_addition_percentage + tetris_ability_manager.speed_percentage); // 讀取當前等級的速度 * 能力提升 * 額外加成
        Calculating_Values_health = characterValuesIngame.health + characterValues.health_addition + tetris_ability_manager.health;
        Calculating_Values_attackCooldown = characterValuesIngame.cooldown + characterValues.cooldown_addition + tetris_ability_manager.cooldown;
        if(Legend_speed == true){
            speed += 3;
        }
        // 最大速度限制 (20)
        if(speed > 20)
        {
            speed = 20;
        }
        //暴擊率限制 (70%)
        if(Calculating_Values_criticalHitRate > 50)
        {
            Calculating_Values_criticalHitRate = 50;
        }
        

        
        rig.velocity = new Vector2(speed * InputX , speed * InputY);    
       

        if (math.abs(rig.velocity.x) > 0 || rig.velocity.y != 0)
        {
            ani.SetBool("move",true);
        }
        else
        {
            ani.SetBool("move",false);
        }

        if (!isFlip)
        {
            if (rig.velocity.x > 0f)
            {
                isFlip = true;
                
                transform.Rotate(0.0f,180.0f,0.0f);
                //面向右邊
                
                if (axeSlash != null)
                {
                    axeSlash.transform.Rotate(0.0f, -180.0f, 0.0f);
                }
            }
        }
        else
        {
            if (rig.velocity.x < 0f)
            {
                isFlip = false;
                transform.Rotate(0.0f,180.0f,0.0f);
                if (axeSlash != null)
                {
                    axeSlash.transform.Rotate(0.0f, -180.0f, 0.0f);
                }
            }
        }
        

        

    }
    IEnumerator hurtDelay(){  //設定每0.2秒就會執行一次受傷判定
        if (rig.IsTouchingLayers(MonsterLayer))
        {
            TakeDamage(5);
        }

        yield return new WaitForSeconds(0.2f);
        StartCoroutine(hurtDelay());
    }
   // 扣血方法
    public void TakeDamage(float damage)
    {
        HP -= damage * (1 + characterValuesIngame.damage_taken_addtion_percentage);
        Renderer targetRenderer = rig.GetComponent<Renderer>();
        Material mat = targetRenderer.material;
        StartCoroutine(SetBoolWithDelay_red(mat,targetRenderer));
        Debug.Log("玩家受到傷害: " + damage + ", 目前血量: " + HP);
        //你可以在這裡加入其他受傷的特效, 或是播放受傷音效
    }
    public void Collision_skill_damage()
    {
        float damage = 0; //碰撞怪物傷害計算
        //傷害公式 : 最大生命 * (150% + speed% * 2)
        damage = Healthbar.slider.maxValue * (1 + (0.15f +(speed / 100 * 2))); 

        // 判定如果碰撞到tag為monster
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(transform.position, 0.5f, MonsterLayer);
        
        foreach (Collider2D monster in hitMonsters)
        {
            if (monster.CompareTag("Monster"))
            {
                // 獲取怪物的腳本和渲染器
                NormalMonster_setting monsterScript = monster.GetComponent<NormalMonster_setting>();
                Renderer targetRenderer = monster.GetComponent<Renderer>();
                
                if (monsterScript != null && targetRenderer != null)
                {
                    // 對怪物造成傷害
                    monsterScript.HP -= damage;
                    
                    // 播放受擊特效
                    Material mat = targetRenderer.material;
                    StartCoroutine(SetBoolWithDelay(mat, targetRenderer));
                }
            }
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        InputX = context.ReadValue<Vector2>().x;
        InputY = context.ReadValue<Vector2>().y;
    }

    
    public IEnumerator SetBoolWithDelay(Material mat ,Renderer targetRenderer)   //開啟hit(白色)特效然後0.1秒後關閉
    {
        mat = targetRenderer.material;
        // 设置布尔值为 true
        mat.SetInt(boolPropertyName, 1);
        AudioManager.Instance.PlaySFX("monsterHit"); // 打擊音效
        
        // 等待 0.1 秒
        yield return new WaitForSeconds(0.2f);  //hit閃白時間 
        // 设置布尔值为 false
        mat.SetInt(boolPropertyName, 0);
        
    }
    public IEnumerator SetBoolWithDelay_red(Material mat ,Renderer targetRenderer)   //開啟hit(紅色)特效然後0.1秒後關閉
    {
        if (isDead) yield break;
        mat = targetRenderer.material;
        // 设置布尔值为 true

        mat.SetInt(boolPropertyName, 1);
        
        // 等待 0.1 秒
        yield return new WaitForSeconds(0.2f);  //hit閃紅時間 
        // 设置布尔值为 false
        mat.SetInt(boolPropertyName, 0);
        
    }

    public void ALLdemageCheck(){     
        demageCheck();
        demageCheck2();
        demageCheck3();
        demageCheck4();
    }
    public void demageCheck(){          //史萊姆傷害判定
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, MonsterLayer); 

        // 如果有怪物進入範圍   (確定史萊姆類型的)
        foreach (Collider2D slimemonster in hitMonsters)  //這裡monster指進到攻擊範圍內的gameObject 
        {
            Renderer targetRenderer = slimemonster.GetComponent<Renderer>(); //抓取複製怪的renderer
            monsterMove cloneSlime_Scripts = slimemonster.GetComponent<monsterMove>(); //讀取在攻擊範圍內的怪物腳本
            Transform slime_T = slimemonster.GetComponent<Transform>();
            if (cloneSlime_Scripts != null)
            {           
                cloneSlime_Scripts.HP -= 1;                      //改變攻擊範圍內怪物的HP變數
                if (targetRenderer != null)
                {
                    // 获取材质实例（确保不会修改共享材质）
                    Material mat = targetRenderer.material;
                    // 打擊特效       
                    StartCoroutine(SetBoolWithDelay(mat,targetRenderer));  
                }
                Debug.Log("怪物HP: " + cloneSlime_Scripts.HP);
            }
            if (slime_T != null){                      //對史萊姆的擊退設定
                Vector3 direction = slime_T.position - transform.position; // slime 到玩家的方向
                direction.Normalize();
                Rigidbody2D slimeRb = slime_T.GetComponent<Rigidbody2D>();
                if (slimeRb != null) {
                    slimeRb.AddForce(direction * 10 * Knockback_strength, ForceMode2D.Impulse);
                }
            }
            else{break;}

        }

    }
    public void demageCheck2(){        //一般平移怪物傷害判定
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, MonsterLayer); 
        // 如果有怪物進入範圍   (確定一般怪物類型的)
        
        foreach (Collider2D Normalmonster in hitMonsters)  //這裡monster指進到攻擊範圍內的gameObject 
        {
            Renderer targetRenderer = Normalmonster.GetComponent<Renderer>(); //抓取複製怪的renderer
            NormalMonster_setting clone_Scripts = Normalmonster.GetComponent<NormalMonster_setting>(); //讀取在攻擊範圍內的怪物腳本
            Transform Normal_T = Normalmonster.GetComponent<Transform>();
            if (clone_Scripts != null){
                clone_Scripts.HP -= attack_damage;                      //改變攻擊範圍內怪物的HP變數
                if (targetRenderer != null)
                {
                    // 获取材质实例（确保不会修改共享材质）
                    Material mat = targetRenderer.material;
                    // 打擊特效       
                    StartCoroutine(SetBoolWithDelay(mat,targetRenderer));    //飛斧測試
                }
                //Debug.Log("怪物HP: " + clone_Scripts.HP);
            }
            if (Normal_T != null){                      //對一般的擊退設定
                Vector3 direction = Normal_T.position - transform.position; // 一般怪物 到玩家的方向
                direction.Normalize();
                Rigidbody2D NormalRb = Normal_T.GetComponent<Rigidbody2D>();
                if (NormalRb != null) {
                    NormalRb.AddForce(direction * 10 * Knockback_strength, ForceMode2D.Impulse);
                }
            }

        }
        
    }   

    public void demageCheck3(){          //bossflower傷害判定
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, BossMonsterLayer); 

        // 如果有怪物進入範圍   (確定bossflower的)
        foreach (Collider2D bossFlower in hitMonsters)  //這裡monster指進到攻擊範圍內的gameObject 
        {
            Renderer targetRenderer = bossFlower.GetComponent<Renderer>(); //抓取複製怪的renderer
            BossFlower cloneflower_Scripts = bossFlower.GetComponent<BossFlower>(); //讀取在攻擊範圍內的怪物腳本
            if (cloneflower_Scripts != null)
            {           
                cloneflower_Scripts.HP -= attack_damage;                      //改變攻擊範圍內怪物的HP變數
                if (targetRenderer != null)
                {
                    // 获取材质实例（确保不会修改共享材质）
                    Material mat = targetRenderer.material;
                    // 打擊特效       
                    StartCoroutine(SetBoolWithDelay(mat,targetRenderer));  
                }
                Debug.Log("怪物HP: " + cloneflower_Scripts.HP);
            }
        
            else{break;}

        }

    }   
    public void demageCheck4(){          //box傷害判定
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, award_box_layer); 

        // 如果有怪物進入範圍   (確定box的)
        foreach (Collider2D box in hitMonsters)  //這裡monster指進到攻擊範圍內的gameObject 
        {
            Renderer targetRenderer = box.GetComponent<Renderer>(); //抓取複製怪的renderer
            Award_box_body box_Scripts = box.GetComponent<Award_box_body>(); //讀取在攻擊範圍內的怪物腳本
            if (box_Scripts != null)
            {           
                box_Scripts.HP -= attack_damage;                      //改變攻擊範圍內怪物的HP變數
                if (targetRenderer != null)
                {
                    // 获取材质实例（确保不会修改共享材质）
                    Material mat = targetRenderer.material;
                    // 打擊特效       
                    StartCoroutine(SetBoolWithDelay(mat,targetRenderer));  
                }
                Debug.Log("怪物HP: " + box_Scripts.HP);
            }
        
            else{break;}

        }

    }  

    public void Attack(InputAction.CallbackContext context)
    {
        if (isDead) return;
        
        // 当按键被按下时
        if (context.started)
        {
            isAttacking = true;
            // 立即执行一次攻击
            PerformAttack();
            // 开始协程处理持续按住的情况
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(ContinuousAttackRoutine());
            }
        }
        // 当按键被释放时
        else if (context.canceled)
        {
            isAttacking = false;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    // 实际执行攻击的方法
    private void PerformAttack()
    {
        // 检查攻击冷却
        if (Time.time - lastAttackTime < attackCooldown)
            return;
            
        lastAttackTime = Time.time;
        ani.SetBool("attack", true);
        
        // 如果您的动画需要重置，可以添加一个延迟重置
        StartCoroutine(ResetAttackAnimation());
    }

    // 重置攻击动画状态
    private IEnumerator ResetAttackAnimation()
    {
        // 等待一小段时间后重置动画状态
        yield return new WaitForSeconds(0.2f); // 调整这个时间以匹配您的动画
        ani.SetBool("attack", false);
    }

    // 处理持续按住按键的协程
    private IEnumerator ContinuousAttackRoutine()
    {
        // 等待第一次攻击完成的冷却时间
        yield return new WaitForSeconds(attackCooldown);
        
        // 只要按键仍被按住，就持续执行攻击
        while (isAttacking)
        {
            PerformAttack();
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(AttackPoint.position,AttackRange);    
    }
    public void SetBoolWithDelay_void(Material mat, Renderer renderer)
    {
        StartCoroutine(SetBoolWithDelay(mat, renderer));
    }

    // 新增獲取擊殺數量的方法
    public static int GetKillCount()
    {
        return kill_monster_count;
    }

    // 新增重置擊殺數量的方法
    public static void ResetKillCount()
    {
        kill_monster_count = 0;
    }
}