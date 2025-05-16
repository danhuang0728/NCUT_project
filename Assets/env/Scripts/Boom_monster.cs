using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom_monster : MonoBehaviour
{
    
    private NormalMonster_setting normalMonster;
    public float attackRange = 1.18f; // 攻擊範圍半徑
    public GameObject boom_effect_prefab;
    private PlayerControl player_control;
    private float distance;
    void Start()
    {
        normalMonster = GetComponent<NormalMonster_setting>();
        player_control = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= attackRange)
            {
                normalMonster.movespeed = 0;
                StartCoroutine(Boom_effect());
                //AudioManager.Instance.PlaySFX("");
            }
        }
        IEnumerator Boom_effect()
        {
            GetComponent<Collider2D>().enabled = false; // 關閉碰撞體避免後續碰撞檢測
            yield return new WaitForSeconds(1f);
            GameObject boom_effect = Instantiate(boom_effect_prefab, transform.position, Quaternion.identity);
            if (distance <= attackRange)
            {
                player_control.TakeDamage(30);
            }
            Destroy(gameObject);
            Destroy(boom_effect, 1f);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
