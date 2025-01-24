using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class InventoryGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Grid Config")]
    public Vector2Int gridSize = new(5, 5);
    public RectTransform rectTransform;
    public Item[,] items { get; set; }
    public Tetris inventory { get; private set; }

    private void Awake()
    {
        if (rectTransform != null)
        {
            inventory = GameObject.FindObjectOfType<Tetris>();
            InitializeGrid();
            inventory.RegisterGrid(this);
        }
        else
        {
            Debug.LogError("(InventoryGrid) RectTransform not found!");
        }
    }

    private void OnDestroy() {
       inventory.UnregisterGrid(this);
    }

    private void InitializeGrid()
    {
        items = new Item[gridSize.x, gridSize.y];

        Vector2 size =
            new(
                gridSize.x * InventorySettings.slotSize.x,
                gridSize.y * InventorySettings.slotSize.y
            );
        rectTransform.sizeDelta = size;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventory.gridOnMouse = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(inventory.gridOnMouse == this)
        {
            inventory.gridOnMouse = null;
        }
    }
}