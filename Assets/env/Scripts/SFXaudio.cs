using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SFXaudio : MonoBehaviour
{
    public Slider slider; // 指定滑桿
    public string sfxName = "attack1"; // 音效名稱

    // 此方法需要手動從 Unity Editor 添加 EventTrigger
    public void OnEndDrag()
    {
        // 播放音效（當滑動結束）
        AudioManager.Instance.PlaySFX(sfxName);
    }
}
