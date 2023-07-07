using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    //PlayerDataManager playerDataManager;
    CardDataManager cardData;       // カードのデータを処理
    CardController cardController;
    public Lottery lottery;
    [SerializeField]
    UIManagerShopScene uiManager;

    int Card1, Card2, Card3;    //抽選されたカードのIDを入れる変数
    int heelPotionCardID = 3;   //回復カードのID

    bool isHeelPotion = false;  //回復カードを持っているか？


    [SerializeField]
    GameObject Canvas, cardPlace1, cardPlace2, cardPlace3, cardPlace4;  //CanvasとCardの生成位置
    [SerializeField]
    GameObject cardPrefab;

    GameObject card1, card2, card3, card4;
    Sprite cardImage;


    private CardController SelectController;    //生成したカードPrefabにアタッチされているCardControllerを格納する


    CardController selectCard1, selectCard2, selectCard3, selectCard4;



    [SerializeField]
    int tmpID = 0;

    private void Start()
    {





    }



    void Update()
    {

        if (Lottery.isInitialize)
        {
            lottery1();


            //左のカードの生成
            card1 = Instantiate(cardPrefab, cardPlace1.transform.position, cardPlace1.transform.rotation);                  //カードのPrefabを生成
            card1.transform.SetParent(Canvas.transform);
            selectCard1 = card1.GetComponent<CardController>();                        //生成したPrefabのCardControllerを取得
            selectCard1.Init(Card1);                                                   //そのCardControllerのInitメソッドを使いカードの生成と表示をする

            //真ん中のカードの生成
            card2 = Instantiate(cardPrefab, cardPlace2.transform.position, cardPlace2.transform.rotation);
            card2.transform.SetParent(Canvas.transform);
            selectCard2 = card2.GetComponent<CardController>();
            selectCard2.Init(Card2);

            //右のカードの生成
            card3 = Instantiate(cardPrefab, cardPlace3.transform.position, cardPlace3.transform.rotation);
            card3.transform.SetParent(Canvas.transform);
            selectCard3 = card3.GetComponent<CardController>();
            selectCard3.Init(Card3);

            //回復カードの生成
            card4 = Instantiate(cardPrefab, cardPlace4.transform.position, cardPlace4.transform.rotation);
            card4.transform.SetParent(Canvas.transform);
            selectCard4 = card4.GetComponent<CardController>();
            selectCard4.Init(heelPotionCardID);

            uiManager.ReloadUI();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (tmpID >= 20)
                tmpID = 0;

            tmpID++;

            selectCard1.Init(tmpID);
        }

    }



    void lottery1()
    {
        (Card1, Card2, Card3) = lottery.ShopLottery(2,1,1);     // メモ: タプルと言って複数の戻り値を受け取れる
        Debug.Log("カード1:" + Card1 + "\nカード2:" + Card2 + "\nカード3:" + Card3);

        Lottery.isInitialize = false;
    }
    
}
