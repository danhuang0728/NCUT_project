using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Award_box_body : MonoBehaviour
{
    private PlayerControl playerControl;
    private GameObject burn_effect;  // 燃燒效果
    bool isFlip = false;
    private bool isBurning = false; // 新增：標記是否正在燃燒
    private Transform player1; // 修正：新增變數
    public GameObject monster;
    public float movespeed;
    public float HP;

    void Start()
    {
        burn_effect = GameObject.Find("fire_0");
        playerControl = GameObject.Find("player1").GetComponent<PlayerControl>();
        player1 = GameObject.Find("player1").transform; // 修正：初始化 player1
    }

    void Update()
    {
        if (HP <= 0)
        {
            MonsterDead(monster);
        }

    }

    public IEnumerator burn_monster(int burn_time)
    {
        isBurning = true;

        GameObject burnEffectClone = Instantiate(burn_effect, transform.position, transform.rotation);
        burnEffectClone.transform.SetParent(transform);
        burnEffectClone.transform.localPosition = new Vector3(0f, -0.01f, 0f);

        for (int i = 0; i < burn_time; i++)
        {
            HP -= 1 + playerControl.attack_damage * 0.2f; // 每秒減少的血量
            yield return new WaitForSeconds(1f);
        }

        Destroy(burnEffectClone);
        isBurning = false; // 燃燒結束後重置標記
    }

    public void burn_monster_start(int burn_time)
    {
        if (!isBurning)
        {
            StartCoroutine(burn_monster(burn_time));
        }
    }

    public void MonsterDead(GameObject monster)
    {
        monster.SetActive(false);
    }
}
