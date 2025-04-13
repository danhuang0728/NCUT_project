using UnityEngine;

public class AxeSpawner : MonoBehaviour
{
    [Header("飛斧設定")]
    public GameObject axePrefab;         // 飛斧預製體
    public int numberOfAxes = 3;         // 同時發射的飛斧數量
    public float spiralGrowthRate = 0.2f;// 螺旋線增長率
    public float baseRadius = 1f;        // 基礎半徑
    public float rotationSpeed = 360f;   // 旋轉速度（度/秒）
    public float axeSpeed = 10f;         // 飛斧速度
    public float baseFireRate = 0.25f;   // 基礎發射頻率

    [Header("等級設定")]
    [SerializeField]
    [Range(1, 5)]
    private int _level = 1;
    public int level 
    {
        get => _level;
        set
        {
            _level = Mathf.Clamp(value, 1, 5);
            UpdateStats();
        }
    }

    [Header("當前狀態")]
    [SerializeField] private float currentDamage;
    [SerializeField] private float currentFireRate;
    public bool Is_in_range = false;

    private Transform playerTransform;
    private float nextFireTime = 0f;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        UpdateStats();
    }

    private void Update()
    {
        transform.position = playerTransform.position;
        

        if (Time.time >= nextFireTime && Is_in_range)
        {
            SpawnAxesInSpiral();
            nextFireTime = Time.time + (1f / currentFireRate);
        }
    }

    public void UpdateStats()
    {
        switch (_level)
        {
            case 1:
                currentFireRate = baseFireRate;
                currentDamage = 10f;
                numberOfAxes = 3;
                break;
            case 2:
                currentFireRate = baseFireRate * 1.2f;
                currentDamage = 15f;
                numberOfAxes = 4;
                break;
            case 3:
                currentFireRate = baseFireRate * 1.4f;
                currentDamage = 20f;
                numberOfAxes = 5;
                break;
            case 4:
                currentFireRate = baseFireRate * 1.6f;
                currentDamage = 25f;
                numberOfAxes = 6;
                break;
            case 5:
                currentFireRate = baseFireRate * 2f;
                currentDamage = 30f;
                numberOfAxes = 8;
                break;
        }
    }

    private void SpawnAxesInSpiral()
    {
        float angleStep = 360f / numberOfAxes;
        
        for (int i = 0; i < numberOfAxes; i++)
        {
            float startAngle = i * angleStep;
            SpawnSpiralAxe(startAngle);
        }
    }

    private void SpawnSpiralAxe(float startAngle)
    {
        GameObject axe = Instantiate(axePrefab, transform.position, Quaternion.identity);
        SpiralAxeMovement movement = axe.GetComponent<SpiralAxeMovement>();
        if (movement == null)
        {
            movement = axe.AddComponent<SpiralAxeMovement>();
        }

        movement.SetDamage(currentDamage);
        movement.Initialize(transform.position, startAngle, spiralGrowthRate, baseRadius, rotationSpeed, axeSpeed);
    }

    public void OnLevelChanged()
    {
        UpdateStats();
    }

    private void OnValidate()
    {
        _level = Mathf.Clamp(_level, 1, 5);
        UpdateStats();
    }
}

public class SpiralAxeMovement : MonoBehaviour
{
    [Header("生命週期設定")]
    public float lifeTime = 5f;          // 飛斧存活時間

    [Header("攻擊設定")]
    [SerializeField]
    private float _damage = 10f;         // 傷害值

    private Vector3 center;
    private float currentAngle;
    private float growthRate;
    private float baseRadius;
    private float rotationSpeed;
    private float moveSpeed;
    private float currentRadius;
    private float travelTime;
    private float aliveTime;
    private PlayerControl playerControl;
    private bool isDestroyed = false;    // 添加標記，防止重複銷毀

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void Awake()
    {
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(1f, 1f); // 設置適當的碰撞器大小
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerControl = player.GetComponent<PlayerControl>();
        }
    }

    public void Initialize(Vector3 centerPos, float startAngle, float spiralGrowthRate, float startRadius, float rotSpeed, float speed)
    {
        center = centerPos;
        currentAngle = startAngle;
        growthRate = spiralGrowthRate;
        baseRadius = startRadius;
        rotationSpeed = rotSpeed;
        moveSpeed = speed;
        currentRadius = baseRadius;
        travelTime = 0;
        aliveTime = 0;
    }

    private void Update()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime >= lifeTime)
        {
            DestroyAxe();
            return;
        }

        travelTime += Time.deltaTime;
        currentRadius = baseRadius + (growthRate * travelTime * moveSpeed);
        currentAngle += rotationSpeed * Time.deltaTime;

        float radian = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(
            Mathf.Cos(radian) * currentRadius,
            Mathf.Sin(radian) * currentRadius,
            0
        );

        transform.position = center + offset;
        
        Vector3 direction = new Vector3(
            -Mathf.Sin(radian),
            Mathf.Cos(radian),
            0
        );
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDestroyed) return; // 如果已經標記為銷毀，直接返回

        if (other.CompareTag("wall"))
        {
            Debug.Log("飛斧碰到牆壁");
            isDestroyed = true;
            DestroyAxe();
            return;
        }

        if (other.CompareTag("Monster"))
        {
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            BossFlower bossFlower = other.GetComponent<BossFlower>();
            Renderer renderer = other.GetComponent<Renderer>();

            if (bossFlower != null)
            {
                Renderer renderer_flower = bossFlower.GetComponent<Renderer>();
                bossFlower.HP -= _damage;
                if (playerControl != null)
                {
                    playerControl.SetBoolWithDelay_void(renderer_flower.material, renderer_flower);
                }
            }

            if (monster != null)
            {
                monster.HP -= _damage;
                AudioManager.Instance.PlaySFX("Boomerrang_hit");

                Material Mat = renderer.material;
                if (playerControl != null)
                {
                    playerControl.SetBoolWithDelay_void(Mat, renderer);
                }
            }
        }
    }

    private void DestroyAxe()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
} 