using UnityEngine;

public abstract class InventoryPanel : MonoBehaviour
{
    [Header("Panel Config")]
    public GameObject panel;

    public virtual void Show()
    {
        panel.SetActive(true);
    }

    public virtual void Hide()
    {
        panel.SetActive(false);
    }
}