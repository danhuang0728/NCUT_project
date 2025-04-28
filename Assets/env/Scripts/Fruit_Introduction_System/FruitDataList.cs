using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitData", menuName = "FruitMonsterData/FruitDataList")]
public class FruitDataList : ScriptableObject
{
    public List<FruitData> fruitDatas;
}
