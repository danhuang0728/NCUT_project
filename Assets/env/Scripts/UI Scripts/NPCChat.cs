using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCChat : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMPro.TextMeshProUGUI dialogueText; // 修改：從陣列改為單一 TextMeshProUGUI
    [SerializeField] private string[] dialogue;
    private int index;
    public float wordspeed;
    public bool playerIsClose;
    public bool isDialogueActive;
    public GameObject ContinueButton;

    void Update()
    {
        if (!IsValidReference()) return;
        isDialogueActive = dialoguePanel.activeInHierarchy;

        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                ContinueButton.GetComponent<Button>().onClick.RemoveAllListeners();
                ContinueButton.GetComponent<Button>().onClick.AddListener(() => NextLine());
                StartCoroutine(Typing());
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Space) && isDialogueActive)
        {
            ContinueButton.GetComponent<Button>().onClick.Invoke();
        }
        if (dialogueText.text == dialogue[index])
        {
            ContinueButton.SetActive(true);
        }
    }

    public void zeroText()
    {
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
        index = 0;
        
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    IEnumerator Typing()
    {
        if (dialogueText == null || ContinueButton == null) yield break;
        
        if (index < dialogue.Length)
        {
            dialogueText.text = "";
            foreach (char letter in dialogue[index].ToCharArray())
            {
                if (dialogueText == null) yield break;
                dialogueText.text += letter;
                yield return new WaitForSeconds(wordspeed);
            }
        }
    }

    public void NextLine()
    {
        if (!IsValidReference()) return;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            
            if (dialoguePanel != null && dialoguePanel.activeSelf)
            {
                zeroText();
            }
        }
    }

    private void OnDestroy()
    {
        if (ContinueButton != null)
        {
            ContinueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    private bool IsValidReference()
    {
        return dialoguePanel != null && 
               dialogueText != null && 
               ContinueButton != null;
    }
}