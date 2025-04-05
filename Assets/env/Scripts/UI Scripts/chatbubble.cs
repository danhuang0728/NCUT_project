using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class chatbubble : MonoBehaviour
{
    public GameObject bubble;
    public bool playerIsClose;
    public int index;
    public TextMeshPro bubbletext; // 使用3D文字組件
    public string[] dialogue;
    public float wordspeed;
    public float waitTime;
    public bool isTyping;
    // Start is called before the first frame update
    void Start()
    {
        zeroText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void zeroText()
    {
        if (bubbletext != null)
        {
            bubbletext.text = "";
        }
        index = 0;
        if (bubble != null)
        {
            bubble.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isTyping == false)
        {
            bubble.SetActive(true);
            StartCoroutine(autonextline());
            isTyping = true;
            Debug.Log("Player is close to the NPC");
        }
    }
    IEnumerator Typing()
    {
        if (bubbletext == null) yield break;
        isTyping = true;
        if (index < dialogue.Length)
        {
            bubbletext.text = "";
            foreach (char letter in dialogue[index].ToCharArray())
            {
                bubbletext.text += letter;
                yield return new WaitForSeconds(wordspeed);
            }
        }
    }
    public void NextLine()
    {
        if ( index < dialogue.Length)
        {
            StartCoroutine(Typing());
            index++;
        }
    }
    IEnumerator autonextline()
    {
        
        NextLine();
        yield return new WaitForSeconds(waitTime);
        if (index < dialogue.Length)
        {
            StartCoroutine(autonextline());
        }
        else
        {
            zeroText();
            bubble.SetActive(false);
            yield return new WaitForSeconds(waitTime);
            isTyping = false;
        }
        
    }
}