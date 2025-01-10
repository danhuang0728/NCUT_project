using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_blue_explosion : MonoBehaviour
{
    public GameObject blueEyeEffectPrefab;  // 藍眼特效 Prefab
    public GameObject warningIconPrefab;  // 警告圖示 Prefab
    public GameObject explosionPrefab;     // 爆炸特效 Prefab
    public float warningTime = 2f;       // 警告圖示顯示時間 (秒)
    public float explosionRadius = 1f;   // 爆炸範圍 (半徑)
    public int explosionDamage = 10;     // 爆炸傷害值
    public LayerMask playerLayer;        // 玩家所在的圖層
    public float attackCooldown = 6f;    // 攻擊冷卻時間 (秒)
    public int explosionCount = 10;        // 基礎爆炸數量
    
    private Transform player;           // 玩家的 Transform 元件
    private float attackTimer = 0f;      // 攻擊計時器
    private Transform blueEye;          // 藍眼物件的 Transform 元件 (用於產生藍眼特效)


    void Start()
    {
          // 在 Start 方法中找到玩家的 Transform 元件
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        // 在 Start 方法中找到藍眼物件的 Transform 元件
        blueEye = transform.Find("eyes")?.Find("eyeball_blue_0");

        if (blueEye == null)
        {
            Debug.LogError("Error: Blue eye object 'eyeball_blue_0' not found under 'eyes' GameObject");
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
             if (player != null && blueEye != null)
            {
               StartCoroutine(AreaExplosionAttack()); // 啟動藍眼攻擊協程
            }
             attackTimer = 0f; // 重置攻擊計時器
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) // 按下數字鍵 2 觸發攻擊
        {
            if (player != null && blueEye != null)
            {
                StartCoroutine(AreaExplosionAttack()); // 啟動藍眼攻擊協程
            }
        }
    }

     IEnumerator AreaExplosionAttack()
    {
         // 在藍眼位置產生藍眼特效
        GameObject blueEyeEffect = Instantiate(blueEyeEffectPrefab, blueEye.position, Quaternion.identity, blueEye);
        Destroy(blueEyeEffect, 1f); // 藍眼特效在 1 秒後自動銷毀

        // 取得實際爆炸數量
        int actualExplosionCount = explosionCount + Random.Range(1, 6);

         // 產生爆炸點的列表
        List<Vector3> explosionPositions = GetRandomExplosionPositions(actualExplosionCount);

         // 在每個爆炸點產生警告圖示
        foreach (Vector3 explosionPosition in explosionPositions)
        {
               // 在爆炸點位置產生警告圖示
               GameObject warningIcon = Instantiate(warningIconPrefab, explosionPosition, Quaternion.identity);
                // 延遲一段時間後產生爆炸和傷害判定
                 float randomDelay = Random.Range(0.1f, 0.25f);
              yield return new WaitForSeconds(randomDelay);
              StartCoroutine(SpawnExplosion(warningIcon));
        }
         yield return null;
    }
      // 產生隨機的爆炸點的函式
    List<Vector3> GetRandomExplosionPositions(int count)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < count; i++)
        {
            float randomX = Random.Range(271f, 290f);
            float randomY = Random.Range(-96f, -71f);
           positions.Add(new Vector3(randomX,randomY, 0));
        }
        return positions;
    }

   IEnumerator SpawnExplosion(GameObject warningIcon){
         // 取得警告圖示的 Transform 元件, 並將爆炸特效的位置設為警告圖示的位置
         Vector3 explosionPosition = warningIcon.transform.position;
        //  延遲一段時間
         yield return new WaitForSeconds(warningTime);

         // 在警告圖示的位置產生爆炸特效
         GameObject explosion = Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);

         //偵測雷擊範圍內是否有玩家
         Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius, playerLayer);

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
           // 爆炸特效在 1 秒後自動銷毀
           Destroy(explosion, 1f);
         // 銷毀警告圖示
         Destroy(warningIcon);
    }

    // 用於在場景視窗中顯示雷擊範圍 (僅在編輯器中顯示)
    private void OnDrawGizmosSelected()
    {
         if (player == null) return;
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(player.position, explosionRadius);
    }
}