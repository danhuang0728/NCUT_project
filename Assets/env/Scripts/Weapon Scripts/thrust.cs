using UnityEngine;

public class thrust : MonoBehaviour 
{
    public float damage = 1f; // 傷害值
    public float knockbackForce = 10f; // 擊退力度

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 確認碰撞到的物件是怪物且不是玩家
        if (other.CompareTag("Monster") && !other.CompareTag("Player"))
        {
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            if (monster != null)
            {
                monster.HP -= damage;
                Debug.Log("Monster =" + monster);

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
