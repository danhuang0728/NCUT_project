using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 管理整個 Tetris 物品欄的腳本
public class TetrisInventoryManager : MonoBehaviour
{
    [Header("UI 設定")]
    public GameObject inventoryPanel;           // 整個物品欄面板
    public Transform equippedGridContainer;     // 左側裝備格子的容器
    public Transform storageGridContainer;      // 右側存儲格子的容器
    public Transform storageGridContainer_2;   // 右側存儲格子的容器第2頁
    public Transform storageGridContainer_3;   // 右側存儲格子的容器第3頁
    public GameObject gridCellPrefab;           // 網格單元格預置體
    public GameObject tetrisPiecePrefab;        // Tetris 方塊預置體
    public Sprite blockSprite;  // 新增：方塊的圖片
    
    [Header("網格設定")]
    public int equippedGridSize = 8;        // 左側裝備網格大小 (8x8)
    public int storageGridSize = 20;        // 右側存儲網格大小 (20x20)
    public float cellSize = 50f;            // 單元格大小
    
    [Header("控制設定")]
    public KeyCode toggleKey = KeyCode.B;   // 開關物品欄的按鍵
    
    // 修改為公開變量，以便其他類可以訪問
    [HideInInspector] public GridCell[,] equippedGrid;  // 左側裝備網格
    [HideInInspector] public GridCell[,] storageGrid;   // 右側存儲網格
    [HideInInspector] public GridCell[,] storageGrid_2;  // 右側存儲網格第二頁
    [HideInInspector] public GridCell[,] storageGrid_3;  // 右側存儲網格第三頁
    
    private bool isInventoryOpen = false;
    private List<TetrisPiece> tetrisPieces = new List<TetrisPiece>();
    private TetrisPiece currentDraggedPiece;
    private TetrisPiece selectedPiece;
    
    // 新增：Canvas引用
    private Canvas inventoryCanvas;
    private tetrisPanel_ TetrisPanel_;
    
    void Start()
    {
        // 初始化物品欄
        inventoryPanel.SetActive(false);
        InitializeGrids();
        CreateSampleTetrisPieces();
        Debug.Log(tetrisPieces[0].GetWorldPositions(0,0));
        TetrisPanel_ = GetComponent<tetrisPanel_>();
        // 新增：獲取Canvas並設置為使用非縮放時間
        SetupCanvasForPauseIndependence();
    }
    
    // 新增：設置Canvas以不受時間暫停影響
    private void SetupCanvasForPauseIndependence()
    {
        // 嘗試從當前物件或父物件獲取Canvas組件
        inventoryCanvas = GetComponentInParent<Canvas>();
        if (inventoryCanvas == null)
        {
            // 如果在父物件中沒有找到，嘗試從inventoryPanel獲取
            if (inventoryPanel != null)
            {
                inventoryCanvas = inventoryPanel.GetComponentInParent<Canvas>();
            }
        }
        
        // 如果找到Canvas，設置其更新模式為不受縮放影響
        if (inventoryCanvas != null)
        {
            inventoryCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            // 獲取或添加CanvasScaler組件
            CanvasScaler scaler = inventoryCanvas.GetComponent<CanvasScaler>();
            if (scaler == null)
            {
                scaler = inventoryCanvas.gameObject.AddComponent<CanvasScaler>();
            }
            
            // 設置為不受時間影響
            scaler.scaleFactor = scaler.scaleFactor; // 保持當前縮放比例
            Debug.Log("物品欄Canvas已設置為不受時間縮放影響");
        }
        else
        {
            Debug.LogWarning("無法找到物品欄的Canvas組件，請手動設置Canvas.updateMode為UnscaledTime");
        }
    }
    
    void Update()
    {
        // 使用unscaledTime確保即使在遊戲暫停時也能檢測按鍵輸入
        // 按 B 鍵切換物品欄開關
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }
        
        // 當物品欄打開時處理旋轉功能
        if (isInventoryOpen)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (selectedPiece != null)
                {
                    Debug.Log("正在旋轉方塊");
                    selectedPiece.Rotate();
                }
                else
                {
                    Debug.Log("未選擇方塊，無法旋轉");
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            choosePage(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            choosePage(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            choosePage(3);
        }
    }
    private Vector3 storageGridContainerOriginalPos; // 新增：存儲初始位置
    private Vector3 storageGridContainer2OriginalPos; // 新增：存儲第二容器初始位置
    private Vector3 storageGridContainer3OriginalPos; // 新增：存儲第三容器初始位置

    public void choosePage(int page)
    {
        // 只在第一次調用時記錄初始位置
        if(storageGridContainerOriginalPos == Vector3.zero) {
            storageGridContainerOriginalPos = storageGridContainer.position;
            storageGridContainer2OriginalPos = storageGridContainer_2.position;
            storageGridContainer3OriginalPos = storageGridContainer_3.position;
        }

        if (page == 1)
        {
            // 顯示第一頁
            storageGridContainer.position = storageGridContainerOriginalPos;
            storageGridContainer_2.position = storageGridContainer2OriginalPos;
            storageGridContainer_3.position = storageGridContainer3OriginalPos;
        }
        else if (page == 2)
        {
            // 顯示第二頁
            Vector3 tempPosition = storageGridContainerOriginalPos;
            storageGridContainer.position = storageGridContainer2OriginalPos;
            storageGridContainer_2.position = tempPosition;
            storageGridContainer_3.position = storageGridContainer3OriginalPos;
        }
        else if (page == 3)
        {
            // 顯示第三頁
            Vector3 tempPosition = storageGridContainerOriginalPos;
            storageGridContainer.position = storageGridContainer3OriginalPos;
            storageGridContainer_3.position = tempPosition;
            storageGridContainer_2.position = storageGridContainer2OriginalPos;
        }
    }
    // 初始化兩側的網格
    void InitializeGrids()
    {
        equippedGrid = CreateGrid(equippedGridContainer, "Equipped", equippedGridSize);
        storageGrid = CreateGrid(storageGridContainer, "Storage", storageGridSize);
        storageGrid_2 = CreateGrid(storageGridContainer_2, "Storage2", storageGridSize);
        storageGrid_3 = CreateGrid(storageGridContainer_3, "Storage3", storageGridSize);
    }
    
    // 創建一個網格
    GridCell[,] CreateGrid(Transform container, string prefix, int size)
    {
        GridCell[,] grid = new GridCell[size, size];
        
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                GameObject cellObj = Instantiate(gridCellPrefab, container);
                RectTransform rectTransform = cellObj.GetComponent<RectTransform>();
                
                // 設置位置和大小
                rectTransform.anchoredPosition = new Vector2(x * cellSize, -y * cellSize);
                rectTransform.sizeDelta = new Vector2(cellSize, cellSize);
                
                // 初始化單元格
                GridCell cell = cellObj.AddComponent<GridCell>();
                cell.Initialize(x, y, this);
                cellObj.name = $"{prefix}_Cell_{x}_{y}";
                
                grid[x, y] = cell;
            }
        }
        
        return grid;
    }
    
    // 創建一些示例 Tetris 方塊
    void CreateSampleTetrisPieces()
    {
        CreateTetrisPiece(
            new Vector2Int[] { 
                new Vector2Int(0, 0),
                new Vector2Int(0, 7),
                new Vector2Int(1, 1),
                new Vector2Int(1, 6),
                new Vector2Int(2, 2),
                new Vector2Int(2, 5),
                new Vector2Int(3, 3),
                new Vector2Int(3, 4),
                new Vector2Int(4, 3),
                new Vector2Int(4, 4),
                new Vector2Int(5, 2),
                new Vector2Int(5, 5),
                new Vector2Int(6, 1),
                new Vector2Int(6, 6),
                new Vector2Int(7, 0),
                new Vector2Int(7, 7),
                
            }, 
            Color.yellow,
            new Vector2(0.5f, 0.5f), 
            "(S_1)", //居合斬
            false
        );
        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(4, 0),
                new Vector2Int(3, 1), new Vector2Int(5, 1),
                new Vector2Int(3, 2), new Vector2Int(6, 2),
                new Vector2Int(1, 3), new Vector2Int(7, 3),
                new Vector2Int(0, 4), new Vector2Int(6, 4),
                new Vector2Int(1, 5), new Vector2Int(4, 5),
                new Vector2Int(2, 6), new Vector2Int(4, 6),
                new Vector2Int(3, 7)
            },
            Color.yellow,
            new Vector2(3.5f, 3.5f), // 中心點可依需要微調
            "(S_2)", //亂砍
            false
        );
        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0),
                new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(3, 1),
                new Vector2Int(0, 2), new Vector2Int(1, 2),
                new Vector2Int(0, 3), new Vector2Int(1, 3)
            },
            Color.yellow,
            new Vector2(1.5f, 1.5f), // 中心點可依實際旋轉需求調整
            "(A_1)" //飛斧
        );
        CreateTetrisPiece(
            new Vector2Int[] {
                // 上邊界
                new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0),
                new Vector2Int(3, 0), new Vector2Int(4, 0), new Vector2Int(5, 0),
                
                // 下邊界
                new Vector2Int(0, 5), new Vector2Int(1, 5), new Vector2Int(2, 5),
                new Vector2Int(3, 5), new Vector2Int(4, 5), new Vector2Int(5, 5),

                // 左邊界
                new Vector2Int(0, 1), new Vector2Int(0, 2),
                new Vector2Int(0, 3), new Vector2Int(0, 4),

                // 右邊界
                new Vector2Int(5, 1), new Vector2Int(5, 2),
                new Vector2Int(5, 3), new Vector2Int(5, 4)
            },
            new Color(0.5f, 0f, 1f), // 紫色（可依需求微調）
            new Vector2(0,0), // 中心點正中央
            "(C_1)", //圓環
            false
        );
        CreateTetrisPiece(
            new Vector2Int[] { 
                // 第一行 (頂部)
                new Vector2Int(0, 0), new Vector2Int(1, 0), 
                // 第二行
                new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1),
                // 第三行
                new Vector2Int(2, 2),
                // 第四行
                new Vector2Int(1, 3),
                // 第五行
                new Vector2Int(0, 4),
                // 第六行 (底部)
                new Vector2Int(1, 5),
                new Vector2Int(2, 6)
            }, 
            Color.cyan,
            new Vector2(1.0f, 2.5f),  // 設置中心點在形狀的幾何中心附近
            "F_1" //跟蹤火球
        );
        CreateTetrisPiece(
            new Vector2Int[] { 
                // 從上到下
                new Vector2Int(5, 0), 
                new Vector2Int(4, 1), new Vector2Int(5, 1),new Vector2Int(6, 1),
                new Vector2Int(3, 2), new Vector2Int(4, 2),new Vector2Int(5, 2), new Vector2Int(6, 2),new Vector2Int(7, 2),
                new Vector2Int(2, 3), new Vector2Int(3, 3),new Vector2Int(4, 3), new Vector2Int(5, 3),new Vector2Int(6, 3),new Vector2Int(7, 3),
                new Vector2Int(1, 4), new Vector2Int(2, 4),new Vector2Int(5, 4), new Vector2Int(6, 4),
                new Vector2Int(0, 5), new Vector2Int(1, 5),
                new Vector2Int(0, 6), 
            }, 
            new Color(0.5f, 0f, 1f), // 紫色
            new Vector2(3.5f, 3.0f), // 設置中心點在形狀的大致中心
            "A_2" //巨斧
        );
        CreateTetrisPiece(
            new Vector2Int[] {

                new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0), new Vector2Int(4, 0),
                new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(3, 1), new Vector2Int(4, 1), new Vector2Int(5, 1),
                new Vector2Int(0, 2), new Vector2Int(5, 2),
                new Vector2Int(0, 3), new Vector2Int(1, 3),new Vector2Int(2, 3),new Vector2Int(3, 3),new Vector2Int(4, 3),new Vector2Int(5, 3),
                new Vector2Int(1, 4), new Vector2Int(2, 4),new Vector2Int(3, 4), new Vector2Int(4, 4)
            }, 
            new Color(0.5f, 0f, 1f), // 紫色
            new Vector2(3,3), // 設置中心點在形狀的幾何中心
            "F_2", //單顆大火球
            false // 設置為不可旋轉
        );
        CreateTetrisPiece(
            new Vector2Int[] {
                // 水平線段 (y=1)
                new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), 
                new Vector2Int(3, 0), new Vector2Int(4, 0),
                
                // 垂直線段 (x=6)
                new Vector2Int(5, 1), new Vector2Int(5, 2), new Vector2Int(5, 3), 
                new Vector2Int(5, 4), new Vector2Int(5, 5)
            }, 
            Color.blue,  // 深藍色
            new Vector2(3, 1),  // 中心點設在整個形狀的大致中心
            "B_1" //迴力鏢無限彈
        );
        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(2, 0), new Vector2Int(3, 0),
                new Vector2Int(2, 1), new Vector2Int(3, 1), 
                new Vector2Int(2, 2), new Vector2Int(3, 2),
                new Vector2Int(2, 3), new Vector2Int(3, 3),
                new Vector2Int(2, 4), new Vector2Int(3, 4),
                new Vector2Int(2, 5), new Vector2Int(3, 5),

                new Vector2Int(0, 3), new Vector2Int(1, 4),
                new Vector2Int(4, 4), new Vector2Int(5, 3),
                
            },
            new Color(0.5f, 0f, 1f), // 紫色
            new Vector2(1f, 1f), // 設置中心點在形狀的幾何中心
            "T_1", //西洋劍全場下戳
            false //不旋轉
        );
        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(0, 0)
            },
            new Color(1f, 0.7f, 0.85f), // 粉色
            new Vector2(1, 1), // 設置中心點在形狀的幾何中心
            "Pink_1",//粉色組件1
            false  //不旋轉
        );

        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(0, 0), new Vector2Int(1, 0),new Vector2Int(2, 0), new Vector2Int(3, 0),
                new Vector2Int(1, 1), new Vector2Int(2, 1),
            },
            new Color(1f, 0.7f, 0.85f), // 粉色
            new Vector2(1, 1), // 設置中心點在形狀的幾何中心
            "Pink_2" //粉色組件2
        );

        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(0, 0), new Vector2Int(1, 0),new Vector2Int(2, 0), 
                new Vector2Int(0, 1), new Vector2Int(2, 1),
                new Vector2Int(0, 2), new Vector2Int(1, 2),new Vector2Int(2, 2),
            },
            new Color(1f, 0.7f, 0.85f), // 粉色
            new Vector2(1, 1), // 設置中心點在形狀的幾何中心
            "Pink_3",//粉色組件3
            false  //不旋轉
        );

        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(0, 3),
                new Vector2Int(0, 4),
                new Vector2Int(0, 5),
                new Vector2Int(0, 6),
                new Vector2Int(0, 7)
            },
            new Color(1f, 0.7f, 0.85f), // 粉色
            new Vector2(1, 1), // 設置中心點在形狀的幾何中心
            "Pink_4"//粉色組件4
        );

        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),
                new Vector2Int(1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(1, 2),   
                new Vector2Int(2,0),new Vector2Int(3, 0),

            },
            new Color(1f, 0.7f, 0.85f), // 粉色
            new Vector2(1, 1), // 設置中心點在形狀的幾何中心
            "Pink_5"//粉色組件5
        );
        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(2, 0),
                new Vector2Int(1, 1),new Vector2Int(3, 1),
                new Vector2Int(0, 2),new Vector2Int(2, 2),new Vector2Int(4, 2),
                new Vector2Int(0, 3),new Vector2Int(2, 3),new Vector2Int(4, 3),
                new Vector2Int(1, 4),new Vector2Int(3, 4),
                new Vector2Int(2, 5),   
            },
            new Color(1f, 0.7f, 0.85f), // 粉色
            new Vector2(3, 2), // 設置中心點在形狀的幾何中心
            "Pink_6"//粉色組件6
        );

        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(1, 0),
                new Vector2Int(0, 1),new Vector2Int(0, 2),
                new Vector2Int(2, 1),new Vector2Int(2, 2),
                new Vector2Int(1, 3)
            },
            new Color(1f, 0.7f, 0.85f), // 粉色
            new Vector2(3, 2), // 設置中心點在形狀的幾何中心
            "Pink_7"//粉色組件7
        );

        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),new Vector2Int(1, 1),new Vector2Int(2, 1),
                new Vector2Int(2, 2),
                new Vector2Int(2, 3)
            },
            new Color(0.5f, 1f, 0.5f), // 亮綠色
            new Vector2(2, 2), // 設置中心點在形狀的幾何中心
            "Green_1"//綠色組件1
        );

        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, 2),new Vector2Int(1, 2),new Vector2Int(2, 2),
                new Vector2Int(0, 3),new Vector2Int(1, 3),new Vector2Int(2, 3)
            },
            new Color(0.5f, 1f, 0.5f), // 亮綠色
            new Vector2(2, 2), // 設置中心點在形狀的幾何中心
            "Green_2",//綠色組件2
            false //不旋轉
        );

        CreateTetrisPiece(
            new Vector2Int[] {
                new Vector2Int(0, 0),new Vector2Int(1, 0),new Vector2Int(2, 0),
                new Vector2Int(0, 1),new Vector2Int(1, 1),new Vector2Int(2, 1),
                new Vector2Int(2, 2),
                new Vector2Int(2, 3)
            },
            new Color(0.5f, 1f, 0.5f), // 亮綠色
            new Vector2(2, 2), // 設置中心點在形狀的幾何中心
            "Green_3",//綠色組件3
            false //不旋轉
        );

                
    }
    
    // 創建一個 Tetris 方塊
    void CreateTetrisPiece(Vector2Int[] shape, Color color, Vector2 Pivot = default, string pieceName = "TetrisPiece", bool canRotate = true)
    {
        GameObject pieceObj = Instantiate(tetrisPiecePrefab, storageGridContainer);
        pieceObj.name = pieceName;
        //pieceObj.GetComponent<RectTransform>().pivot = Pivot;
        TetrisPiece piece = pieceObj.AddComponent<TetrisPiece>();
        
        // 計算自動中心點 (如果沒有指定)
        if(Pivot == default) {
            Vector2 sum = Vector2.zero;
            foreach(var pos in shape) {
                sum += new Vector2(pos.x, pos.y);
            }
            Pivot = sum / shape.Length;
        }
        
        piece.Initialize(shape, color, cellSize, this, Pivot, canRotate);
        tetrisPieces.Add(piece);
        
        piece.transform.localPosition = Vector2.zero;
        TryAutoPlaceInStorage(piece);
    }
    
    // 新增自動放置方法
    private void TryAutoPlaceInStorage(TetrisPiece piece)
    {
        // 先嘗試放在第一個存儲容器
        if (TryPlaceInSpecificStorage(piece, storageGrid, storageGridContainer))
        {
            return;
        }
        
        // 如果第一個容器放不下，嘗試放在第二個容器
        if (TryPlaceInSpecificStorage(piece, storageGrid_2, storageGridContainer_2))
        {
            return;
        }
        
        // 如果第二個容器放不下，嘗試放在第三個容器
        if (TryPlaceInSpecificStorage(piece, storageGrid_3, storageGridContainer_3))
        {
            return;
        }
        
        Debug.LogWarning("無法在任何存儲容器中放置物品：" + piece.name);
    }
    
    // 新增：嘗試在指定的存儲容器中放置物品
    private bool TryPlaceInSpecificStorage(TetrisPiece piece, GridCell[,] targetGrid, Transform container)
    {
        // 從右下角開始掃描以容納較大物件
        for (int y = storageGridSize-1; y >= 0; y--)
        {
            for (int x = storageGridSize-1; x >= 0; x--)
            {
                GridCell cell = targetGrid[x, y];
                if (CanPlacePiece(piece, cell, targetGrid))
                {
                    piece.transform.SetParent(container);
                    piece.transform.localPosition = new Vector3(
                        (cell.X+1) * cellSize,
                        -cell.Y * cellSize,
                        0
                    );
                    PlacePiece(piece, cell, targetGrid);
                    return true;
                }
            }
        }
        return false;
    }
    
    // 切換物品欄開關
    public void ToggleInventory()
    {
        if (isInventoryOpen) {
            TetrisPanel_.CloseInventory();
        } else {
            TetrisPanel_.OpenInventory();
        }
        
        isInventoryOpen = TetrisPanel_.isInventoryOpen;
        
        // 當關閉物品欄時，取消選擇
        if (!isInventoryOpen && selectedPiece != null)
        {
            selectedPiece.SetSelected(false);
            selectedPiece = null;
        }
        
        // 新增：當打開物品欄時，確保它顯示在最上層
        if (isInventoryOpen && inventoryCanvas != null)
        {
            inventoryCanvas.sortingOrder = 100;
        }
    }
    
    // 設置當前被拖拽的方塊
    public void SetDraggedPiece(TetrisPiece piece)
    {
        currentDraggedPiece = piece;
    }
    
    // 獲取當前被拖拽的方塊
    public TetrisPiece GetDraggedPiece()
    {
        return currentDraggedPiece;
    }
    
    // 選擇方塊
    public void SelectPiece(TetrisPiece piece)
    {
        // 如果之前有選擇其他方塊，取消選擇
        if (selectedPiece != null && selectedPiece != piece)
        {
            selectedPiece.SetSelected(false);
        }
        
        // 更新選擇
        selectedPiece = piece;
    }
    
    // 檢查方塊是否可以放置在指定位置
    public bool CanPlacePiece(TetrisPiece piece, GridCell startCell, GridCell[,] targetGrid)
    {
        Vector2Int[] worldPositions = piece.GetWorldPositions(startCell.X, startCell.Y);
        int gridSize = (targetGrid == equippedGrid) ? equippedGridSize : storageGridSize;
        
        foreach (Vector2Int pos in worldPositions)
        {
            // 添加偵錯日誌
            Debug.Log($"嘗試放置在位置: ({pos.x}, {pos.y})");
            
            // 檢查是否超出網格範圍
            if (pos.x < 0 || pos.x >= gridSize || pos.y < 0 || pos.y >= gridSize)
            {
                Debug.Log($"超出邊界: ({pos.x}, {pos.y})");
                return false;
            }
            
            // 檢查目標位置是否已被其他方塊佔用
            // 只有當這個位置確實是新方塊的一部分，且被其他方塊佔用時才返回 false
            if (targetGrid[pos.x, pos.y].IsOccupied)
            {
                TetrisPiece occupyingPiece = targetGrid[pos.x, pos.y].OccupyingPiece;
                if (occupyingPiece != piece)
                {
                    Debug.Log($"位置已被其他方塊佔用: ({pos.x}, {pos.y})");
                    return false;
                }
            }
        }
        
        return true;
    }
    
    // 放置方塊到網格
    public void PlacePiece(TetrisPiece piece, GridCell startCell, GridCell[,] targetGrid)
    {
        // 先清除之前的佔用狀態
        piece.ClearOccupiedCells();
        
        // 獲取世界坐標下的所有方塊位置
        Vector2Int[] worldPositions = piece.GetWorldPositions(startCell.X, startCell.Y);
        
        // 設置新的佔用狀態
        foreach (Vector2Int pos in worldPositions)
        {
            targetGrid[pos.x, pos.y].SetOccupied(piece);
            piece.AddOccupiedCell(targetGrid[pos.x, pos.y]);
        }
        
        // 更新方塊位置，向右偏移1個單位修正整體坐標系
        piece.transform.SetParent(startCell.transform.parent);
        piece.transform.localPosition = new Vector3(
            (startCell.X+1) * cellSize,
            -startCell.Y * cellSize,
            0);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        // 繪製裝備網格線
        Gizmos.color = Color.white;
        for (int y = 0; y < equippedGridSize; y++)
        {
            for (int x = 0; x < equippedGridSize; x++)
            {
                Vector3 pos = new Vector3(
                    x * cellSize, 
                    -y * cellSize, 
                    0
                );
                Gizmos.DrawWireCube(pos, new Vector3(cellSize, cellSize, 0));
            }
        }
        
        // 繪製存儲網格線
        Gizmos.color = Color.gray;
        for (int y = 0; y < storageGridSize; y++)
        {
            for (int x = 0; x < storageGridSize; x++)
            {
                Vector3 pos = new Vector3(
                    (x + equippedGridSize + 1) * cellSize, 
                    -y * cellSize, 
                    0
                );
                Gizmos.DrawWireCube(pos, new Vector3(cellSize, cellSize, 0));
            }
        }
        
        // 繪製Tetris方塊
        Gizmos.color = Color.yellow;
        foreach (var piece in tetrisPieces)
        {
            if (piece != null)
            {
                Gizmos.DrawWireSphere(piece.transform.position, 5f);
            }
        }
    }

    public GridCell GetCellAtPosition(Vector2 screenPosition, bool isEquippedGrid, int storagePageIndex = 1)
    {
        // 選擇正確的容器
        RectTransform targetContainer;
        if (isEquippedGrid)
        {
            targetContainer = equippedGridContainer as RectTransform;
        }
        else
        {
            switch (storagePageIndex)
            {
                case 1:
                    targetContainer = storageGridContainer as RectTransform;
                    break;
                case 2:
                    targetContainer = storageGridContainer_2 as RectTransform;
                    break;
                case 3:
                    targetContainer = storageGridContainer_3 as RectTransform;
                    break;
                default:
                    targetContainer = storageGridContainer as RectTransform;
                    break;
            }
        }

        // 將屏幕座標轉換為本地座標
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetContainer,
            screenPosition,
            null,
            out Vector2 localPoint
        );

        // 計算網格座標
        int x = Mathf.FloorToInt(localPoint.x / cellSize);
        int y = Mathf.FloorToInt(-localPoint.y / cellSize);

        // 獲取對應的網格
        GridCell[,] targetGrid;
        if (isEquippedGrid)
        {
            targetGrid = equippedGrid;
        }
        else
        {
            switch (storagePageIndex)
            {
                case 1:
                    targetGrid = storageGrid;
                    break;
                case 2:
                    targetGrid = storageGrid_2;
                    break;
                case 3:
                    targetGrid = storageGrid_3;
                    break;
                default:
                    targetGrid = storageGrid;
                    break;
            }
        }
        
        int gridSize = isEquippedGrid ? equippedGridSize : storageGridSize;

        // 檢查座標是否在有效範圍內
        if (x >= 0 && x < gridSize && y >= 0 && y < gridSize)
        {
            return targetGrid[x, y];
        }

        return null;
    }
}

// 網格單元格類
public class GridCell : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsOccupied { get; private set; }
    public TetrisPiece OccupyingPiece { get; private set; }
    
    private TetrisInventoryManager manager;
    private Image image;
    
    public void Initialize(int x, int y, TetrisInventoryManager manager)
    {
        this.X = x;
        this.Y = y;
        this.manager = manager;
        this.image = GetComponent<Image>();
        this.IsOccupied = false;
        
        // 確保網格單元格軸心點在中心
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(
            x * manager.cellSize + manager.cellSize/2, 
            -y * manager.cellSize - manager.cellSize/2
        );
    }
    
    // 設置單元格被佔用
    public void SetOccupied(TetrisPiece piece)
    {
        IsOccupied = piece != null;
        OccupyingPiece = piece;
    }
    
    // 處理點擊事件
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsOccupied && OccupyingPiece != null)
        {
            OccupyingPiece.SetSelected(true);
            manager.SelectPiece(OccupyingPiece);
        }
    }
    
    // 處理拖放事件
    public void OnDrop(PointerEventData eventData)
    {
        TetrisPiece draggedPiece = manager.GetDraggedPiece();
        
        if (draggedPiece != null)
        {
            // 檢查拖拽的方塊是否可以放在此位置
            GridCell[,] targetGrid = transform.parent == manager.equippedGridContainer ? 
                                     manager.equippedGrid : manager.storageGrid;
            
            if (manager.CanPlacePiece(draggedPiece, this, targetGrid))
            {
                manager.PlacePiece(draggedPiece, this, targetGrid);
            }
        }
    }
}

// Tetris 方塊類
public class TetrisPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Vector2Int[] shape;          // 方塊的形狀定義
    private Color color;                 // 方塊顏色
    private float cellSize;              // 單元格大小
    private TetrisInventoryManager manager;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private List<GridCell> occupiedCells = new List<GridCell>();
    private Vector3 startPosition;
    private Transform startParent;
    private bool isSelected = false;
    private bool canRotate = true;  // 新增：控制是否可以旋轉
    
    // 初始化方塊
    public void Initialize(Vector2Int[] shape, Color color, float cellSize, TetrisInventoryManager manager, Vector2 pivot = default, bool canRotate = true)
    {
        this.shape = shape;
        this.color = color;
        this.cellSize = cellSize;
        this.manager = manager;
        this.canRotate = canRotate;  // 新增：設置是否可旋轉
        
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
        // 創建方塊的視覺表示
        CreateVisual();
    }
    
    // 創建方塊的視覺表示
    void CreateVisual()
    {
        // 找出形狀的邊界
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;
        
        foreach (Vector2Int cell in shape)
        {
            minX = Mathf.Min(minX, cell.x);
            minY = Mathf.Min(minY, cell.y);
            maxX = Mathf.Max(maxX, cell.x);
            maxY = Mathf.Max(maxY, cell.y);
        }
        
        // 設置父物件的RectTransform
        rectTransform.anchorMin = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1);
        
        // 設置方塊的大小
        int width = maxX - minX + 1;
        int height = maxY - minY + 1;
        rectTransform.sizeDelta = new Vector2(width * cellSize, height * cellSize);
        
        // 清除現有子物件
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        // 創建每個小方塊
        foreach (Vector2Int cell in shape)
        {
            GameObject blockObj = new GameObject("Block");
            blockObj.transform.SetParent(transform);
            
            // 添加圖像組件
            Image blockImage = blockObj.AddComponent<Image>();
            blockImage.color = color;
            // 設置圖片
            blockImage.sprite = manager.blockSprite;
            
            // 設置位置和大小
            RectTransform blockRect = blockObj.GetComponent<RectTransform>();
            blockRect.anchorMin = new Vector2(1, 1);
            blockRect.anchorMax = new Vector2(1, 1);
            blockRect.pivot = new Vector2(1, 1);
            blockRect.sizeDelta = new Vector2(cellSize * 0.9f, cellSize * 0.9f);
            
            // 計算相對於父物件左上角的偏移量
            float xOffset = (cell.x - minX) * cellSize;
            float yOffset = -(cell.y - minY) * cellSize;
            
            blockRect.anchoredPosition = new Vector2(xOffset, yOffset);
        }
    }
    
    // 旋轉方塊
    public void Rotate()
    {
        // 檢查是否允許旋轉
        if (!canRotate)
        {
            Debug.Log("此方塊不能旋轉");
            return;
        }
        
        // 保存原始形状，以便无法旋转时恢复
        Vector2Int[] oldShape = (Vector2Int[])shape.Clone();
        
        // 创建旋转后的新形状
        Vector2Int[] newShape = new Vector2Int[shape.Length];
        
        for (int i = 0; i < shape.Length; i++)
        {
            // 90度顺时针旋转变换: (x, y) -> (y, -x)
            newShape[i] = new Vector2Int(shape[i].y, -shape[i].x);
        }
        
        // 找出旋转后形状可能的偏移量
        int minX = int.MaxValue;
        int minY = int.MaxValue;
        
        foreach (Vector2Int pos in newShape)
        {
            minX = Mathf.Min(minX, pos.x);
            minY = Mathf.Min(minY, pos.y);
        }
        
        // 如果有负坐标，进行规范化以确保所有坐标非负
        if (minX < 0 || minY < 0)
        {
            for (int i = 0; i < newShape.Length; i++)
            {
                newShape[i] = new Vector2Int(newShape[i].x - minX, newShape[i].y - minY);
            }
        }
        
        // 获取当前位置基准
        if (occupiedCells.Count == 0)
        {
            Debug.LogWarning("无法旋转：方块未放置在网格中");
            return;
        }
        
        // 获取参考单元格和目标网格
        GridCell referenceCell = occupiedCells[0];
        Transform gridParent = referenceCell.transform.parent;
        GridCell[,] targetGrid = gridParent == manager.equippedGridContainer ? 
                               manager.equippedGrid : manager.storageGrid;
        
        // 尝试应用旋转
        shape = newShape;
        
        // 尝试多个可能的放置点
        bool canPlace = false;
        GridCell bestReferenceCell = referenceCell;
        
        // 获取原始参考单元格周围的单元格作为候选放置点
        List<GridCell> candidateCells = new List<GridCell>();
        candidateCells.Add(referenceCell);
        
        // 添加原始单元格周围的单元格
        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                if (xOffset == 0 && yOffset == 0) continue;
                
                int newX = referenceCell.X + xOffset;
                int newY = referenceCell.Y + yOffset;
                
                // 检查坐标是否在网格范围内
                if (newX >= 0 && newX < manager.equippedGridSize && newY >= 0 && newY < manager.equippedGridSize)
                {
                    candidateCells.Add(targetGrid[newX, newY]);
                }
            }
        }
        
        // 尝试每个候选单元格
        foreach (GridCell candidateCell in candidateCells)
        {
            if (manager.CanPlacePiece(this, candidateCell, targetGrid))
            {
                canPlace = true;
                bestReferenceCell = candidateCell;
                break;
            }
        }
        
        if (!canPlace)
        {
            // 不能旋转，恢复形状
            shape = oldShape;
            Debug.Log("旋转失败：旋转后的形状无法放置");
            return;
        }
        
        // 应用旋转
        ClearOccupiedCells();
        manager.PlacePiece(this, bestReferenceCell, targetGrid);
        
        // 重新创建视觉表示
        CreateVisual();
        
        // 如果之前是选中状态，保持选中状态
        if (isSelected)
        {
            SetSelected(true);
        }
    }
    
    // 獲取相對於網格起始位置的世界坐標
    public Vector2Int[] GetWorldPositions(int startX = -1, int startY = -1)
    {
        // 如果沒有提供起始位置，嘗試從佔據的單元格獲取
        if (startX < 0 || startY < 0)
        {
            if (occupiedCells.Count > 0)
            {
                GridCell firstCell = occupiedCells[0];
                startX = firstCell.X;
                startY = firstCell.Y;
            }
            else
            {
                return new Vector2Int[0];
            }
        }
        
        Vector2Int[] worldPositions = new Vector2Int[shape.Length];
        
        for (int i = 0; i < shape.Length; i++)
        {
            worldPositions[i] = new Vector2Int(startX + shape[i].x, startY + shape[i].y);
        }
        
        return worldPositions;
    }
    
    // 添加佔據的單元格
    public void AddOccupiedCell(GridCell cell)
    {
        if (!occupiedCells.Contains(cell))
        {
            occupiedCells.Add(cell);
        }
    }
    
    // 清除佔據的單元格
    public void ClearOccupiedCells()
    {
        foreach (GridCell cell in occupiedCells)
        {
            cell.SetOccupied(null);
        }
        occupiedCells.Clear();
    }
    
    // 設置選擇狀態
    public void SetSelected(bool selected)
    {
        isSelected = selected;
        
        // 視覺上表示選擇狀態
        foreach (Transform child in transform)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                image.color = selected ? Color.Lerp(color, Color.white, 0.3f) : color;
            }
        }
        
        if (selected)
        {
            manager.SelectPiece(this);
        }
    }
    
    // 開始拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startParent = transform.parent;
        
        // 提高層級以便在拖動時顯示在其他 UI 元素上方
        transform.SetParent(transform.root);
        
        // 設置半透明，表示拖拽狀態
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        
        // 設置當前拖拽的方塊
        manager.SetDraggedPiece(this);
        
        // 清除之前的佔用狀態
        ClearOccupiedCells();
    }
    
    // 拖拽中
    public void OnDrag(PointerEventData eventData)
    {
        // 使用正確的座標轉換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);
        
        // 對齊網格系統
        int gridX = Mathf.FloorToInt(localPoint.x / cellSize);
        int gridY = Mathf.FloorToInt(-localPoint.y / cellSize);
        // 即時預覽位置偏移量
        int offsetX = -10; 

        // 向右偏移1個單位修正整體坐標系
        gridX += 1; 
        // 即時預覽位置
        transform.localPosition = new Vector3(
            gridX * cellSize + cellSize/2 + offsetX,
            -gridY * cellSize - cellSize/2,
            0);
    }
    
    // 結束拖拽
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        bool placed = false;

        // 嘗試在裝備格子中放置
        GridCell targetCell = manager.GetCellAtPosition(eventData.position, true);
        if (targetCell != null && manager.CanPlacePiece(this, targetCell, manager.equippedGrid))
        {
            manager.PlacePiece(this, targetCell, manager.equippedGrid);
            placed = true;
        }

        // 如果裝備格子放置失敗，嘗試在第一個存儲格子中放置
        if (!placed)
        {
            targetCell = manager.GetCellAtPosition(eventData.position, false, 1); // 第一個存儲格
            if (targetCell != null && manager.CanPlacePiece(this, targetCell, manager.storageGrid))
            {
                manager.PlacePiece(this, targetCell, manager.storageGrid);
                placed = true;
            }
        }

        // 如果第一個存儲格子也放置失敗，嘗試在第二個存儲格子中放置
        if (!placed)
        {
            targetCell = manager.GetCellAtPosition(eventData.position, false, 2); // 第二個存儲格
            if (targetCell != null && manager.CanPlacePiece(this, targetCell, manager.storageGrid_2))
            {
                manager.PlacePiece(this, targetCell, manager.storageGrid_2);
                placed = true;
            }
        }

        // 如果第二個存儲格子也放置失敗，嘗試在第三個存儲格子中放置
        if (!placed)
        {
            targetCell = manager.GetCellAtPosition(eventData.position, false, 3); // 第三個存儲格
            if (targetCell != null && manager.CanPlacePiece(this, targetCell, manager.storageGrid_3))
            {
                manager.PlacePiece(this, targetCell, manager.storageGrid_3);
                placed = true;
            }
        }

        // 如果都無法放置，返回原始位置
        if (!placed)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
            
            if (occupiedCells.Count > 0)
            {
                GridCell firstCell = occupiedCells[0];
                GridCell[,] targetGrid = GetTargetGridForCell(firstCell);
                manager.PlacePiece(this, firstCell, targetGrid);
            }
        }

        manager.SetDraggedPiece(null);
    }
    
    // 處理點擊
    public void OnPointerClick(PointerEventData eventData)
    {
        SetSelected(!isSelected);
    }

    // 獲取指定單元格所屬的網格
    private GridCell[,] GetTargetGridForCell(GridCell cell)
    {
        Transform parent = cell.transform.parent;
        if (parent == manager.equippedGridContainer)
            return manager.equippedGrid;
        else if (parent == manager.storageGridContainer)
            return manager.storageGrid;
        else if (parent == manager.storageGridContainer_2)
            return manager.storageGrid_2;
        else if (parent == manager.storageGridContainer_3)
            return manager.storageGrid_3;
        return null;
    }
}
