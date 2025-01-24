using UnityEngine;

[CreateAssetMenu(fileName = "TetrisItemData", menuName = "Inventory/TetrisItemData")]
public class TetrisItemData : ScriptableObject
{
    [Header("Main")]
    public SizeInt size = new();

    [Header("Visual")]
    public Sprite icon;
    public Color backgroundColor;
}