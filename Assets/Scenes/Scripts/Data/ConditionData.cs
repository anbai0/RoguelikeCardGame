using UnityEngine;

[CreateAssetMenu(fileName = "ConditionData", menuName = "Create ConditionData")]
public class ConditionData : ScriptableObject
{
    /// <summary>
    /// ��Ԉُ�̖��O
    /// </summary>
    public string conditionName;

    /// <summary>
    /// ��Ԉُ�̌���
    /// </summary>
    public string conditionEffect;

    public Sprite conditionImage;
}
