using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// ShopScene上のアイテムの生成、値段チェック、購入処理を管理します。
/// ToDo: HasHealPotion()メソッドの処理があんまり良くない感じがするので修正したい。
/// </summary>
public class ShopManager : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private UIManagerShop uiManager;
    [SerializeField] private SceneFader sceneFader;

    private const int healCardID = 3;                       // 回復カードのID
    private const int deckLimitIncRelicID = 1;              // デッキの上限を1枚増やすレリックのID
    private Vector3 scaleReset = Vector3.one * 0.37f;       // カードのデフォルトの大きさ
    private GameObject buyCard;                             // カードを買うときに一時的に格納

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


    [SerializeField] int tmpID = 0;      // デバッグ用

    private void Start()
    {
        // GameManager取得(変数名省略)
        gm = GameManager.Instance;

        ShopLottery();
        shopCardsID.Add(healCardID);                        // 回復カードを追加
        shopRelicsID.Insert(0, deckLimitIncRelicID);        // デッキの上限を1枚増やすレリックを追加
        //Debug.Log("レリック1:   " + shopRelicsID[0] + "\nレリック2:   " + shopRelicsID[1] + "\nレリック3:  " + shopRelicsID[2]);

        // ショップに並ぶアイテムを表示
        ShowItem();

        uiManager.UIEventsReload();          // UIEvent更新
    }

    void Update()
    {


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

        //    uiManager.UIEventsReload();          // UIEvent更新      
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

        //if (Input.GetKeyDown(KeyCode.RightAlt))
        //{
        //    tmpID++;
        //    if(tmpID == 12)
        //    {
        //        tmpID = 0;
        //    }
        //    shopRelicsID[2] = tmpID;

        //    ShowItem();
        //    uiManager.UIEventsReload();
        //}

    }

    /// <summary>
    /// Lotteryスクリプトから抽選したカードIDを受け取るメソッド
    /// </summary>
    void ShopLottery()
    {
        //(Card1, Card2, Card3) = lottery.SelectCardByRarity(new int[] { 2, 1, 1 });     // メモ: タプルと言って複数の戻り値を受け取れる
        shopCardsID = Lottery.Instance.SelectCardByRarity(new List<int> { 2, 1, 1 },true);
        shopRelicsID = Lottery.Instance.SelectRelicByRarity(new List<int> { 2, 1 });

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
            CardController cardController = cardObject.GetComponent<CardController>();                                              // 生成したPrefabのCardControllerを取得
            cardController.Init(shopCardsID[cardID]);                                                                               // 取得したCardControllerのInitメソッドを使いカードの生成と表示をする
            cardObject.transform.Find("PriceBackGround").gameObject.SetActive(true);                                                // 値札を表示
            shopCards.Add(cardObject);
        }

        // レリック表示
        for (int relicID = 0; relicID < shopRelicsID.Count; relicID++)
        {
            GameObject relicObject = Instantiate(relicPrefab, relicPlace[relicID].transform.position, relicPlace[relicID].transform.rotation);     // レリックのPrefabを生成
            relicObject.transform.SetParent(shoppingUI.transform);                                                                      // shoppingUIの子にする
            RelicController relicController = relicObject.GetComponent<RelicController>();                                              // 生成したPrefabのRelicControllerを取得
            relicController.Init(shopRelicsID[relicID]);                                                                                // 取得したRelicControllerのInitメソッドを使いレリックの生成と表示をする
            relicObject.transform.Find("RelicPriceBG").gameObject.SetActive(true);                                                      // 値札を表示
            shopRelics.Add(relicObject);
        }
    }

    /// <summary>
    /// 値段テキストを更新します。
    /// アイテムを買えるかどうかを判定し、
    /// 買えない場合値段を赤く表示します
    /// </summary>
    public void PriceTextCheck()
    {
        // カードの値段チェック
        for (int i = 0; i < shopCards.Count; i++)
        {
            CardController card = shopCards[i].GetComponent<CardController>();
            if (gm.playerData._playerMoney >= card.cardDataManager._cardPrice)     // 所持金が足りるなら
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
            if (gm.playerData._playerMoney >= relic.relicDataManager._relicPrice)
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
        foreach(int cardsID in gm.playerData._deckList)
        {
            if(cardsID == healCardID)      // 回復カードを持っている場合
            {
                // 回復カードをグレーアウトにする
                shopCards[healCardNum].transform.GetChild(1).GetComponent<Image>().color = Color.gray;        // 正直あまりいい書き方ではないので修正したい
                return true;
            }
        }
        // 回復カードをハイライトにする
        shopCards[healCardNum].transform.GetChild(1).GetComponent<Image>().color = Color.white;
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
        {
            CardController card = selectedItem.GetComponent<CardController>();
            int selectedCardID = card.cardDataManager._cardID; // 選択されたカードのIDを取得

            if (gm.playerData._playerMoney >= card.cardDataManager._cardPrice)           // 所持金が足りるなら
            {
                if (selectedCardID != healCardID)   // 選んだカードが回復カードではなかった場合
                {
                    // デッキ上限チェック
                    if (gm.CheckDeckFull())     // デッキ上限に達している場合
                    {
                        gm.OnCardDiscard += RetryBuyItem;   // カードを破棄した後、もう一度メソッドを呼ぶためにデリゲートに追加

                        buyCard = selectedItem;             // カード破棄画面に移るため一時的に格納

                        // アイテムの見た目の選択状態を解除
                        selectedItem.transform.localScale = scaleReset;
                        selectedItem.transform.GetChild(0).gameObject.SetActive(false);
                        // 選択状態リセット
                        uiManager.lastSelectedItem = null;
                        uiManager.isSelected = false;
                        return;
                    }


                    AudioManager.Instance.PlaySE("買い物");
                    gm.playerData._playerMoney -= card.cardDataManager._cardPrice;       // 所持金から値段分のお金を引いて
                    gm.AddCard(selectedCardID);                         // デッキに加える

                    selectedItem.SetActive(false);
                }
                else if (!HasHealPotion())          // 選んだカードが回復カードで、回復カードを所持していない場合
                {
                    AudioManager.Instance.PlaySE("買い物");
                    gm.playerData._playerMoney -= card.cardDataManager._cardPrice;       // 所持金から値段分のお金を引いて
                    gm.AddCard(selectedCardID);                         // デッキに加える

                    selectedItem.transform.localScale = scaleReset;                     // スケールを戻す
                }
            }
            HasHealPotion();        // 回復カードの見た目を更新
        }


        if (itemType == "Relic")
        {
            RelicController relic = selectedItem.GetComponent<RelicController>();
            int selectedRelicID = relic.relicDataManager._relicID; // 選択されたカードのIDを取得     

            if (gm.playerData._playerMoney >= relic.relicDataManager._relicPrice)         // 所持金が足りるなら
            {
                AudioManager.Instance.PlaySE("買い物");
                gm.playerData._playerMoney -= relic.relicDataManager._relicPrice;         // 所持金から値段分のお金を引いて
                gm.hasRelics[selectedRelicID]++;                                          // レリックを取得

                selectedItem.SetActive(false);
            }
        }

        gm.ShowRelics();        // オーバーレイのレリック表示を更新
    }


    /// <summary>
    /// カード破棄画面でカードを破棄した後呼び出されるメソッドです。
    /// </summary>
    public void RetryBuyItem()
    {
        BuyItem(buyCard, "Card");
        buyCard = null;                     // 一時的に格納していただけなのでnullにします。
    }

    /// <summary>
    /// 店から出る処理です。ショップシーンを非表示にします。
    /// </summary>
    public void ExitShop()
    {
        // フェードインフェードアウトをし、シーンを非表示に
        sceneFader.ToggleSceneWithFade("ShopScene", false);
    }
}
