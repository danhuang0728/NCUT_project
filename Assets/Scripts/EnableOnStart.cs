using UnityEngine;

public class EnableOnStart : MonoBehaviour
{
    private void Start()
    {
        // 确保游戏对象被启用
        gameObject.SetActive(true);
    }
} 