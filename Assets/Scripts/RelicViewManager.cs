using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicViewManager : MonoBehaviour
{
    [SerializeField] Text relicNameText,relicEffectText,relicCostText, relicPriceText;
    [SerializeField] Image relicImage;

    public void ViewRelic(RelicDataManager relicDataManager) // relicDataManager�̃f�[�^�擾�Ɣ��f
    {
        relicNameText.text = relicDataManager._relicName;
        relicEffectText.text = relicDataManager._relicEffect;
        relicPriceText.text = relicDataManager._relicPrice.ToString();
        relicImage.sprite = relicDataManager._relicImage;
    }
}
