using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class mazeEffect : MonoBehaviour
{
    public Collider2D collider2d;
    public LayerMask player;
    public Volume postProcessingVolume;
    private Vignette vignette;

    void Start()
    {
        // 嘗試從 Volume 中抓取 Vignette 效果
        collider2d = GetComponent<Collider2D>();
        if (postProcessingVolume.profile.TryGet<Vignette>(out vignette))
        {
            
        }
    }

    void Update()
    {
        if (vignette != null)
        {
            // 用鍵盤上下鍵調整亮度（只是示例）
            if (collider2d.IsTouchingLayers(player))
            {
                if (vignette.intensity.value < 1f)
                {
                vignette.intensity.value = Mathf.Clamp(vignette.intensity.value + 0.005f, 0f, 1f);
                }
            }
            
            else
            {
                if (vignette.intensity.value > 0.2f)
                {
                vignette.intensity.value = Mathf.Clamp(vignette.intensity.value - 0.005f, 0f, 1f);
                }
            }
        }
    }  
}

