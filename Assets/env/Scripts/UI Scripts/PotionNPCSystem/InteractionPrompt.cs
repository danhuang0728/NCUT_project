using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionPrompt : MonoBehaviour
{
    public float interactionRange = 3f;
    public GameObject interactionPromptObject;
    public string interactionText = "按 [E] 互動";
    public Sprite interactionImage;
    public bool useTextPrompt = true;
    public bool useImagePrompt = false;
    public Vector3 offset = new Vector3(0, 2, 0);

    private TextMeshProUGUI textComponent;
    private Image imageComponent;
    private Camera mainCamera;
    public bool isPlayerInRange;

    void Start()
    {
        if (interactionPromptObject == null) return;

        if (useTextPrompt)
            textComponent = interactionPromptObject.GetComponentInChildren<TextMeshProUGUI>();
        if (useImagePrompt)
            imageComponent = interactionPromptObject.GetComponentInChildren<Image>();

        interactionPromptObject.SetActive(false);
        mainCamera = Camera.main;
       Debug.Log("mainCamera: " + mainCamera);
    }

    void Update()
    {
        CheckPlayerInRange();
        UpdatePromptDisplay();
    }

    void CheckPlayerInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRange);
        isPlayerInRange = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                isPlayerInRange = true;
                break;
            }
        }
    }

    void UpdatePromptDisplay()
    {
        if (interactionPromptObject == null) return;

        interactionPromptObject.SetActive(isPlayerInRange);

        if (isPlayerInRange && interactionPromptObject.activeSelf)
        {
             interactionPromptObject.transform.position = mainCamera.WorldToScreenPoint(transform.position + offset);


            if (useTextPrompt)
            {
                if (textComponent != null)
                    textComponent.text = interactionText;
                else
                    Debug.LogWarning("沒有找到 TextMeshProUGUI 物件，請確認你的 UI 物件內有 TextMeshProUGUI 組件");
            }
            if (useImagePrompt)
            {
                if (imageComponent != null && interactionImage != null)
                    imageComponent.sprite = interactionImage;
                else
                    Debug.LogWarning("沒有找到 Image 物件 或 Image沒有圖片，請確認你的 UI 物件內有 Image 組件和設定圖片");
            }
        }
    }

     void OnGUI()
     {
         if (isPlayerInRange)
        {
            // 將世界座標轉換成螢幕座標
            Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position + offset);
             // 繪製一個文字在畫面上
           GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 200, 20), interactionText);

        }
     }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
          Gizmos.color = Color.blue;
          Gizmos.DrawSphere(transform.position + offset, 0.2f);
    }
}