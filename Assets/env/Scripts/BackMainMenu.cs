using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackMainMenu : MonoBehaviour
{
   public GameObject confirmPanel;
   [SerializeField]private UIManager escopenMenu;
   [SerializeField]private GameOver gameOver;
   private PlayerControl playerControl;
   public void Awake()
   {
      playerControl = FindObjectOfType<PlayerControl>();
   }
   public void MainMenuRequest()
   {
      confirmPanel.SetActive(true);
   }
   public void MainMenuCancel()
   {
      confirmPanel.SetActive(false);
   }
   public void MainMenuConfirm()
   {
      if(SaveManager.Instance != null)
      {
         SaveManager.Instance.SaveCharacterValues();
         SaveManager.Instance.SaveDataToPlayerPrefs_Tetr();
      }
      else
      {
         Debug.LogError("未正確儲存檔案");
      }
      escopenMenu.CloseAllPanels();
      playerControl.HP = -1;
      gameOver.StartCoroutine(gameOver.dead());
   }
}