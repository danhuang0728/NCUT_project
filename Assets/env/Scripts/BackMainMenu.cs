using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackMainMenu : MonoBehaviour
{

   public void MainMenu()
   {
        SceneManager.LoadScene("MainMenu");
   }

}