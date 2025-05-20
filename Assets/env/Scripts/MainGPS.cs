using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGPS : MonoBehaviour
{
    private GameObject player;
    public GameObject[] targetPoint;
    public float offset_angle = -45; // 偏移角度
    void Start()
    {
        player = FindObjectOfType<PlayerControl>().gameObject;
        transform.position = player.transform.position;
        gameObject.SetActive(false); // 初始隱藏GPS物件
    }

    // Update is called once per frame
    void Update()
    {
        //將GPS物件移動到玩家身上

        GameObject Target = targetPoint[LevelTrigger.levelFinish - 1]; //第一關就會+1 讓陣列從0開始所以-1
        if (Target != null)
        {
            Vector2 direction = Target.transform.position - player.transform.position;
            
            // 計算角度（弧度）並轉換為度數
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // 旋轉到計算出的角度
            gameObject.transform.rotation = Quaternion.Euler(0, 0, angle + offset_angle);
            
            Vector2 playerPos = player.transform.position;
            Vector2 targetPos = Target.transform.position; // 你需要指定目標點的 Transform
            Vector2 direction2 = (targetPos - playerPos).normalized;

            transform.position = playerPos + direction2 * 1.5f;
                        
        }

        //暫時限制在第三關
        if(LevelTrigger.levelFinish > 2)
        {
            closeGPS();
        }
    }

    public void openGPS()
    {
        gameObject.SetActive(true);
    }
    public void closeGPS()
    {
        gameObject.SetActive(false);
    }
}
