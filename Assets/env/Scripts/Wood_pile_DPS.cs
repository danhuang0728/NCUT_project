using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Wood_pile_DPS : MonoBehaviour
{
    private NormalMonster_setting normalMonster_setting;
    private float monster_HP;
    private float previousHP;
    public TextMeshPro dps_text;
    private bool isQuitting = false;
    private bool isForceActive = true;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        normalMonster_setting = GetComponent<NormalMonster_setting>();
        monster_HP = normalMonster_setting.HP;
        StartCoroutine(CalculateDPS());
        StartCoroutine(ForceActiveCheck());
    }

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    void OnEnable()
    {
        // 防止在編輯器中被刪除
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        #endif
    }

    void OnDisable()
    {
        if (!isQuitting && isForceActive)
        {
            gameObject.SetActive(true);
            
            // 重新啟用所有組件
            Component[] components = gameObject.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component != null && component is Behaviour)
                {
                    ((Behaviour)component).enabled = true;
                }
            }
        }
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        #endif
    }

    #if UNITY_EDITOR
    private void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
    {
        if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
        {
            isQuitting = true;
        }
    }
    #endif

    void OnDestroy()
    {
        if (!isQuitting)
        {
            try
            {
                // 立即在相同位置創建一個新的物件
                GameObject newObj = Instantiate(gameObject, transform.position, transform.rotation);
                newObj.name = gameObject.name;
                
                if (transform.parent != null)
                {
                    newObj.transform.SetParent(transform.parent);
                }
                
                // 確保新物件也不會被摧毀
                DontDestroyOnLoad(newObj);
            }
            catch (Exception e)
            {
                Debug.LogWarning("無法重新創建物件: " + e.Message);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator CalculateDPS()
    {
        previousHP = monster_HP;
        float currentHP;
        float damageDealt;
        float dps;

        while (true)
        {
            // 等待1秒
            yield return new WaitForSeconds(1f);
            
            // 獲取當前血量
            currentHP = normalMonster_setting.HP;
            
            // 計算這一秒造成的傷害
            damageDealt = previousHP - currentHP;
            
            // 如果有傷害，計算並顯示DPS
            if (damageDealt > 0)
            {
                dps = damageDealt;
                dps = Mathf.RoundToInt(dps);
                dps_text.text = "DPS: " + dps + "/s";
            }
            else{
                dps = 0;
                dps_text.text = "DPS: " + dps + "/s";
            }
            
            // 更新前一秒的血量記錄
            previousHP = currentHP;
        }
    }

    private IEnumerator ForceActiveCheck()
    {
        while (isForceActive)
        {
            // 檢查主物件
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            // 檢查所有組件
            Component[] components = gameObject.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component != null && component is Behaviour)
                {
                    Behaviour behaviour = (Behaviour)component;
                    if (!behaviour.enabled)
                    {
                        behaviour.enabled = true;
                    }
                }
            }
            
            // 檢查所有子物件及其組件
            CheckChildrenAndComponents(transform);
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CheckChildrenAndComponents(Transform parent)
    {
        // 檢查當前物件的所有組件
        Component[] components = parent.gameObject.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (component != null && component is Behaviour)
            {
                Behaviour behaviour = (Behaviour)component;
                if (!behaviour.enabled)
                {
                    behaviour.enabled = true;
                }
            }
        }

        // 檢查子物件
        foreach (Transform child in parent)
        {
            if (!child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(true);
            }
            
            // 遞迴檢查子物件的子物件
            CheckChildrenAndComponents(child);
        }
    }
}
