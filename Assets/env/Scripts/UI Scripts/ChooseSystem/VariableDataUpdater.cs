using UnityEngine;
using System.Collections;

public class VariableDataUpdater : MonoBehaviour
{
    public VariableData variableData;

    private Coroutine _updateCoroutine;

    private void OnEnable()
    {
        if (_updateCoroutine == null)
        {
            _updateCoroutine = StartCoroutine(UpdateRandomValue());
        }
    }

    private void OnDisable()
    {
        if (_updateCoroutine != null)
        {
            StopCoroutine(_updateCoroutine);
            _updateCoroutine = null;
        }
    }

    private IEnumerator UpdateRandomValue()
    {
        while (true)
        {
            if (variableData != null)
            {
                variableData.UpdateRandomValue();
            }
            yield return new WaitForSeconds(1f); // 每秒更新一次
        }
    }
} 