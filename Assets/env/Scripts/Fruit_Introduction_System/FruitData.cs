using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitDataList", menuName = "FruitMonsterData/FruitData")]

public class FruitData : ScriptableObject
{
    public enum FruitMonsterTYPE
    {
        apple_wolf_box,
        bee_box,
        bell_box,
        lemonSlime_box,
        lychee_box,
        peach_box,
        sketletlon_0_box,
        spider_0_box,
        Boss

    }
    public Sprite FruitSprite;
    public FruitMonsterTYPE fruitMonsterTYPE;
    public string[] introduce;
    public bool isIntroduced = false;
}
