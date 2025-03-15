using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class damage_effect : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public Color32 DamageColor100 = new Color32(255, 255, 255, 255); // 大于100傷害顏色
    public Color32 DamageColor500 = new Color32(255, 0, 0, 255);   // 大于500傷害顏色
    public Color32 DamageColor1000 = new Color32(94, 64, 0, 255);       // 大于1000傷害顏色
    void Start()
    {
        TextMeshPro damageTextMesh = damageTextPrefab.GetComponent<TextMeshPro>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void damageEffect_Pop_up(float damage,Transform target)
    {
        // 生成傷害數字
        GameObject damageText = Instantiate(damageTextPrefab, target.position, Quaternion.identity);
        TextMeshPro damageTextMesh = damageText.GetComponent<TextMeshPro>();
        damageTextMesh.text = damage.ToString();
        // 設置傷害數字顏色
        if (damage > 100) 
        {
            damageTextMesh.color = DamageColor100;
            damageTextMesh.fontSize = 9;
        }
        else if (damage > 500)
        {
            damageTextMesh.color = DamageColor500;
            damageTextMesh.fontSize = 10;
        }
        else if (damage > 1000)
        {
            damageTextMesh.color = DamageColor1000;
            damageTextMesh.fontSize = 12;
        }
        //弹跳效果
        float randomX = Random.Range(-0.3f, 0.3f); // 隨機X方向偏移
        
        Rigidbody2D rb = damageText.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(randomX, 1f), ForceMode2D.Impulse);
        
        Destroy(damageText, 0.8f);
    }   
    IEnumerator DamageTextArcJump(Transform textTransform)
    {
        Vector3 originalPosition = textTransform.position;
        float time = 0f;
        float randomX = Random.Range(-0.5f, 0.5f); // 隨機X方向偏移
        float upwardSpeed = Random.Range(1f, 2f); // 隨機向上速度
        while (time < 1f)
        {   
            float upwardOffset = time * upwardSpeed; // 向上移動
            float horizontalWave = Mathf.Sin(time * 10) * 0.3f; // 水平擺動
            textTransform.position = originalPosition + new Vector3(randomX + horizontalWave, upwardOffset + Mathf.Sin(time * 10) * 0.3f, 0);
            time += Time.deltaTime;
            yield return null;
        }
        textTransform.position = originalPosition;
    }
}

