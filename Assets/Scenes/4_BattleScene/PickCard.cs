using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

/// <summary>
/// デッキのカードにカーソルを合わせた際に拡大表示するピックカードのスクリプト
/// </summary>
public class PickCard : MonoBehaviour
{
    /// <summary>
    /// デッキの順番と同じ順番に並んでいるピックカードを選択する
    /// </summary>
    /// <param name="card">デッキのカード</param>
    /// <returns>デッキのカードに対応するピックカード</returns>
    public GameObject ChoosePickCard(GameObject card)
    {
        string pickName = card.name;
        string pattern = @"\d+";// 半角数字を抽出するための正規表現パターン
        Match match = Regex.Match(pickName, pattern);// 正規表現パターンに一致する半角数字を検索
        string pickCardNum = match.Value;
        GameObject pickCard = GameObject.Find("Pick" + pickCardNum);
        return pickCard;
    }

    /// <summary>
    /// デッキカードのデータをピックカードに転写する
    /// </summary>
    /// <param name="deckCard">デッキのカード</param>
    /// <param name="pickCard">ピックした際に表示するカード</param>
    /// <returns>デッキのデータを転写したピックカード</returns>
    public GameObject SetPickCardStatus(GameObject deckCard, GameObject pickCard)
    {
        //ピックされたカードは参照するデッキのカードのdataManagerを反映
        var deckData = deckCard.GetComponent<CardController>().cardDataManager;
        pickCard.GetComponent<CardViewManager>().ViewCard(deckData);
        //ピックされたカードは参照するデッキのカードの効果Textを反映
        var deckEffect = deckCard.transform.Find("CardInfo/CardEffect").GetComponent<TextMeshProUGUI>().text;
        pickCard.transform.Find("CardInfo/CardEffect").GetComponent<TextMeshProUGUI>().text = deckEffect;
        //ピックされたカードは参照するデッキのカードのcardStateを反映
        pickCard.GetComponent<CardController>().cardDataManager._cardState = deckData._cardState;
        //ピックされたカードは参照するデッキのカードのcardCostを反映
        pickCard.GetComponent<CardController>().cardDataManager._cardCost = deckData._cardCost;
        //カード情報を表示する
        pickCard.transform.Find("CardInfo").gameObject.SetActive(true);
        pickCard.GetComponent<CardState>().isActive = true;
        return pickCard;
    }
}
