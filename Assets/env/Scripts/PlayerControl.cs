using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerControl : MonoBehaviour
{
    public float speed = 5f;
    public Transform AttackPoint;
    public LayerMask MonsterLayer;
    public float AttackRange;
    private monsterMove slime_Scripts;
    private NormalMonster_setting normalMonster_setting; 
    private float InputX;
    private float InputY;
    private bool isFlip = false;

    private Rigidbody2D rig;
    private Animator ani;

    private void Start() 
    {
        rig = GetComponent<Rigidbody2D>();    
        ani = GetComponent<Animator>();
    }
    private void Update() 
    {
        rig.velocity = new Vector2(speed * InputX , speed * InputY);    
       
        if (math.abs(rig.velocity.x) > 0 || rig.velocity.y != 0)
        {
            ani.SetBool("move",true);
        }
        else
        {
            ani.SetBool("move",false);
        }

        if (!isFlip)
        {
            if (rig.velocity.x > 0)
            {
                isFlip = true;
                transform.Rotate(0.0f,180.0f,0.0f);
            }
        }
        else
        {
            if (rig.velocity.x < 0)
            {
                isFlip = false;
                transform.Rotate(0.0f,180.0f,0.0f);
            }
        }
      

    }
    public void Move(InputAction.CallbackContext context)
    {
        InputX = context.ReadValue<Vector2>().x;
        InputY = context.ReadValue<Vector2>().y;
    }

    public void ALLdemageCheck(){
        demageCheck();
        demageCheck2();
    }
    public void demageCheck(){
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, MonsterLayer); 

        // 如果有怪物進入範圍   (確定史萊姆類型的)
        foreach (Collider2D slimemonster in hitMonsters)  //這裡monster指進到攻擊範圍內的gameObject 
        {
            monsterMove cloneSlime_Scripts = slimemonster.GetComponent<monsterMove>(); //讀取在攻擊範圍內的怪物腳本
            if (cloneSlime_Scripts != null)
                {           
                    cloneSlime_Scripts.HP -= 1;                      //改變攻擊範圍內怪物的HP變數
                    Debug.Log("怪物HP: " + cloneSlime_Scripts.HP);
                }
            else{break;}

        }

    }
    public void demageCheck2(){
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, MonsterLayer); 
        // 如果有怪物進入範圍   (確定一班怪物類型的)
        
        foreach (Collider2D Normalmonster in hitMonsters)  //這裡monster指進到攻擊範圍內的gameObject 
        {
            
            NormalMonster_setting clone_Scripts = Normalmonster.GetComponent<NormalMonster_setting>(); //讀取在攻擊範圍內的怪物腳本
                if (clone_Scripts != null){
            clone_Scripts.HP -= 1;                      //改變攻擊範圍內怪物的HP變數
            Debug.Log("怪物HP: " + clone_Scripts.HP);
                }

        }
        
    }   

    public void Attack(InputAction.CallbackContext context)
    {
        ani.SetBool("attack",true);
       
    }
    public void Attack_end()
    {
        ani.SetBool("attack",false);
        
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(AttackPoint.position,AttackRange);    
    }
}
