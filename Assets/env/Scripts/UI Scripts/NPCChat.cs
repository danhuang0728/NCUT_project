using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCChat : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMPro.TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;
    public float wordspeed;
    public GameObject ContinueButton;
    private InteractionPrompt interactionPrompt;



    void Update()
    {
        interactionPrompt = GetComponent<InteractionPrompt>();

         if (interactionPrompt != null && Input.GetKeyDown(KeyCode.E) && interactionPrompt.isPlayerInRange)
         {
             if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
         }
          if (interactionPrompt != null)
        {
            if (interactionPrompt.isPlayerInRange)
            {
                // 玩家在範圍內時執行的程式碼
                 Debug.Log("玩家在範圍內");
            }
            else
            {
                // 玩家不在範圍內時執行的程式碼
                 Debug.Log("玩家不在範圍內");
            }
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        if (index < dialogue.Length)
        {
            dialogueText.text = "";
            foreach (char letter in dialogue[index].ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(wordspeed);
            }
                ContinueButton.SetActive(true); // 確認文字顯示完畢後，再顯示 ContinueButton
        }
    }

    public void NextLine()
    {
        ContinueButton.SetActive(false);
        if (index < dialogue.Length - 1)
        {
            index++;
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }
}