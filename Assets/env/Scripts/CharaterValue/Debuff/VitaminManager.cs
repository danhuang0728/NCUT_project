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
    private const float MAX_VITAMIN_LEVEL = 300f;
    private const float VITAMIN_DECREASE_RATE = 1.5f; // 每秒减少量
    private const float VITAMIN_INCREASE_AMOUNT = 20f; // 吃到水果时增加量
    private const float DEBUFF_THRESHOLD = 50f; // 低于此值触发Debuff
    private const float BUFF_THRESHOLD = 200f; // 高于此值触发Buff

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
            vitaminLevels[vitamin] = 175f;  // 将初始值设为175
            vitaminStates[vitamin] = VitaminState.Normal;  // 初始状态为正常
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
        
        // 按下5键，将所有维生素设为5
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            foreach (VitaminType vitamin in System.Enum.GetValues(typeof(VitaminType)))
            {
                vitaminLevels[vitamin] = 50f;
                UpdateVitaminState(vitamin);
            }
            //Debug.Log("所有維生素值設為50");
        }
        
        // 按下6键，将所有维生素设为80
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            foreach (VitaminType vitamin in System.Enum.GetValues(typeof(VitaminType)))
            {
                vitaminLevels[vitamin] = 150f;
                UpdateVitaminState(vitamin);
            }
            //Debug.Log("所有維生素值設為80");
        }

        // 按下7键，将所有维生素设为100
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            foreach (VitaminType vitamin in System.Enum.GetValues(typeof(VitaminType)))
            {
                vitaminLevels[vitamin] = 250f;
                UpdateVitaminState(vitamin);
            }
            //Debug.Log("所有維生素值設為100");
        }

        foreach (VitaminType vitamin in System.Enum.GetValues(typeof(VitaminType)))
        {
            // 降低维生素值
            vitaminLevels[vitamin] = Mathf.Max(0, vitaminLevels[vitamin] - VITAMIN_DECREASE_RATE * Time.deltaTime);

            // 检查并更新状态
            UpdateVitaminState(vitamin);
        }

        // 每秒输出一次Debug信息
        if (debugTimer >= 5f)
        {
            foreach (VitaminType vitamin in System.Enum.GetValues(typeof(VitaminType)))
            {
                //Debug.Log($"維生素{vitamin} - 數值:{vitaminLevels[vitamin]:F1} | 狀態:{vitaminStates[vitamin]}");
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
            VitaminState oldState = vitaminStates[vitamin];
            
            // 只在特定情况下移除旧效果
            if (oldState == VitaminState.Buff && newState != VitaminState.Buff)
            {
                RemoveBuffEffect(vitamin);
            }
            else if (oldState == VitaminState.Debuff && newState != VitaminState.Debuff)
            {
                FruitType fruitType = GetCorrespondingFruitType(vitamin);
                if (debuffManager.HasDebuff(fruitType))
                {
                    debuffManager.RemoveDebuff(fruitType);
                }
            }
            
            // 应用新状态的效果
            ApplyNewEffect(vitamin, newState);
            
            // 更新状态
            vitaminStates[vitamin] = newState;
        }
    }

    private void RemoveCurrentEffect(VitaminType vitamin)
    {
        // 移除 Debuff 效果
        FruitType fruitType = GetCorrespondingFruitType(vitamin);
        if (debuffManager.HasDebuff(fruitType))
        {
            debuffManager.RemoveDebuff(fruitType);
        }

        // 移除 Buff 效果
        RemoveBuffEffect(vitamin);
    }

    private void RemoveBuffEffect(VitaminType vitamin)
    {
        // 根据不同维生素类型移除相应的 Buff 效果
        switch (vitamin)
        {
            case VitaminType.A: // 视野
                if (playerLight != null)
                {
                    playerLight.intensity /= 1.5f; // 恢复原来的视野
                    BuffGroup_manager.instance.setCloseIcon(BuffGroup_manager.BuffType.Night_Vision);
                    Debug.Log($"恢復原本視野亮度: {playerLight.intensity}");
                }
                break;
                
            case VitaminType.B: // 移动速度
                if (playerControl != null)
                {
                    playerControl.speed /= 1.2f; // 恢复原来的速度
                    BuffGroup_manager.instance.setCloseIcon(BuffGroup_manager.BuffType.speed_up);
                    Debug.Log($"恢復原本移動速度: {playerControl.speed}");
                }
                break;
                
            case VitaminType.C: // 生命值
                if (playerControl != null && playerControl.HP > 0 && characterValue != null)
                {
                    characterValue.health -= 50f; // 减少50点生命值上限
                    playerControl.HP = Mathf.Min(playerControl.HP, 100 + characterValue.health); // 确保当前生命值不超过新的上限
                    BuffGroup_manager.instance.setCloseIcon(BuffGroup_manager.BuffType.health_up);
                    Debug.Log($"減少50點生命值上限，當前生命值: {playerControl.HP}，最大生命值加成: {characterValue.health}");
                }
                break;
                
            case VitaminType.D: // 攻击力
                if (characterValue != null)
                {
                    characterValue.damage /= 1.2f; // 恢复原来的攻击力
                    BuffGroup_manager.instance.setCloseIcon(BuffGroup_manager.BuffType.damage_up);
                    Debug.Log($"恢復原本攻擊力: {characterValue.damage}");
                }
                break;
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
                if (playerControl != null && playerControl.HP > 0 && characterValue != null)
                {
                    characterValue.health += 50f; // 增加50点生命值上限
                    playerControl.HP += 50f; // 同时增加当前生命值
                    BuffGroup_manager.instance.setOpenIcon(BuffGroup_manager.BuffType.health_up);
                    Debug.Log($"增加50點生命值上限和當前生命值，當前生命值: {playerControl.HP}，最大生命值加成: {characterValue.health}");
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