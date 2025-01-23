using System.Collections;
using UnityEngine;

public class ToggleChildObjects : MonoBehaviour
{
    public float enableDuration = 1f; // 啟用持續時間    #1.28剛好一輪戳完
    public float disableDuration = 3f; // 關閉持續時間

    public Transform player;

    private void Start()
    {
        StartCoroutine(ToggleChildren());
    }

    private void Update()
    {
        transform.position = new Vector3(player.position.x - 0.3f, player.position.y - 0.2f, player.position.z);

    }

    private IEnumerator ToggleChildren()
    {
        // 獲取所有子物件
        Transform[] childObjects = GetComponentsInChildren<Transform>();
        
        // 移除父物件本身（因為它也會包含在陣列中）
        for (int i = 0; i < childObjects.Length; i++)
        {
            if (childObjects[i] == transform)
            {
                childObjects[i] = null;
                break;
            }
        }

        while (true)
        {
            // 啟用所有子物件
            foreach (var child in childObjects)
            {
                if (child != null)
                    child.gameObject.SetActive(true);
            }

            // 等待啟用持續時間
            yield return new WaitForSeconds(enableDuration);

            // 關閉所有子物件
            foreach (var child in childObjects)
            {
                if (child != null)
                    child.gameObject.SetActive(false);
            }

            // 等待關閉持續時間
            yield return new WaitForSeconds(disableDuration);
        }
    }
}
