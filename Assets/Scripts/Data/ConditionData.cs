using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConditionData", menuName = "Create ConditionData")]
public class ConditionData : ScriptableObject
{
    /// <summary>
    /// ��Ԉُ��ID
    /// </summary>
    public int conditionID;
    /// <summary>
    /// ��Ԉُ�̖��O
    /// </summary>
    public string conditionName;
}
