using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject bulletPrefab; // 子彈 Prefab
    public float fireRate = 1f; // 發射頻率 (每秒發射多少顆子彈)
    public float fireAngleRange = 90f; // 扇形發射角度範圍
    public float bulletSpeed = 5f; // 子彈速度
    private float fireTimer = 0f; // 發射計時器

    void Start()
    {
    }
    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= 1f / fireRate)
        {
            FireBullet();
            fireTimer = 0f;
        }
    }
    void FireBullet()
    {
        // 計算子彈起始位置 (在BOSS附近)
        Vector3 bulletPosition = transform.position;

        // 計算隨機發射角度
        float randomAngle = Random.Range(-fireAngleRange / 2f, fireAngleRange / 2f);
        float fireAngle =  randomAngle; //將發射角度改為只有 randomAngle
        // 將角度轉為向量
        Vector2 direction = new Vector2(Mathf.Cos((fireAngle + 180) * Mathf.Deg2Rad), Mathf.Sin((fireAngle + 180) * Mathf.Deg2Rad));

         // 產生子彈
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // 設定子彈速度
        bulletRb.velocity = direction.normalized * bulletSpeed;

        // 設定子彈的旋轉角度
        bullet.transform.rotation = Quaternion.Euler(0, 0, fireAngle);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 碰到玩家或牆壁就銷毀子彈
        if (other.CompareTag("Player") || other.CompareTag("wall"))
        {
            Destroy(gameObject);
        }
    }
}