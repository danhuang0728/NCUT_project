using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class award_box : MonoBehaviour
{
   public NormalMonster_setting normalMonsterSetting; 
   public GameObject greenHeartPrefab; //綠心物品預製體
   public GameObject bitcoinPrefab; //金幣物品預製體
    void Start()
    {
        
    }


    void Update()
    {
        if(normalMonsterSetting.HP <= 0)
        {
            int randomItem = Random.Range(0, 2); //隨機生成物品，0為綠心，1為金幣
            if (randomItem == 0)
            {
                Instantiate(greenHeartPrefab, transform.position, Quaternion.identity); //生成綠心物品
            }
            else if (randomItem == 1)
            {
                Instantiate(bitcoinPrefab, transform.position, Quaternion.identity); //生成金幣物品
            }
        }
    }
}
