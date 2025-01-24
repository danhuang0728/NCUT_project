using UnityEngine;

public class thrust : MonoBehaviour 
{
    public float damage = 1f; // 傷害值
    public float knockbackForce = 10f; // 擊退力度

    public PlayerControl playerControl; //抓玩家腳本
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 確認碰撞到的物件是怪物且不是玩家
        if (other.CompareTag("Monster") && !other.CompareTag("Player"))
        {
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            if (monster != null)
            {
                monster.HP -= damage;
                Debug.Log("Monster = " + monster);

                // 嘗試取得 Renderer 組件
                Renderer renderer = other.GetComponent<Renderer>();
                if (renderer != null)
                {
                    
                    Material mat = renderer.material;

                    StartCoroutine(playerControl.SetBoolWithDelay(mat,renderer));

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