using UnityEditor;
using UnityEngine;

public class ToggleUICanvasEditor : EditorWindow
{
    [MenuItem("Tools/ToggleAllUICanvas")]
    public static void ToggleAllUICanvas()
    {
        var canvases = GameObject.FindObjectsOfType<Canvas>(true); // true: 包含未啟用物件
        bool anyActive = false;
        foreach (var canvas in canvases)
        {
            if (canvas.gameObject.activeSelf)
            {
                anyActive = true;
                break;
            }
        }
        // 如果有任何一個是啟用，就全部關閉；否則全部打開
        foreach (var canvas in canvases)
        {
            Undo.RecordObject(canvas.gameObject, "切換Canvas顯示");
            canvas.gameObject.SetActive(!anyActive);
            EditorUtility.SetDirty(canvas.gameObject);
        }
    }
} 