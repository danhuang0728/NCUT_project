using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFlower_final : MonoBehaviour
{
    public GameObject player;
    public GameObject crystal_TP_effect_prb;
    public Transform crystal_TP_target;

    private void Start()
    {
        crystal_TP_effect_prb.SetActive(false);
    }

    private void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            player.transform.position = crystal_TP_target.position;
            StartCoroutine(TeleportEffect());
        }
    }
    private IEnumerator TeleportEffect()
    {

        crystal_TP_effect_prb.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        crystal_TP_effect_prb.SetActive(false);
    }
}
