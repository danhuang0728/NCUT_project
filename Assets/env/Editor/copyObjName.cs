using UnityEngine;
using UnityEditor;
using System.Text;

public class ListHierarchyObjects : EditorWindow
{
    [MenuItem("Tools/Copy Selected Object Names to Code Format")]
    static void CopySelectedObjectNames()
    {
        var selectedObjects = Selection.gameObjects;

        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("警告", "請先在 Hierarchy 中選取一些物件。", "確定");
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("string[] monsterNames = {");

        bool hasValidObjects = false;
        foreach (GameObject obj in selectedObjects)
        {
            if (obj != null && !string.IsNullOrEmpty(obj.name))
            {
                // 處理特殊字元
                string escapedName = obj.name.Replace("\"", "\\\"");
                sb.AppendLine($"    \"{escapedName}\",");
                hasValidObjects = true;
            }
        }

        if (!hasValidObjects)
        {
            EditorUtility.DisplayDialog("警告", "選取的物件中沒有有效的名稱。", "確定");
            return;
        }

        sb.AppendLine("};");

        GUIUtility.systemCopyBuffer = sb.ToString();
        EditorUtility.DisplayDialog("成功", "已成功複製物件名稱到剪貼簿！", "確定");
        Debug.Log("已複製以下內容到剪貼簿：\n" + sb.ToString());
    }
}