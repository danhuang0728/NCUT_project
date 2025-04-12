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
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }    
}