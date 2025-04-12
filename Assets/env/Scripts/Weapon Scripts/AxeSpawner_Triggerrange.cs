using UnityEngine;

public class AxeSpawner_Triggerrange : MonoBehaviour
{
    public Transform player_t;
    public AxeSpawner axeSpawner;
    public float detectionRadius = 5f; // 檢測範圍

    void Start()
    {
        // 設置觸發器碰撞體
        CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = detectionRadius;
        circleCollider.isTrigger = true;

        // 獲取AxeSpawner組件
        if (axeSpawner == null)
        {
            axeSpawner = GetComponent<AxeSpawner>();
        }
    }

    void Update()
    {
        if (player_t != null)
        {
            transform.position = player_t.position;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            axeSpawner.Is_in_range = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {  
            axeSpawner.Is_in_range = false;
        }
    }

    // 在編輯器中繪製檢測範圍（方便調試）
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
