using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tameshigiri_body : MonoBehaviour
{
    public Sword_Tameshigiri sword_Tameshigiri;
    private PlayerControl playerControl;
    public GameObject blood_effect; //出血特效
    void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Renderer renderer = other.GetComponent<Renderer>();
            Material material = renderer.material;
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            BossFlower bossFlower = other.GetComponent<BossFlower>();
            if (bossFlower != null)
            {
                Renderer renderer_flower = bossFlower.GetComponent<Renderer>();
                bossFlower.HP -= 1;
                playerControl.SetBoolWithDelay_void(renderer_flower.material, renderer_flower);
            }
            if (monster != null)
            {
                float randomAngle = Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngle);
                GameObject nearestMonster = sword_Tameshigiri.FindNearestMonster();
                if (nearestMonster != null) // 添加空值检查
                {
                    Vector3 monsterPosition = nearestMonster.transform.position;
                    //新增出血特效，放到最近怪物位置
                    GameObject bloodInstance = Instantiate(blood_effect, monsterPosition, randomRotation);
                    ParticleSystem bloodParticleSystem = bloodInstance.GetComponent<ParticleSystem>();
                    if (bloodParticleSystem != null)
                    {
                        var main = bloodParticleSystem.main;
                        main.startRotation = randomRotation.eulerAngles.z * Mathf.Deg2Rad; // 设置粒子系统的起始旋转
                    }
                    bloodInstance.transform.position = nearestMonster.transform.position;
                    Destroy(bloodInstance, 0.5f);  // 0.5秒后销毁出血特效
                    // 造成傷害
                    monster.HP -= sword_Tameshigiri.damage;
                    //閃白效果
                    playerControl.SetBoolWithDelay_void(material, renderer);
                    // 擊退效果
                    Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                    Rigidbody2D monsterRb = other.GetComponent<Rigidbody2D>();
                    if (monsterRb != null)
                    {
                        monsterRb.AddForce(knockbackDir * 1f, ForceMode2D.Impulse);
                    }
                }
                else
                {
                    Debug.LogWarning("未找到最近的怪物！"); // 添加调试信息
                }
            }
        }
    }
}
