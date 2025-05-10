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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")&&Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.Instance.PlaySFX("open_box");
            weapon_choose_manager.OpenPanel();
            Destroy(gameObject);
        }
    }
}
