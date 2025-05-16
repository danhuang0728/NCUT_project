using UnityEngine;

public class ElectricAuraEffect : MonoBehaviour
{
    [Header("电气效果设置")]
    [SerializeField] private GameObject electricEffectPrefab; // 电气效果预制体
    [SerializeField] private float rotationSpeed = 30f; // 旋转速度
    [SerializeField] private float radius = 1.5f; // 圆环半径
    [SerializeField] private float heightOffset = 0.5f; // 高度偏移
    [SerializeField] private float scaleSpeed = 1f; // 缩放速度
    [SerializeField] private float minScale = 0.5f; // 最小缩放
    [SerializeField] private float maxScale = 2f; // 最大缩放

    private GameObject electricEffect; // 电气效果实例
    private Transform playerTransform; // 玩家Transform引用
    private float currentScale = 1f; // 当前缩放值
    private bool isScaling = false; // 是否正在缩放
    private float targetScale = 1f; // 目标缩放值

    private void Start()
    {
        // 获取玩家Transform
        playerTransform = transform;
        
        // 初始化电气效果
        InitializeElectricEffect();
    }

    private void InitializeElectricEffect()
    {
        // 创建电气效果实例
        electricEffect = Instantiate(electricEffectPrefab, transform);
        electricEffect.transform.localPosition = Vector3.zero;
        // 设置初始缩放为0.03
        currentScale = 0.05f;
        targetScale = 0.05f;
        electricEffect.transform.localScale = Vector3.one * currentScale;
    }

    private void Update()
    {
        if (electricEffect == null) return;

        // 更新电气效果的位置
        float angle = Time.time * rotationSpeed;
        float radian = angle * Mathf.Deg2Rad;
        
        // 计算位置
        float x = Mathf.Cos(radian) * radius;
        float z = Mathf.Sin(radian) * radius;
        
        // 设置位置
        Vector3 newPosition = new Vector3(x, heightOffset, z);
        electricEffect.transform.localPosition = newPosition;
        
        // 让电气效果面向圆心
        electricEffect.transform.LookAt(transform.position);

        // 处理缩放
        if (isScaling)
        {
            currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * scaleSpeed);
            electricEffect.transform.localScale = Vector3.one * currentScale;

            // 如果接近目标缩放值，停止缩放
            if (Mathf.Abs(currentScale - targetScale) < 0.01f)
            {
                currentScale = targetScale;
                isScaling = false;
            }
        }
    }

    // 启用/禁用电气效果
    public void SetActive(bool active)
    {
        if (electricEffect != null)
        {
            electricEffect.SetActive(active);
        }
    }

    // 设置电气效果大小
    public void SetScale(float scale)
    {
        // 限制缩放范围
        targetScale = Mathf.Clamp(scale, minScale, maxScale);
        isScaling = true;
    }

    // 设置电气效果强度
    public void SetIntensity(float intensity)
    {
        if (electricEffect != null)
        {
            var particleSystem = electricEffect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                var main = particleSystem.main;
                main.startSize = main.startSize.constant * intensity;
                main.startSpeed = main.startSpeed.constant * intensity;
            }
        }
    }
} 