using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow_trigger : MonoBehaviour
{
    [SerializeField] private Transform player_t;
    public crossbow CrossbowController;
    void Start()
    {


        player_t = GameObject.FindGameObjectWithTag("Player").transform;
        CrossbowController.GetComponent<crossbow>();
    }

    void Update()
    {
        transform.position = player_t.transform.position; // 設定觸發範圍的位置
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            CrossbowController.Is_in_range = true;
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {  
            CrossbowController.Is_in_range = false;
        }

    }
}
