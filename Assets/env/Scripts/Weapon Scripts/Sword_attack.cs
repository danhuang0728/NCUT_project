using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword_attack : MonoBehaviour
{
    private ParticleSystem[] particleSystems;
    public Transform playerTransform;
    private float cooldown = 0.5f; // 冷却时间
    private float lastPlayTime = -0.5f; // 上次播放时间
    public PlayerControl playerControl;

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
        // 示例：按下空格键时播放效果
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastPlayTime + cooldown)
        {
            PlayEffects();
            StartCoroutine(CheckForMonsterCollision());
            lastPlayTime = Time.time; // 更新上次播放时间
        }

        // 绑定到玩家位置
        if (playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
    }

    private IEnumerator CheckForMonsterCollision()
    {
        for (int i = 0; i < 5; i++) // 執行五次
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(GetComponent<Collider2D>().bounds.center, 3f);
            foreach (var hitCollider in hitColliders) // 改名为 hitCollider 避免重复
            {
                if (hitCollider.CompareTag("Monster"))
                {
                    NormalMonster_setting monster = hitCollider.GetComponent<NormalMonster_setting>();
                    Renderer renderer = hitCollider.GetComponent<Renderer>(); // 获取 Renderer 组件
                    
                    if (monster != null && renderer != null)
                    {
                        monster.HP -= 1; // 將怪物的HP減少1
                        Material mat = renderer.material;
                        playerControl.SetBoolWithDelay_void(mat, renderer); 
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetComponent<Collider2D>().bounds.center, 3f);
    }
    

}
