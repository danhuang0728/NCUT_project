using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToPlayer : MonoBehaviour
{
   private GameObject player;
    void Start()
    {
        player = GameObject.Find("player1");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y - 1/2, -10f);
        
    }
}
