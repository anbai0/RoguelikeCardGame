using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �J�[�h�f�[�^�̕\�����s���X�N���v�g
/// </summary>
public class CardViewManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cardNameText,cardEffectText,cardCostText, cardPriceText;
    [SerializeField] Image cardImage;

    public void ViewCard(CardDataManager cardDataManager) // cardDataManager�̃f�[�^�擾�Ɣ��f
    {
        cardNameText.text = cardDataManager._cardName;
        cardEffectText.text = cardDataManager._cardEffect;
        cardCostText.text = cardDataManager._cardCost.ToString();
        cardPriceText.text = cardDataManager._cardPrice.ToString();
        cardImage.sprite = cardDataManager._cardImage;
    }
}
