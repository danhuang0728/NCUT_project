using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBook_Triggerrange : MonoBehaviour
{
    public Transform player_t;
    public MagicBook magicBook;
    void Start()
    {
        magicBook.GetComponent<BoomerangController>();
    }
    void Update()
    {
        transform.position = player_t.transform.position; // 設定觸發範圍的位置
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            magicBook.Is_in_range = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {  
            magicBook.Is_in_range = false;
        }
    }
}
