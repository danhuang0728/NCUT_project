using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitDataList", menuName = "FruitMonsterData/FruitData")]

public class FruitData : ScriptableObject
{
    public enum FruitMonsterTYPE
    {
        Lychee,
        Spider
        //水果怪種類...
    }
    public Sprite FruitSprite;
    public FruitMonsterTYPE fruitMonsterTYPE;
    public string[] introduce;
    public bool isIntroduced = false;
}
