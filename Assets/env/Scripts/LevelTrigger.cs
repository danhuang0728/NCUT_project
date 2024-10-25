using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelTrigger : MonoBehaviour
{
    public Collider2D collider2d;
    public LayerMask player;
    private int levelTime = 0;

    void Start()
    {
        collider2d = GetComponent<Collider2D>(); 
    }

    void Update()
    {
        if (collider2d.IsTouchingLayers(player))  //進入房間的觸發器
        {
            if (collider2d != null)        //讓觸發器只能觸發一次
            {
                collider2d.enabled = false;
            } 

            StartCoroutine(levelstart(levelTime));  
        }
    }
    IEnumerator levelstart(int leveltime){  //leaveltime為房間的運作時間
        
        yield return new WaitForSeconds(leveltime); 
      
    }
}
