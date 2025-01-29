using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GearWeaponSystem : MonoBehaviour
{
    public GameObject gearPrefab;       // 齒輪預製體
    public int gearCount = 3;           // 齒輪數量
    public float rotateSpeed = 100f;    // 旋轉速度
    public float radius = 2f;           // 旋轉半徑
    private List<GameObject> gears = new List<GameObject>();  // 儲存所有齒輪
    private Transform player;
    private float angle = 0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SpawnGears();
    }

    void SpawnGears()
    {
        // 清除現有齒輪
        foreach (var gear in gears)
        {
            if (gear != null)
                Destroy(gear);
        }
        gears.Clear();

        // 生成新齒輪
        float angleStep = 360f / gearCount;  // 平均分配角度
        for (int i = 0; i < gearCount; i++)
        {
            float startAngle = i * angleStep;
            GameObject newGear = Instantiate(gearPrefab, Vector3.zero, Quaternion.identity);
            GearWeapon gearScript = newGear.AddComponent<GearWeapon>();
            gearScript.startAngle = startAngle;
            gearScript.rotateSpeed = rotateSpeed;
            gearScript.radius = radius;
            gears.Add(newGear);
        }
    }

    // 可以用這個方法動態改變齒輪數量
    public void UpdateGearCount(int newCount)
    {
        gearCount = newCount;
        SpawnGears();
    }
}

// 修改後的齒輪個體腳本
public class GearWeapon : MonoBehaviour
{
    public float startAngle = 0f;       // 起始角度
    public float rotateSpeed = 100f;    // 旋轉速度
    public float radius = 2f;           // 旋轉半徑
    public float damage = 1f;          // 傷害值
    public float knockbackForce = 5f;   // 擊退力道
    
    private PlayerControl playerControl; //抓玩家腳本
    

    private Transform player;
    private float currentAngle;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerControl = player.GetComponent<PlayerControl>();
        currentAngle = startAngle;
    }

    void Update()
    {
        // 更新角度
        currentAngle += rotateSpeed * Time.deltaTime;
        
        // 計算齒輪的新位置
        float x = Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius;
        
        // 更新齒輪位置
        transform.position = player.position + new Vector3(x, y, 0);
        
        // 齒輪自轉
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            BossFlower bossFlower = other.GetComponent<BossFlower>();
            
            if (bossFlower != null)
            {
                Renderer renderer_flower = bossFlower.GetComponent<Renderer>();
                bossFlower.HP -= damage;
                playerControl.SetBoolWithDelay_void(renderer_flower.material, renderer_flower);
            }

            if (monster != null)
            {
                // 造成傷害
                monster.HP -= damage;
                // 取得 Renderer 組件
                Renderer renderer = other.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material mat = renderer.material;
                    playerControl.SetBoolWithDelay_void(mat, renderer);

                    // 若需要延遲恢復顏色，可用協程
                    //StartCoroutine(ResetColorAfterDelay(renderer, Color.white, 0.3f));
                }
                // 擊退效果
                Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                Rigidbody2D monsterRb = other.GetComponent<Rigidbody2D>();
                if (monsterRb != null)
                {
                    monsterRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}