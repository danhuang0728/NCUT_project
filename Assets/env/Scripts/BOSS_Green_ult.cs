using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class BOSS_Green_ult : MonoBehaviour
{
    public GameObject GreenEyeEffectPrefab;  // 綠眼特效 Prefab
    public GameObject warningIconPrefab;  // 警告圖示 Prefab
    public GameObject VinePrefab;  //藤蔓 Prefab 
    public Transform GreenEye; //抓取綠眼睛位置(特效用)
    public float warningTime = 2f; 
    public LayerMask playerlayer; //抓取玩家圖層
     public float attackCooldown = 10f; // 攻擊間隔冷卻(秒)
    private float attackTimer = 0f;    
    public Transform Centerpoint; // 中心點
    public GameObject playerObject;
    public float attackRadius = 1f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        // 如果攻擊計時器達到冷卻時間，則執行攻擊
        if (attackTimer >= attackCooldown)
        {
            if (GreenEye != null){
                StartCoroutine(GreenEyeAttack()); // 啟動綠眼攻擊協程
            }
             attackTimer = 0f; // 重置攻擊計時器
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 按下數字鍵 1 觸發攻擊
        {
            if (GreenEye != null){
                StartCoroutine(GreenEyeAttack()); // 啟動綠眼攻擊協程
            }
        }
    }

    IEnumerator GreenEyeAttack()
    {
        // 在綠眼位置產生綠眼特效
        GameObject GreenEyesEffect = Instantiate(GreenEyeEffectPrefab, GreenEye.position + new Vector3(0, 1.5f, 0), Quaternion.identity, GreenEye);
        Destroy(GreenEyesEffect, 1f); // 綠眼特效在 1 秒後自動銷毀
        Vector3 centerPosition = Centerpoint.position;

        // 產生警告圖示
        GameObject warningIcon = Instantiate(warningIconPrefab, centerPosition, Quaternion.identity);


        // 延遲一段時間
        yield return new WaitForSeconds(warningTime);
        // 銷毀警告圖示
        Destroy(warningIcon);

        // 在警告圖示的位置產生藤蔓特效
        GameObject vine = Instantiate(VinePrefab, centerPosition , Quaternion.identity);
        yield return new WaitForSeconds(2f);
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(centerPosition, attackRadius, playerlayer);
        foreach (Collider2D hitPlayer in hitPlayers)
        {
            // 取得玩家的 GameObject
            GameObject playerGO = hitPlayer.gameObject;

            // 取得 Rigidbody2D 组件
            Rigidbody2D playerRB = playerGO.GetComponent<Rigidbody2D>();

            // 确保 Rigidbody2D 存在后调用方法
            if (playerRB != null)
            {
                StartCoroutine(ImmobilizePlayer(playerRB));
            }
            else
            {
                Debug.LogWarning("Rigidbody2D not found on object: " + playerGO.name);
            }
        }
        
        //藤蔓特效銷毀
        Destroy(vine, 0f);
        

    
    }
    private void OnDrawGizmosSelected()
     {
         Gizmos.color = Color.green;
         Gizmos.DrawWireSphere(Centerpoint.position, attackRadius);
    }

     IEnumerator ImmobilizePlayer(Rigidbody2D playerRigidbody){
          // 玩家定身
        Debug.Log("Player immobilized");
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        // 延遲一段時間
        yield return new WaitForSeconds(3f);

        // 移除定身效果
        playerRigidbody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.None;
        Debug.Log("Player can move");
     }

}
