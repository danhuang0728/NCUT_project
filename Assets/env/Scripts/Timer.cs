using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText; // 顯示倒數計時的文字
    [SerializeField] Slider timerSlider;       // 滑動條元件S
    public float remainingTime;               // 剩餘時間（秒）
    public float maxTime;                     // 最大時間（初始倒數時間）

    void Start()
    {

        // 初始化 Slider 最大值
        timerSlider.maxValue = maxTime;
        timerSlider.value = maxTime; // 起始值設為最大時間
    }

    void Update()
    {
        if (remainingTime > 0)
        {
            // 每秒減少剩餘時間
            remainingTime -= Time.deltaTime;

            // 更新顯示的時間
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // 更新 Slider 的值
            timerSlider.value = remainingTime;
        }
        else
        {
            // 倒計時結束
            remainingTime = 0;
            timerText.text = "00:00";
        }
    }
}
