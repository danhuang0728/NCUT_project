using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;


public class PlayerControl : MonoBehaviour
{
    public float speed = 5f;
    public Transform AttackPoint;
    public LayerMask MonsterLayer;
    public float AttackRange;
    public float Knockback_strength;
    public int HP;
    public AudioSource audioSource;
    public AudioClip audioClip;

    //--------------------打擊特效開關的bool-----------------------------
    public string boolPropertyName = "_hitBool";
    //--------------------------------------------------------

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
        StartCoroutine(hurtDelay());  //啟動傷害判定的延遲迴圈
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
    IEnumerator hurtDelay(){  //設定每0.2秒就會執行一次受傷判定
        if (rig.IsTouchingLayers(MonsterLayer))
        {
            HP = HP - 5; 
        }
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(hurtDelay());
    }

    public void Move(InputAction.CallbackContext context)
    {
        InputX = context.ReadValue<Vector2>().x;
        InputY = context.ReadValue<Vector2>().y;
    }

    
    private IEnumerator SetBoolWithDelay(Material mat ,Renderer targetRenderer)   //開啟hit特效然後0.1秒後關閉
    {
        mat = targetRenderer.material;
        // 设置布尔值为 true
        mat.SetInt(boolPropertyName, 1);
        audioSource.PlayOneShot(audioClip, 0.7f); // 打擊音效
        
        // 等待 0.1 秒
        yield return new WaitForSeconds(0.2f);  //hit閃白時間 

        // 设置布尔值为 false
        mat.SetInt(boolPropertyName, 0);
    }

    public void ALLdemageCheck(){     
        demageCheck();
        demageCheck2();
    }
    public void demageCheck(){          //史萊姆傷害判定
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, MonsterLayer); 

        // 如果有怪物進入範圍   (確定史萊姆類型的)
        foreach (Collider2D slimemonster in hitMonsters)  //這裡monster指進到攻擊範圍內的gameObject 
        {
            Renderer targetRenderer = slimemonster.GetComponent<Renderer>(); //抓取複製怪的renderer
            monsterMove cloneSlime_Scripts = slimemonster.GetComponent<monsterMove>(); //讀取在攻擊範圍內的怪物腳本
            Transform slime_T = slimemonster.GetComponent<Transform>();
            if (cloneSlime_Scripts != null)
            {           
                cloneSlime_Scripts.HP -= 1;                      //改變攻擊範圍內怪物的HP變數
                if (targetRenderer != null)
                {
                    // 获取材质实例（确保不会修改共享材质）
                    Material mat = targetRenderer.material;
                    // 打擊特效       
                    StartCoroutine(SetBoolWithDelay(mat,targetRenderer));  
                }
                Debug.Log("怪物HP: " + cloneSlime_Scripts.HP);
            }
            if (slime_T != null){                      //對史萊姆的擊退設定
                Vector3 direction = slime_T.position - transform.position; // slime 到玩家的方向
                direction.Normalize();
                slime_T.position = Vector3.MoveTowards(slime_T.position, slime_T.position + direction, 10 * Knockback_strength * Time.deltaTime);   
            }
            else{break;}

        }

    }
    public void demageCheck2(){        //一般平移怪物傷害判定
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, MonsterLayer); 
        // 如果有怪物進入範圍   (確定一般怪物類型的)
        
        foreach (Collider2D Normalmonster in hitMonsters)  //這裡monster指進到攻擊範圍內的gameObject 
        {
            Renderer targetRenderer = Normalmonster.GetComponent<Renderer>(); //抓取複製怪的renderer
            NormalMonster_setting clone_Scripts = Normalmonster.GetComponent<NormalMonster_setting>(); //讀取在攻擊範圍內的怪物腳本
            Transform Normal_T = Normalmonster.GetComponent<Transform>();
            if (clone_Scripts != null){
                clone_Scripts.HP -= 1;                      //改變攻擊範圍內怪物的HP變數
                if (targetRenderer != null)
                {
                    // 获取材质实例（确保不会修改共享材质）
                    Material mat = targetRenderer.material;
                    // 打擊特效       
                    StartCoroutine(SetBoolWithDelay(mat,targetRenderer));  
                }
                Debug.Log("怪物HP: " + clone_Scripts.HP);
            }
            if (Normal_T != null){                      //對一般的擊退設定
                Vector3 direction = Normal_T.position - transform.position; // 一般怪物 到玩家的方向
                direction.Normalize();
                Normal_T.position = Vector3.MoveTowards(Normal_T.position, Normal_T.position + direction, 100 * Knockback_strength * Time.deltaTime);   
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
