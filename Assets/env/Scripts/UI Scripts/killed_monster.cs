using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class killed_monster : MonoBehaviour
{
    public TextMeshProUGUI killed_monster_text;
    private int lastKillCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 初始化時設置初始值
        lastKillCount = PlayerControl.GetKillCount();
        killed_monster_text.text = lastKillCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        int currentKills = PlayerControl.GetKillCount();
        // 只有在擊殺數改變時才更新UI
        if (currentKills != lastKillCount)
        {
            lastKillCount = currentKills;
            killed_monster_text.text = currentKills.ToString();
        }
    }
}
