using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Transform player;
    public float delaySeconds = 60f; // Time between animation plays

    private Animator animator;
    private GameObject axeSlash;
    private character_value_ingame characterValuesIngame;
    public Character_Values_SETUP characterValues;
    public float damage = 1f; // 傷害值
    public float knockbackForce = 10f; // 擊退力度
    private Collider2D attackCollider; // 武器的 Collider，用於檢測碰撞區域
    public PlayerControl playerControl; // 抓玩家腳本（處理變色延遲等）
    public int level;

    // Start is called before the first frame update
    void Start()
    {
        characterValuesIngame = GameObject.Find("player1").GetComponent<character_value_ingame>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        axeSlash = GameObject.Find("Axe_Slashh_0");
        attackCollider = GetComponent<Collider2D>(); // 取得武器的 Collider
        if (axeSlash != null)
        {
            animator = axeSlash.GetComponent<Animator>();
        }
        StartCoroutine(PlayAnimationPeriodically());
    }

    private IEnumerator PlayAnimationPeriodically()
    {
        while (true)
        {
            if (animator != null)
            {
                animator.Play("axe_swing", -1, 0f);         // 斧頭動畫
                AudioManager.Instance.PlaySFX("axe_swing"); // 斧頭揮擊音效
            }
            yield return new WaitForSeconds(delaySeconds- (delaySeconds * (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage)));
        }
    }
    // Animation Event 綁定的方法
    public void CheckCollision()
    {
        // 檢測所有與攻擊範圍碰撞的物體
        List<Collider2D> hitObjects = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        Physics2D.OverlapCollider(attackCollider, filter, hitObjects);
        foreach (Collider2D other in hitObjects)
        {
            // 確認碰撞到的物件是怪物
            if (other.CompareTag("Monster"))
            {
                NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
                BossFlower bossFlower = other.GetComponent<BossFlower>(); // 加入對 BossFlower 的判定
                


                if (bossFlower != null)


                {
                    Renderer renderer_flower = bossFlower.GetComponent<Renderer>();
                    if (renderer_flower != null) // 確保 renderer_flower 存在
                    {
                        bossFlower.HP -= damage;
                        playerControl.SetBoolWithDelay_void(renderer_flower.material, renderer_flower);
                        
                        // 檢查是否有燃燒效果
                        PlayerControl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
                        if (player != null && player.hasBurnEffect)
                        {
                            monster.burn_monster_start(player.burnDuration);
                        }
                    }
                    else
                    {
                        Debug.Log("Renderer not found on BossFlower.");
                    }
                }
                if (monster != null)
                {
                    // 處理傷害
                    monster.HP -= damage;
                    //Debug.Log("Hit Monster: " + monster);

                    // 處理材質變色效果
                    Renderer renderer = other.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Material mat = renderer.material;
                        playerControl.SetBoolWithDelay_void(mat, renderer);
                    }
                        
                    // 處理擊退效果
                    Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                    Rigidbody2D monsterRb = other.GetComponent<Rigidbody2D>();
                    if (monsterRb != null)
                    {
                        monsterRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                    }
                    // 檢查是否有燃燒效果
                        PlayerControl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
                        if (player != null && player.hasBurnEffect)
                    {
                        monster.burn_monster_start(player.burnDuration);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        level_check(level);
        transform.position = new Vector3(player.position.x , player.position.y + 0.3f, player.position.z);
        
        // 獲取玩家的旋轉值並同步斧頭的Y軸旋轉
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        if (playerControl != null)
        {
            float playerRotationY = player.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180 + playerRotationY, transform.rotation.eulerAngles.z);
        }
    }
    public void level_check(int level)
    {
        if(level == 1)
        {
            delaySeconds = 5f;
            damage = 50f;
        }
        else if(level == 2)
        {
            delaySeconds = 4f;
            damage = 85f;
        }
        else if(level == 3)
        {
            delaySeconds = 3f;
            damage = 120f;
        }
        else if(level == 4)
        {
            delaySeconds = 2f;
            damage = 170f;
        }
        else if(level == 5)
        {
            delaySeconds = 1f;
            damage = 250f;
        }
    }
}