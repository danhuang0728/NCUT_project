using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class NormalMonster_setting : MonoBehaviour
{
    // Start is called before the first frame update
    private float previousXPosition; // 用來儲存物件的上一幀位置
    private PlayerControl playerControl;
    public int monster_type; //區分水果or一般怪物 1==水果 0==一般
    public int exp_type; //區分經驗值類別 初級,中級,高級
    public GameObject LowExpPrefab;    // 低級經驗值預製體
    public GameObject MediumExpPrefab;  // 中級經驗值預製體
    public GameObject HighExpPrefab;    // 高級經驗值預製體
    private GameObject burn_effect;  // 燃燒效果
    bool isFlip = false;
    void Start()
    {
        burn_effect = GameObject.Find("fire_0");
        playerControl = GameObject.Find("player1").GetComponent<PlayerControl>();
    }


    // Update is called once per frame
    public Transform player1;
    public GameObject monster;
    public float movespeed;

    public float HP;
    void Update()
    {
        if (HP <= 0)
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
    public IEnumerator burn_monster(int burn_time)
    {
        
        GameObject burnEffectClone = Instantiate(burn_effect, transform.position, transform.rotation);
        Material material = this.GetComponent<Renderer>().material;
        burnEffectClone.transform.SetParent(transform);
        burnEffectClone.transform.localPosition = new Vector3(0f, -0.01f, 0f);
        for(int i=0;i<burn_time;i++)

        {
            HP -= 1;
            playerControl.SetBoolWithDelay_void(material, this.GetComponent<Renderer>());
            yield return new WaitForSeconds(1f);
        }


        Destroy(burnEffectClone);
    }

    public void burn_monster_start(int burn_time) // 避免別的腳本引用協程發生問題
    {
        StartCoroutine(burn_monster(burn_time));
    }






    public void MonsterDead(GameObject monster)

    {
        monster.SetActive(false);

        // 判斷怪物類型和經驗值等級
        if (monster_type==0)  // 假設 monster_type = 0 代表一般怪物
        {
            switch (exp_type)
            {
                case 1:  // 低級經驗值
                    // 在這裡生成15經驗值的掉落物
                    GameObject expObject = Instantiate(LowExpPrefab, transform.position, Quaternion.identity);
                    expObject.SetActive(true);
                    AudioManager.Instance.PlaySFX("drop_exp");
                    break;

                case 2:  // 中級經驗值
                    // 在這裡生成40經驗值的掉落物
                    GameObject expObject2 = Instantiate(MediumExpPrefab, transform.position, Quaternion.identity);
                    expObject2.SetActive(true);
                    AudioManager.Instance.PlaySFX("drop_exp");
                    break;

                case 3:  // 高級經驗值

                    // 在這裡生成100經驗值的掉落物
                    GameObject expObject3 = Instantiate(HighExpPrefab, transform.position, Quaternion.identity);
                    expObject3.SetActive(true);
                    AudioManager.Instance.PlaySFX("drop_exp");
                    break;

            }
        }
        else if (monster_type==1)  // monster_type = 1 代表水果怪
        {
            switch (exp_type)
            {
                case 1:  // 低級經驗值
                    // 在這裡生成水果怪的15經驗值掉落物
                    // Instantiate(fruitLowExpPrefab, transform.position, Quaternion.identity);
                    AudioManager.Instance.PlaySFX("drop_exp");
                    break;
                case 2:  // 中級經驗值
                    // 在這裡生成水果怪的40經驗值掉落物
                    // Instantiate(fruitMediumExpPrefab, transform.position, Quaternion.identity);
                    AudioManager.Instance.PlaySFX("drop_exp");
                    break;
                case 3:  // 高級經驗值
                    // 在這裡生成水果怪的100經驗值掉落物
                    // Instantiate(fruitHighExpPrefab, transform.position, Quaternion.identity);
                    AudioManager.Instance.PlaySFX("drop_exp");
                    break;
            }
        }
    }

}
