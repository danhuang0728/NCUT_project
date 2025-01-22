using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataInitializer : MonoBehaviour
{
    public LevelData levelData;

    private void Start()
    {
        if (levelData != null)
        {
            levelData.InitializeLevelData(100); // 設置到 100 級
        }
    }
}
