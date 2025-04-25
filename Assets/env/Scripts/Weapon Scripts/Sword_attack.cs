using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword_attack : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    public Transform playerTransform;
    private float cooldown = 1f; // 冷却时间
    private float lastPlayTime = -0.5f; // 上次播放时间
    public PlayerControl playerControl;
    public BossFlower bossFlower;
    private bool isAttacking = false;
    private Coroutine attackCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        // 获取所有子对象中的粒子系统
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        playerControl = FindObjectOfType<PlayerControl>();

    }

    public void PlayEffects()
    {
        // 依次播放每个粒子系统
        foreach (var ps in particleSystems)
        {
            ps.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {

        // 绑定到玩家位置
        if (playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        // 当按键被按下时
        if (context.started)
        {
            isAttacking = true;
            // 立即执行一次攻击
            PerformAttack();
            // 开始协程处理持续按住的情况
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(ContinuousAttackRoutine());
            }
        }
        // 当按键被释放时
        else if (context.canceled)
        {
            isAttacking = false;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private void PerformAttack()
    {
        // 检查攻击冷却
        if (Time.time - lastPlayTime < cooldown)
            return;
            
        lastPlayTime = Time.time;
        PlayEffects();
        StartCoroutine(CheckForMonsterCollision());
    }

    private IEnumerator ContinuousAttackRoutine()
    {
        // 等待第一次攻击完成的冷却时间
        yield return new WaitForSeconds(cooldown);
        
        // 只要按键仍被按住，就持续执行攻击
        while (isAttacking)
        {
            PerformAttack();
            yield return new WaitForSeconds(cooldown);
        }
    }

    private IEnumerator CheckForMonsterCollision()
    {
        for (int i = 0; i < 5; i++) // 執行五次
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(GetComponent<Collider2D>().bounds.center, 3f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Monster"))
                {
                    NormalMonster_setting monster = hitCollider.GetComponent<NormalMonster_setting>();
                    BossFlower bossFlower = hitCollider.GetComponent<BossFlower>();
                    Renderer renderer = hitCollider.GetComponent<Renderer>();
                    if (bossFlower != null)
                    {
                        Renderer renderer_flower = bossFlower.GetComponent<Renderer>();
                        bossFlower.HP -= 1;
                        playerControl.SetBoolWithDelay_void(renderer_flower.material, renderer_flower);
                    }

                    if (monster != null && renderer != null)
                    {
                        monster.HP -= 1;
                        Material mat = renderer.material;
                        playerControl.SetBoolWithDelay_void(mat, renderer); 
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    void OnDrawGizmos()  // 繪製輔助線
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(GetComponent<Collider2D>().bounds.center, 3f);
    }
    

}
