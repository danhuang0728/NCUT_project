using System.Collections;
using UnityEngine;

public class ToggleChildObjects : MonoBehaviour
{
    public float damage = 1f;
    public int level = 1;
    public float enableDuration = 1f; // 啟用持續時間    #1.28剛好一輪戳完
    public float disableDuration = 3f; // 關閉持續時間
    private character_value_ingame characterValuesIngame;
    public Character_Values_SETUP characterValues;
    public Transform player;
    [Header("西洋劍武器進化參數設定")]
    public bool is_evolution1 = false;
    public GameObject down_obj;
    public GameObject crackeffect;
    public int total_spawn_count = 20;
    public float spawnRadius = 20f; // 可以調整這個值來控制生成範圍

    private void Start()
    {
        characterValuesIngame = GameObject.Find("player1").GetComponent<character_value_ingame>();
        player = GameObject.Find("player1").transform;
        StartCoroutine(ToggleChildren());
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x - 0.3f, player.position.y - 0.2f, player.position.z);
        level_determination(level);
    }
    
    //等級判定
    public void level_determination(int level)
    {
        if(level == 1)
        {
            damage = 10f;
            disableDuration = 3f;
        }
        else if(level == 2)
        {
            damage = 20f;
            disableDuration = 2.5f;
        }
        else if(level == 3)
        {
            damage = 40f;
            disableDuration = 2f;
        }
        else if(level == 4)
        {
            damage = 80f;
            disableDuration = 1.5f;
        }
        else if(level == 5)
        {
            damage = 100f;
            disableDuration = 1f;
        }
    }

    private IEnumerator ToggleChildren()
    {
        // 獲取所有子物件
        Transform[] childObjects = GetComponentsInChildren<Transform>();
        
        // 移除父物件本身（因為它也會包含在陣列中）
        for (int i = 0; i < childObjects.Length; i++)
        {
            if (childObjects[i] == transform)
            {
                childObjects[i] = null;
                break;
            }
        }

        while (true)
        {
            // 啟用所有子物件
            foreach (var child in childObjects)
            {
                if (child != null)
                    child.gameObject.SetActive(true);
            }

            // 等待啟用持續時間
            yield return new WaitForSeconds(enableDuration);

            // 關閉所有子物件
            foreach (var child in childObjects)
            {
                if (child != null)
                    child.gameObject.SetActive(false);
            }

            if(is_evolution1 == true)
            {
                yield return new WaitForSeconds(1);
                // 在玩家周圍一定範圍內隨機生成down_obj
                for(int i = 0; i < total_spawn_count; i++)
                {
                    yield return new WaitForSeconds(0.05f - (0.05f * (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage)));
                    StartCoroutine(SpawnRandomObjects());
                }
            }
            // 等待關閉持續時間
            yield return new WaitForSeconds(disableDuration- (disableDuration * (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage)));
        }
    }

    private IEnumerator SpawnRandomObjects()
    {
        // 計算玩家周圍的範圍
        
        // 在指定半徑範圍內隨機生成down_obj
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);
        GameObject down_obj = Instantiate(this.down_obj, spawnPosition+new Vector3(0,2.5f,0), Quaternion.identity);
        down_obj.transform.Rotate(0, 0, 90);
        down_obj.SetActive(true);

        // 等待一段時間後銷毀down_obj
        yield return new WaitForSeconds(0.28f);
        GameObject crackeffect = Instantiate(this.crackeffect, spawnPosition, Quaternion.identity);
        crackeffect.SetActive(true);
        Destroy(down_obj);
        // 為 crackeffect 添加透明淡出效果
        if (crackeffect != null)
        {
            SpriteRenderer spriteRenderer = crackeffect.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                StartCoroutine(FadeOut(spriteRenderer, 0.8f));
            }
        }
        
        // 淡出協程
        IEnumerator FadeOut(SpriteRenderer renderer, float duration)
        {
            Color originalColor = renderer.color;
            float elapsedTime = 0f;
            
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / duration);
                renderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }
        Destroy(crackeffect, 1f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position+new Vector3(0,2.5f,0), spawnRadius);
    }
}
