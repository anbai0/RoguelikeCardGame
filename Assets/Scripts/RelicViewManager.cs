using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelicViewManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI relicNameText,relicEffectText, relicPriceText;
    [SerializeField] Image relicImage;

    public void ViewRelic(RelicDataManager relicDataManager) // relicDataManagerÇÃÉfÅ[É^éÊìæÇ∆îΩâf
    {
        relicNameText.text = relicDataManager._relicName;
        relicEffectText.text = relicDataManager._relicEffect;
        relicPriceText.text = relicDataManager._relicPrice.ToString();
        relicImage.sprite = relicDataManager._relicImage;
    }
}
