using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class BossFloat : MonoBehaviour
{
    public float floatHeight = 0.5f; // 上下浮動的高度
    public float floatSpeed = 2f; // 浮動的速度
    public int mainHp = 5;   //BOSS本體血量
    public GameObject monster;

    private Vector3 startPos;

    void Start()
    {
        // 記錄初始位置
        startPos = transform.position;
    }

    void Update()
    {
        // 使用正弦波實現上下浮動效果
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        if (mainHp <= 0) //判定死亡
        {
            MonsterDead(monster);
        }
    }
    public void MonsterDead(GameObject monster)
    {
        monster.SetActive(false);
    }
}
