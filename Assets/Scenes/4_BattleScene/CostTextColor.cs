using UnityEngine;
using TMPro;

public class CostTextColor : MonoBehaviour
{
    [SerializeField]
    CardController cardController;
    [SerializeField]
    TextMeshProUGUI costText;

    PlayerBattleAction player;
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerBattleAction>();
    }
    void Update()
    {
        //���݂�AP���J�[�h�̃R�X�g�������Ă��Ȃ����
        if (cardController.cardDataManager._cardCost <= player.GetSetCurrentAP)
        {
            WithinCostColor();
        }
        else //���݂�AP���J�[�h�̃R�X�g�������Ă����
        {
            OverCostColor();
        }
    }

    /// <summary>
    /// �J�[�h�̃R�X�g�̐F�𔒂ɂ���
    /// </summary>
    void WithinCostColor()
    {
        costText.color = Color.white;
    }

    /// <summary>
    /// �J�[�h�̃R�X�g�̐F��Ԃɂ���
    /// </summary>
    void OverCostColor()
    {
        costText.color = Color.red;
    }
}
