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

    // 参照するUI
    [SerializeField] GameObject shoppingUI;
    [SerializeField] GameObject rest;

    const int healCardID = 3;   // 回復カードのID
    const int restPrice = 70;   // 休憩の値段

    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject Canvas;
    [SerializeField] List<GameObject> cardPlace;     // Cardの生成位置

    GameObject cardObject;  // 生成したカードIDを格納する

    /// <summary>
    /// ショップに出ているカードのIDを格納します
    /// </summary>
    [SerializeField] List<int> shopCardsID = null;

    /// <summary>
    /// カードの値段を表示するために必要なオブジェクトを格納します
    /// </summary>
    [SerializeField] List<GameObject> shopCards = null;


    Vector3 scaleReset = Vector3.one * 0.37f;     // カードのデフォルトの大きさ
 
    [SerializeField]
    int tmpID = 0;      // デバッグ用

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
        cardObject.transform.SetParent(shoppingUI.transform);                                                                   // Canvasの子にする
        cardController = cardObject.GetComponent<CardController>();                                                             // 生成したPrefabのCardControllerを取得
        cardController.Init(shopCardsID[cardID]);                                                                               // 取得したCardControllerのInitメソッドを使いカードの生成と表示をする
        cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);                                                // 値札を表示
        shopCards.Add(cardObject);
    }

    /// <summary>
    /// カードを買えるかどうかを判定し、
    /// 変えなかった場合値段を赤く表示します
    /// </summary>
    void PriceCheck()
    {
        //所持金チェック
        for (int i = 0; i < shopCards.Count; i++)
        {
            CardController card = shopCards[i].GetComponent<CardController>();
            if (playerData._money >= card.cardDataManager._cardPrice)     // 所持金が足りるなら
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

    /// <summary>
    /// 休憩できるか判定します
    /// </summary>
    /// <returns>休憩できるできる場合true</returns>
    public bool CheckRest()
    {
        if (playerData._playerHP == playerData._playerCurrentHP)     // 現在のHPがMAXの場合
        {
            rest.GetComponent<Image>().color = Color.gray;
            return false;
        }
        if (playerData._money < restPrice)          // お金が足りない場合
        {
            rest.GetComponent<Image>().color = Color.gray;
            ///  プライステキストを赤く表示
            return false;
        }
        return true;

    }

    public void Rest()
    {
        if (playerData._playerHP == playerData._playerCurrentHP)
        {
            
        }
    }


    /// <summary>
    /// 魔女の霊薬を持っているか判定します
    /// </summary>
    /// <returns>持っている場合trueを返します</returns>
    public bool HasHealPotion()
    {
        foreach(int cardsID in playerData._deckList)
        {
            if(cardsID == shopCardsID[healCardID])      // 回復カードを持っている場合
            {
                return true;
            }
        }
        return false;
    }
    
    public void BuyCards(GameObject selectCard)
    {
        for (int i = 0; i < shopCards.Count; i++)
        {
            if (selectCard == shopCards[i])         // クリックしたカードとショップに並んでるカードが一致したら
            {
                CardController card = shopCards[i].GetComponent<CardController>();

                if (playerData._money >= card.cardDataManager._cardPrice)           // 所持金が足りるなら
                {
                    if (shopCardsID[i] != shopCardsID[healCardID])                  // 選んだカードが回復カードではなかった場合
                    {
                        playerData._money -= card.cardDataManager._cardPrice;       // 所持金から値段分のお金を引いて
                        playerData._deckList.Add(shopCardsID[i]);                   // デッキに加える

                        selectCard.GetComponent<Image>().color = Color.gray;        // 買ったカードを暗くする
                        selectCard.transform.localScale = scaleReset;               // スケールを戻す

                        selectCard.SetActive(false);

                    } else if (!HasHealPotion())   // 選んだカードが回復カードで、回復カードを所持していない場合
                    {
                        playerData._money -= card.cardDataManager._cardPrice;
                        playerData._deckList.Add(shopCardsID[i]);

                        selectCard.GetComponent<Image>().color = Color.gray;
                        selectCard.transform.localScale = scaleReset;
                    }
                }
                    
            }
        }


    }


}
