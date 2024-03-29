using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RelicViewManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI relicNameText,relicEffectText, relicPriceText;
    [SerializeField] Image relicImage;

    public void ViewRelic(RelicDataManager relicDataManager) // relicDataManagerのデータ取得と反映
    {
        relicNameText.text = relicDataManager._relicName;
        relicEffectText.text = relicDataManager._relicEffect;
        relicPriceText.text = relicDataManager._relicPrice.ToString();
        relicImage.sprite = relicDataManager._relicImage;
    }
}
