using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{

   public void PLayGame()
   {
        SceneManager.LoadScene("SampleScene");
        AudioManager.Instance.PlaySFX("play_game");
   }


   public void QuitGame()
   {
    Application.Quit();
   }
}
