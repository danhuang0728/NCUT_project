using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystal_TP : MonoBehaviour
{
    public GameObject player;
    public GameObject crystal_TP_effect_prb;
    public Transform crystal_TP_target;
    private GameObject player_camera;
    private float distance;

    private void Start()
    {
        crystal_TP_effect_prb.SetActive(false);
        player_camera = GameObject.Find("Main Camera");
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < 1.5f && Input.GetKeyDown(KeyCode.E))
        {
            player.transform.position = crystal_TP_target.position;
            player_camera.transform.position = crystal_TP_target.position;
            StartCoroutine(TeleportEffect());
        }
    }

    private IEnumerator TeleportEffect()
    {

        crystal_TP_effect_prb.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        crystal_TP_effect_prb.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }
}
