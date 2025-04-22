using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class light_breath : MonoBehaviour
{
    private Light2D light2d;
    public float lightMin = 0f;
    public float lightMax = 1f;
    public int n = 10; // 分成10段
    public float time = 1f;


    void Start()
    {
        light2d = GetComponent<Light2D>();
        StartCoroutine(Breath());
    }

    void Update()
    {
        
    }
    IEnumerator Breath()
    {
        while (true)
        {
            float step = (lightMax - lightMin) / n;
            
            // 亮度漸強
            for(float i = lightMin; i <= lightMax; i += step) {
                light2d.intensity = i;
                yield return new WaitForSeconds(time);
            }
            
            // 亮度漸弱
            for(float i = lightMax; i >= lightMin; i -= step) {
                light2d.intensity = i;

                yield return new WaitForSeconds(time); 
            }
        }
    }
}
