using UnityEngine;

[CreateAssetMenu(fileName = "ConditionData", menuName = "Create ConditionData")]
public class ConditionData : ScriptableObject
{
    /// <summary>
    /// ó‘ÔˆÙí‚Ì–¼‘O
    /// </summary>
    public string conditionName;

    /// <summary>
    /// ó‘ÔˆÙí‚ÌŒø‰Ê
    /// </summary>
    public string conditionEffect;

    public Sprite conditionImage;
}
