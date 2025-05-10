using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySelectObject : MonoBehaviour
{
    public DifficultyLevel thisDifficulty; // 在 Inspector 設定這個物件代表的難度

    private bool playerInRange = false;
    private Vector3 originalScale;
    public float pulseScale = 1.2f; // 放大倍率
    public float pulseSpeed = 2f;   // 呼吸動畫速度

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // 難度選中時做呼吸動畫
        if (RandomSpawn.GlobalDifficulty == thisDifficulty)
        {
            float scale = Mathf.Lerp(1f, pulseScale, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
            transform.localScale = originalScale * scale;
        }
        else
        {
            transform.localScale = originalScale;
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            RandomSpawn.GlobalDifficulty = thisDifficulty;
            AudioManager.Instance.PlaySFX("button_click");
            Debug.Log("難度已切換為：" + thisDifficulty);
            // 這裡可以加特效或UI提示
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            // 這裡可以顯示提示UI，例如「按E切換難度」
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            // 隱藏提示UI
        }
    }
}
