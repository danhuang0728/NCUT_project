using System.Collections.Generic;
using UnityEngine;

public class Tetris : MonoBehaviour
{
    [Header("Settings")]
    public TetrisItemData[] itemsData; // Changed to TetrisItemData
    public Item itemPrefab;
    public List<InventoryGrid> grids { get; private set; } = new List<InventoryGrid>();
    public Item selectedItem { get; private set; }
    public InventoryGrid gridOnMouse { get; set; }

    [Header("Panel References")]
    public TetrisPlacementPanel tetrisPlacementPanel;
    public TetrisStoragePanel tetrisStoragePanel;


    private void Awake()
    {
    }

    public void RegisterGrid(InventoryGrid grid)
    {
        grids.Add(grid);
    }

    public void UnregisterGrid(InventoryGrid grid)
    {
        grids.Remove(grid);
    }

    public void SelectItem(Item item)
    {
        ClearItemReferences(item);
        selectedItem = item;
        selectedItem.rectTransform.SetParent(transform);
        selectedItem.rectTransform.SetAsLastSibling();
    }

    private void DeselectItem()
    {
        selectedItem = null;
    }

     public void AddItem(TetrisItemData itemData, InventoryGrid targetGrid) // Changed to TetrisItemData
    {
       if(!AddItemToGrid(targetGrid,itemData))
       {
            Debug.Log("(Inventory) Not enough slots found to add the item!");
       }
    }

       private bool AddItemToGrid(InventoryGrid grid, TetrisItemData itemData) // Changed to TetrisItemData
    {
        for (int y = 0; y < grid.gridSize.y; y++)
        {
            for (int x = 0; x < grid.gridSize.x; x++)
            {
                Vector2Int slotPosition = new Vector2Int(x, y);

                for (int r = 0; r < 2; r++)
                {
                    if (r == 0)
                    {
                        if (!ExistsItem(slotPosition, grid, itemData.size.width, itemData.size.height))
                        {
                            CreateAndPlaceItem(grid, itemData, slotPosition, false);
                            return true;
                        }
                    }

                    if (r == 1)
                    {
                        if (!ExistsItem(slotPosition, grid, itemData.size.height, itemData.size.width))
                        {
                            CreateAndPlaceItem(grid, itemData, slotPosition, true);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private void CreateAndPlaceItem(InventoryGrid grid, TetrisItemData itemData, Vector2Int slotPosition, bool rotated) // Changed to TetrisItemData
    {
        Item newItem = Instantiate(itemPrefab);
        newItem.rectTransform = newItem.GetComponent<RectTransform>();
        newItem.rectTransform.SetParent(grid.rectTransform);
        newItem.rectTransform.sizeDelta = new Vector2(
            itemData.size.width * InventorySettings.slotSize.x,
            itemData.size.height * InventorySettings.slotSize.y
        );
        if(rotated)
        {
            newItem.Rotate();
        }

        newItem.indexPosition = slotPosition;
        newItem.inventory = this;


        SizeInt itemSize = new(rotated ? itemData.size.height : itemData.size.width,rotated ? itemData.size.width : itemData.size.height);


         for (int xx = 0; xx < itemSize.width; xx++)
        {
            for (int yy = 0; yy < itemSize.height; yy++)
            {
                int slotX = slotPosition.x + xx;
                int slotY = slotPosition.y + yy;

                grid.items[slotX, slotY] = newItem;
                grid.items[slotX, slotY].data = itemData; // Changed to itemData
            }
        }

        newItem.rectTransform.localPosition = IndexToInventoryPosition(newItem);
        newItem.inventoryGrid = grid;

    }

    public void RemoveItem(Item item)
    {
        if (item != null)
        {
            ClearItemReferences(item);
            Destroy(item.gameObject);
        }
    }

    public void MoveItem(Item item, bool deselectItemInEnd = true)
    {
        Vector2Int slotPosition = GetSlotAtMouseCoords();

        if (ReachedBoundary(slotPosition, gridOnMouse, item.correctedSize.width, item.correctedSize.height))
        {
            return;
        }

        if (ExistsItem(slotPosition, gridOnMouse, item.correctedSize.width, item.correctedSize.height))
        {
            return;
        }

        item.indexPosition = slotPosition;
        item.rectTransform.SetParent(gridOnMouse.rectTransform);

        for (int x = 0; x < item.correctedSize.width; x++)
        {
            for (int y = 0; y < item.correctedSize.height; y++)
            {
                int slotX = item.indexPosition.x + x;
                int slotY = item.indexPosition.y + y;

                gridOnMouse.items[slotX, slotY] = item;
            }
        }

        item.rectTransform.localPosition = IndexToInventoryPosition(item);
        item.inventoryGrid = gridOnMouse;

        if (deselectItemInEnd)
        {
            DeselectItem();
        }
    }

    public void SwapItem(Item overlapItem, Item oldSelectedItem)
    {
        if (ReachedBoundary(overlapItem.indexPosition, gridOnMouse, oldSelectedItem.correctedSize.width, oldSelectedItem.correctedSize.height))
        {
            return;
        }

        ClearItemReferences(overlapItem);

        if (ExistsItem(overlapItem.indexPosition, gridOnMouse, oldSelectedItem.correctedSize.width, oldSelectedItem.correctedSize.height))
        {
            RevertItemReferences(overlapItem);
            return;
        }

        SelectItem(overlapItem);
        MoveItem(oldSelectedItem, false);
    }

    public void ClearItemReferences(Item item)
    {
        if(item.inventoryGrid == null) return;
        for (int x = 0; x < item.correctedSize.width; x++)
        {
            for (int y = 0; y < item.correctedSize.height; y++)
            {
                int slotX = item.indexPosition.x + x;
                int slotY = item.indexPosition.y + y;

                if(slotX >= 0 && slotX < item.inventoryGrid.items.GetLength(0) && slotY >= 0 && slotY < item.inventoryGrid.items.GetLength(1))
                    item.inventoryGrid.items[slotX, slotY] = null;
            }
        }
    }

    public void RevertItemReferences(Item item)
    {
        for (int x = 0; x < item.correctedSize.width; x++)
        {
            for (int y = 0; y < item.correctedSize.height; y++)
            {
                int slotX = item.indexPosition.x + x;
                int slotY = item.indexPosition.y + y;

                if(slotX >= 0 && slotX < item.inventoryGrid.items.GetLength(0) && slotY >= 0 && slotY < item.inventoryGrid.items.GetLength(1))
                     item.inventoryGrid.items[slotX, slotY] = item;
            }
        }
    }

    public bool ExistsItem(Vector2Int slotPosition, InventoryGrid grid, int width = 1, int height = 1)
    {
         if (ReachedBoundary(slotPosition, grid, width, height))
        {
            return true;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int slotX = slotPosition.x + x;
                int slotY = slotPosition.y + y;

                if(slotX >= 0 && slotX < grid.items.GetLength(0) && slotY >= 0 && slotY < grid.items.GetLength(1))
                {
                    if (grid.items[slotX, slotY] != null)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

     public bool ReachedBoundary(Vector2Int slotPosition, InventoryGrid gridReference, int width = 1, int height = 1)
    {
       if (gridReference == null) return true;
        if (slotPosition.x + width > gridReference.gridSize.x || slotPosition.x < 0)
        {
            return true;
        }

        if (slotPosition.y + height > gridReference.gridSize.y || slotPosition.y < 0)
        {
            return true;
        }

        return false;
    }

    public Vector3 IndexToInventoryPosition(Item item)
    {
        Vector3 inventorizedPosition =
            new()
            {
                x = item.indexPosition.x * InventorySettings.slotSize.x
                    + InventorySettings.slotSize.x * item.correctedSize.width / 2,
                y = -(item.indexPosition.y * InventorySettings.slotSize.y
                    + InventorySettings.slotSize.y * item.correctedSize.height / 2
                )
            };

        return inventorizedPosition;
    }

    public Vector2Int GetSlotAtMouseCoords()
    {
        if (gridOnMouse == null)
        {
            return Vector2Int.zero;
        }

        Vector2 gridPosition =
            new(
                Input.mousePosition.x - gridOnMouse.rectTransform.position.x,
                gridOnMouse.rectTransform.position.y - Input.mousePosition.y
            );

        Vector2Int slotPosition =
            new(
                (int)(gridPosition.x / (InventorySettings.slotSize.x * InventorySettings.slotScale)),
                (int)(gridPosition.y / (InventorySettings.slotSize.y * InventorySettings.slotScale))
            );

        return slotPosition;
    }

    public Item GetItemAtMouseCoords()
    {
        Vector2Int slotPosition = GetSlotAtMouseCoords();

        if (!ReachedBoundary(slotPosition, gridOnMouse))
        {
            return GetItemFromSlotPosition(slotPosition);
        }

        return null;
    }

    public Item GetItemFromSlotPosition(Vector2Int slotPosition)
    {
        if(gridOnMouse != null && slotPosition.x >= 0 && slotPosition.x < gridOnMouse.items.GetLength(0) && slotPosition.y >= 0 && slotPosition.y < gridOnMouse.items.GetLength(1))
        {
             return gridOnMouse.items[slotPosition.x, slotPosition.y];
        }
         return null;
    }
}