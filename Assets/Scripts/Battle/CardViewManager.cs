using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardViewManager : MonoBehaviour
{
    [SerializeField] Text cardNameText,cardEffectText,cardCostText;
    [SerializeField] Image cardImage;

    public void ViewCard(CardDataManager cardDataManager) // cardDataManager‚Ìƒf[ƒ^æ“¾‚Æ”½‰f
    {
        cardNameText.text = cardDataManager._cardName;
        cardEffectText.text = cardDataManager._cardEffect;
        cardCostText.text = cardDataManager._cardCost.ToString();
        cardImage = cardDataManager._cardImage;
    }
}
