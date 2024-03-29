using UnityEngine;

[CreateAssetMenu(fileName = "ConditionData", menuName = "Create ConditionData")]
public class ConditionData : ScriptableObject
{
    /// <summary>
    /// 状態異常の名前
    /// </summary>
    public string conditionName;

    /// <summary>
    /// 状態異常の効果
    /// </summary>
    public string conditionEffect;

    public Sprite conditionImage;
}
