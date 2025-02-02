using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Blue_Lighting_Strike : MonoBehaviour
{
    public GameObject blueEyeEffectPrefab;  // 藍眼特效 Prefab
    public GameObject warningIconPrefab;  // 警告圖示 Prefab
    public GameObject lightningStrikePrefab;     // 雷擊特效 Prefab
    public float warningTime = 2f;       // 警告圖示顯示時間 (秒)
    public float lightningRadius = 1f;   // 雷擊範圍 (半徑)
    public int lightningDamage = 10;     // 雷擊傷害值
    public LayerMask playerLayer;        // 玩家所在的圖層
    public float attackCooldown = 3f;    // 攻擊冷卻時間 (秒)

    private Transform player;           // 玩家的 Transform 元件
    private float attackTimer = 0f;      // 攻擊計時器
    private Transform blueEye;          // 藍眼物件的 Transform 元件 (用於產生藍眼特效)

    void Start()
    {
        // 在 Start 方法中找到玩家的 Transform 元件
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        // 在 Start 方法中找到藍眼物件的 Transform 元件
        blueEye = transform.Find("eyes")?.Find("eyeball_blue_0");

        // 注释错误提示
        if (blueEye == null)
        {
            //Debug.LogError("Error: Blue eye object 'eyeball_blue_0' not found under 'eyes' GameObject");
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
               StartCoroutine(BlueEyeAttack()); // 啟動藍眼攻擊協程
            }
             attackTimer = 0f; // 重置攻擊計時器
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 按下數字鍵 1 觸發攻擊
        {
             if (player != null && blueEye != null)
            {
                StartCoroutine(BlueEyeAttack()); // 啟動藍眼攻擊協程
            }
        }
    }

    IEnumerator BlueEyeAttack()
    {
        // 在藍眼位置產生藍眼特效
        GameObject blueEyeEffect = Instantiate(blueEyeEffectPrefab, blueEye.position, Quaternion.identity, blueEye);
        Destroy(blueEyeEffect, 1f); // 藍眼特效在 1 秒後自動銷毀

        // 取得玩家位置
        Vector3 playerPosition = player.position;

        // 在玩家位置產生警告圖示
        GameObject warningIcon = Instantiate(warningIconPrefab, playerPosition, Quaternion.identity);

        // 取得警告圖示的 Transform 元件, 並將雷擊的位置設為警告圖示的位置
        Vector3 lightningPosition = warningIcon.transform.position;

        // 延遲一段時間
        yield return new WaitForSeconds(warningTime);

        // 在警告圖示的位置產生雷擊特效
        GameObject lightningStrike = Instantiate(lightningStrikePrefab, lightningPosition + new Vector3(0, 4f, 0), Quaternion.identity);

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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(player.position, lightningRadius);
    }
}