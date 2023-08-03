using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ShopScene上のアイテムの生成、値段チェック、購入処理を管理します。
/// ToDo: HasHealPotion()メソッドの処理があんまり良くない感じがするので修正したい。
/// </summary>
public class ShopController : MonoBehaviour
{
    PlayerDataManager playerData;
    CardController cardController;
    RelicController relicController;
    RelicStatus relicStatus;

    [SerializeField] Lottery lottery;
    [SerializeField] UIManagerShopScene uiManager;

    const int healCardID = 3;                       // 回復カードのID
    const int deckLimitIncRelicID = 1;              // デッキの上限を1枚増やすレリックのID
    Vector3 scaleReset = Vector3.one * 0.37f;       // カードのデフォルトの大きさ

    [Header("参照するUI")]
    [SerializeField] GameObject shoppingUI;

    [Header("ショップに並ぶアイテムのPrefab")]
    [SerializeField] GameObject cardPrefab;
    [SerializeField] GameObject relicPrefab;

    [Header("各Prefabの生成位置")]
    [SerializeField] List<GameObject> cardPlace;     // Cardの生成位置
    [SerializeField] List<GameObject> relicPlace;    // Relicの生成位置


    [Header("ここから下はデバッグ用に表示させてます")]
    /// <summary>
    /// ショップに出ているカードのIDを格納します
    /// </summary>
    [SerializeField] List<int> shopCardsID = null;

    /// <summary>
    /// ショップに出ているレリックのIDを格納します
    /// </summary>
    [SerializeField] List<int> shopRelicsID = null;

    /// <summary>
    /// カードの値段を表示するために必要なオブジェクトを格納します
    /// </summary>
    [SerializeField] List<GameObject> shopCards = null;

    /// <summary>
    /// レリックの値段を表示するために必要なオブジェクトを格納します
    /// </summary>
    [SerializeField] List<GameObject> shopRelics = null;


    [SerializeField]    int tmpID = 0;      // デバッグ用

    private void Start()
    {
        playerData = GameManager.Instance.playerData;
        relicStatus = new RelicStatus();                       //後でGameManagerにプレイヤーのRelicStatusを作成して参照します
    }

    void Update()
    {

        if (Lottery.isInitialize)
        {
            ShopLottery();
            shopCardsID.Add(healCardID);                        // 回復カードを追加
            shopRelicsID.Insert(0, deckLimitIncRelicID);        // デッキの上限を1枚増やすレリックを追加
            Debug.Log("レリック1:   " + shopRelicsID[0] + "\nレリック2:   " + shopRelicsID[1] + "\nレリック3:  " + shopRelicsID[2]);

            // ショップに並ぶアイテムを表示
            ShowItem();
            
            uiManager.UIEventReload();          // UIEvent更新      
            Lottery.isInitialize = false;
        }

        //if (Input.GetKeyDown(KeyCode.RightAlt))
        //{
        //    tmpID++;
        //    if(tmpID == 12)
        //    {
        //        tmpID = 0;
        //    }
        //    shopRelicsID[2] = tmpID;

        //    ShowItem();
        //    uiManager.UIEventReload();
        //}

        //// デバッグ用
        //if (Input.GetKeyDown(KeyCode.Space))
        //{    
        //    shopCardsID = lottery.SelectCardByRarity(new List<int> { 2, 1, 1 });
        //    shopCardsID.Add(healCardID);                        // 回復カードを追加
        //    Debug.Log("カード1:    " + shopCardsID[0] + "\nカード2:   " + shopCardsID[1] + "\nカード3:   " + shopCardsID[2]);

        //    shopRelicsID = lottery.SelectRelicByRarity(new List<int> { 2, 1 });
        //    shopRelicsID.Insert(0, deckLimitIncRelicID);        // デッキの上限を1枚増やすレリックを追加
        //    Debug.Log("レリック1:   " + shopRelicsID[0] + "\nレリック2:   " + shopRelicsID[1] + "\nレリック3:  " + shopRelicsID[2]);

        //    // ショップに並ぶアイテムを表示
        //    ShowItem();

        //    uiManager.UIEventReload();          // UIEvent更新      
        //    Lottery.isInitialize = false;
        //}

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
        shopRelicsID = lottery.SelectRelicByRarity(new List<int> { 2, 1 });

    }

    /// <summary>
    /// ショップに並ぶアイテムの表示を行います。
    /// </summary>
    void ShowItem()
    {
        // カード表示
        for (int cardID = 0; cardID < shopCardsID.Count; cardID++)
        {
            GameObject cardObject = Instantiate(cardPrefab, cardPlace[cardID].transform.position, cardPlace[cardID].transform.rotation);       // カードのPrefabを生成
            cardObject.transform.SetParent(shoppingUI.transform);                                                                   // shoppingUIの子にする
            cardController = cardObject.GetComponent<CardController>();                                                             // 生成したPrefabのCardControllerを取得
            cardController.Init(shopCardsID[cardID]);                                                                               // 取得したCardControllerのInitメソッドを使いカードの生成と表示をする
            cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);                                                // 値札を表示
            shopCards.Add(cardObject);
        }

        // レリック表示
        for (int relicID = 0; relicID < shopRelicsID.Count; relicID++)
        {
            GameObject relicObject = Instantiate(relicPrefab, relicPlace[relicID].transform.position, relicPlace[relicID].transform.rotation);     // レリックのPrefabを生成
            relicObject.transform.SetParent(shoppingUI.transform);                                                                      // shoppingUIの子にする
            relicController = relicObject.GetComponent<RelicController>();                                                              // 生成したPrefabのRelicControllerを取得
            relicController.Init(shopRelicsID[relicID]);                                                                                // 取得したRelicControllerのInitメソッドを使いレリックの生成と表示をする
            relicObject.transform.Find("RelicPriceBG").gameObject.SetActive(true);                                                      // 値札を表示
            shopRelics.Add(relicObject);
        }
    }

    /// <summary>
    /// 値段テキストを更新します。
    /// アイテムを買えるかどうかを判定し、
    /// 変えなかった場合値段を赤く表示します
    /// </summary>
    public void PriceTextCheck()
    {
        // カードの値段チェック
        for (int i = 0; i < shopCards.Count; i++)
        {
            CardController card = shopCards[i].GetComponent<CardController>();
            if (playerData._playerMoney >= card.cardDataManager._cardPrice)     // 所持金が足りるなら
            {
                TextMeshProUGUI textComponent = shopCards[i].transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();       // Price表示テキストを取得
                textComponent.color = Color.white;                                                                                    // 白で表示
            }
            else
            {
                TextMeshProUGUI textComponent = shopCards[i].transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();       // Price表示テキストを取得
                textComponent.color = Color.red;                                                                                      // 赤で表示
            }

        }

        // レリックの値段チェック
        for (int i = 0; i < shopRelics.Count; i++)
        {
            RelicController relic = shopRelics[i].GetComponent<RelicController>();
            if (playerData._playerMoney >= relic.relicDataManager._relicPrice)
            {
                TextMeshProUGUI textComponent = shopRelics[i].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
                textComponent.color = Color.white;
            }
            else
            {
                TextMeshProUGUI textComponent = shopRelics[i].transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
                textComponent.color = Color.red;
            }
        }
    }

    int healCardNum = 3;
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
                // 回復カードをグレーアウトにする
                shopCards[healCardNum].transform.GetChild(1).GetComponent<Image>().color = Color.gray;        // 正直あまりいい書き方ではないので修正したい
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// アイテムを買う処理です
    /// </summary>
    /// <param name="selectedItem">クリックしたUIObject</param>
    /// <param name="itemType">CardまたはRelicを指定</param>
    public void BuyItem(GameObject selectedItem, string itemType)
    {
        if (itemType == "Card")
        for (int i = 0; i < shopCards.Count; i++)
        {
            if (selectedItem == shopCards[i])         // クリックしたカードとショップに並んでるカードが一致したら
            {
                CardController card = shopCards[i].GetComponent<CardController>();

                if (playerData._playerMoney >= card.cardDataManager._cardPrice)           // 所持金が足りるなら
                {
                    if (shopCardsID[i] != shopCardsID[healCardID])                        // 選んだカードが回復カードではなかった場合
                    {
                        playerData._playerMoney -= card.cardDataManager._cardPrice;       // 所持金から値段分のお金を引いて
                        playerData._deckList.Add(shopCardsID[i]);                         // デッキに加える

                        selectedItem.SetActive(false);

                    } else if (!HasHealPotion())   // 選んだカードが回復カードで、回復カードを所持していない場合
                    {
                        playerData._playerMoney -= card.cardDataManager._cardPrice;
                        playerData._deckList.Add(shopCardsID[i]);


                        // 回復カードをグレーアウトにする
                        selectedItem.transform.GetChild(1).GetComponent<Image>().color = Color.gray;        // 正直あまりいい書き方ではないので修正したい
                        selectedItem.transform.localScale = scaleReset;
                    }
                }               
            }
        }

        if (itemType == "Relic")
        for (int i = 0; i < shopRelics.Count; i++)
        {
            if (selectedItem == shopRelics[i])         // クリックしたレリックとショップに並んでるレリックが一致したら
            {
                RelicController relic = shopRelics[i].GetComponent<RelicController>();

                if (playerData._playerMoney >= relic.relicDataManager._relicPrice)          // 所持金が足りるなら
                {
                    playerData._playerMoney -= relic.relicDataManager._relicPrice;          // 所持金から値段分のお金を引いて
                    AddRelicStatus(shopRelicsID[i]);                                        //レリックステータスに加える
                    //playerData._relicList.Add(shopRelicsID[i]);                             // レリックリストに加える
                    selectedItem.transform.localScale = scaleReset;                           // スケールを戻す

                    selectedItem.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// レリックを追加する処理です
    /// </summary>
    /// <param name="relicID">レリックのID</param>
    void AddRelicStatus(int relicID)
    {
        switch (relicID) //レリックのIDに応じてrelicStatusに1つ追加する
        {
            case 1:
                relicStatus.hasRelicID1++;
                break;
            case 2:
                relicStatus.hasRelicID2++;
                break;
            case 3:
                relicStatus.hasRelicID3++;
                break;
            case 4:
                relicStatus.hasRelicID4++;
                break;
            case 5:
                relicStatus.hasRelicID5++;
                break;
            case 6:
                relicStatus.hasRelicID6++;
                break;
            case 7:
                relicStatus.hasRelicID7++;
                break;
            case 8:
                relicStatus.hasRelicID8++;
                break;
            case 9:
                relicStatus.hasRelicID9++;
                break;
            case 10:
                relicStatus.hasRelicID10++;
                break;
            case 11:
                relicStatus.hasRelicID11++;
                break;
            case 12:
                relicStatus.hasRelicID12++;
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

}
