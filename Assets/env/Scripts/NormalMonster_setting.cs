using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMonster_setting : MonoBehaviour
{
    // Start is called before the first frame update
    private float previousXPosition; // 用來儲存物件的上一幀位置
    bool isFlip = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    public Transform player1;
    public GameObject monster;
    public float movespeed;

    public float HP;
    void Update()
    {
        if (HP == 0)
        {
            MonsterDead(monster);
        }
        previousXPosition = transform.position.x; //previousXPosition 為移動前位置
        
        Vector2 direction = player1.position - transform.position;
        direction.Normalize();
        transform.position = Vector2.MoveTowards(transform.position, player1.position, movespeed * Time.deltaTime );

    

        float currentXPosition = transform.position.x;  //currentXPosition 為移動後的位置
        if (currentXPosition < previousXPosition) //x變大往左移動
        {
            if (isFlip == false) 
            {
                isFlip = true;
                transform.Rotate(0.0f,180.0f,0.0f);
            }
            else{}
        }
        if (currentXPosition > previousXPosition) //x變大往右移動
        {
            if (isFlip == true)
            {
                isFlip = false;
                transform.Rotate(0.0f,180.0f,0.0f);
            }
            else{}
        }
        

    }

    public void MonsterDead(GameObject monster)
    {
        monster.SetActive(false);
    }

}
