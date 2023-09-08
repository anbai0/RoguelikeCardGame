using System.Collections.Generic;
using UnityEngine;


public class BonfireManager : MonoBehaviour
{

    [SerializeField] GameObject cardEmptyText;

    // カード表示
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform cardPlace;
    [SerializeField] Transform enhancedCardHolder;
    [SerializeField] CardController enhancedCard;
    private List<int> deckNumberList;                    //プレイヤーのもつデッキナンバーのリスト

    private Vector3 CardScale = Vector3.one * 0.25f;     // 生成するカードのスケール

    void Start()
    {
        enhancedCard = Instantiate(cardPrefab, enhancedCardHolder);   // 強化後のカードを表示するPrefabを生成
        enhancedCard.gameObject.GetComponent<UIController>().enabled = false;
        enhancedCard.transform.SetParent(enhancedCardHolder);
        InitDeck();
    }

    private void InitDeck() //デッキ生成
    {
        deckNumberList = GameManager.Instance.playerData._deckList;
        var selectableList = ExcludeDeckCards(deckNumberList);
        if (selectableList.Count <= 0) //デッキの枚数が0枚なら生成しない
        {
            cardEmptyText.SetActive(true); //強化できるカードがないことをTextで伝える
            return;
        }

        for (int init = 0; init < selectableList.Count; init++)// 選択出来るデッキの枚数分
        {
            CardController card = Instantiate(cardPrefab, cardPlace);   //カードを生成する
            card.transform.localScale = CardScale;
            card.name = "Deck" + (init).ToString();                     //生成したカードに名前を付ける
            card.Init(selectableList[init]);                            //デッキデータの表示
        }
    }

    /// <summary>
    /// 魔女の霊薬や強化済みのカードはリストから除外する
    /// </summary>
    /// <param name="deckList">プレイヤーの所持しているデッキリスト</param>
    /// <returns>強化できないものを除いたデッキリスト</returns>
    List<int> ExcludeDeckCards(List<int> deckList)
    {
        List<int> selectableList = new List<int>();
        for (int deckNum = 0; deckNum < deckList.Count; deckNum++)
        {
            if (deckList[deckNum] != 3 && deckList[deckNum] <= 20) //ID3の魔女の霊薬ではなく、IDが20以下の未強化カードの場合
            {
                selectableList.Add(deckList[deckNum]);
            }
        }
        return selectableList;
    }


    /// <summary>
    /// カードの強化をするメソッドです
    /// </summary>
    /// <param name="selectCard">選択されたCard</param>
    public void CardEnhance(GameObject selectCard)
    {
        int id = selectCard.GetComponent<CardController>().cardDataManager._cardID; //選択されたカードのIDを取得
        for (int count = 0; count < deckNumberList.Count; count++)
        {
            if (deckNumberList[count] == id) //デッキからIDのカードを探す 
            {
                deckNumberList[count] += 100; //そのカードの強化版のIDに変更する
            }
        }
    }

    /// <summary>
    /// 強化後のカードを表示するメソッドです
    /// </summary>
    /// <param name="selectCard">選択されたCard</param>
    public void DisplayEnhancedCard(GameObject selectCard)
    {
        int id = selectCard.GetComponent<CardController>().cardDataManager._cardID; //選択されたカードのIDを取得
        enhancedCard.Init(id+100);                            //デッキデータの表示
        
    }

    public void UnLoadBonfireScene()
    {
        // 焚火シーンをアンロード
        SceneFader.Instance.SceneChange(unLoadSceneName: "BonfireScene", allowPlayerMove: true);
    }
}
