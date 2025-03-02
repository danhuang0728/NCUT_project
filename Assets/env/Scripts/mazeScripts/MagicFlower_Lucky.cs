using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class MagicFlower_Lucky : MonoBehaviour
{
    private PlayerControl playerController;
    private SelectionManager selectionManager;
    public Light2D[] light2d;
    private float init_lightIntensity;
    private float targetLightIntensity;
    private void Start()
    {
        playerController = GameObject.Find("player1").GetComponent<PlayerControl>();
        selectionManager = GameObject.Find("ChoosePanel_Manager").GetComponent<SelectionManager>();
        light2d = GetComponentsInChildren<Light2D>();
        init_lightIntensity = light2d[0].intensity;
        targetLightIntensity = init_lightIntensity;
    }

    private void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            light2d[i].intensity = Mathf.Lerp(light2d[i].intensity, targetLightIntensity, Time.deltaTime);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            float randomValue = Random.Range(0f, 1f);
            
            if (randomValue <= 0.8f) // 80% 機率
            {
                playerController.TakeDamage(20);
            }
            else // 20% 機率 
            {
                selectionManager.OpenPanel();
            }
            for (int i = 0; i < 2; i++)
            {
                targetLightIntensity = init_lightIntensity * 0.2f;
            }
            GetComponent<Collider2D>().enabled = false;
        }
    }

}
