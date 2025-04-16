using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fly_axe : MonoBehaviour
{
    [Header("基本設定")]
    public float damage = 5f;                    // 斧頭傷害
    public float knockbackForce = 5f;            // 擊退力度
    public float rotateSpeed = 720f;             // 旋轉速度 (每秒旋轉度數)
    public float flySpeed = 10f;                 // 飛行速度
    public float maxFlyTime = 3f;               // 最大飛行時間
    private PlayerControl playerController;
    private Vector2 flyDirection;
    private float currentFlyTime = 0f;
    private bool isReturning = false;
    private Vector3 startPosition;
    private Transform playerTransform;

    private void Awake()
    {
        // 確保有碰撞器並正確設置
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
        }
        col.isTrigger = true;

        // 確保有Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void InitializeAxe(Vector2 direction)
    {
        flyDirection = direction.normalized;
        
        // 獲取玩家引用
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerControl>();
            playerTransform = player.transform;
        }

        // 記錄起始位置
        startPosition = transform.position;
        
        // 給予初始速度
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = flyDirection * flySpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 旋轉斧頭
        transform.Rotate(0, 0, 2*rotateSpeed * Time.deltaTime);
    }    

    // 碰撞檢測
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("碰撞觸發，碰撞對象: " + other.gameObject.name + ", Tag: " + other.tag);
        
        if (other.CompareTag("wall"))
        {
            //Debug.Log("碰到牆壁，準備銷毀飛斧");
            Destroy(gameObject); // 銷毀飛斧
        }
    }

    // 添加物理碰撞檢測作為備用
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("物理碰撞觸發，碰撞對象: " + collision.gameObject.name + ", Tag: " + collision.gameObject.tag);
        
        if (collision.gameObject.CompareTag("wall"))
        {
            //Debug.Log("碰到牆壁，準備銷毀飛斧");
            Destroy(gameObject);
        }
    }
}