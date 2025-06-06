using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin_Knight_spawn : MonoBehaviour
{
    public static Pumpkin_Knight_spawn instance;
    public GameObject pumpkinKnightPrefab;
    public GameObject pumpkinPrefab;
    public GameObject effect1;
    public GameObject effect2;
    public Transform spawnPoint;
    void Start()
    {
        instance = this;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PumpkinKnight_spawn()
    {
        StartCoroutine(spawnEffect());
    }
    IEnumerator spawnEffect()
    {
        yield return new WaitForSeconds(4f);
        AudioManager.Instance.PlayMusic("Boss_music");
        //激活特效物件
        effect1.SetActive(true);
        effect2.SetActive(true);
        //等待特效播放完畢
        yield return new WaitForSeconds(0.2f);
        pumpkinPrefab.SetActive(false);
        //生成南瓜騎士
        Instantiate(pumpkinKnightPrefab, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.6f);
        effect1.SetActive(false);
        effect2.SetActive(false);
    }
}
