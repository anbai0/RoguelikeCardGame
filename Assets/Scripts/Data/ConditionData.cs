using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConditionData", menuName = "Create ConditionData")]
public class ConditionData : ScriptableObject
{
    /// <summary>
    /// 状態異常のID
    /// </summary>
    public int conditionID;
    /// <summary>
    /// 状態異常の名前
    /// </summary>
    public string conditionName;
}
