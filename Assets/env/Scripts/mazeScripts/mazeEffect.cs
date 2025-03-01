using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class mazeEffect : MonoBehaviour
{
    public Collider2D collider2d;
    public LayerMask player;
    public Light2D playerLight;
    public Light2D GB_light;
    
    [Header("過渡設定")]
    public float lerpSpeed = 3f; // 新增可調整的過渡速度
    
    // 儲存原始值和當前值
    private float originalGBIntensity;
    private float originalPlayerIntensity;
    private Color originalBackgroundColor;
    private float originalPlayerOuterRadius; // 新增外圈半徑儲存變數
    
    // 新增目標值和當前值
    private float targetGBIntensity;
    private float targetPlayerIntensity;
    private Color targetBackgroundColor;
    private float targetPlayerOuterRadius;
    
    private float currentGBIntensity;
    private float currentPlayerIntensity;
    private Color currentBackgroundColor;
    private float currentPlayerOuterRadius;

    void Start()
    {
        collider2d = GetComponent<Collider2D>();
        
        // 初始化原始值
        originalGBIntensity = GB_light.intensity;
        originalPlayerIntensity = playerLight.intensity;
        originalPlayerOuterRadius = playerLight.pointLightOuterRadius; // 儲存原始外圈大小
        
        if (Camera.main != null)
        {
            originalBackgroundColor = Camera.main.backgroundColor;
        }
        
        // 設定初始目標值和當前值
        targetGBIntensity = originalGBIntensity;
        targetPlayerIntensity = originalPlayerIntensity;
        targetBackgroundColor = originalBackgroundColor;
        targetPlayerOuterRadius = originalPlayerOuterRadius;
        
        currentGBIntensity = originalGBIntensity;
        currentPlayerIntensity = originalPlayerIntensity;
        currentBackgroundColor = originalBackgroundColor;
        currentPlayerOuterRadius = originalPlayerOuterRadius;
    }

    void Update()
    {
        // 根據碰撞狀態設定目標值
        if (collider2d.IsTouchingLayers(player))
        {
            targetGBIntensity = 0f;
            targetPlayerIntensity = 0.2f;
            targetBackgroundColor = Color.black;
            targetPlayerOuterRadius = 3f; // 設定目標外圈大小
        }
        else
        {
            targetGBIntensity = originalGBIntensity;
            targetPlayerIntensity = originalPlayerIntensity;
            targetBackgroundColor = originalBackgroundColor;
            targetPlayerOuterRadius = originalPlayerOuterRadius;
        }

        // 使用線性插值平滑過渡
        currentGBIntensity = Mathf.Lerp(currentGBIntensity, targetGBIntensity, lerpSpeed * Time.deltaTime);
        currentPlayerIntensity = Mathf.Lerp(currentPlayerIntensity, targetPlayerIntensity, lerpSpeed * Time.deltaTime);
        currentBackgroundColor = Color.Lerp(currentBackgroundColor, targetBackgroundColor, lerpSpeed * Time.deltaTime);
        currentPlayerOuterRadius = Mathf.Lerp(currentPlayerOuterRadius, targetPlayerOuterRadius, lerpSpeed * Time.deltaTime);

        // 應用當前值
        GB_light.intensity = currentGBIntensity;
        playerLight.intensity = currentPlayerIntensity;
        playerLight.pointLightOuterRadius = currentPlayerOuterRadius; // 應用外圈大小
        
        if (Camera.main != null)
        {
            Camera.main.backgroundColor = currentBackgroundColor;
        }
    }  
}

