using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class Heartlight_controll : MonoBehaviour
{
    private PlayerControl playerController;
    private healthbar healthbar;
    private Volume volume;
    private Light2D light2D;
    private float initialIntensity;
    public float pulseSpeed = 2.0f; // 呼吸速度
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerControl>();
        healthbar = GameObject.FindObjectOfType<healthbar>();
        volume = GameObject.FindObjectOfType<Volume>();
        light2D = GetComponent<Light2D>();
        initialIntensity = light2D.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        //血量低於30%時，開啟呼吸燈效果
        if(playerController.HP <= healthbar.slider.maxValue * 0.3f){
            // 使用線性插值實現呼吸燈效果
            float minIntensity = 2.0f; // 最小亮度
            float maxIntensity = initialIntensity; // 最大亮度
            float t = (Mathf.Sin(Time.time * pulseSpeed) + 1.0f) / 2.0f; // 產生0到1之間的值
            light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t); // 線性插值亮度
            //邊框泛紅濾鏡效果
            Vignette vignette;
            if (volume.profile.TryGet(out vignette))
            {
                vignette.color.value = new Color(52/255f, 0, 0, 1);
            }
        }
        else{
            Vignette vignette;
            //濾鏡效果切回正常
            light2D.intensity = initialIntensity;
            if (volume.profile.TryGet(out vignette))
            {
                vignette.color.value = new Color(0, 0, 0, 1);
            }
        }
    }
}
