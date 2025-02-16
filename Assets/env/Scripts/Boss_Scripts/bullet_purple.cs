using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class bullet_purple : MonoBehaviour
{
    public float speed = 5f; // 子彈速度
    public float damage = 2f; // 子彈傷害
    private Rigidbody2D rb; // 子彈的 Rigidbody

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 設定子彈速度 (子彈速度會依照先前發射的角度決定)
    }

    // 使用 OnTriggerEnter2D 偵測碰撞
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 碰到玩家或牆壁就銷毀子彈
        if (other.CompareTag("Player") || other.CompareTag("wall"))
        
        {
            if(other.CompareTag("Player"))
            {
                PlayerControl playerControl = other.GetComponent<PlayerControl>();
            // 如果成功取得 PlayerControl 腳本, 則呼叫 TakeDamage 方法
                if (playerControl != null)
                    {
                        playerControl.TakeDamage(damage);
                    }
            }
            Destroy(gameObject);
            
        }
    }

}

