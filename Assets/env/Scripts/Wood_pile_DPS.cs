using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using TMPro;
public class Wood_pile_DPS : MonoBehaviour
{
    private NormalMonster_setting normalMonster_setting;
    private float monster_HP;
    private float previousHP;
    public TextMeshPro dps_text;
    void Start()
    {
        normalMonster_setting = GetComponent<NormalMonster_setting>();
        monster_HP = normalMonster_setting.HP;
        StartCoroutine(CalculateDPS());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator CalculateDPS()
    {
        previousHP = monster_HP;
        float currentHP;
        float damageDealt;
        float dps;

        while (true)
        {
            // 等待1秒
            yield return new WaitForSeconds(1f);
            
            // 獲取當前血量
            currentHP = normalMonster_setting.HP;
            
            // 計算這一秒造成的傷害
            damageDealt = previousHP - currentHP;
            
            // 如果有傷害，計算並顯示DPS
            if (damageDealt > 0)
            {
                dps = damageDealt;
                dps = Mathf.RoundToInt(dps);
                dps_text.text = "DPS: " + dps + "/s";
            }
            else{
                dps = 0;
                dps_text.text = "DPS: " + dps + "/s";
            }
            
            // 更新前一秒的血量記錄
            previousHP = currentHP;
        }
    }
}
