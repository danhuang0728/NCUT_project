using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Linq;
public class MagicFlower_TP : MonoBehaviour
{

    public GameObject player;
    public GameObject magicFlower_TP_effect_prb;
    private Transform magicFlower_TP_target;
    private MagicFlower_TP[] allMagicFlowers; //儲存所有掛載此腳本的物件
    private List<MagicFlower_TP> otherFlowers = new List<MagicFlower_TP>(); //去掉自己後的物件

    private void Start()
    {
        // 找到所有掛載此腳本的物件
        allMagicFlowers = FindObjectsOfType<MagicFlower_TP>();
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
