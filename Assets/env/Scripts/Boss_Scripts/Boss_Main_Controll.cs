using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss_Main_Controll : MonoBehaviour
{
    List<string> OpenSkills = new List<string>(){"self","blueEyes","GreenEyes" ,"RedEyes", "PurpleEyes","YellowEyes"};
    public List<string> skills = new List<string>(){};
    List<string> skills_pool = new List<string>();
    public int stage = 0;
    private float timer = 0f;
    int select_amount = 1;
    public GameObject Purple_slowdown;
    //----------------------------藍色--------------------------------
    private BOSS_blue_explosion bOSS_Blue_Explosion; //藍眼睛爆炸技能 
    private BOSS_Blue_Lighting_Strike bOSS_Blue_Lighting_Strike; // 藍色閃電 
    //----------------------------------------------------------------

    //----------------------------本體--------------------------------
    private Boss_shoot_bullet boss_Shoot_Bullet; 
    //----------------------------------------------------------------

    //----------------------------綠色--------------------------------
    private BOSS_Green_ult bOSS_Green_Ult;
    //----------------------------------------------------------------

    //----------------------------紅色--------------------------------
    private BOSS_red_line_explosion bOSS_Red_Line_Explosion;  //橫向連續爆炸
    //----------------------------------------------------------------

    //----------------------------紫色--------------------------------
    private BOSS_Purple_Blackhole bOSS_Purple_Blackhole;
    //----------------------------------------------------------------

    //----------------------------黃色--------------------------------
    private BOSS_Yellow_Circle bOSS_Yellow_Circle;  //環形落雷
    private BOSS_Yellow_Square bOSS_Yellow_Square; //方形落雷
    private BOSS_yellow_Xattack bOSS_Yellow_Xattack; //叉叉落雷
    //----------------------------------------------------------------
    void Start()
    {   
        bOSS_Blue_Explosion = GetComponent<BOSS_blue_explosion>();
        bOSS_Blue_Lighting_Strike = GetComponent<BOSS_Blue_Lighting_Strike>();
        boss_Shoot_Bullet = GetComponent<Boss_shoot_bullet>();
        bOSS_Green_Ult = GetComponent<BOSS_Green_ult>();
        bOSS_Red_Line_Explosion = GetComponent<BOSS_red_line_explosion>();
        bOSS_Purple_Blackhole = GetComponent<BOSS_Purple_Blackhole>();
        bOSS_Yellow_Circle = GetComponent<BOSS_Yellow_Circle>();
        bOSS_Yellow_Xattack = GetComponent<BOSS_yellow_Xattack>();
        bOSS_Yellow_Square = GetComponent<BOSS_Yellow_Square>();
        skills.Add(OpenSkills[stage]);
        lottery();
    }

    void Update()
    {
         timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1)) // 按下數字鍵 1 觸發攻擊
        {
            lottery();
        }
        
        if (timer > 10 )
        {
            Debug.Log("倒數重抽");
            lottery();
            timer = 0;
        } 
        
    }
    public void AddSkills()
    {
        skills.Add(OpenSkills[stage]);
    }
    public void lottery()  //抽技能
    {
        //全部技能關閉-----
        boss_Shoot_Bullet.enabled = false;
        bOSS_Blue_Lighting_Strike.enabled = false;
        bOSS_Blue_Explosion.enabled = false;
        bOSS_Green_Ult.enabled = false;
        bOSS_Red_Line_Explosion.enabled = false;
        bOSS_Purple_Blackhole.enabled = false;
        bOSS_Yellow_Circle.enabled = false;
        Purple_slowdown.SetActive(false); 
        //----------------

        Shuffle(skills);
        skills_pool.Clear();
        int skills_amount = 1;
        Debug.Log("總技能量:"+skills.Count);
        if (skills.Count > 1)
        {
            skills_amount = Random.Range(2, skills.Count - 1);   //抽技能施放數量 -1為扣掉self
        }
        Debug.Log("抽到的施放技能量:" + skills_amount);
        for (int i = 0; i < skills_amount; i++)
        {
            Debug.Log(skills[i]);
            skills_pool.Add(skills[i]);   

        }
        foreach (string skill in skills_pool) //歷遍技能池
        {
            if (skill == "self") 
            {
                boss_Shoot_Bullet.enabled = true;
            }
            if (skill == "blueEyes") 
            {
                int blue_skills_selcet = Random.Range(1,3); // 閃電、爆炸機率各一半 
                if (blue_skills_selcet == 1)
                {
                    bOSS_Blue_Lighting_Strike.enabled = true;
                }
                else
                {
                    bOSS_Blue_Explosion.enabled = true;
                }
            }
            if (skill == "GreenEyes") 
            {
                bOSS_Green_Ult.enabled = true;
            }
            if (skill == "RedEyes" )
            {
                bOSS_Red_Line_Explosion.enabled = true;
            }
            if (skill == "PurpleEyes" )
            {
                int purple_skills_selcet = Random.Range(1,3);
                if (purple_skills_selcet == 1)
                {
                    Purple_slowdown.SetActive(true);
                }
                else
                {
                bOSS_Purple_Blackhole.enabled = true;
                }
            }
            if (skill == "YellowEyes")
            {
                int Yellow_skills_selcet = Random.Range(1,3);
                if (Yellow_skills_selcet == 1)
                {
                    bOSS_Yellow_Circle.enabled = true;
                }
                else
                {
                    bOSS_Yellow_Square.enabled = true;
                    bOSS_Yellow_Xattack.enabled = true;
                }
            }
        }
        
        

    }
    public void Shuffle<T>(List<T> list)  //洗牌函数
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
