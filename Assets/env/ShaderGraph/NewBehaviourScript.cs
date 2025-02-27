using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // 通过脚本设置相机目标纹理
public Camera renderCamera;
public RenderTexture cameraSortingLayerRT;

    void Start() 
    {
    }

    // Update is called once per frame
    public Material materialUsingTexture;
    void Update() {
        materialUsingTexture.SetTexture("_CameraSortingLayerTexture", cameraSortingLayerRT);
    }
}
