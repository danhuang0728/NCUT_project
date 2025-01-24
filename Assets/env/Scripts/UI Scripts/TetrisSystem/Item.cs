using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Item : MonoBehaviour
{
    public TetrisItemData data; // Changed to TetrisItemData
    public Image icon;
    public Image background;
    private Vector3 rotateTarget;
    public bool isRotated;
    public int rotateIndex;
    public Vector2Int indexPosition { get; set; }
    public Tetris inventory { get; set; }
    public RectTransform rectTransform { get; set; }
    public InventoryGrid inventoryGrid { get; set; }

    public SizeInt correctedSize
    {
        get
        { return new(!isRotated ? data.size.width : data.size.height, !isRotated ? data.size.height : data.size.width); }
    }

    private void Start()
    {
        icon.sprite = data.icon;
        background.color = data.backgroundColor;
    }

    private void LateUpdate()
    {
        UpdateRotateAnimation();
    }

    public void Rotate()
    {
        if (rotateIndex < 3)
        {
            rotateIndex++;
        }
        else if (rotateIndex >= 3)
        {
            rotateIndex = 0;
        }

        UpdateRotation();
    }

    public void ResetRotate()
    {
        rotateIndex = 0;
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        switch (rotateIndex)
        {
            case 0:
                rotateTarget = new(0, 0, 0);
                isRotated = false;
                break;

            case 1:
                rotateTarget = new(0, 0, -90);
                isRotated = true;
                break;

            case 2:
                rotateTarget = new(0, 0, -180);
                isRotated = false;
                break;

            case 3:
                rotateTarget = new(0, 0, -270);
                isRotated = true;
                break;
        }
    }

    private void UpdateRotateAnimation()
    {
        Quaternion targetRotation = Quaternion.Euler(rotateTarget);

        if (rectTransform.localRotation != targetRotation)
        {
            rectTransform.localRotation = Quaternion.Slerp(
                rectTransform.localRotation,
                targetRotation,
                InventorySettings.rotationAnimationSpeed * Time.deltaTime
            );
        }
    }
}