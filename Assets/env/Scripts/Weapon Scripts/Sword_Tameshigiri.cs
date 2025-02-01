using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("斬擊次數")]
    public int spawnCount = 5; // 新增生成次数参数

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }
 

    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= cooldownTime && Is_in_range)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(SpawnShinyEffect());
                timer = 0f;
            }
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
                
                // 新增出血特效，放到最近怪物的子物件底下
                GameObject bloodInstance = Instantiate(blood_effect, monsterPosition , Quaternion.identity);
                bloodInstance.transform.position = nearestMonster.transform.position;
                Destroy(bloodInstance, 0.5f);  // 0.1秒后销毁出血特效

                yield return new WaitForSeconds(0.1f);
                Destroy(shinyInstance, 0.2f);
            }
        }
    }

    GameObject FindNearestMonster()
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
                playerController.SetBoolWithDelay_void(renderer_flower.material, renderer_flower);
            }
            if (monster != null)

            {

                // 造成傷害
                monster.HP -= damage;
                //閃白效果
                playerController.SetBoolWithDelay_void(material, renderer);
                // 擊退效果
                Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
                Rigidbody2D monsterRb = other.GetComponent<Rigidbody2D>();
                if (monsterRb != null)
                {
                    monsterRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
        
    }
}
