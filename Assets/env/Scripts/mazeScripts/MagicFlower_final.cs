using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFlower_final : MonoBehaviour
{
    public GameObject player;
    public GameObject magicFlower_TP_effect_prb;
    private Transform magicFlower_TP_target;
    private MagicFlower_final[] allMagicFlowers; //儲存所有掛載此腳本的物件
    private List<MagicFlower_final> otherFlowers = new List<MagicFlower_final>(); //去掉自己後的物件

    private void Start()
    {
        // 找到所有掛載此腳本的物件
        allMagicFlowers = FindObjectsOfType<MagicFlower_final>();
        foreach (var flower in allMagicFlowers) //去掉自己
        {
            if (flower != this)
            {
                otherFlowers.Add(flower);
            }
        }
        magicFlower_TP_effect_prb.SetActive(false);
    }

    private void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            int randomNumber = Random.Range(0, otherFlowers.Count); 
            magicFlower_TP_target = otherFlowers[randomNumber].transform;
            player.transform.position = magicFlower_TP_target.position;
            StartCoroutine(TeleportEffect());
        }
    }
    private IEnumerator TeleportEffect()
    {

        magicFlower_TP_effect_prb.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        magicFlower_TP_effect_prb.SetActive(false);
    }
}
