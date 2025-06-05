using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PumpkinBoss_main : MonoBehaviour
{
    private GameObject player;
    private Animator ani;  
    private Rigidbody2D rig;
    private NormalMonster_setting normalMonster_setting;
    private float timer = 0f;
    private float previousXPosition; // 添加移動前位置的變量
    private bool isFlip = false; // 添加翻轉狀態的變量
    private enum AttackType
    {
        none,
        slash,
        spike, 
        dash,
        throw_seed
    }
    private AttackType attackType;
    private bool isSlash = false;
    private bool isSpell = false;
    private bool isSpike = false;
    public Light2D headlight;
    public GameObject slashEffect;
    public GameObject slashEffect2;
    public GameObject spikeEffect;
    void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        normalMonster_setting = GetComponent<NormalMonster_setting>();
        player = GameObject.Find("player1");
        StartCoroutine(randomSet_attackType());
    }

    // Update is called once per frame
    void Update()
    {
        if (normalMonster_setting.movespeed > 0)
        {
            ani.SetBool("move",true);
        }
        else
        {
            ani.SetBool("move",false);
        }
        previousXPosition = transform.position.x; //previousXPosition 為移動前位置
        
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, normalMonster_setting.movespeed * Time.deltaTime );

        float currentXPosition = transform.position.x;  //currentXPosition 為移動後的位置
        if (currentXPosition < previousXPosition) //x變小往左移動
        {
            if (isFlip == false) 
            {
                isFlip = true;
                var main = spikeEffect.GetComponent<ParticleSystem>().main;
                main.startRotation = new ParticleSystem.MinMaxCurve(0f);
            }
            else{}
        }
        if (currentXPosition > previousXPosition) //x變大往右移動
        {
            if (isFlip == true)
            {
                isFlip = false;
                var main = spikeEffect.GetComponent<ParticleSystem>().main;
                main.startRotation = new ParticleSystem.MinMaxCurve(0f);
            }
            else{}
        }
        //三秒後要是攻擊方式沒發生變化就抽一次
        timer += Time.deltaTime;
        if (timer >= 5f)
        {
            StartCoroutine(randomSet_attackType());
            Debug.Log(attackType);
            timer = 0f; // 重置计时器
        }
        //=================攻擊觸發=================
        if(attackType == AttackType.none)
        {
            normalMonster_setting.movespeed = 2;
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
            normalMonster_setting.movespeed = 4;
            if(Vector2.Distance(transform.position,player.transform.position) < 5f)
            {
                StartCoroutine(attack_spike());
            }
        }

    }
    //======================隨機抽當輪攻擊方式=============
    IEnumerator randomSet_attackType()
    {
        yield return new WaitForSeconds(3f);
        int randomint = Random.Range(0,3);
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
                attackType = AttackType.dash;
                break;
            case 4:
                attackType = AttackType.throw_seed;
                break;
        }
    }
    //======================刺擊攻擊======================
    IEnumerator attack_spike()
    {
        isSpike = true;
        normalMonster_setting.movespeed = 0;
        yield return new WaitForSeconds(1f);
        headlight.intensity = 0;
        ani.SetBool("attack_spike",true);
        yield return new WaitForSeconds(0.1f);
        ani.SetBool("attack_spike",false);
        yield return new WaitForSeconds(1);
        if(Vector2.Distance(transform.position,player.transform.position) < 4f)
        {
            if (player != null)
            {
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.AddForce(knockbackDirection * 30f, ForceMode2D.Impulse);
                }
            }
        }
        normalMonster_setting.movespeed = 4;
        headlight.intensity = 5;
        randomSet_attackType();
        isSpike = false;
    }
    //======================揮劍攻擊======================
    IEnumerator attack_Slash()
    {
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
        slashEffect2.SetActive(true);
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
        yield return new WaitForSeconds(3);
        slashEffect2.SetActive(false);
        normalMonster_setting.movespeed = 4;
        headlight.intensity = 5;
        randomSet_attackType();
        isSlash = false;
    }
    //======================法術攻擊======================
    IEnumerator attack_spell()
    {
        isSpell = true;
        normalMonster_setting.movespeed = 0;
        yield return new WaitForSeconds(1f);
        headlight.intensity = 0;
        ani.SetBool("attack_spell",true);
        yield return new WaitForSeconds(0.1f);
        ani.SetBool("attack_spell",false);
        yield return new WaitForSeconds(5);
        normalMonster_setting.movespeed = 4;
        headlight.intensity = 5;
        randomSet_attackType();
        isSpell = false;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red; //紅色線為slash攻擊範圍
        Gizmos.DrawWireSphere(transform.position,5f);
        Gizmos.color = Color.blue; //藍色線為spike攻擊範圍
        Gizmos.DrawWireSphere(transform.position,3f);
    }
}
