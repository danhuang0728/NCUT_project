using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
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
     private List<GameObject> spawnedObjects = new List<GameObject>();



    void Start()
    {
        StartCoroutine(BlackHoleIE());
    }

    void Update()
    {
        
    }

    IEnumerator BlackHoleIE()
    {
        // 计算随机位置
        Vector2 randomPosition = new Vector2(
            Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2),
            Random.Range(spawnAreaCenter.y - spawnAreaSize.y / 2, spawnAreaCenter.y + spawnAreaSize.y / 2)
        );
        
        // 生成黑洞組件
        GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
        Debug.Log("randompoS:"+randomPosition);
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(randomPosition, attackRadius, playerLayer);
        foreach (Collider2D hitPlayer in  hitPlayers)
        {
            // 取得玩家的 Transform
            Debug.Log("玩家在範圍內");
            Transform playerTransfrom = hitPlayer.transform;
            movePlayerToBlackHole(playerTransfrom,randomPosition);    
        }   
        yield return new WaitForSeconds(5f);
        //Destroy(spawnedObject, 0f);
            
        
    }

    

    // 可视化生成范围（在编辑器中显示）

    public void movePlayerToBlackHole(Transform player, Vector3 centerPoint)
    {
        Debug.Log("吸");
        Vector3 direction = (player.transform.position - centerPoint).normalized;
        player_.AddForce(direction * 5);

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // 绿色半透明
        Gizmos.DrawCube(new Vector3(spawnAreaCenter.x, spawnAreaCenter.y, 0), new Vector3(spawnAreaSize.x, spawnAreaSize.y, 1));
        Gizmos.DrawWireSphere(blackhole_position.position, attackRadius);
    }
}