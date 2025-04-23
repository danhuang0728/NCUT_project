using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameOver : MonoBehaviour
{
    private PlayerControl playerControl;
    private GameObject obj;
    void Start()
    {
        playerControl = GameObject.Find("player1").GetComponent<PlayerControl>();
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
        Time.timeScale = 0f; 
        obj.SetActive(true);    
    }
}
