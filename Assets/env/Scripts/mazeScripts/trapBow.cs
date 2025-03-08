using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapBow : MonoBehaviour
{
    public GameObject bulletPrefab; // 子彈 Prefab
    public float fireRate = 1f; // 發射頻率 (每秒發射多少顆子彈)
    public float bulletSpeed = 5f; // 子彈速度
    private float timer = 0f;
    public enum BowDirection {
        Right = 0,      // 0度
        Up = 90,        // 90度
        Left = 180,     // 180度 
        Down = 270      // 270度
    }
    public BowDirection bowDirection = BowDirection.Right; // 預設朝右
    void Start()

    {

    }

    void Update()
    {
        timer += Time.deltaTime;

        // 每当timer达到1秒时发射
        if (timer >= 1f / fireRate)
        {
            FireBullet();
            //AudioManager.Instance.PlaySFX("");
            timer = 0f; // 重置计时器
        }

    }
    void FireBullet()
    {
        // 計算子彈起始位置
        Vector3 bulletPosition = transform.position;

        // 使用本體物件的角度
        float fireAngle = transform.rotation.eulerAngles.z;
        Vector2 direction = Quaternion.Euler(0, 0, (float)bowDirection) * Vector2.right;

        // 產生子彈
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
        bullet.SetActive(true);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        bulletRb.velocity = direction * bulletSpeed;
        bullet.transform.rotation = Quaternion.Euler(0, 0, fireAngle);
    }
}
