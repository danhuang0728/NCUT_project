using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Purple_Eyes_Slowdown : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Vector2 minRange; // 矩形範圍的左下角
    public Vector2 maxRange; // 矩形範圍的右上角
    public LayerMask playerLayer; // 玩家物理圖層
    public GameObject Slowdown_effect1;
    public GameObject Slowdown_effect2;
    private PlayerControl playerControl;
    public float Slowdown_Percent; //緩數程度(單位%)
    private float Slowdown; //緩速倍率 
    private Vector2 targetPosition;
    public Collider2D collider2d;
    private float initial_speed; 
    void Start()
    {
        
        playerControl = GameObject.Find("player1").GetComponent<PlayerControl>();
        Debug.Log(playerControl);

        if (playerControl != null)
        {
            initial_speed = playerControl.speed; // 保存原始速度
        }
        else
        {
            Debug.LogError("PlayerControl 尚未正確初始化！");
        }
        collider2d = GetComponent<Collider2D>(); 
        Slowdown = 1-(Slowdown_Percent/100);  
        SetNewTargetPosition();
    }

    void Update()
    {
        //Debug.Log(playerControl.speed);
        if (collider2d.IsTouchingLayers(playerLayer))  // 判定在範圍內
        {
            if (collider2d != null) 
            {
                playerControl.speed = initial_speed * Slowdown;  //玩家速度設為緩速過的速度
                Slowdown_effect1.SetActive(true);
                Slowdown_effect2.SetActive(true);
                
            }
            
        }
        else
        {
            Slowdown_effect1.SetActive(false);
            Slowdown_effect2.SetActive(false);
            playerControl.speed = initial_speed;  //離開時將速度恢復
        }
        // 移動到目標點
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 如果接近目標點，設置新的目標點
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition();
        }
    }

    void SetNewTargetPosition()
    {
        float randomX = Random.Range(minRange.x, maxRange.x);
        float randomY = Random.Range(minRange.y, maxRange.y);
        targetPosition = new Vector2(randomX, randomY);
    }
    
}
