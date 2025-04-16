using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToPlayer : MonoBehaviour
{
   private GameObject player;
   public float smoothTime = 10f;
   private Vector3 targetPosition;
    void Awake()
    {
        player = GameObject.Find("player1");
        targetPosition = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y - 1/2, -10f);
        transform.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // 使用線性插值平滑過渡到目標位置
        targetPosition = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y - 1/2, -10f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime * Time.deltaTime);
    }
}
