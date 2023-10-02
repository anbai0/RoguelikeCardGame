using UnityEngine;
using TMPro;

/// <summary>
/// �A�N�Z�����[�g�̌��ʂɂ���ăJ�[�h�̃R�X�g��ύX����X�N���v�g
/// </summary>
public class CardCostChange : MonoBehaviour
{
    [SerializeField, Header("�f�b�L�̂���ꏊ")]
    Transform cardPlace;
    [SerializeField, Header("�\���J�[�h�̂���ꏊ")]
    Transform pickCardPlace;

    /// <summary>
    /// CardEffectList�̃A�N�Z�����[�g�̌��ʂŃJ�[�h�̃R�X�g������������
    /// </summary>
    public void CardCostDown(int accelerateValue)
    {
        foreach (Transform child in cardPlace) //cardPlace�ɂ���S�Ẵf�b�L��T��
        {
            CardController deckCard = child.GetComponent<CardController>();
            if (deckCard.cardDataManager._cardType == "Attack" ) //�J�[�h�̃^�C�v��Attack�̏ꍇ
            {
                if (deckCard.cardDataManager._cardID != 13 && deckCard.cardDataManager._cardID != 113) //�t���o�[�X�g����Ȃ��ꍇ
                {
                    //�J�[�h�̃R�X�g��������
                    deckCard.cardDataManager._cardCost -= accelerateValue;
                    if (deckCard.cardDataManager._cardCost < 1)
                    {
                        deckCard.cardDataManager._cardCost = 1;
                    }
                }
            }
            TextMeshProUGUI costText = child.transform.Find("CardInfo/CardCost").GetComponentInChildren<TextMeshProUGUI>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }

    /// <summary>
    /// �A�N�Z�����[�g�ŕω������l��߂�
    /// </summary>
    public void UndoCardCost()
    {
        foreach (Transform child in cardPlace)
        {
            CardController deckCard = child.GetComponent<CardController>();
            if (deckCard.cardDataManager._cardType == "Attack")
            {
                int cardID = deckCard.cardDataManager._cardID;
                CardData cardData = Resources.Load<CardData>("CardDataList/Card" + cardID); //���̃R�X�g�����\�[�X����ǂݍ���
                deckCard.cardDataManager._cardCost = cardData.cardCost; //�R�X�g�����ɖ߂�
            }
            TextMeshProUGUI costText = child.transform.Find("CardInfo/CardCost").GetComponentInChildren<TextMeshProUGUI>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }
}
