using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearWeapon : MonoBehaviour
{
    public float startAngle = 0f;       // 起始角度
    public float rotateSpeed = 100f;    // 旋轉速度
    public float radius = 2f;           // 旋轉半徑
    public float damage = 1f;          // 傷害值
    public float knockbackForce = 5f;   // 擊退力道
    
    private PlayerControl playerControl; //抓玩家腳本
    

    private Transform player;
    private float currentAngle;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerControl = player.GetComponent<PlayerControl>();
        currentAngle = startAngle;
    }

    void Update()
    {
        // 更新角度
        currentAngle += rotateSpeed * Time.deltaTime;
        
        // 計算齒輪的新位置
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
        
        // 更新齒輪位置
        transform.position = player.position + new Vector3(x, y, 0);
        
        // 齒輪自轉
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            BossFlower bossFlower = other.GetComponent<BossFlower>();
            
            if (bossFlower != null)
            {
                Renderer renderer_flower = bossFlower.GetComponent<Renderer>();
                bossFlower.HP -= damage;
                // 檢查是否有燃燒效果
                PlayerControl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
                if (player != null && player.hasBurnEffect)
                {
                    monster.burn_monster_start(player.burnDuration);
                }
                playerControl.SetBoolWithDelay_void(renderer_flower.material, renderer_flower);
            }

            if (monster != null)
            {
                // 造成傷害
                monster.HP -= damage;
                // 檢查是否有燃燒效果
                PlayerControl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
                if (player != null && player.hasBurnEffect)
                {
                    monster.burn_monster_start(player.burnDuration);
                }
                // 取得 Renderer 組件
                Renderer renderer = other.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = renderer.material;
                    playerControl.SetBoolWithDelay_void(mat, renderer);

                    // 若需要延遲恢復顏色，可用協程
                    //StartCoroutine(ResetColorAfterDelay(renderer, Color.white, 0.3f));
                }
                // 擊退效果
                Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                Rigidbody2D monsterRb = other.GetComponent<Rigidbody2D>();
                if (monsterRb != null)
                {
                    monsterRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
