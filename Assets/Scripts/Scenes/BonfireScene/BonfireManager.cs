using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireManager : MonoBehaviour
{
    PlayerDataManager playerData;

    //カード
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform upperCardPlace;
    [SerializeField] Transform lowerCardPlace;
    List<int> deckNumberList;                    //プレイヤーのもつデッキナンバーのリスト

    Vector3 CardScale = Vector3.one * 0.25f;     // 生成するカードのスケール


    void Start()
    {
        //playerData = GameManager.Instance.playerData;
        playerData = new PlayerDataManager("Wizard");
        InitDeck();
    }

    private void InitDeck() //デッキ生成
    {
        deckNumberList = playerData._deckList;
        int distribute = DistributionOfCards(deckNumberList.Count);
        if (distribute <= 0) //デッキの枚数が0枚なら生成しない
            return;
        for (int init = 1; init <= deckNumberList.Count; init++)// デッキの枚数分
        {
            if (init <= distribute) //決められた数をupperCardPlaceに生成する
            {
                CardController card = Instantiate(cardPrefab, upperCardPlace);//カードを生成する
                card.transform.localScale = CardScale;
                card.name = "Deck" + (init-1).ToString();//生成したカードに名前を付ける
                card.Init(deckNumberList[init - 1]);//デッキデータの表示
            }
            else //残りはlowerCardPlaceに生成する
            {
                CardController card = Instantiate(cardPrefab, lowerCardPlace);//カードを生成する
                card.transform.localScale = CardScale;
                card.name = "Deck" + (init-1).ToString();//生成したカードに名前を付ける
                card.Init(deckNumberList[init - 1]);//デッキデータの表示
            }
        }
    }

    /// <summary>
    /// デッキのカード枚数によって上下のCardPlaceに振り分ける数を決める
    /// </summary>
    /// <param name="deckCount">デッキの枚数</param>
    /// <returns>上のCardPlaceに生成するカードの枚数</returns>
    int DistributionOfCards(int deckCount) 
    {
        int distribute = 0;
        if (0 <= deckCount && deckCount <= 5)//デッキの数が0以上5枚以下だったら 
        {
            distribute = deckCount;//デッキの枚数分生成
        }
        else if (deckCount > 5)//デッキの数が6枚以上だったら
        {
            if (deckCount % 2 == 0)//デッキの枚数が偶数だったら
            {
                int value = deckCount / 2;
                distribute = value;//デッキの半分の枚数を生成
            }
            else //デッキの枚数が奇数だったら
            {
                int value = (deckCount - 1) / 2;
                distribute = value + 1;//デッキの半分+1の枚数を生成
            }
        }
        else //デッキの数が0枚未満だったら
        {
            distribute = 0;//生成しない
        }
        return distribute;
    }

    /// <summary>
    /// カードの強化をするメソッドです
    /// </summary>
    /// <param name="selectCard">選択されたCard</param>
    public void CardEnhance(GameObject selectCard)
    {
        Debug.Log("カード強化！");
        int id = selectCard.GetComponent<CardController>().cardDataManager._cardID; //選択されたカードのIDを取得
        for (int count = 0; count < deckNumberList.Count; count++)
        {
            if (deckNumberList[count] == id) //デッキからIDのカードを探す 
            {
                if(deckNumberList[count] <= 20 && id != 3) //IDのカードが未強化で魔女の霊薬でなければ
                {
                    deckNumberList[count] += 100; //そのカードの強化版のIDに変更する
                }
            }
            Debug.Log("現在のPlayerのデッキリストは：" + deckNumberList[count]);
        }
    }
}
