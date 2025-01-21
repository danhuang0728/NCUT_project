using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class exp_low : MonoBehaviour
{
    private GameObject drop_exp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 經驗值偵測
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
