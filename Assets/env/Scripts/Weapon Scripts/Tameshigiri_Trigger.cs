using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tameshigiri_Trigger : MonoBehaviour
{
    public Transform player_transform;  // 玩家位置参考
    public Sword_Tameshigiri swordTameshigiri;  // 主武器脚本

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // 使触发范围跟随玩家
        transform.position = player_transform.position;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // 当有怪物在范围内时设置状态
        if (other.CompareTag("Monster"))
        {
            swordTameshigiri.Is_in_range = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 当怪物离开范围时重置状态
        if (other.CompareTag("Monster"))
        {
            swordTameshigiri.Is_in_range = false;
        }
    }
}
