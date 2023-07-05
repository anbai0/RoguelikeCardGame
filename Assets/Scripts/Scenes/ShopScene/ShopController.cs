using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    PlayerDataManager playerDataManager;
    CardDataManager cardData;       // カードのデータを処理
    CardController cardController;
    public Lottery lottery;

    int Card1, Card2, Card3;    //抽選されたカードのIDを入れる変数
    int heelPotionCardID = 3;   //回復カードのID

    bool isHeelPotion = false;  //回復カードを持っているか？


    [SerializeField]
    Transform cardPlace1, cardPlace2, cardPlace3;
    [SerializeField]
    Object cardPrefab;
    
    private CardController SelectController;    //生成したカードPrefabにアタッチされているCardControllerを格納する

    [SerializeField]
    int tmpID = 0;

    private void Start()
    {
        Object card1 = Instantiate(cardPrefab, cardPlace1);
        Object card2 = Instantiate(cardPrefab, cardPlace2);
        Object card3 = Instantiate(cardPrefab, cardPlace3);




    }



    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    (Card1, Card2, Card3) = lottery.ShopLottery();     // メモ: タプルと言って複数の戻り値を受け取れる
        //    Debug.Log("カード1:" + Card1 + "\nカード2:" + Card2 + "\nカード3:" + Card3);
        //}


        if (Lottery.isInitialize)
        {
            lottery1();

            Card1.

            CardController card = SelectController.GetComponent<CardController>();//カードを生成する
            card.Init(Card1);

        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(tmpID >= 21)
                tmpID = 0;
            
            tmpID++;

            CreateCard(tmpID);
        }

    }

    void CreateCard(int ID)
    {
        CardController card = SelectController.GetComponent<CardController>();//カードを生成する
        card.Init(ID);
    }

    void lottery1()
    {
        (Card1, Card2, Card3) = lottery.ShopLottery();     // メモ: タプルと言って複数の戻り値を受け取れる
        Debug.Log("カード1:" + Card1 + "\nカード2:" + Card2 + "\nカード3:" + Card3);

        Lottery.isInitialize = false;
    }
    
}
