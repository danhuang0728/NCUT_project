using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class MagicFlower_SpawnMonster : MonoBehaviour
{
    public GameObject monster;
    public List<GameObject> monster_prb = new List<GameObject>();
     public Light2D[] light2d;

    void Start()
    {
        // 使用List的Add方法添加子物件
        foreach (Transform child in monster.transform)
        {
            monster_prb.Add(child.gameObject);
        }
        light2d = GetComponentsInChildren<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SpawnMonster());
            GetComponent<Collider2D>().enabled = false;
        }
    }
    IEnumerator SpawnMonster()
    {
        foreach (var item in light2d)
        {
            item.intensity = item.intensity * 2f;
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 5; i++)
        {
            int randomValue = Random.Range(0, monster_prb.Count);
            GameObject monster_obj = Instantiate(monster_prb[randomValue], transform.position, Quaternion.identity);
            monster_obj.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
        foreach (var item in light2d)
        {
            item.intensity = 0.1f;
        }
    }
}
