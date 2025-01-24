using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang_Triggerrange : MonoBehaviour
{
    public Transform player_t;
    public BoomerangController boomerangController;
    void Start()
    {
        boomerangController.GetComponent<BoomerangController>();
    }
    void Update()
    {
        transform.position = player_t.transform.position; // 設定觸發範圍的位置
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            boomerangController.Is_in_range = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {  
            boomerangController.Is_in_range = false;
        }
    }
}
