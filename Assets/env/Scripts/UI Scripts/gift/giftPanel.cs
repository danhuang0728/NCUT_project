using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class giftPanel : MonoBehaviour
{
    [SerializeField]private GameObject panel;

    void Start()
    {
        panel = gameObject;
        // 確保面板一開始是關閉的
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    // 打開面板的公共方法
    public void OpenPanel()
    {
        if (panel != null)
        {
            AudioManager.Instance.PlaySFX("button_click_left");
            panel.SetActive(true);
        }
    }

    // 關閉面板的公共方法
    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && panel.activeSelf)
        {
            ClosePanel();
        }
    }
}
