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
    private bool isTyping = false;
    void Start()
    {   
        dialoguePanel.SetActive(false);
    }

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
        if (Input.GetKeyDown(KeyCode.Space) && isDialogueActive && !isTyping)
        {
            if (ContinueButton != null && ContinueButton.activeSelf)
            {
                Button btn = ContinueButton.GetComponent<Button>();
                if (btn != null && btn.onClick != null)
                {
                    btn.onClick.Invoke();
                }
                else
                {
                    Debug.LogWarning("按鈕元件缺失");
                }
            }
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
        
        isTyping = false;
    }

    public void NextLine()
    {
        if (!IsValidReference()) return;
        
        if (index >= dialogue.Length - 1)
        {
            zeroText();
            return;
        }

        ContinueButton.SetActive(false);
        index++;
        StartCoroutine(Typing());
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