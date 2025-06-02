using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VitaminUI : MonoBehaviour
{
    public Slider A;
    public Image Afill_image;
    public Slider B;
    public Image Bfill_image;
    public Slider C;
    public Image Cfill_image;
    public Slider D;
    public Image Dfill_image;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        A.value = VitaminManager.Instance.VitaminA;
        B.value = VitaminManager.Instance.VitaminB;
        C.value = VitaminManager.Instance.VitaminC;
        D.value = VitaminManager.Instance.VitaminD;
        Afill_image.color = new Color(1f, A.value / VitaminManager.Instance.MaxVitaminLevel, A.value / VitaminManager.Instance.MaxVitaminLevel);
        Bfill_image.color = new Color(1f, B.value / VitaminManager.Instance.MaxVitaminLevel, B.value / VitaminManager.Instance.MaxVitaminLevel);
        Cfill_image.color = new Color(1f, C.value / VitaminManager.Instance.MaxVitaminLevel, C.value / VitaminManager.Instance.MaxVitaminLevel);
        Dfill_image.color = new Color(1f, D.value / VitaminManager.Instance.MaxVitaminLevel, D.value / VitaminManager.Instance.MaxVitaminLevel);
    }
}
