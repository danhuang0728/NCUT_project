using UnityEngine;
using System.Collections;

public class FruitItem : MonoBehaviour
{
    public FruitType fruitType;
    private bool isCollected = false;
    private float disappearTime = 10f;
    private PlayerControl player;
    private DebuffManager debuffManager;

    private void Start()
    {
        Debug.Log($"水果 {fruitType} 已生成");
        player = FindObjectOfType<PlayerControl>();
        if (player == null)
        {
            Debug.LogError("找不到玩家物件！");
        }

        debuffManager = DebuffManager.Instance;
        if (debuffManager == null)
        {
            Debug.LogError("找不到 DebuffManager！");
        }

        StartCoroutine(DisappearTimer());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            Debug.Log($"玩家碰觸到水果 {fruitType}");
            isCollected = true;
            
            if (debuffManager == null)
            {
                Debug.LogError("碰觸水果時 DebuffManager 為空！");
                Destroy(gameObject);
                return;
            }

            // 如果玩家已經有這個水果的Debuff，則解除
            if (debuffManager.HasDebuff(fruitType))
            {
                Debug.Log($"玩家已有 {fruitType} 的Debuff，正在解除");
                debuffManager.RemoveDebuff(fruitType);
                AudioManager.Instance.PlaySFX("collect_fruit");
            }
            else
            {
                Debug.Log($"玩家收集到水果 {fruitType}，但沒有對應的Debuff需要解除");
            }
            
            Destroy(gameObject);
        }
    }

    private IEnumerator DisappearTimer()
    {
        Debug.Log($"水果 {fruitType} 開始倒計時 {disappearTime} 秒");
        yield return new WaitForSeconds(disappearTime);
        
        if (!isCollected)
        {
            Debug.Log($"水果 {fruitType} 倒計時結束，準備給予Debuff");
            
            if (debuffManager != null)
            {
                debuffManager.ApplyDebuff(fruitType);
                Debug.Log($"已給予玩家 {fruitType} 的Debuff效果");
            }
            else
            {
                Debug.LogError("倒計時結束時 DebuffManager 為空！");
            }
        }
    }
} 