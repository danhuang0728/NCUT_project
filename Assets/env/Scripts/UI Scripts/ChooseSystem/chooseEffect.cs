using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class chooseEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isHovering = false;
    private float scaleMultiplier = 1.2f;
    private float lerpSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void OnEnable()  // 當物件激活時，將目標縮放設為原始縮放(避免卡在變大狀態)
    {
        targetScale = originalScale;
        isHovering = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * lerpSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleMultiplier;
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        isHovering = false;
    }
}
