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
    public GameObject gridCellPrefab;           // 網格單元格預置體
    public GameObject tetrisPiecePrefab;        // Tetris 方塊預置體
    
    [Header("網格設定")]
    public int gridSize = 8;                    // 網格大小 (8x8)
    public float cellSize = 50f;                // 單元格大小
    
    [Header("控制設定")]
    public KeyCode toggleKey = KeyCode.B;       // 開關物品欄的按鍵
    
    // 修改為公開變量，以便其他類可以訪問
    [HideInInspector] public GridCell[,] equippedGrid;  // 左側裝備網格
    [HideInInspector] public GridCell[,] storageGrid;   // 右側存儲網格
    
    private bool isInventoryOpen = false;
    private List<TetrisPiece> tetrisPieces = new List<TetrisPiece>();
    private TetrisPiece currentDraggedPiece;
    private TetrisPiece selectedPiece;
    
    void Start()
    {
        // 初始化物品欄
        inventoryPanel.SetActive(false);
        InitializeGrids();
        CreateSampleTetrisPieces();
        Debug.Log(tetrisPieces[0].GetWorldPositions(0,0));
    }
    
    void Update()
    {
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
    }
    
    // 初始化兩側的網格
    void InitializeGrids()
    {
        equippedGrid = CreateGrid(equippedGridContainer, "Equipped");
        storageGrid = CreateGrid(storageGridContainer, "Storage");
    }
    
    // 創建一個網格
    GridCell[,] CreateGrid(Transform container, string prefix)
    {
        GridCell[,] grid = new GridCell[gridSize, gridSize];
        
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
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
        // 創建幾種不同形狀的 Tetris 方塊做示例
        CreateTetrisPiece(
            new Vector2Int[] { 
                new Vector2Int(0, 0), 
                new Vector2Int(1, 0), 
                new Vector2Int(0, 1), 
                new Vector2Int(1, 1) 
            }, 
            Color.cyan
        );    // 方形 (O型)
        
        CreateTetrisPiece(
            new Vector2Int[] { 
                new Vector2Int(0, 0), 
                new Vector2Int(0, 1), 
                new Vector2Int(0, 2), 
                new Vector2Int(0, 3) 
            }, 
            Color.blue
        );    // I型
        
        CreateTetrisPiece(
            new Vector2Int[] { 
                new Vector2Int(0, 0),   // 底部
                new Vector2Int(0, 1),   // 中間
                new Vector2Int(0, 2),   // 頂部
                new Vector2Int(1, 0)    // 右側底部
            }, 
            new Color(1f, 0.5f, 0f)     // L型 (橘色)
        );
        
        CreateTetrisPiece(
            new Vector2Int[] { 
                new Vector2Int(0, 0), 
                new Vector2Int(1, 0), 
                new Vector2Int(1, 1), 
                new Vector2Int(2, 1) 
            }, 
            Color.green                 // Z型
        );
        
        CreateTetrisPiece(
            new Vector2Int[] { 
                new Vector2Int(0, 0),   // 左側
                new Vector2Int(1, 0),   // 中間
                new Vector2Int(2, 0),   // 右側
                new Vector2Int(1, 1)    // 頂部中間
            }, 
            Color.magenta               // T型
        );
    }
    
    // 創建一個 Tetris 方塊
    void CreateTetrisPiece(Vector2Int[] shape, Color color, Vector2 pivot = default)
    {
        GameObject pieceObj = Instantiate(tetrisPiecePrefab, storageGridContainer);
        TetrisPiece piece = pieceObj.AddComponent<TetrisPiece>();
        
        // 計算自動中心點 (如果沒有指定)
        if(pivot == default) {
            Vector2 sum = Vector2.zero;
            foreach(var pos in shape) {
                sum += new Vector2(pos.x, pos.y);
            }
            pivot = sum / shape.Length;
        }
        
        piece.Initialize(shape, color, cellSize, this, pivot); // 需要更新 TetrisPiece 的 Initialize 方法
        tetrisPieces.Add(piece);
        
        // 修正位置計算，使用網格座標系統
        piece.transform.localPosition = Vector2.zero;
        // 自動尋找合適的放置位置
        TryAutoPlaceInStorage(piece);
    }
    
    // 新增自動放置方法
    private void TryAutoPlaceInStorage(TetrisPiece piece)
    {
        // 從右下角開始掃描以容納較大物件
        for (int y = gridSize-1; y >= 0; y--)
        {
            for (int x = gridSize-1; x >= 0; x--)
            {
                GridCell cell = storageGrid[x, y];
                if (CanPlacePiece(piece, cell, storageGrid))
                {
                    // 修正偏移量計算，向右偏移1個單位修正整體坐標系
                    piece.transform.localPosition = new Vector3(
                        (cell.X+1) * cellSize,
                        -cell.Y * cellSize,
                        0
                    );
                    PlacePiece(piece, cell, storageGrid);
                    return;
                }
            }
        }
    }
    
    // 切換物品欄開關
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryPanel.SetActive(isInventoryOpen);
        
        // 當關閉物品欄時，取消選擇
        if (!isInventoryOpen && selectedPiece != null)
        {
            selectedPiece.SetSelected(false);
            selectedPiece = null;
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
            
            // 檢查目標位置是否已被佔用
            if (targetGrid[pos.x, pos.y].IsOccupied && targetGrid[pos.x, pos.y].OccupyingPiece != piece)
            {
                Debug.Log($"位置已佔用: ({pos.x}, {pos.y})");
                return false;
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
        
        // 繪製網格線
        Gizmos.color = Color.white;
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Vector3 pos = new Vector3(
                    x * cellSize, 
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
    
    // 初始化方塊
    public void Initialize(Vector2Int[] shape, Color color, float cellSize, TetrisInventoryManager manager, Vector2 pivot = default)
    {
        this.shape = shape;
        this.color = color;
        this.cellSize = cellSize;
        this.manager = manager;
        
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
            
            // 設置位置和大小
            RectTransform blockRect = blockObj.GetComponent<RectTransform>();
            blockRect.anchorMin = new Vector2(1, 1);
            blockRect.anchorMax = new Vector2(1, 1);
            blockRect.pivot = new Vector2(1, 1);
            blockRect.sizeDelta = new Vector2(cellSize * 0.9f, cellSize * 0.9f);  // 略小於單元格，留出邊距
            
            // 計算相對於父物件左上角的偏移量
            float xOffset = (cell.x - minX) * cellSize;
            float yOffset = -(cell.y - minY) * cellSize;
            
            blockRect.anchoredPosition = new Vector2(xOffset, yOffset);
        }
    }
    
    // 旋轉方塊
    public void Rotate()
    {
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
                if (newX >= 0 && newX < manager.gridSize && newY >= 0 && newY < manager.gridSize)
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
        // 向右偏移1個單位修正整體坐標系
        gridX += 1; 
        // 即時預覽位置
        transform.localPosition = new Vector3(
            gridX * cellSize + cellSize/2,
            -gridY * cellSize - cellSize/2,
            0);
    }
    
    // 結束拖拽
    public void OnEndDrag(PointerEventData eventData)
    {
        // 恢復透明度和射線檢測
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        // 清除拖拽狀態
        manager.SetDraggedPiece(null);
        
        // 如果沒有放置到有效位置，返回起始位置
        if (occupiedCells.Count == 0)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }
    
    // 處理點擊
    public void OnPointerClick(PointerEventData eventData)
    {
        SetSelected(!isSelected);
    }
}
