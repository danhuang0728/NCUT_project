using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject shopPanel;
    public float interactionRange = 3f;
    private bool isPlayerInRange;

    void Update()
    {
        CheckPlayerInRange();
        HandleInput();
    }

    void CheckPlayerInRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                isPlayerInRange = true;
                return;
            }
        }

        isPlayerInRange = false;
    }
    
    void HandleInput()
    {
        if (isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleShopPanel();
            }
        }
    }

    void ToggleShopPanel()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("商店介面未綁定");
        }
    }
}