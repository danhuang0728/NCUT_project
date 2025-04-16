using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    private Weapon_Choose_Manager weapon_choose_manager;
    void Start()
    {
        weapon_choose_manager = GameObject.FindObjectOfType<Weapon_Choose_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")&&Input.GetKeyDown(KeyCode.E))
        {
            weapon_choose_manager.OpenPanel();
            Destroy(gameObject);
        }
    }
}
