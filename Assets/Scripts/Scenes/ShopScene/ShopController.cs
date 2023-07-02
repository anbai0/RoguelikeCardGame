using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    PlayerDataManager playerDataManager;
    CardController cardController;

    Lottery lottery;

    bool isHeelPotion = false;

    private void Start()
    {
        //playerDataManager = GameManager.Instance.playerData;
        //foreach (var deck in playerDataManager._deckList)
        //{
        //    if (deck == 3)
        //    {
        //        isHeelPotion = true;
        //    }

        //}

        ////プレイヤーのデッキを作成
        //deckNumberList = playerData._deckList;
        //for (int init = 0; init < deckNumberList.Count; init++)// デッキの枚数分
        //{
        //    CardController card = Instantiate(cardPrefab, CardPlace);//カードを生成する
        //    card.name = "Deck" + init.ToString();//生成したカードに名前を付ける
        //    card.Init(deckNumberList[init]);//デッキデータの表示
        //}

    }
    void Update()
    {


        //playerDataManager._money -= 50;
        //playerDataManager._deckList.Add(1);

    }

    public void ShowCards()
    {
        (int Card1, int Card2, int Card3) = lottery.ShopLottery();     // メモ: タプルと言って複数の戻り値を受け取れる
        cardController.Init(rare2); 
        
    }
}
