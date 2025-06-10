using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
public class Pumpkin_Knight_spawn : MonoBehaviour
{
    public static Pumpkin_Knight_spawn instance;
    public GameObject pumpkinKnightPrefab;
    public GameObject pumpkinPrefab;
    public GameObject effect1;
    public GameObject effect2;

    public Slider BossHealthBar; //南瓜騎士血條
    public Transform spawnPoint;
    private ChromaticAberration chromaticAberration;
    public Volume postProcessVolume;
    void Start()
    {
        instance = this;   
        if (postProcessVolume != null)
        {
            // 取得色差效果組件
            postProcessVolume.profile.TryGet(out chromaticAberration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PumpkinKnight_spawn()
    {
        StartCoroutine(spawnEffect());
    }
    IEnumerator spawnEffect()
    {
        yield return new WaitForSeconds(2f);
         AudioManager.Instance.PlayMusic("Boss_music");
         yield return new WaitForSeconds(1.5f);
        BossHealthBar.gameObject.SetActive(true);
        TimerPanel.isfighting = false;
        //激活特效物件
        effect1.SetActive(true);
        effect2.SetActive(true);
        StartCoroutine(ClosechromaticAberration(1.5f));
        //等待特效播放完畢
        yield return new WaitForSeconds(0.2f);
        pumpkinPrefab.SetActive(false);
        //生成南瓜騎士
        Instantiate(pumpkinKnightPrefab, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.6f);
        effect1.SetActive(false);
        effect2.SetActive(false);
    }
    IEnumerator ClosechromaticAberration(float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            chromaticAberration.intensity.value = Mathf.Lerp(1f, 0f, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
