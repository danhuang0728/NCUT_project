using System.Security.Cryptography;
using UnityEngine;

public class BossFloat : MonoBehaviour
{
    public float floatHeight = 0.5f;
    public float floatSpeed = 2f;
    public int mainHp = 20;
    public GameObject monster;
    private Vector3 startPos;
    
    // 添加 Animator 引用
    private Animator eyesAnimator_blue;
    private Animator eyesAnimator_green;
    private Animator eyesAnimator_red;
    private Animator eyesAnimator_purple;
    private Animator eyesAnimator_yellow;
    public Boss_Main_Controll boss_Main_Controll;
    private bool blueTriggered = false;
    private bool greenTriggered = false;
    private bool redTriggered = false;
    private bool purpleTriggered = false;
    private bool yellowTriggered = false;

    void Start()
    {
        startPos = transform.position;
        // 獲取 eyes_close_blue 物件上的 Animator 組件
        // 假設 eyes_close_blue 是子物件
        eyesAnimator_blue = transform.Find("eyes/eyeball_blue_0/eyes_close_blue").GetComponent<Animator>();
        eyesAnimator_green = transform.Find("eyes/eyeball_green_0/eyes_close_green").GetComponent<Animator>();
        eyesAnimator_red = transform.Find("eyes/eyeball_red_0/eyes_close_red").GetComponent<Animator>();
        eyesAnimator_purple = transform.Find("eyes/eyeball_purple_0/eyes_close_purple").GetComponent<Animator>();
        eyesAnimator_yellow = transform.Find("eyes/eyeball_yellow_0/eyes_close_yellow").GetComponent<Animator>();
        
        // 如果 eyes_close_blue 不在這個路徑，你需要調整路徑
        // 或者，你可以通過 Inspector 拖曳指定：
        // public Animator eyesAnimator; // 改為 public 然後在 Inspector 中指定
    }

    void Update()
    {
        // 浮動移動代碼
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // 檢查血量並控制動畫器
        if (mainHp <= 18 && !blueTriggered && eyesAnimator_blue != null)
        {
            //Debug.Log("藍眼開啟");
            blueTriggered = true; // 標記已觸發
            eyesAnimator_blue.enabled = true;
            boss_Main_Controll.stage = 1;
            boss_Main_Controll.AddSkills();
            boss_Main_Controll.lottery();
        }

        if (mainHp <= 16 && !greenTriggered && eyesAnimator_green != null)
        {
            greenTriggered = true; // 標記已觸發
            eyesAnimator_green.enabled = true;
            boss_Main_Controll.stage = 2;
            boss_Main_Controll.AddSkills();
            boss_Main_Controll.lottery();
        }

        if (mainHp <= 14 && !redTriggered && eyesAnimator_red != null)
        {
            redTriggered = true; // 標記已觸發
            eyesAnimator_red.enabled = true;
            boss_Main_Controll.stage = 3;
            boss_Main_Controll.AddSkills();
            boss_Main_Controll.lottery();
        }

        if (mainHp <= 12 && !purpleTriggered && eyesAnimator_purple != null)
        {
            purpleTriggered = true; // 標記已觸發
            eyesAnimator_purple.enabled = true;
            boss_Main_Controll.stage = 4;
            boss_Main_Controll.AddSkills();
            boss_Main_Controll.lottery();
        }

        if (mainHp <= 10 && !yellowTriggered && eyesAnimator_yellow != null)
        {
            yellowTriggered = true; // 標記已觸發
            eyesAnimator_yellow.enabled = true;
            boss_Main_Controll.stage = 5;
            boss_Main_Controll.AddSkills();
            boss_Main_Controll.lottery();
        
        }


        if (mainHp <= 0)
        {
            //Debug.Log("BOSS 死亡");
            MonsterDead(monster);
        }
    }

    public void MonsterDead(GameObject monster)
    {
        monster.SetActive(false);
    }
}