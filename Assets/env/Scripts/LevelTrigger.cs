using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelTrigger : MonoBehaviour
{
    public Collider2D collider2d;
    public LayerMask player;
    public LayerMask monsterlayer;
    public int levelminTime = 180;
    public int levelmaxTime = 300;

    private int levelTime = 0;
    private TrapControll[] trapControlls;  //一次抓取全部linktrap的TrapControll腳本
    private spawn[] spawns; 

    void Start()
    {
        
        collider2d = GetComponent<Collider2D>(); 
        trapControlls = FindObjectsOfType<TrapControll>(); //一次抓取全部linktrap的物件
        spawns = FindObjectsOfType<spawn>();
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
        ClearAllClones(); 

        foreach (spawn repOb in spawns) //修改全部重生點為關閉狀態
            {
                repOb.enabled = false;
            }
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
    public void ClearAllClones()   //清除複製物件
    {
        // 獲取所有場景中的物件
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // 遍歷所有物件，查找名稱包含 "Clone" 的物件
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Clone")) // 替換為適當的名稱檢查
            {
                Destroy(obj);
            }
        }
    }
}
