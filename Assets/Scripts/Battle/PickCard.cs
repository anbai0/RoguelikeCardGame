using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class PickCard : MonoBehaviour
{
    string thisName = null; //このオブジェクトの名前
    GameObject deckCard = null; //このカードに対応するデッキのカード
    CardController cardController;
    // Start is called before the first frame update
    void Start()
    {
        thisName = gameObject.name;
        cardController = gameObject.GetComponent<CardController>();
        string pattern = @"\d+";// 半角数字を抽出するための正規表現パターン
        Match match = Regex.Match(thisName, pattern);// 正規表現パターンに一致する半角数字を検索
        string thisCardNum = match.Value;
        deckCard = GameObject.Find("Deck" + thisCardNum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject ChoosePickCard(GameObject card)
    {
        string pickName = card.name;
        string pattern = @"\d+";// 半角数字を抽出するための正規表現パターン
        Match match = Regex.Match(pickName, pattern);// 正規表現パターンに一致する半角数字を検索
        string pickCardNum = match.Value;
        Debug.Log("pickCardNum number is: " + pickCardNum);
        GameObject pickCard = GameObject.Find("Pick" + pickCardNum);
        return pickCard;
    }
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
        Debug.Log("pickCard name is: " + pickCard);
        //カード情報を表示する
        pickCard.transform.GetChild(0).gameObject.SetActive(true);
        pickCard.GetComponent<CardState>().isActive = true;
        return pickCard;
    }
}
