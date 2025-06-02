using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffGroup_manager : MonoBehaviour
{
    public static BuffGroup_manager instance;
    public GameObject damage_down_icon;
    public GameObject damage_up_icon;
    public GameObject speed_up_icon;
    public GameObject speed_down_icon;
    public GameObject health_up_icon;
    public GameObject health_down_icon;
    public GameObject blindness_icon;
    public GameObject Night_Vision_icon;
    public enum BuffType
    {
        damage_down,
        damage_up,
        speed_up,
        speed_down,
        health_up,
        health_down,
        blindness,
        Night_Vision
    }
    
    void Awake()
    {
        Debug.Log("BuffGroup_manager 初始化開始");
        if (instance == null)
        {
            instance = this;
            Debug.Log("BuffGroup_manager 實例已創建");
        }
        else
        {
            Debug.LogWarning("重複的 BuffGroup_manager 被銷毀");
        }
    }

    void Start()
    {
        setCloseIcon(BuffType.damage_down);
        setCloseIcon(BuffType.damage_up);
        setCloseIcon(BuffType.speed_up);
        setCloseIcon(BuffType.speed_down);
        setCloseIcon(BuffType.health_up);
        setCloseIcon(BuffType.health_down);
        setCloseIcon(BuffType.blindness);
        setCloseIcon(BuffType.Night_Vision);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setOpenIcon(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.damage_down:
                damage_down_icon.SetActive(true);
                break;
            case BuffType.damage_up:
                damage_up_icon.SetActive(true);
                break;
            case BuffType.speed_up:
                speed_up_icon.SetActive(true);
                break;
            case BuffType.speed_down:
                speed_down_icon.SetActive(true);
                break;
            case BuffType.health_up:
                health_up_icon.SetActive(true);
                break;
            case BuffType.health_down:
                health_down_icon.SetActive(true);
                break;
            case BuffType.blindness:
                blindness_icon.SetActive(true);
                break;
            case BuffType.Night_Vision:
                Night_Vision_icon.SetActive(true);
                break;
        }
    }
    public void setCloseIcon(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.damage_down:
                damage_down_icon.SetActive(false);
                break;
            case BuffType.damage_up:
                damage_up_icon.SetActive(false);
                break;
            case BuffType.speed_up:
                speed_up_icon.SetActive(false);
                break;
            case BuffType.speed_down:
                speed_down_icon.SetActive(false);
                break;
            case BuffType.health_up:
                health_up_icon.SetActive(false);
                break;
            case BuffType.health_down:
                health_down_icon.SetActive(false);
                break;
            case BuffType.blindness:
                blindness_icon.SetActive(false);
                break;
            case BuffType.Night_Vision:
                Night_Vision_icon.SetActive(false);
                break;
        }
    }
}
