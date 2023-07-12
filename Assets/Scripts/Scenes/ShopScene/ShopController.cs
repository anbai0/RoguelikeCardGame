using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    PlayerDataManager playerData;
    CardController cardController;

    [SerializeField] Lottery lottery;
    [SerializeField] UIManagerShopScene uiManager;


    const int healCardID = 3;   //回復カードのID
    [SerializeField] bool hasHealPotion = false;  //回復カードを持っているか？

    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject Canvas;
    [SerializeField] List<GameObject> cardPlace;     // Cardの生成位置

    GameObject cardObject;  // 生成したカードIDを格納する
    [SerializeField] List<int> shopCardsID = null;

    // カードの値段を表示するために必要なオブジェクトを格納する
    [SerializeField] List<GameObject> shopCards = null;


    [SerializeField]
    int tmpID = 0;      //デバッグ用

    private void Start()
    {
        playerData = GameManager.Instance.playerData;

    }

    void Update()
    {

        if (Lottery.isInitialize)
        {
            ShopLottery();
            shopCardsID.Add(healCardID);       // 回復カードを追加

            // ショップに並ぶカード表示
            for (int i = 0; i < shopCardsID.Count; i++)
            {
                CardsShow(i);
            }

            uiManager.UIEventReload();          // UI(カード)を表示
            Lottery.isInitialize = false;
        }

        PriceCheck();
        PotionCheck();



        //if (Input.GetKeyDown(KeyCode.Space))        // Spaceを押すごとに次のCardIDのカードが表示される
        //{
        //    if (tmpID >= 20)
        //        tmpID = 0;

        //    tmpID++;

        //    cardController.Init(tmpID);
        //    //DebugLottery();
        //}

    }

    /// <summary>
    /// Lotteryスクリプトから抽選したカードIDを受け取るメソッド
    /// </summary>
    void ShopLottery()
    {
        lottery.fromShopController = true;
        //(Card1, Card2, Card3) = lottery.SelectCardByRarity(new int[] { 2, 1, 1 });     // メモ: タプルと言って複数の戻り値を受け取れる
        shopCardsID = lottery.SelectCardByRarity(new List<int> { 2, 1, 1 });
        Debug.Log("カード1:" + shopCardsID[0] + "\nカード2:" + shopCardsID[1] + "\nカード3:" + shopCardsID[2]);

    }

    /// <summary>
    /// カードの表示を行います。
    /// </summary>
    /// <param name="cardID">表示したいカードのID</param>
    void CardsShow(int cardID)
    {
        cardObject = Instantiate(cardPrefab, cardPlace[cardID].transform.position, cardPlace[cardID].transform.rotation);       // カードのPrefabを生成
        cardObject.transform.SetParent(Canvas.transform);                                                                       // Canvasの子にする
        cardController = cardObject.GetComponent<CardController>();                                                             // 生成したPrefabのCardControllerを取得
        cardController.Init(shopCardsID[cardID]);                                                                              // 取得したCardControllerのInitメソッドを使いカードの生成と表示をする
        cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);                                                // 値札を表示
        shopCards.Add(cardObject);
    }

    /// <summary>
    /// カードを買えるかどうかを判定します
    /// </summary>
    void PriceCheck()
    {
        //所持金チェック
        for (int i = 0; i < shopCards.Count; i++)
        {
            CardController card = shopCards[i].GetComponent<CardController>();
            if (playerData._money > card.cardDataManager._cardPrice)     // 所持金が足りるなら
            {
                Text textComponent = shopCards[i].transform.GetChild(3).GetChild(0).GetComponent<Text>();       // Price表示テキストを取得
                textComponent.color = Color.white;                                                              // 白で表示
            }
            else
            {
                Text textComponent = shopCards[i].transform.GetChild(3).GetChild(0).GetComponent<Text>();       // Price表示テキストを取得
                textComponent.color = Color.red;                                                                // 赤で表示
            }

        }
    }
    
    public void PotionCheck()
    {
        foreach(int cardsID in playerData._deckList)
        {
            if(cardsID == shopCardsID[healCardID])      // 回復カードを持っている場合
            {
                hasHealPotion = true;
            }
        }
    }
    
    public void BuyCards(GameObject selectCard)
    {
        for (int i = 0; i < shopCards.Count; i++)
        {
            if (selectCard == shopCards[i])         // クリックしたカードとショップに並んでるカードが一致したら
            {
                CardController card = shopCards[i].GetComponent<CardController>();

                if (playerData._money > card.cardDataManager._cardPrice)            // 所持金が足りるなら
                {
                    if (shopCardsID[i] != shopCardsID[healCardID])                  // 選んだカードが回復カードではなかった場合
                    {
                        playerData._money -= card.cardDataManager._cardPrice;       // 所持金から値段分のお金を引いて
                        playerData._deckList.Add(shopCardsID[i]);                   // デッキに加える
                        selectCard.SetActive(false);

                    } else if (!hasHealPotion)   // 選んだカードが回復カードで、回復カードを所持していない場合
                    {
                        playerData._money -= card.cardDataManager._cardPrice;
                        playerData._deckList.Add(shopCardsID[i]);
                    }
                }
                    
            }
        }


    }



    /// <summary>
    /// カード抽選のデバッグ用です。
    /// </summary>
    private void DebugLottery()
    {
        lottery.fromShopController = true;
        shopCardsID = lottery.SelectCardByRarity(new List<int> { 2, 1, 1 });
        Debug.Log("カード1:" + shopCardsID[0] + "\nカード2:" + shopCardsID[1] + "\nカード3:" + shopCardsID[2]);

        // ショップに並ぶカード表示
        for (int i = 0; i < shopCardsID.Count; i++)
        {
            cardObject = Instantiate(cardPrefab, cardPlace[i].transform.position, cardPlace[i].transform.rotation);       // カードのPrefabを生成
            cardObject.transform.SetParent(Canvas.transform);                                                                   // Canvasの子にする
            cardController = cardObject.GetComponent<CardController>();                                                         // 生成したPrefabのCardControllerを取得
            cardController.Init(shopCardsID[i]);                                                                                  // 取得したCardControllerのInitメソッドを使いカードの生成と表示をする

        }

        // 回復カードは固定なので別途表示
        cardObject = Instantiate(cardPrefab, cardPlace[3].transform.position, cardPlace[3].transform.rotation);
        cardObject.transform.SetParent(Canvas.transform);
        cardController = cardObject.GetComponent<CardController>();
        cardController.Init(healCardID);

        uiManager.UIEventReload();
    }
}
