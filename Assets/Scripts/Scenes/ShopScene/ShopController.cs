using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    //PlayerDataManager playerDataManager;
    CardDataManager cardData;       // カードのデータを処理
    CardController cardController;

    [SerializeField]
    Lottery lottery;
    [SerializeField]
    UIManagerShopScene uiManager;


    int heelPotionCardID = 3;   //回復カードのID
    bool isHeelPotion = false;  //回復カードを持っているか？

    [SerializeField]
    GameObject cardPrefab;
    [SerializeField]
    GameObject Canvas;
    [SerializeField]
    List<GameObject> cardPlace;     // Cardの生成位置


    GameObject cardObject;
    int[] ShopCards;



    [SerializeField]
    int tmpID = 0;      //デバッグ用


    void Update()
    {

        if (Lottery.isInitialize)
        {
            ShopLottery();

            // ショップに並ぶカード表示
            for(int i = 0; i < ShopCards.Length; i++)
            {
                cardObject = Instantiate(cardPrefab, cardPlace[i].transform.position, cardPlace[i].transform.rotation);       // カードのPrefabを生成
                cardObject.transform.SetParent(Canvas.transform);                                                             // Canvasの子にする
                cardController = cardObject.GetComponent<CardController>();                                                   // 生成したPrefabのCardControllerを取得
                cardController.Init(ShopCards[i]);                                                                            // 取得したCardControllerのInitメソッドを使いカードの生成と表示をする
                cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);                                      // 値札を表示
            }

            // 回復カードは固定なので別途表示
            cardObject = Instantiate(cardPrefab, cardPlace[3].transform.position, cardPlace[3].transform.rotation);
            cardObject.transform.SetParent(Canvas.transform);
            cardController = cardObject.GetComponent<CardController>();
            cardController.Init(heelPotionCardID);
            cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);

            uiManager.UIEventReload();
        }

        if (Input.GetKeyDown(KeyCode.Space))        // Spaceを押すごとに次のCardIDのカードが表示される
        {
            if (tmpID >= 20)
                tmpID = 0;

            tmpID++;

            cardController.Init(tmpID);
            //DebugLottery();
        }

    }


    // Lotteryスクリプトから抽選したカードIDを受け取るメソッド
    void ShopLottery()
    {
        lottery.fromShopController = true;
        //(Card1, Card2, Card3) = lottery.SelectCardByRarity(new int[] { 2, 1, 1 });     // メモ: タプルと言って複数の戻り値を受け取れる
        ShopCards = lottery.SelectCardByRarity(new int[] { 2, 1, 1 });
        Debug.Log("カード1:" + ShopCards[0] + "\nカード2:" + ShopCards[1] + "\nカード3:" + ShopCards[2]);

        Lottery.isInitialize = false;
    }








    private void DebugLottery()
    {
        lottery.fromShopController = true;
        ShopCards = lottery.SelectCardByRarity(new int[] { 2, 1, 1 });
        Debug.Log("カード1:" + ShopCards[0] + "\nカード2:" + ShopCards[1] + "\nカード3:" + ShopCards[2]);

        // ショップに並ぶカード表示
        for (int i = 0; i < ShopCards.Length; i++)
        {
            cardObject = Instantiate(cardPrefab, cardPlace[i].transform.position, cardPlace[i].transform.rotation);       // カードのPrefabを生成
            cardObject.transform.SetParent(Canvas.transform);                                                                   // Canvasの子にする
            cardController = cardObject.GetComponent<CardController>();                                                         // 生成したPrefabのCardControllerを取得
            cardController.Init(ShopCards[i]);                                                                                  // 取得したCardControllerのInitメソッドを使いカードの生成と表示をする

        }

        // 回復カードは固定なので別途表示
        cardObject = Instantiate(cardPrefab, cardPlace[3].transform.position, cardPlace[3].transform.rotation);
        cardObject.transform.SetParent(Canvas.transform);
        cardController = cardObject.GetComponent<CardController>();
        cardController.Init(heelPotionCardID);

        uiManager.UIEventReload();
    }
}
