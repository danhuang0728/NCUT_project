using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;


public class exp_low : MonoBehaviour
{
    private GameObject drop_exp;
    public LevelManager levelManager;
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
        int Current_level = levelManager.GetCurrentLevel();
        // 經驗值偵測
        if (other.CompareTag("Player"))
        {
            levelManager.AddExperience(5 * Mathf.RoundToInt( Mathf.Pow(Current_level, 1.2f) + 10));
            AudioManager.Instance.PlaySFX("pickup_exp");
            ExpObjectPool.Instance.ReturnExp(gameObject);
        }
    }
}
