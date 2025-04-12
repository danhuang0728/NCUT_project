using UnityEngine;
using System.Collections;

public class FlyingAxeProjectile : MonoBehaviour
{
    private Vector3 startPosition;
    private float flightTime = 0f;
    private float maxFlightTime = 3f;
    private float rotationSpeed = 720f;
    private float spiralRadius = 2f;
    private float spiralSpeed = 5f;
    private float currentAngle = 0f;
    
    public float damage;
    public Transform target;
    private Vector3 direction;
    public float moveSpeed = 8f;

    void Start()
    {
        
        startPosition = transform.position;
        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
        }
        else
        {
            direction = transform.right;
        }
    }

    void Update()
    {
        flightTime += Time.deltaTime;
        
        // 旋轉飛斧
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        
        // 螺旋運動
        currentAngle += spiralSpeed * Time.deltaTime;
        Vector3 spiralOffset = new Vector3(
            Mathf.Cos(currentAngle) * spiralRadius,
            Mathf.Sin(currentAngle) * spiralRadius,
            0
        );
        
        // 基本前進運動加上螺旋偏移
        Vector3 baseMovement = direction * moveSpeed * Time.deltaTime;
        transform.position += baseMovement;
        transform.position = transform.position + spiralOffset - (spiralOffset - new Vector3(
            Mathf.Cos(currentAngle - spiralSpeed * Time.deltaTime) * spiralRadius,
            Mathf.Sin(currentAngle - spiralSpeed * Time.deltaTime) * spiralRadius,
            0
        ));

        // 超過最大飛行時間後銷毀
        if (flightTime >= maxFlightTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            // 對怪物造成傷害
            NormalMonster_setting monster = other.GetComponent<NormalMonster_setting>();
            monster.HP -= damage;
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            // 碰到牆壁時銷毀
            Destroy(gameObject);
        }
    }
} 