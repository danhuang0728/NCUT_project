using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MagicFlower_GPS : MonoBehaviour
{
    public GameObject light_obj;
    public Light2D light2d;
    public Transform bossArea_center;
    public Transform player;
    private float offset_angle = 30;

    void Start()
    {
        light_obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (bossArea_center != null && light_obj != null)
        {
            // 計算從本身位置到boss_center的方向向量
            Vector2 direction = bossArea_center.position - player.position;
            
            // 計算角度（弧度）並轉換為度數
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // 將light_obj旋轉到計算出的角度
            light_obj.transform.rotation = Quaternion.Euler(0, 0, angle + offset_angle);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Light_On());
        }
    }
    IEnumerator Light_On()
    {
        light_obj.SetActive(true);
        yield return new WaitForSeconds(2f);
        light_obj.SetActive(false);
    }
}
