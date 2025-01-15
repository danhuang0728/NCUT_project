using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Yellow_Circle : MonoBehaviour
{
    public GameObject yellowEyeEffectPrefab;  // 黃眼特效 Prefab
    public GameObject lightningStrikePrefab;     // 雷擊特效 Prefab
    public GameObject warningIconPrefab;     // 警告圖示 Prefab
    public float lightningRadius = 1f;   // 雷擊範圍 (半徑)
    public int lightningDamage = 10;     // 雷擊傷害值
    public LayerMask playerLayer;        // 玩家所在的圖層
    public float attackCooldown = 6f;    // 攻擊冷卻時間 (秒)
    public int circleCount = 4;           // 同心圓次數
    public float circleInterval = 0.5f;   // 每圈同心圓間隔時間
    public float warningTime = 0.5f;           // 警告圖示顯示時間

    public int LightingCount = 6;        // 洛雷密集程度
    private Transform player;           // 玩家的 Transform 元件
    private float attackTimer = 0f;      // 攻擊計時器
    private Transform yellowEye;          // 黃眼物件的 Transform 元件 (用於產生藍眼特效)
     private Vector3 centerPosition = new Vector3(280f, -85f, 0f);


    void Start()
    {
        // 在 Start 方法中找到玩家的 Transform 元件
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        // 在 Start 方法中找到黃眼物件的 Transform 元件
         yellowEye = transform.Find("eyes")?.Find("eyeball_yellow_0");

         if (yellowEye == null)
        {
            Debug.LogError("Error: yellow eye object 'eyeball_yellow_0' not found under 'eyes' GameObject");
        }
         if(player == null){
            Debug.LogError("Error: Player object not found or has incorrect Tag");
         }
    }
    void Update()
    {
        attackTimer += Time.deltaTime;
         // 如果攻擊計時器達到冷卻時間，則執行攻擊
        if (attackTimer >= attackCooldown)
        {
            if (player != null && yellowEye != null)
            {
                StartCoroutine(ConcentricLightningAttack()); // 啟動黃眼攻擊協程
            }
            attackTimer = 0f; // 重置攻擊計時器
        }
         if (Input.GetKeyDown(KeyCode.Alpha4)) // 按下數字鍵 4 觸發攻擊
         {
            if (player != null && yellowEye != null)
            {
                StartCoroutine(ConcentricLightningAttack()); // 啟動黃眼攻擊協程
            }
        }

    }

    IEnumerator ConcentricLightningAttack()
     {
          // 在黃眼位置產生黃眼特效
        GameObject yellowEyeEffect = Instantiate(yellowEyeEffectPrefab, yellowEye.position, Quaternion.identity, yellowEye);
         Destroy(yellowEyeEffect, 1f); // 藍眼特效在 1 秒後自動銷毀

        for (int i = circleCount - 1; i >= 0; i--)
        {
            // 等待一段時間，在執行下一圈
            yield return new WaitForSeconds(circleInterval);

           // 計算半徑
            float radius = 2 + i * 2f;

            // 計算每個雷擊特效的旋轉角度 調整雷擊的密集程度
            int numLightning = LightingCount + i * 3;

              for (int j = 0; j < numLightning; j++)
              {
                float angle = j * 2 * Mathf.PI / numLightning;
                Vector3 lightningPosition = centerPosition+ new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0);

                   // 產生警告圖示
                  GameObject warningIcon = Instantiate(warningIconPrefab, lightningPosition, Quaternion.identity);
                //延遲一段時間後產生雷擊動畫
                StartCoroutine(SpawnLightning(warningIcon));

            }
         }
       yield return null;
    }
   IEnumerator SpawnLightning(GameObject warningIcon){
          // 取得警告圖示的 Transform 元件, 並將雷擊的位置設為警告圖示的位置
         Vector3 lightningPosition = warningIcon.transform.position;
         yield return new WaitForSeconds(warningTime);
        // 產生雷擊特效 //調整雷擊動畫位置
          GameObject lightningStrike = Instantiate(lightningStrikePrefab, lightningPosition+new Vector3(-0.2f,22.3f,0), Quaternion.identity);
         

        //偵測雷擊範圍內是否有玩家
         Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(lightningPosition, lightningRadius, playerLayer);

         // 對範圍內玩家造成傷害
         foreach (Collider2D hitPlayer in hitPlayers)
         {
           // 取得玩家的 PlayerControl 腳本
             PlayerControl playerControl = hitPlayer.GetComponent<PlayerControl>();
             // 如果成功取得 PlayerControl 腳本, 則呼叫 TakeDamage 方法
            if (playerControl != null)
           {
              playerControl.TakeDamage(lightningDamage);
            }
         }
        // 雷擊特效在 2 秒後自動銷毀
         Destroy(lightningStrike, 1f);
       // 銷毀警告圖示
       Destroy(warningIcon);
    }
    // 用於在場景視窗中顯示雷擊範圍 (僅在編輯器中顯示)
    private void OnDrawGizmosSelected()
    {
          if (player == null) return;
          Gizmos.color = Color.yellow;
        for (int i = 0; i < circleCount; i++){
            float radius = 2 + i * 2f;
             Gizmos.DrawWireSphere(centerPosition, radius);
        }
    }
}