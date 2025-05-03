using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floateffect : MonoBehaviour
{
    public float amplitude = 0.5f;  // 浮動幅度
    public float speed = 1f;        // 浮動速度
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0, newY, 0);
    }
}
