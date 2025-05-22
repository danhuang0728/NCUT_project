using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
   public void Start()
   {
        LoadData();
   }
   public void LoadData()
   {
        SaveManager.Instance.LoadCharacterValues();
        SaveManager.Instance.LoadDataFromPlayerPrefs_Tetr();
   }
   public void PLayGame()
   {
        // 保存Tetris數據
        SaveManager.Instance.SaveDataToPlayerPrefs_Tetr();
        // 保存角色數值
        SaveManager.Instance.SaveCharacterValues();
        SceneManager.LoadScene("SampleScene");
        AudioManager.Instance.PlaySFX("play_game");
   }


   public void QuitGame()
   {
    Application.Quit();
   }
}
