using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class VitaminManager : MonoBehaviour
{
    private static VitaminManager _instance;
    public static VitaminManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<VitaminManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("VitaminManager");
                    _instance = go.AddComponent<VitaminManager>();
                }
            }
            return _instance;
        }
    }

    // 维生素计量值
    private Dictionary<VitaminType, float> vitaminLevels = new Dictionary<VitaminType, float>();
    private Dictionary<VitaminType, VitaminState> vitaminStates = new Dictionary<VitaminType, VitaminState>();
    
    // 常量
    private const float MAX_VITAMIN_LEVEL = 120f;
    private const float VITAMIN_DECREASE_RATE = 1f; // 每秒减少量
    private const float VITAMIN_INCREASE_AMOUNT = 20f; // 吃到水果时增加量
    private const float DEBUFF_THRESHOLD = 40f; // 低于此值触发Debuff
    private const float BUFF_THRESHOLD = 90f; // 高于此值触发Buff

    private DebuffManager debuffManager;
    private float debugTimer = 0f; // 用于控制Debug输出的计时器

    // 组件引用
    private Light2D playerLight;
    private PlayerControl playerControl;
    private character_value_ingame characterValue;

    // 维生素状态枚举
    private enum VitaminState
    {
        Debuff,
        Normal,
        Buff
    }

    // 添加便捷屬性
    public float VitaminA => GetVitaminLevel(VitaminType.A);
    public float VitaminB => GetVitaminLevel(VitaminType.B);
    public float VitaminC => GetVitaminLevel(VitaminType.C);
    public float VitaminD => GetVitaminLevel(VitaminType.D);

    // 獲取維生素狀態的便捷方法
    public bool IsVitaminLow(VitaminType vitamin) => vitaminLevels[vitamin] <= DEBUFF_THRESHOLD;
    public bool IsVitaminNormal(VitaminType vitamin) => vitaminLevels[vitamin] > DEBUFF_THRESHOLD && vitaminLevels[vitamin] < BUFF_THRESHOLD;
    public bool IsVitaminHigh(VitaminType vitamin) => vitaminLevels[vitamin] >= BUFF_THRESHOLD;

    // 獲取維生素狀態的常量
    public float MaxVitaminLevel => MAX_VITAMIN_LEVEL;
    public float DebuffThreshold => DEBUFF_THRESHOLD;
    public float BuffThreshold => BUFF_THRESHOLD;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始化维生素值和状态
        foreach (VitaminType vitamin in System.Enum.GetValues(typeof(VitaminType)))
        {
            vitaminLevels[vitamin] = MAX_VITAMIN_LEVEL;
            vitaminStates[vitamin] = VitaminState.Buff;
        }
    }

    private void Start()
    {
        debuffManager = DebuffManager.Instance;
        
        // 获取玩家相关组件
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerLight = player.GetComponent<Light2D>();
            playerControl = player.GetComponent<PlayerControl>();
            characterValue = player.GetComponent<character_value_ingame>();
        }
        else
        {
            Debug.LogError("找不到玩家物件！");
        }
    }

    private void Update()
    {
        debugTimer += Time.deltaTime;
        
        foreach (VitaminType vitamin in System.Enum.GetValues(typeof(VitaminType)))
        {
            // 降低维生素值
            vitaminLevels[vitamin] = Mathf.Max(0, vitaminLevels[vitamin] - VITAMIN_DECREASE_RATE * Time.deltaTime);

            // 检查并更新状态
            UpdateVitaminState(vitamin);
        }

        // 每秒输出一次Debug信息
        if (debugTimer >= 1f)
        {
            foreach (VitaminType vitamin in System.Enum.GetValues(typeof(VitaminType)))
            {
                Debug.Log($"維生素{vitamin} - 數值:{vitaminLevels[vitamin]:F1} | 狀態:{vitaminStates[vitamin]}");
            }
            debugTimer = 0f;
        }
    }

    private void UpdateVitaminState(VitaminType vitamin)
    {
        float value = vitaminLevels[vitamin];
        VitaminState newState;
        
        // 确定新状态
        if (value <= DEBUFF_THRESHOLD)
            newState = VitaminState.Debuff;
        else if (value >= BUFF_THRESHOLD)
            newState = VitaminState.Buff;
        else
            newState = VitaminState.Normal;

        // 如果状态发生变化
        if (newState != vitaminStates[vitamin])
        {
            // 移除旧状态的效果
            RemoveCurrentEffect(vitamin);
            
            // 应用新状态的效果
            ApplyNewEffect(vitamin, newState);
            
            // 更新状态
            vitaminStates[vitamin] = newState;
        }
    }

    private void RemoveCurrentEffect(VitaminType vitamin)
    {
        FruitType fruitType = GetCorrespondingFruitType(vitamin);
        if (debuffManager.HasDebuff(fruitType))
        {
            debuffManager.RemoveDebuff(fruitType);
        }
    }

    private void ApplyNewEffect(VitaminType vitamin, VitaminState state)
    {
        FruitType fruitType = GetCorrespondingFruitType(vitamin);
        
        switch (state)
        {
            case VitaminState.Debuff:
                Debug.Log($"維生素{vitamin}過低，觸發Debuff效果");
                debuffManager.ApplyDebuff(fruitType);
                break;
                
            case VitaminState.Buff:
                Debug.Log($"維生素{vitamin}充足，觸發Buff效果");
                // 这里应用Buff效果，效果与Debuff相反
                ApplyBuffEffect(vitamin);
                break;
                
            case VitaminState.Normal:
                Debug.Log($"維生素{vitamin}恢復正常狀態");
                break;
        }
    }

    private void ApplyBuffEffect(VitaminType vitamin)
    {
        // 根据不同维生素类型应用相反的Buff效果
        switch (vitamin)
        {
            case VitaminType.A: // 视野
                if (playerLight != null)
                {
                    float currentIntensity = playerLight.intensity;
                    playerLight.intensity = currentIntensity * 1.5f; // 增加50%视野
                    BuffGroup_manager.instance.setOpenIcon(BuffGroup_manager.BuffType.Night_Vision);
                    Debug.Log($"提升視野亮度至: {playerLight.intensity}");
                }
                break;
                
            case VitaminType.B: // 移动速度
                if (playerControl != null)
                {
                    playerControl.speed *= 1.2f; // 增加20%速度
                    BuffGroup_manager.instance.setOpenIcon(BuffGroup_manager.BuffType.speed_up);
                    Debug.Log($"提升移動速度至: {playerControl.speed}");
                }
                break;
                
            case VitaminType.C: // 生命值
                if (playerControl != null && playerControl.HP > 0)
                {
                    float maxHP = playerControl.HP * 1.2f; // 增加20%最大生命值
                    playerControl.HP = maxHP;
                    BuffGroup_manager.instance.setOpenIcon(BuffGroup_manager.BuffType.health_up);
                    Debug.Log($"提升最大生命值至: {maxHP}");
                }
                break;
                
            case VitaminType.D: // 攻击力
                if (characterValue != null)
                {
                    characterValue.damage *= 1.2f; // 增加20%攻击力
                    BuffGroup_manager.instance.setOpenIcon(BuffGroup_manager.BuffType.damage_up);
                    Debug.Log($"提升攻擊力至: {characterValue.damage}");
                }
                break;
        }
    }

    public void AddVitamin(VitaminType vitamin, float amount)
    {
        vitaminLevels[vitamin] = Mathf.Min(MAX_VITAMIN_LEVEL, vitaminLevels[vitamin] + amount);
        UpdateVitaminState(vitamin);
    }

    public float GetVitaminLevel(VitaminType vitamin)
    {
        return vitaminLevels[vitamin];
    }

    private FruitType GetCorrespondingFruitType(VitaminType vitamin)
    {
        switch (vitamin)
        {
            case VitaminType.A:
                return FruitType.Blueberry;
            case VitaminType.B:
                return FruitType.Banana;
            case VitaminType.C:
                return FruitType.Apple;
            case VitaminType.D:
                return FruitType.Orange;
            default:
                return FruitType.Banana;
        }
    }
} 