using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;
using Unity.VisualScripting;
using Unity.Mathematics;

public class GameOver : MonoBehaviour
{
    private PlayerControl playerControl;
    private character_value_ingame Character_Value_Ingame;
    private float award;
    private GameObject obj;
    public Character_Values_SETUP character_Values_SETUP;
    public TextMeshProUGUI gold_t;
    public TextMeshProUGUI kill_M_t;
    public TextMeshProUGUI endAward;
    void Start()
    {
        playerControl = GameObject.Find("player1").GetComponent<PlayerControl>();
        Character_Value_Ingame = GameObject.Find("player1").GetComponent<character_value_ingame>();
        obj = gameObject;
        obj.SetActive(false); 

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu1");
    }
    public IEnumerator dead()
    {
        yield return new WaitForSeconds(2f);
        UIstate.isAnyPanelOpen = true;
        setTEXT();
        //結算計算
        award = math.round((Character_Value_Ingame.gold + PlayerControl.kill_monster_count)/1000); 
        character_Values_SETUP.GIFT_Value = character_Values_SETUP.GIFT_Value + (int)award;
        Time.timeScale = 0f; 
        obj.SetActive(true);    
    }
    void setTEXT()
    {
        gold_t.text = Character_Value_Ingame.gold.ToString();
        kill_M_t.text = PlayerControl.kill_monster_count.ToString();  
        endAward.text = award.ToString();
    }
}
