using UnityEngine;

public class FlyingAxeTriggerRange : MonoBehaviour
{
    public Transform player_t;
    public FlyingAxeController axeController;

    void Start()
    {
        axeController.GetComponent<FlyingAxeController>();
    }

    void Update()
    {
        transform.position = player_t.transform.position;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            axeController.Is_in_range = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            axeController.Is_in_range = false;
        }
    }
} 