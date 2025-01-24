using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject shopPanel;
    private InteractionPrompt interactionPromptSystem;
    private bool isPlayerInRange;

     void Start()
    {
      interactionPromptSystem = GetComponent<InteractionPrompt>();

       if(interactionPromptSystem != null && shopPanel != null)
        {
          shopPanel.SetActive(false);
        }
    }
    void Update()
    {
        CheckPlayerInRange();
        HandleInput();
    }
     void CheckPlayerInRange()
    {
        if(interactionPromptSystem != null)
        {
            isPlayerInRange = interactionPromptSystem.isPlayerInRange;
        }
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
        else if(shopPanel != null) // 如果玩家離開範圍，則關閉商店介面
        {
           shopPanel.SetActive(false);
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