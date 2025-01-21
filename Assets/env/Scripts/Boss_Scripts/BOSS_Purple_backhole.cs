using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BOSS_Purple_Blackhole : MonoBehaviour
{
    [Header("Prefab and Spawn Settings")]
    public GameObject objectToSpawn; // 要复制的对象
    public int spawnCount = 10; // 要生成的物体数量

    [Header("Spawn Area")]
    public Vector2 spawnAreaCenter; // 生成范围的中心点
    public Vector2 spawnAreaSize; // 生成范围的宽和高
    public GameObject targetObject; // 用于检测接触的目标对象
    public LayerMask playerLayer;
    public float attackRadius;
    public Transform blackhole_position;
    public Rigidbody2D player_;
    private GameObject spawnedObject;
    public Vector2 randomPosition;
    public int blackhole_Count=1;



    void Start()
    {
        for (int i=0;i<blackhole_Count;i++)
        {
            create_BlackHole();
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7)) // 按下數字鍵 7 觸發攻擊
        {     
            Recreate_BlackHole(spawnedObject);    
            for (int i=0;i<blackhole_Count;i++)
            {
                create_BlackHole();
            }
        }
    }
    void create_BlackHole()
    {
        randomPosition = new Vector2(
                Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
                Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2)
        );
        StartCoroutine(BlackHoleIE());
    }
    void Recreate_BlackHole(GameObject gameObject)
    {
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("BlackHoleSpawnedObject");
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }

    }

    IEnumerator BlackHoleIE()
    {
        Collider2D[] hitMonsters = Physics2D.OverlapCircleAll(randomPosition, 5, playerLayer);
        foreach (Collider2D player in hitMonsters)  //避免身在玩家底下 
        {
            create_BlackHole();
            break;
        }
        // 生成黑洞組件
        spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        spawnedObject.tag = "BlackHoleSpawnedObject";
       
        yield return new WaitForSeconds(10f);
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("BlackHoleSpawnedObject");
        foreach (GameObject obj in spawnedObjects)
        {
            Destroy(obj);
        }
            
        
    }

    

    // 可视化生成范围（在编辑器中显示）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // 绿色半透明
        Gizmos.DrawCube(new Vector3(spawnAreaCenter.x, spawnAreaCenter.y, 0), new Vector3(spawnAreaSize.x, spawnAreaSize.y, 1));
        Gizmos.DrawWireSphere(blackhole_position.position, attackRadius);
    }
}