using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PumpkinBoss_main : MonoBehaviour
{
    private GameObject player;
    private Animator ani;  
    private Rigidbody2D rig;
    private End end;
    public float HP = 70000f;
    private NormalMonster_setting normalMonster_setting;
    private float timer = 0f;
    private float previousXPosition; // 添加移動前位置的變量
    private bool isFlip = false; // 添加翻轉狀態的變量
    private enum AttackType
    {
        none,
        slash,
        spike, 
        spell,
        spawn
    }
    private AttackType attackType;
    private bool isSlash = false;
    private bool isSpell = false;
    private bool isSpike = false;
    private bool isSpawn = false;
    private bool spawn_intro = false;
    public Light2D headlight;
    public GameObject fireball;
    public GameObject slashEffect;
    public GameObject slashEffect2;
    public GameObject spikeEffect;
    public GameObject spawnEffect;
    public GameObject PunchLight;
    public Vector3 limitspawnArea_point1 = new Vector3(494.241699f,-121.400566f,0.120918632f);
    public Vector3 limitspawnArea_point2 = new Vector3(529.141724f,-140.120575f,0.120918632f);
    private PlayerControl playerControl;
    void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        normalMonster_setting = GetComponent<NormalMonster_setting>();
        end = FindObjectOfType<End>();
        player = GameObject.Find("player1");
        playerControl = player.GetComponent<PlayerControl>();
        StartCoroutine(randomSet_attackType());
        Debug.Log("攻擊類型"+attackType);
        PunchLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        HP = normalMonster_setting.HP; //引數給UI用的可以不用管
        
        if (normalMonster_setting.movespeed > 0)
        {
            ani.SetBool("move", true);
        }
        else
        {
            ani.SetBool("move", false);
        }
        previousXPosition = transform.position.x; //previousXPosition 為移動前位置
        
        //三秒後要是攻擊方式沒發生變化就抽一次
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            StartCoroutine(randomSet_attackType());
            Debug.Log(attackType);
            timer = 0f; // 重置计时器
        }
        // 如果正在進行任何攻擊，確保速度為0
        if(isSlash || isSpike || isSpawn || isSpell)
        {
            normalMonster_setting.movespeed = 0;
            return;
        }
        //=================攻擊觸發=================
        if(attackType == AttackType.none)
        {
            if(!isSlash && !isSpike && !isSpawn && !isSpell)
            {
                normalMonster_setting.movespeed = 2;
            }
        }
        
        if (attackType == AttackType.slash && !isSlash)
        {
            normalMonster_setting.movespeed = 4;
            if(Vector2.Distance(transform.position,player.transform.position) < 5f)
            {
                StartCoroutine(attack_Slash());
            }
        }
        if (attackType == AttackType.spike && !isSpike)
        {
            normalMonster_setting.movespeed = 10;
            if(Vector2.Distance(transform.position,player.transform.position) < 2.5f)
            {
                StartCoroutine(attack_spike());
            }
        }
        if (attackType == AttackType.spell && !isSpell)
        {
            normalMonster_setting.movespeed = 3;
            if(Vector2.Distance(transform.position,player.transform.position) < 10f)
            {
                StartCoroutine(attack_spell());
            }
        }
        if (attackType == AttackType.spawn && !isSpawn)
        {
            normalMonster_setting.movespeed = 3;
            if(Vector2.Distance(transform.position,player.transform.position) < 10f)
            {
                StartCoroutine(attack_spawn());
            }
        }

    }
    //======================隨機抽當輪攻擊方式=============
    IEnumerator randomSet_attackType()
    {
        yield return new WaitForSeconds(3f);
        int randomint = Random.Range(0,5);
        switch(randomint)
        {
            case 0:
                attackType = AttackType.none;
                break;
            case 1:
                attackType = AttackType.slash;
                break;
            case 2:
                attackType = AttackType.spike;
                break;
            case 3:
                attackType = AttackType.spell;
                break;
            case 4:
                attackType = AttackType.spawn;
                break;
        }
        Debug.Log(attackType);
    }
    //======================刺擊攻擊======================
    IEnumerator attack_spike()
    {
        if(isSlash || isSpike || isSpawn||isSpell)
        {
            yield break;
        }
        isSpike = true;
        normalMonster_setting.movespeed = 0;
        yield return new WaitForSeconds(1f);
        headlight.intensity = 0;
        ani.SetBool("attack_spike",true);
        yield return new WaitForSeconds(0.1f);
        ani.SetBool("attack_spike",false);
        yield return new WaitForSeconds(1);
        spikeEffect.GetComponent<Animator>().SetTrigger("play");
        if(Vector2.Distance(transform.position,player.transform.position) < 4f)
        {
            if (player != null)
            {
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.AddForce(knockbackDirection * 30f, ForceMode2D.Impulse);
                    playerControl.TakeDamage(40);
                }
            }
        }
        normalMonster_setting.movespeed = 3;
        headlight.intensity = 5;
        randomSet_attackType();
        isSpike = false;
        yield return new WaitForSeconds(0.5f);
    }
    //======================揮劍攻擊======================
    IEnumerator attack_Slash()
    {
        if(isSlash || isSpike || isSpawn||isSpell)
        {
            yield break;
        }
        isSlash = true;
        normalMonster_setting.movespeed = 0;
        yield return new WaitForSeconds(1f);
        headlight.intensity = 0;
        ani.SetBool("attack_slash",true);
        yield return new WaitForSeconds(0.1f);
        ani.SetBool("attack_slash",false);
        yield return new WaitForSeconds(0.5f);
        //蓄力時決定批砍方向
        Vector2 direction = (player.transform.position - transform.position).normalized;
        yield return new WaitForSeconds(1.5f);
        slashEffect2.GetComponent<Animator>().SetTrigger("play");
        if (slashEffect != null)
        {
            GameObject slash = Instantiate(slashEffect, transform.position, Quaternion.identity);
            slash.SetActive(true);
            slash.transform.right = direction;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            slash.transform.rotation = Quaternion.Euler(0, 0, angle - 95);
            Rigidbody2D bulletRb = slash.GetComponent<Rigidbody2D>();
            bulletRb.velocity = direction * 70;
            Destroy(slash, 1f);
        }
        yield return new WaitForSeconds(0.75f);
        yield return new WaitForSeconds(0.75f);

        yield return new WaitForSeconds(3);
        normalMonster_setting.movespeed = 3;
        headlight.intensity = 5;
        randomSet_attackType();
        isSlash = false;
        yield return new WaitForSeconds(0.5f);
    }
    //======================法術攻擊======================
    IEnumerator attack_spell()
    {
        if(isSlash || isSpike || isSpawn||isSpell)
        {
            yield break;
        }
        isSpell = true;
        normalMonster_setting.movespeed = 0;
        yield return new WaitForSeconds(1f);
        headlight.intensity = 0;
        yield return new WaitForSeconds(0.1f);
        for(int i = 0; i < 10; i++)
        {
            //發射火球
            ani.SetTrigger("attack_magic");
            PunchLight.SetActive(true);
            yield return new WaitForSeconds(0.4f);
            GameObject fireball_clone = Instantiate(fireball, transform.position, Quaternion.identity);
            fireball_clone.SetActive(true);
            Vector2 direction = (player.transform.position - transform.position).normalized;
            float randomAngle = Random.Range(-30f, 30f);
            direction = Quaternion.Euler(0, 0, randomAngle) * direction;
            fireball_clone.transform.right = direction;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            fireball.transform.rotation = Quaternion.Euler(0, 0, angle);
            Rigidbody2D bulletRb = fireball_clone.GetComponent<Rigidbody2D>();
            bulletRb.velocity = direction * 10;

        }
        yield return new WaitForSeconds(5);
        normalMonster_setting.movespeed = 3;
        headlight.intensity = 5;
        randomSet_attackType();
        isSpell = false;
    }
    //======================生成小怪======================
    IEnumerator attack_spawn()
    {
        if(isSlash || isSpike || isSpawn||isSpell)
        {
            yield break;
        }
        isSpawn = true;
        normalMonster_setting.movespeed = 0;
        yield return new WaitForSeconds(5);
        spawnEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        spawnEffect.SetActive(false);
        for(int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Vector2 randomPosition;
            randomPosition = Random.insideUnitCircle * 5f;
            GameObject pumpkin_monster1 = MonsterObjectPool.Instance.GetMonster(MonsterObjectPool.MonsterType.pumpkin_monster1);
            pumpkin_monster1.transform.position = transform.position + new Vector3(randomPosition.x, randomPosition.y, 0);
            // 檢查生成位置是否在限制範圍內
            Vector2 spawnPos = pumpkin_monster1.transform.TransformPoint(Vector3.zero);
            Vector2 point1 = limitspawnArea_point1;
            Vector2 point2 = limitspawnArea_point2;
            
            // 如果超出範圍則刪除
            if(spawnPos.x < point1.x || spawnPos.x > point2.x || 
                spawnPos.y > point1.y || spawnPos.y < point2.y) {
                MonsterObjectPool.Instance.ReturnMonster(pumpkin_monster1);
                Debug.Log("超出範圍");
            }
        }
        if(spawn_intro == false)
        {
            GuideSystem.Instance.Guide("(擊敗<color=orange>南瓜分裂體</color>可以<color=green>回復生命及少量維生素</color>)");
            spawn_intro = true;
        }
        isSpawn = false;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red; //紅色線為slash攻擊範圍
        Gizmos.DrawWireSphere(transform.position,5f);
        Gizmos.color = Color.blue; //藍色線為spike攻擊範圍
        Gizmos.DrawWireSphere(transform.position,3f);
        Gizmos.color = Color.yellow; //黃色線為spell攻擊範圍
        Gizmos.DrawWireSphere(transform.position,10f);
        
        // 使用全域座標繪製限制區域
        Vector3 point1 = limitspawnArea_point1;
        Vector3 point2 = limitspawnArea_point2;
        Gizmos.color = Color.green; //綠色線為spawn攻擊範圍
        Gizmos.DrawLine(point1, new Vector3(point2.x, point1.y, 0));
        Gizmos.DrawLine(point1, new Vector3(point1.x, point2.y, 0));
        Gizmos.DrawLine(point2, new Vector3(point1.x, point2.y, 0));
        Gizmos.DrawLine(point2, new Vector3(point2.x, point1.y, 0));
    }
    //======================死亡後遊戲結束======================
    private void OnDestroy() {
        end.startEndCutscene_void();
    }
}
