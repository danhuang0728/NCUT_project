using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIstate : MonoBehaviour
{
    public static bool isAnyPanelOpen = false;
    public void Update()
    {
        if (isAnyPanelOpen)
        {
            Time.timeScale = 0; // 暫停遊戲
        }
        else
        {
            Time.timeScale = PlayerControl.N; // 恢復遊戲
        }
    }
    public void setPanelState(bool state)
    {
        isAnyPanelOpen = state;
    }
}
