using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHint : MonoBehaviour
{
    public GameObject keyPrefab;
    private GameObject keyhint;
    void Start()
    {
        keyhint = Instantiate(keyPrefab, transform.position + new Vector3(0,1.5f,0), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // 當物件被刪除時觸發
    private void OnDestroy()
    {
        Destroy(keyhint);
    }

    // 當物件被關閉時觸發
    private void OnDisable()
    {
        Destroy(keyhint);
    }
}
