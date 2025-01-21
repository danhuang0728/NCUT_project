using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_red_line_explosion : MonoBehaviour
{
    public GameObject blueEyeEffectPrefab;  // 藍眼特效 Prefab
    public GameObject warningIconPrefab;  // 警告圖示 Prefab
    public GameObject explosionPrefab;     // 爆炸特效 Prefab
    public float warningTime = 0f;       // 警告圖示顯示時間 (秒)
    public float explosionRadius = 1f;   // 爆炸範圍 (半徑)
    public int explosionDamage = 10;     // 爆炸傷害值
    public LayerMask playerLayer;        // 玩家所在的圖層
    public float attackCooldown = 8f;     // 攻擊冷卻時間 (秒)
    public int explosionRowCount = 5;      // 爆炸線條數
    public GameObject yellowEyeAttackPrefab; //黃眼攻擊動畫 Prefab
    
    private Transform player;           // 玩家的 Transform 元件
    private float attackTimer = 0f;      // 攻擊計時器
    private Transform blueEye;          // 藍眼物件的 Transform 元件 (用於產生藍眼特效)
    private Transform bossSkin;    // bossSkin 物件的 Transform 元件
     private Transform numTransform;    // num 物件的 Transform 元件

    void Start()    //初始化
    {
        // 在 Start 方法中找到玩家的 Transform 元件
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        // 在 Start 方法中找到藍眼物件的 Transform 元件
        blueEye = transform.Find("eyes")?.Find("eyeball_yellow_0");
        // 在 Start 方法中找到 bossSkin 物件的 Transform 元件
         bossSkin = transform.Find("boss_skin");
        // 在 Start 方法中找到 num 物件的 Transform 元件
        numTransform = transform.Find("num");

          if (blueEye == null)
        {
            Debug.LogError("Error: Blue eye object 'eyeball_yellow_0' not found under 'eyes' GameObject");
        }
         if (player == null){
            Debug.LogError("Error: Player object not found or has incorrect Tag");
        }
        if(numTransform == null){
             Debug.LogError("Error: num  object not found under  GameObject");
        }
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        // 如果攻擊計時器達到冷卻時間，則執行攻擊
        if (attackTimer >= attackCooldown)
        {
             if (player != null && blueEye != null && numTransform != null)
            {
                StartCoroutine(LineExplosionAttack()); // 啟動黃眼攻擊協程
            }
            attackTimer = 0f; // 重置攻擊計時器
        }
           if (Input.GetKeyDown(KeyCode.Alpha3)) // 按下數字鍵 3 觸發攻擊
        {
             if (player != null && blueEye != null && numTransform != null)
            {
                StartCoroutine(LineExplosionAttack());
            }
        }
    }
      IEnumerator LineExplosionAttack()
    {
          // 在藍眼位置產生藍眼特效
        GameObject blueEyeEffect = Instantiate(blueEyeEffectPrefab, blueEye.position, Quaternion.identity);
        Destroy(blueEyeEffect, 1f); // 藍眼特效在 1 秒後自動銷毀

        // 播放黃眼攻擊動畫，並且放到 numTransform 下
        GameObject yellowEyeAttack = Instantiate(yellowEyeAttackPrefab, numTransform.position, Quaternion.identity, bossSkin);

        // 等待 0.7 秒
        yield return new WaitForSeconds(0.7f);
        //BOSS底下火焰動畫播放0.35秒 結束後銷毀
          Destroy(yellowEyeAttack, 0.35f);

        // 取得爆炸線條位置
         List<Vector3> explosionLinePositions = GetExplosionLinePositions();

         foreach(Vector3 y_pos in explosionLinePositions){
              StartCoroutine(SpawnLineExplosions(y_pos));
         }
        yield return null;
    }
     //產生多條爆炸線條位置
    List<Vector3> GetExplosionLinePositions()
    {
       List<Vector3> positions = new List<Vector3>();
        float yStart = -71f;
         float yEnd = -96f;

        float yStep = (yEnd - yStart) / (explosionRowCount -1);

          for (int i = 0; i < explosionRowCount; i++)
           {
               float yPosition = yStart + yStep * i;
               positions.Add(new Vector3(0,yPosition, 0));
          }
       return positions;
    }
    IEnumerator SpawnLineExplosions(Vector3 y_pos)
     {
         // 在每一條線上產生爆炸   爆炸與爆炸的間隔用x控制
        for (float x = 290f; x >= 256f; x -= 3f)
         {
               Vector3 position = new Vector3(x, y_pos.y ,0);
             // 在爆炸點位置產生警告圖示
            GameObject warningIcon = Instantiate(warningIconPrefab, position, Quaternion.identity);
           // 延遲一段時間
           float randomDelay = Random.Range(0.1f,0.1f);
              yield return new WaitForSeconds(randomDelay);
             StartCoroutine(SpawnExplosion(warningIcon));
         }
         yield return null;
    }
    IEnumerator SpawnExplosion(GameObject warningIcon){
        // 取得警告圖示的 Transform 元件, 並將爆炸特效的位置設為警告圖示的位置
        Vector3 explosionPosition = warningIcon.transform.position;
          // 延遲一段時間
        yield return new WaitForSeconds(warningTime);

        // 在警告圖示的位置產生爆炸特效
        GameObject explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);

        //偵測雷擊範圍內是否有玩家
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(explosionPosition + new Vector3(0,-1,0), explosionRadius, playerLayer);

          // 對範圍內玩家造成傷害
         foreach (Collider2D hitPlayer in hitPlayers)
         {
             // 取得玩家的 PlayerControl 腳本
            PlayerControl playerControl = hitPlayer.GetComponent<PlayerControl>();
            // 如果成功取得 PlayerControl 腳本, 則呼叫 TakeDamage 方法
             if (playerControl != null)
           {
                playerControl.TakeDamage(explosionDamage);
             }
          }
           // 爆炸特效在 2 秒後自動銷毀
           Destroy(explosion, 1f);
         // 銷毀警告圖示
         Destroy(warningIcon);
    }

    // 用於在場景視窗中顯示雷擊範圍 (僅在編輯器中顯示)
    private void OnDrawGizmosSelected()
    {
         if (player == null) return;
         Gizmos.color = Color.yellow;
         Gizmos.DrawWireSphere(player.position, explosionRadius);
    }
}