using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlower : MonoBehaviour
{
    public float HP = 5f;
    public GameObject monster;
    public BossFloat bossFloat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            bossFloat.mainHp = bossFloat.mainHp - 1; 
            //Debug.Log("BOSS_HP:"+bossFloat.mainHp);
            MonsterDead(monster);
        }
    }
    public void MonsterDead(GameObject monster)
    {
        monster.SetActive(false);
    }
}
