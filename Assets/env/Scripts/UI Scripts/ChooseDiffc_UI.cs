using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChooseDiffc_UI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI diffcText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void openPanel()
    {
        panel.SetActive(true);
    }
    public void closePanel()
    {
        panel.SetActive(false);
    }
    public void easyBtnClick()
    {
        RandomSpawn.GlobalDifficulty = DifficultyLevel.Easy;
        AudioManager.Instance.PlaySFX("button_click");
        diffcText.text = "簡單";
    }
    public void normalBtnClick()
    {
        RandomSpawn.GlobalDifficulty = DifficultyLevel.Normal;
        AudioManager.Instance.PlaySFX("button_click");
        diffcText.text = "普通";
    }
    public void hardBtnClick()
    {
        RandomSpawn.GlobalDifficulty = DifficultyLevel.Hard;
        AudioManager.Instance.PlaySFX("button_click");
        diffcText.text = "困難";
    }
}


