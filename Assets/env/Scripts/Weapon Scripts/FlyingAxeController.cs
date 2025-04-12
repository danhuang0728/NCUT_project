using UnityEngine;
using System.Collections;

public class FlyingAxeController : MonoBehaviour
{
    public GameObject axePrefab;
    public float axeSpeed = 15f;
    private float fireRate = 1f;
    private float timer = 0f;
    public bool Is_in_range = false;
    [Range(1, 6)]
    public int level;
    public bool is_levelUP = false;
    private int count = 1;
    private Transform player_t;
    private character_value_ingame characterValuesIngame;
    public Character_Values_SETUP characterValues;

    [Header("螺旋設定")]
    public float spiralRadius = 2f;
    public float spiralSpeed = 5f;
    public float spiralRotateSpeed = 3f;

    void Start()
    {
        player_t = GameObject.Find("player1").transform;
        characterValuesIngame = GameObject.Find("player1").GetComponent<character_value_ingame>();
    }

    void Update()
    {
        transform.position = player_t.transform.position;
        timer += Time.deltaTime;

        if (timer >= 1f / fireRate)
        {
            if (Is_in_range == true)
            {
                StartCoroutine(Fire());
            }
            timer = 0f;
        }

        ProcessLevel(level);
    }

    void FireAxe()
    {
        GameObject nearestMonster = FindNearestMonster();
        if (nearestMonster == null)
        {
            return;
        }

        Vector3 axePosition = transform.position;
        Vector2 direction = (nearestMonster.transform.position - transform.position).normalized;
        float fireAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject axe = Instantiate(axePrefab, axePosition, Quaternion.identity);
        axe.SetActive(true);
        FlyingAxeProjectile axeComponent = axe.GetComponent<FlyingAxeProjectile>();
        
        if (axeComponent != null)
        {
            axeComponent.target = nearestMonster.transform;
            axeComponent.damage = GetDamageForLevel(level);
            axeComponent.moveSpeed = axeSpeed;
        }

        axe.transform.rotation = Quaternion.Euler(0, 0, fireAngle);
    }

    float GetDamageForLevel(int level)
    {
        switch (level)
        {
            case 1: return 8f;
            case 2: return 15f;
            case 3: return 25f;
            case 4: return 40f;
            case 5: return 60f;
            default: return 8f;
        }
    }

    GameObject FindNearestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestMonster = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float distanceToMonster = Vector3.Distance(transform.position, monster.transform.position);
            if (distanceToMonster < shortestDistance)
            {
                shortestDistance = distanceToMonster;
                nearestMonster = monster;
            }
        }

        return nearestMonster;
    }

    public void ProcessLevel(int level)
    {
        fireRate = GetFireRateForLevel(level);
        count = GetProjectileCountForLevel(level);
    }

    float GetFireRateForLevel(int level)
    {
        float baseFireRate = 0f;
        switch (level)
        {
            case 1: baseFireRate = 0.33f; break;
            case 2: baseFireRate = 0.5f; break;
            case 3: baseFireRate = 0.75f; break;
            case 4: baseFireRate = 1f; break;
            case 5: baseFireRate = 1.2f; break;
            default: baseFireRate = 0.33f; break;
        }
        return baseFireRate + (characterValuesIngame.cooldown_percentage + characterValues.cooldown_addition_percentage);
    }

    int GetProjectileCountForLevel(int level)
    {
        if (level <= 2) return 1;
        if (level <= 4) return 2;
        return 3;
    }

    IEnumerator Fire()
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.15f);
            FireAxe();
        }
    }
} 