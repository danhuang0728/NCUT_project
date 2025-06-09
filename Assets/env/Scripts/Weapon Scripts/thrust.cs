using UnityEngine;

public class thrust : MonoBehaviour 
{
    private float damage = 1f; // 傷害值
    public float knockbackForce = 1.5f; // 擊退力度
    public bool is_evolution_object = false;
    public PlayerControl playerControl; //抓玩家腳本
    private ToggleChildObjects thrust_script;
    private int level = 1;

    private void Start()
    {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        thrust_script = FindObjectOfType<ToggleChildObjects>();
        level = thrust_script.level;
        UpdateDamage();
    }

    private void UpdateDamage()
    {
        if(is_evolution_object)
        {
            switch(level)
            {
                case 1:
                    damage = 10f * 1.5f;
                    break;
                case 2:
                    damage = 20f * 1.5f;
                    break;
                case 3:
                    damage = 40f * 1.5f;
                    break;
                case 4:
                    damage = 80f * 1.5f;
                    break;
                case 5:
                    damage = 100f * 1.5f;
                    break;
            }
        }
        else
        {
            switch(level)
            {
                case 1:
                    damage = 15f;
                    break;
                case 2:
                    damage = 28f;
                    break;
                case 3:
                    damage = 55f;
                    break;
                case 4:
                    damage = 80f;
                    break;
                case 5:
                    damage = 120f;
                    break;
            }
        }
    }

    private void Update()
    {
        if (thrust_script != null && level != thrust_script.level)
        {
            level = thrust_script.level;
            UpdateDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 確認碰撞到的物件是怪物且不是玩家
        if (other.CompareTag("Monster") && !other.CompareTag("Player"))
        {
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            BossFlower bossFlower = other.GetComponent<BossFlower>();
            if (bossFlower != null)
            {
                Renderer renderer_flower = bossFlower.GetComponent<Renderer>();
                bossFlower.HP -= damage;
                playerControl.SetBoolWithDelay_void(renderer_flower.material, renderer_flower);
            }
            if (monster != null)
            {
                monster.HP -= damage;
                // 檢查是否有燃燒效果
                PlayerControl player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
                if (player != null && player.hasBurnEffect)
                {
                    monster.burn_monster_start(player.burnDuration);
                }
                Debug.Log("Monster = " + monster);

                // 嘗試取得 Renderer 組件
                Renderer renderer = other.GetComponent<Renderer>();
                if (renderer != null)
                {
                    
                    Material mat = renderer.material;

                    playerControl.SetBoolWithDelay_void(mat,renderer);

                    // 若需要延遲恢復顏色，可用協程
                    //StartCoroutine(ResetColorAfterDelay(renderer, Color.white, 0.3f));
                }
                else
                {
                    Debug.Log("Renderer not found on this Monster.");
                }

                // 處理擊退效果
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
/*
     協程：在延遲後將材質顏色恢復
    private System.Collections.IEnumerator ResetColorAfterDelay(Renderer renderer, Color originalColor, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (renderer != null && renderer.material != null)
        {
            renderer.material.color = originalColor;
        }
    }
}
*/