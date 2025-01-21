using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlakHoleTrigger : MonoBehaviour
{
    public Transform player_T;           // 玩家 Transform
    public GameObject player_object;     // 玩家 GameObject
    private Vector3 BlackHole_direction; // 黑洞吸引方向
    private Vector3 BlackHole_P;         // 黑洞位置
    public float attractionSpeed = 5f;  // 吸引速度
    private float nextActionTime = 0f;   // 下一次吸引的執行時間
    private float actionInterval = 0.05f; // 執行間隔時間
    public float minDistance = 0.1f;     // 停止吸引的最小距離
    public float maxAttractionSpeed = 5f; // 最大吸引速度

    void Start()
    {
        BlackHole_P = transform.position; // 設定黑洞位置
        player_object = GameObject.FindWithTag("Player"); // 找到玩家物件
        if (player_object != null)
        {
            player_T = player_object.transform;
        }
        else
        {
            Debug.LogError("未找到玩家物件，請確認場景中是否存在標籤為 'Player' 的物件！");
        }
    }

    void Update()
    {
        if (player_T != null)
        {
            // 更新黑洞到玩家的方向
            BlackHole_direction = BlackHole_P - player_T.position;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && player_T != null)
        {
            if (Time.time >= nextActionTime)
            {
                nextActionTime = Time.time + actionInterval;

                // 計算吸引方向向量並歸一化
                Vector3 normalizedDirection = BlackHole_direction.normalized;

                // 計算玩家到黑洞的距離
                float distanceToCenter = Vector3.Distance(BlackHole_P, player_T.position);

                // 若玩家距離小於 minDistance，停止吸引
                if (distanceToCenter > minDistance)
                {
                    Rigidbody2D rb = player_object.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        // 按吸引速度計算新目標位置
                        float attractionStrength = Mathf.Clamp(10f / distanceToCenter, 0f, maxAttractionSpeed);
                        Vector3 targetPosition = player_T.position + normalizedDirection * (attractionStrength * attractionSpeed * Time.deltaTime);

                        // 平滑移動玩家位置
                        rb.MovePosition(Vector3.Lerp(rb.position, targetPosition, 0.5f));
                        //Debug.Log("玩家正在被吸引！");
                    }
                }
                else
                {
                    //Debug.Log("玩家已到達黑洞中心，停止吸引！");
                }
            }
        }
    }
}
