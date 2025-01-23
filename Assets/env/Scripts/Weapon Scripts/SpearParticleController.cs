using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpearParticleController : MonoBehaviour
{
    public ParticleSystem particle_System;
    public ParticleSystem particle_System2;
    public Collider2D spearCollider1;
    public Collider2D spearCollider2;
    public PlayerControl playerControl;
    public float damage = 1f;
    public float knockbackForce = 1f;

    public void SpearParticle_play1()
    {
        StartCoroutine(Attack_Trigger1());
        Debug.Log("play_SpearParticle1");
        particle_System.Play();
    }
    public void SpearParticle_play2()
    {
        StartCoroutine(Attack_Trigger2());
        Debug.Log("play_SpearParticle2");
        particle_System2.Play();
    }
    public IEnumerator Attack_Trigger1()
    {
        // 等待一段時間
        yield return new WaitForSeconds(0.1f);

        // 檢查碰撞到的物件
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(spearCollider1.bounds.center, spearCollider1.bounds.size, 0f);
        foreach (var hitCollider in hitColliders)
        {
            Renderer renderer = hitCollider.GetComponent<Renderer>();
            if (hitCollider.CompareTag("Monster"))
            {
                Debug.Log("碰撞到怪物");

                // 獲取 Monster 腳本並調用 TakeDamage 方法
                NormalMonster_setting monster = hitCollider.GetComponent<NormalMonster_setting>();
                if (monster != null)
                {
                    Material mat = renderer.material;
                    // 造成傷害
                    StartCoroutine(playerControl.SetBoolWithDelay(mat,renderer)); 
                    monster.HP -= damage;
                    
                }
                else
                {
                    //Debug.Log("未找到 Monster 腳本");
                }
            }
            else
            {
                //Debug.Log("碰撞到非怪物物件");
            }
        }
    }
    public IEnumerator Attack_Trigger2()
    {

        // 等待一段時間
        yield return new WaitForSeconds(0.44f);

        // 檢查碰撞到的物件
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(spearCollider2.bounds.center, spearCollider2.bounds.size, 0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Monster"))
            {
                Renderer renderer = hitCollider.GetComponent<Renderer>();
                // 獲取 Monster 腳本並調用 TakeDamage 方法
                NormalMonster_setting monster = hitCollider.GetComponent<NormalMonster_setting>();
                if (monster != null)
                {
                    Material mat = renderer.material;
                    // 造成傷害
                    StartCoroutine(playerControl.SetBoolWithDelay(mat,renderer));
                    monster.HP -= damage;
                    // 造成傷害
                    monster.HP -= damage;
                }
                else
                {
                    //Debug.Log("未找到 Monster 腳本");
                }
            }
            else
            {
                //Debug.Log("碰撞到非怪物物件");
            }
        }

        
    }
}
