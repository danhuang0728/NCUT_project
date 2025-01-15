using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillEnabler : MonoBehaviour
{
    public float mainHp = 20f; // BOSS 的血量
    public Transform blueEye;     // 藍眼睛的 Transform 元件

    public Transform blueEyeclose; //藍色眼睛眼皮
     public GameObject blueEyeEffectPrefab;  // 藍眼特效 Prefab
    private BOSS_Blue_Lighting_Strike bOSS_Lighting_Strike;
     private BOSS_blue_explosion  bOSS_blue_explosion;
    
    public Animator blueEyeclose2;

  
     void Start()
     {
        // 在 Start 方法中找到藍眼物件的 Transform 元件
           blueEye = transform.Find("eyes")?.Find("eyeball_blue_0");
           blueEyeclose = transform.Find("eyes_close_blue");
           //blueEyeclose2 = GetComponent<Animator>();
           blueEyeclose2.enabled = false;
           
        if (blueEye == null)
        {
            Debug.LogError("Error: Blue eye object 'eyeball_blue_0' not found under 'eyes' GameObject");
        }
            //取得技能腳本
          bOSS_blue_explosion = gameObject.GetComponent<BOSS_blue_explosion>();
           bOSS_Lighting_Strike= gameObject.GetComponent<BOSS_Blue_Lighting_Strike>();
       if(bOSS_blue_explosion == null){
             Debug.LogError("Error: BOSS_blue_explosion not found");
           }
           if(bOSS_Lighting_Strike == null) {
             Debug.LogError("Error: BOSS_Lighting_Strike not found");
           }
      }
    public void TakeDamage(float damage)
    {
        mainHp -= damage;
        Debug.Log("BOSS受到傷害: " + damage + ", 目前血量: " + mainHp);
        CheckHp();
    }

    void CheckHp()
    {
        // 藍色眼睛
        if (mainHp <= 18 )
        {
            if (blueEye != null)
             {
                Debug.Log("進入18hp判斷");
                blueEyeclose2.enabled = true;
             }
            if (bOSS_blue_explosion!= null){
                bOSS_blue_explosion.enabled = true;
            }
             if (bOSS_Lighting_Strike != null)
             {
               bOSS_Lighting_Strike.enabled = true;
            }
           
            Debug.Log("Open Blue Eye, Enabled skill");

        }
    }
}