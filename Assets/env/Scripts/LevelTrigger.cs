using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelTrigger : MonoBehaviour
{
    public Collider2D collider2d;
    public LayerMask player;
    public int levelminTime;
    public int levelmaxTime;

    private int levelTime = 0;
    private TrapControll[] trapControlls;  //一次抓取全部linktrap的TrapControll腳本

    void Start()
    {
        
        collider2d = GetComponent<Collider2D>(); 
        trapControlls = FindObjectsOfType<TrapControll>(); //一次抓取全部linktrap的物件
    }

    void Update()
    {
        if (collider2d.IsTouchingLayers(player))  //進入房間的觸發器
        {
            if (collider2d != null)        //讓觸發器只能觸發一次
            {
                collider2d.enabled = false;
            } 
            levelTime = UnityEngine.Random.Range(levelminTime, levelmaxTime);  // 關卡的隨機時間設定

            StartCoroutine(levelstart(levelTime));

            foreach (TrapControll trap in trapControlls) //修改全部trap物件裡的的bool為true
            {
                trap.close = true;  
            }   
            
        }
    }
    IEnumerator levelstart(int leveltime){  //leaveltime為房間的運作時間
        
        yield return new WaitForSeconds(leveltime); 
        foreach (TrapControll trap in trapControlls) //修改全部trap物件裡的的bool為true
            {
                Animator trapAni = trap.GetComponent<Animator>();
                //把陷阱動畫設回還沒啟動的狀態
                trapAni.Play("TarpAni", 0, 0f);   
                trapAni.Update(0);
                trapAni.Play("TarpAni_single", 0, 0f);
                trapAni.Update(0);
                //--------------------------
                trap.close = false;  
            }
      
    }
}
