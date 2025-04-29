using UnityEngine;
using UnityEditor;

public class ShopItemPrefabsFiller
{
    [MenuItem("Tools/Auto Fill Shop Item Prefabs")]
    public static void FillShopItemPrefabs()
    {
        GameObject selected = Selection.activeGameObject;

        if (selected == null)
        {
            Debug.LogWarning("請先選取一個 GameObject！");
            return;
        }

        LevelTrigger holder = selected.GetComponent<LevelTrigger>();
        if (holder == null)
        {
            Debug.LogWarning("選取的物件上找不到 ShopItemHolder 腳本！");
            return;
        }

        // 找出所有 prefab 路徑
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/env/prefab/tetrDrops" });
        GameObject[] prefabs = new GameObject[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            prefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }

        holder.shopItemPrefabs = prefabs;

        // 告知 Unity 已更改
        EditorUtility.SetDirty(holder);

        Debug.Log($"成功加入 {prefabs.Length} 個 prefabs 到 ShopItemHolder 中！");
    }
}
