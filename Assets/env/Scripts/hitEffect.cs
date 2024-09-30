using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitEffect : MonoBehaviour
{
    // 目标材质
    public Renderer targetRenderer;
    // Shader中的布尔属性名称
    public string boolPropertyName = "_hitBool";

    /// <summary>
    /// 设置布尔属性
    /// </summary>
    /// <param name="value">要设置的布尔值</param>
    public void SetBool(bool value)
    {
        if (targetRenderer != null)
        {
            // 获取材质实例（确保不会修改共享材质）
            Material mat = targetRenderer.material;
            // 设置整数属性，1表示true，0表示false
            mat.SetInt(boolPropertyName, 1);
        }
    }
}
