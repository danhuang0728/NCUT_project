using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleAni : MonoBehaviour
{
    public Transform BlackHole;
    public float Rotate_speed = 1f;
    void Start()
    {
        BlackHole = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        BlackHole.Rotate(0f, 0f, Rotate_speed);
    }
}
