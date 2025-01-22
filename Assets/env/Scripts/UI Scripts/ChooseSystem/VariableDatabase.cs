using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "VariableDatabase", menuName = "Custom/Variable Database")]
public class VariableDatabase : ScriptableObject
{
    public List<VariableData> variableDataList;
}