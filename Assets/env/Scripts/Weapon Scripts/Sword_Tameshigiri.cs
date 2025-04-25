using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword_Tameshigiri : MonoBehaviour
{
    public float damage = 1f;
    public float knockbackForce = 1f;
    public GameObject Shiny_effect; //斬擊特效
    public GameObject blood_effect; //出血特效
    public Transform player_transform;
    private PlayerControl playerController;
    public float cooldownTime = 1f;
    private float timer = 0f;
    public bool Is_in_range = false;
    private bool isHolding = false;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private Coroutine attackCoroutine;

    [Header("斬擊次數")]
    public int spawnCount = 5; // 新增生成次数参数

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!Is_in_range) return;
        
        // 当按键被按下时
        if (context.started && Is_in_range)
        {
            isHolding = true;
            // 开始协程处理持续按住的情况
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(ContinuousAttackRoutine());
            }
        }
        // 当按键被释放时
        else if (context.canceled)
        {
            isHolding = false;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            //重製攻擊速度
            cooldownTime =  1;
        }
    }

    private void PerformAttack()
    {
        lastAttackTime = Time.time;
        timer = 0f;
        damage = playerController.attack_damage * 0.3f;
        StartCoroutine(SpawnShinyEffect());
    }

    private IEnumerator ContinuousAttackRoutine()
    {
        // 等待第一次攻击完成的冷却时间
        yield return new WaitForSeconds(cooldownTime);
        // 只要按键仍被按住，就持续执行攻击
        while (isHolding && Is_in_range)
        {      
            //越砍越快
            if(cooldownTime > 0.5f)
            {
                cooldownTime = cooldownTime - 0.05f;
            }
            PerformAttack();
            yield return new WaitForSeconds(cooldownTime);
        }
    }

    IEnumerator SpawnShinyEffect()
    {
        GameObject nearestMonster = FindNearestMonster();
        if (nearestMonster != null)
        {
            Vector3 monsterPosition = nearestMonster.transform.position;
            
            for(int i = 0; i < spawnCount; i++)
            {
                // 生成閃光特效
                float randomAngle = Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngle);
                GameObject shinyInstance = Instantiate(Shiny_effect, monsterPosition, randomRotation);
                shinyInstance.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                Destroy(shinyInstance, 0.2f);
            }
        }
    }

    public GameObject FindNearestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestMonster = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float distanceToMonster = Vector3.Distance(player_transform.position, monster.transform.position);
            if (distanceToMonster < shortestDistance)
            {
                shortestDistance = distanceToMonster;
                nearestMonster = monster;
            }
        }

        return nearestMonster;
    }
}
