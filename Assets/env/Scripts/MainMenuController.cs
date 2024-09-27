using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NewBehaviourScript : MonoBehaviour
{
   public CanvasGroup OptionPanel;

   public void PLayGame()
   {
        SceneManager.LoadScene("SampleScene");
   }
   public void Option()
   {
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
   }
   public void Back()
   {
        OptionPanel.alpha = 0;
        OptionPanel.blocksRaycasts = false;
   }

   public void QuitGame()
   {
    Application.Quit();
   }
}
