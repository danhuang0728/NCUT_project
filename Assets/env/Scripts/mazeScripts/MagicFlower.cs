using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MagicFlower : MonoBehaviour
{
    [Header("燈光設定")]
    public Light2D flowerLight;
    public float multiplyFactor = 10f;
    public float lerpSpeed = 3f;

    private float originalIntensity;
    private float originalRadius;
    private float targetIntensity;
    private float targetRadius;
    private Coroutine resetCoroutine; // 新增協程引用

    private void Start()
    {
        // 儲存原始燈光數值
        originalIntensity = flowerLight.intensity;
        originalRadius = flowerLight.pointLightOuterRadius;
        
        // 初始化目標值
        targetIntensity = originalIntensity;
        targetRadius = originalRadius;
    }

    private void Update()
    {
        // 使用線性插值平滑過渡
        flowerLight.intensity = Mathf.Lerp(flowerLight.intensity, targetIntensity, lerpSpeed * Time.deltaTime);
        flowerLight.pointLightOuterRadius = Mathf.Lerp(flowerLight.pointLightOuterRadius, targetRadius, lerpSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            ToggleLightEffect();
        }
    }

    void ToggleLightEffect()
    {
        // 如果已有協程在運行，先停止
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        
        // 設定強化數值並啟動計時器
        targetIntensity = originalIntensity * multiplyFactor;
        targetRadius = originalRadius * multiplyFactor;
        resetCoroutine = StartCoroutine(ResetAfterDelay(2f));
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // 恢復原始數值
        targetIntensity = originalIntensity;
        targetRadius = originalRadius;
    }
}
