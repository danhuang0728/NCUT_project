using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHint : MonoBehaviour
{
    public GameObject keyPrefab;
    void Start()
    {
        Instantiate(keyPrefab, transform.position + new Vector3(0,1.5f,0), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
