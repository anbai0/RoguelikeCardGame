using UnityEngine;

public class ConditionDataManager : MonoBehaviour
{
    public string _conditionName;
    public string _conditionEffect;
    public Sprite _conditionImage;

    public ConditionDataManager(string conditionName)
    {
        ConditionData conditionData = Resources.Load<ConditionData>("ConditionDataList/" + conditionName);
        _conditionName = conditionData.conditionName;
        _conditionEffect = conditionData.conditionEffect;
        _conditionImage = conditionData.conditionImage;
    }
}
