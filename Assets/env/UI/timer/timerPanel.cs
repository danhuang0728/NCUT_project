using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerPanel : MonoBehaviour
{
    public GameObject panel;
    public static bool isfighting = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isfighting){open();}
        if(!isfighting){close();}
    }
    public void open()
    {
        panel.SetActive(true);
    }
    public void close()
    {
        panel.SetActive(false);
    }
}
