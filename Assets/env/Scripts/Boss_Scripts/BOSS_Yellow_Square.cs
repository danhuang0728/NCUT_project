using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Yellow_Square : MonoBehaviour
{
    public GameObject yellowEyeEffectPrefab;  // 黃眼特效 Prefab
    public GameObject lightningStrikePrefab;     // 雷擊特效 Prefab
    public GameObject warningIconPrefab;     // 警告圖示 Prefab
    public float lightningRadius = 1f;   // 雷擊範圍 (半徑)
    public int lightningDamage = 10;     // 雷擊傷害值
    public LayerMask playerLayer;        // 玩家所在的圖層
    public float attackCooldown = 8f;    // 攻擊冷卻時間 (秒)
    public float warningTime = 0.5f;    // 警告圖示顯示時間 (秒)
    public int squareSideCount = 5;   // 正方形邊長上的點數量
    private Transform player;           // 玩家的 Transform 元件
    private float attackTimer = 0f;      // 攻擊計時器
    private Transform yellowEye;          // 黃眼物件的 Transform 元件 (用於產生黃眼特效)
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
                StartCoroutine(SquareLightningAttack()); // 啟動黃眼攻擊協程
            }
            attackTimer = 0f; // 重置攻擊計時器
        }
         if (Input.GetKeyDown(KeyCode.Alpha6)) // 按下數字鍵 5 觸發攻擊
        {
            if (player != null && yellowEye != null)
            {
                StartCoroutine(SquareLightningAttack()); // 啟動黃眼攻擊協程
             }
         }
    }
    IEnumerator SquareLightningAttack()
    {
        // 在黃眼位置產生黃眼特效
         GameObject yellowEyeEffect = Instantiate(yellowEyeEffectPrefab, yellowEye.position, Quaternion.identity, yellowEye);
         Destroy(yellowEyeEffect, 1f); // 藍眼特效在 1 秒後自動銷毀

         // 計算正方形的雷擊位置
         List<Vector3> lightningPositions = GetSquareLightningPositions();

       // 在每個雷擊位置產生警告圖示，延遲後產生雷擊
       foreach (Vector3 lightningPosition in lightningPositions)
       {
             GameObject warningIcon = Instantiate(warningIconPrefab, lightningPosition, Quaternion.identity);
           StartCoroutine(SpawnLightning(warningIcon));
       }
         yield return null;
    }
   List<Vector3> GetSquareLightningPositions()
   {
       List<Vector3> positions = new List<Vector3>();

        float sideLength = 4f; // 邊長
        float startX = centerPosition.x - (sideLength * (squareSideCount - 1) / 2f);
        float startY = centerPosition.y - (sideLength * (squareSideCount - 1) / 2f);
        for (int i = 0; i < squareSideCount; i++)
        {
            // 上邊
            positions.Add(new Vector3(startX + (sideLength * i), startY, 0));
              // 下邊
             positions.Add(new Vector3(startX + (sideLength * i), startY + (sideLength * (squareSideCount-1)), 0));
              //左邊
             positions.Add(new Vector3(startX, startY + (sideLength * i), 0));
              //右邊
           positions.Add(new Vector3(startX + (sideLength * (squareSideCount - 1)), startY + (sideLength * i), 0));

        }
       return positions;
    }

   IEnumerator SpawnLightning(GameObject warningIcon){
        // 取得警告圖示的 Transform 元件, 並將雷擊的位置設為警告圖示的位置
        Vector3 lightningPosition = warningIcon.transform.position;

          // 延遲一段時間
        yield return new WaitForSeconds(warningTime);
         // 在警告圖示的位置產生雷擊特效
        GameObject lightningStrike = Instantiate(lightningStrikePrefab, lightningPosition, Quaternion.identity);
         // **將雷擊特效旋轉 90 度**
       lightningStrike.transform.Rotate(Vector3.forward * 90);
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
         // 雷擊特效在 1 秒後自動銷毀
        Destroy(lightningStrike, 1f);
          // 銷毀警告圖示
         Destroy(warningIcon);
 }

    // 用於在場景視窗中顯示雷擊範圍 (僅在編輯器中顯示)
    private void OnDrawGizmosSelected()
    {
         if (player == null) return;
         Gizmos.color = Color.yellow;

         float sideLength = 4f; // 邊長
          float startX = centerPosition.x - (sideLength * (squareSideCount - 1) / 2f);
         float startY = centerPosition.y - (sideLength * (squareSideCount - 1) / 2f);

         for (int i = 0; i < squareSideCount; i++)
        {
            Gizmos.DrawWireSphere(new Vector3(startX + (sideLength * i), startY, 0), lightningRadius);
            Gizmos.DrawWireSphere(new Vector3(startX + (sideLength * i), startY + (sideLength * (squareSideCount-1)), 0), lightningRadius);
             Gizmos.DrawWireSphere(new Vector3(startX, startY + (sideLength * i), 0), lightningRadius);
              Gizmos.DrawWireSphere(new Vector3(startX + (sideLength * (squareSideCount-1)), startY + (sideLength * i), 0), lightningRadius);
       }

    }
}