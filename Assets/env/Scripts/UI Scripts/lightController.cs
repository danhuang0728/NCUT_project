using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class lightController : MonoBehaviour
{
    public Volume volume;
    public Slider slider;
    private float lightIntensity;
    
    private ColorAdjustments colorAdjustments;
    
    void Start()
    {
        // 獲取Volume Profile中的ColorAdjustments組件
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out colorAdjustments);
        }
        
        // 初始化滑塊值
        if (colorAdjustments != null && colorAdjustments.postExposure.overrideState)
        {
            slider.value = colorAdjustments.postExposure.value;
            lightIntensity = slider.value;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnValueChanged()
    {
        lightIntensity = slider.value/10;
        
        // 更新postExposure值
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.Override(lightIntensity);
        }
    }
}
