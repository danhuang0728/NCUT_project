using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public GameObject boss;
    public GameObject boss_spawnpoint;
    public GameObject boss_healthbar;
    public TrapControll[] trapControlls;
    public PlayerIntro_Trigger playerIntro_Trigger;
    public FruitData fruitData;
    void Start()
    {
        trapControlls = FindObjectsOfType<TrapControll>();
        playerIntro_Trigger = FindObjectOfType<PlayerIntro_Trigger>();

        boss.SetActive(false);
        boss_spawnpoint.SetActive(false);
        boss_healthbar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlayNextBattleMusic("Boss_music");
            //介紹boss函數(){}
            playerIntro_Trigger.StartIntroduction_boss(fruitData);
            boss.SetActive(true);
            boss_spawnpoint.SetActive(true);
            boss_healthbar.SetActive(true);
            foreach (TrapControll trap in trapControlls) // 修改全部trap物件裡的的bool為true
            {
                trap.close = true; //尖刺伸出來
            }
        }
    }
}
