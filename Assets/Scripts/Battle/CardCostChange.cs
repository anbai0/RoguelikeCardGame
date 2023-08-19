using UnityEngine;
using TMPro;

public class CardCostChange : MonoBehaviour
{
    [Header("デッキのある場所")]
    [SerializeField]
    Transform cardPlace;
    /// <summary>
    /// CardEffectListのアクセラレートの効果でカードのコストを減少させる
    /// </summary>
    public void CardCostDown(int accelerateValue)
    {
        foreach (Transform child in cardPlace) //cardPlaceにある全てのデッキを探索
        {
            CardController deckCard = child.GetComponent<CardController>();
            if (deckCard.cardDataManager._cardType == "Attack") //カードのタイプがAttackなら
            {
                //カードのコストを下げる
                deckCard.cardDataManager._cardCost -= accelerateValue;
                if (deckCard.cardDataManager._cardCost < 1)
                {
                    deckCard.cardDataManager._cardCost = 1;
                }
            }
            TextMeshProUGUI costText = child.transform.Find("CardInfo/CardCost").GetComponentInChildren<TextMeshProUGUI>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }

    /// <summary>
    /// アクセラレートで変化した値を戻す
    /// </summary>
    public void UndoCardCost()
    {
        foreach (Transform child in cardPlace)
        {
            CardController deckCard = child.GetComponent<CardController>();
            if (deckCard.cardDataManager._cardType == "Attack")
            {
                int cardID = deckCard.cardDataManager._cardID;
                CardData cardData = Resources.Load<CardData>("CardDataList/Card" + cardID); //元のコストをリソースから読み込む
                deckCard.cardDataManager._cardCost = cardData.cardCost; //コストを元に戻す
            }
            TextMeshProUGUI costText = child.transform.Find("CardInfo/CardCost").GetComponentInChildren<TextMeshProUGUI>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }
}
