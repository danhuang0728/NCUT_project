using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exp_magnetic : MonoBehaviour
{
    public float attractRange = 2.3f; // 吸引範圍
    public float attractSpeed = 10f; // 吸引速度
    private Transform playerTransform; // 玩家位置

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // 獲取所有帶有"exp"標籤的物體
        GameObject[] expObjects = GameObject.FindGameObjectsWithTag("exp");
        
        foreach (GameObject expObject in expObjects)
        {
            // 計算與玩家的距離
            float distance = Vector3.Distance(transform.position, expObject.transform.position);
            
            // 如果在吸引範圍內
            if (distance <= attractRange)
            {
                // 計算移動方向
                Vector3 direction = (playerTransform.position - expObject.transform.position).normalized;
                
                // 根據距離調整速度（越近越快）
                float currentSpeed = attractSpeed * (1 - distance / attractRange);
                
                // 移動經驗值物體
                expObject.transform.position += direction * currentSpeed * Time.deltaTime;
            }
        }
    }

    // 在Scene視圖中顯示吸引範圍
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractRange);
    }
}
