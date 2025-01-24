using UnityEngine;

[RequireComponent(typeof(Tetris))]
public class InventoryController : MonoBehaviour
{
    public Tetris inventory { get; private set; }

    private void Awake()
    {
        inventory = GetComponent<Tetris>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(inventory.gridOnMouse != null)
            {
                if (inventory.selectedItem)
                {
                    Item oldSelectedItem = inventory.selectedItem;
                    Item overlapItem = inventory.GetItemAtMouseCoords();

                    if (overlapItem != null)
                    {
                        inventory.SwapItem(overlapItem, oldSelectedItem);
                    }
                    else
                    {
                        inventory.MoveItem(oldSelectedItem);
                    }
                }
                else
                {
                    SelectItemWithMouse();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RemoveItemWithMouse();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Example: Add item to the Tetris placement grid
            if(inventory.tetrisPlacementPanel && inventory.tetrisPlacementPanel.inventoryGrid)
            {
                inventory.AddItem(inventory.itemsData[UnityEngine.Random.Range(0, inventory.itemsData.Length)], inventory.tetrisPlacementPanel.inventoryGrid);
            }
             if(inventory.tetrisStoragePanel && inventory.tetrisStoragePanel.inventoryGrid)
            {
                inventory.AddItem(inventory.itemsData[UnityEngine.Random.Range(0, inventory.itemsData.Length)], inventory.tetrisStoragePanel.inventoryGrid);
            }
        }

        if (inventory.selectedItem != null)
        {
            MoveSelectedItemToMouse();

            if (Input.GetKeyDown(KeyCode.R))
            {
                inventory.selectedItem.Rotate();
            }
        }
    }

    private void SelectItemWithMouse()
    {
        Item item = inventory.GetItemAtMouseCoords();

        if (item != null)
        {
            inventory.SelectItem(item);
        }
    }

    private void RemoveItemWithMouse()
    {
        Item item = inventory.GetItemAtMouseCoords();

        if (item != null)
        {
            inventory.RemoveItem(item);
        }
    }

    private void MoveSelectedItemToMouse()
    {
        inventory.selectedItem.rectTransform.position = new Vector3(
                Input.mousePosition.x
                    + ((inventory.selectedItem.correctedSize.width * InventorySettings.slotSize.x) / 2)
                    - InventorySettings.slotSize.x / 2,
                Input.mousePosition.y
                    - ((inventory.selectedItem.correctedSize.height * InventorySettings.slotSize.y) / 2)
                    + InventorySettings.slotSize.y / 2,
                Input.mousePosition.z
            );
    }
}