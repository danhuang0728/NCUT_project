using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchBTN : MonoBehaviour
{
    public GameObject ValueDisplayPanel;
    public GameObject tetrPanel;
    private tetrisPanel_ TetrisPanel_;
    private ValueDisplay valueDisplay;
    private Vector3 originalText1Pos;
    private Vector3 originalText2Pos;
    private Vector3 originalBtn1Pos;
    private Vector3 originalBtn2Pos;
    public Sprite onpress; // 按下去的圖片
    public Sprite original; // 原本的圖片
    public GameObject btn1_obj; //按鈕圖片切換用
    public GameObject btn1_text;
    public GameObject btn2_obj; //按鈕圖片切換用
    public GameObject btn2_text;
    
    void Start()
    {
        TetrisPanel_ = FindObjectOfType<tetrisPanel_>();
        valueDisplay = FindObjectOfType<ValueDisplay>();
        
        // 儲存原始位置
        if (btn1_text != null) originalText1Pos = btn1_text.transform.position;
        if (btn2_text != null) originalText2Pos = btn2_text.transform.position;
        if (btn1_obj != null) originalBtn1Pos = btn1_obj.transform.position;
        if (btn2_obj != null) originalBtn2Pos = btn2_obj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (ValueDisplayPanel.activeSelf ||  tetrPanel.activeSelf)
        {
            openAllChildren();
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                valueDisplay.closePanel();
                TetrisPanel_.CloseInventory();
            }
        }
        else
        {
           closeAllChildren(); 
        }
    }

    public void closeAllChildren()
    {
        // 遍歷所有子物件並關閉
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void openAllChildren()
    {
        // 遍歷所有子物件並關閉
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    public void btn1()
    {
        TetrisPanel_.CloseInventory();
        valueDisplay.openPanel();

        // 更新按鈕圖片
        if (btn1_obj != null)
        {
            Image btn1Image = btn1_obj.GetComponent<Image>();
            if (btn1Image != null) btn1Image.sprite = onpress;
        }
        if (btn2_obj != null)
        {
            Image btn2Image = btn2_obj.GetComponent<Image>();
            if (btn2Image != null) btn2Image.sprite = original;
        }

        // 更新文字位置
        if (btn1_text != null)
        {
            btn1_text.transform.position = originalText1Pos + new Vector3(0, -3, 0);
        }
        if (btn2_text != null)
        {
            btn2_text.transform.position = originalText2Pos;
        }
    }
    
    public void btn2()
    {
        UIstate.isAnyPanelOpen = false;
        TetrisPanel_.OpenInventory();
        valueDisplay.closePanel();

        // 更新按鈕圖片
        if (btn2_obj != null)
        {
            Image btn2Image = btn2_obj.GetComponent<Image>();
            if (btn2Image != null) btn2Image.sprite = onpress;
        }
        if (btn1_obj != null)
        {
            Image btn1Image = btn1_obj.GetComponent<Image>();
            if (btn1Image != null) btn1Image.sprite = original;
        }

        // 更新文字位置
        if (btn2_text != null)
        {
            btn2_text.transform.position = originalText2Pos + new Vector3(0, -3, 0);
        }
        if (btn1_text != null)
        {
            btn1_text.transform.position = originalText1Pos;
        }
    }

}
