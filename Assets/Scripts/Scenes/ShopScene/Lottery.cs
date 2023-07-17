using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery : MonoBehaviour
{
    PlayerDataManager playerData;   // プレイヤーデータのインスタンスを格納
    CardDataManager cardData;       // カードのデータを処理
    RelicDataManager relicData;     // レリックのデータを処理

    //各レアリティのリスト
    [SerializeField] List<int> cardRarity1List;
    [SerializeField] List<int> cardRarity2List;
    [SerializeField] List<int> cardRarity3List;
    [SerializeField] List<int> relicRarity1List;
    [SerializeField] List<int> relicRarity2List;

    public List<int> shopCards = new List<int>();       // ショップに追加されたカード
    List<int> deckAndShopCards;                         // 現在の所持カード＋ショップに追加されたカードを格納する
    public List<int> shopRelics = new List<int>();      // ショップに追加されたレリック 
    List<int> myRelicAndShopRelics;                     // 現在の所持レリック＋ショップに追加されたレリックを格納する

    public static bool isInitialize = false;

    int MaxNumCards = 20;       //カード枚数
    int MaxNumRelics = 11;

    public bool fromShopController = false;

    void Start()
    {
        for (int i = 1; i <= MaxNumCards; i++)
        {
            cardData = new CardDataManager(i);

            // 各カードのレアリティに分ける
            if (cardData._cardRarity == 1)
            {
                cardRarity1List.Add(cardData._cardID);
            }
            if (cardData._cardRarity == 2)
            {
                cardRarity2List.Add(cardData._cardID);
            }
            if (cardData._cardRarity == 3)
            {
                cardRarity3List.Add(cardData._cardID);
            }
        }

        for (int i = 1; i <= MaxNumRelics; i++)
        {
            relicData = new RelicDataManager(i);

            // 各レリックのレアリティに分ける
            if (relicData._relicRarity == 1)
            {
                relicRarity1List.Add(relicData._relicID);
            }
            if (relicData._relicRarity == 2)
            {
                relicRarity2List.Add(relicData._relicID);
            }
        }

        playerData = GameManager.Instance.playerData;

        isInitialize = true;
    }

    /// <summary>
    /// カードの抽選メソッド
    /// </summary>
    /// <param name="rarity">抽選したいレアリティ</param>
    /// <returns>指定したレアリティのListの要素</returns>
    int CardLottery(int rarity)
    {
        List<int> SelectedRarityList;

        //引数で指定されたレアリティのListをSelectedRarityListへ代入
        switch (rarity)
        {
            case 1:
                SelectedRarityList = cardRarity1List;
                break;
            case 2:
                SelectedRarityList = cardRarity2List;
                break;
            case 3:
                SelectedRarityList = cardRarity3List;
                break;
            default:
                Debug.Log("指定されたレアリティのカードがありません。");
                return -1;
        }

        //所持カードとショップに出ているカードをDeckAndShopCardsへ追加
        deckAndShopCards = new List<int>(playerData._deckList);
        deckAndShopCards.AddRange(shopCards);

        int cardLottery = -1;

        //抽選処理
        int maxAttempts = 100;  // 最大試行回数を設定
        int attempts = 0;

        while (cardLottery == -1 || (deckAndShopCards.Contains(SelectedRarityList[cardLottery])) && attempts < maxAttempts)
        {
            cardLottery = Random.Range(0, SelectedRarityList.Count);
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            cardLottery = -1;
            Debug.Log("カードを抽選できませんでした。");
        }

        deckAndShopCards = null;
        return cardLottery;
    }


    /// <summary>
    /// CardLotteryメソッドを使って3回抽選を行います
    /// 指定したレアリティがなかった時に代わりのレアリティで再抽選をします
    /// </summary>
    /// <param name="selectRarity">抽選したいレアリティ1~3をListで指定</param>
    /// <returns>抽選したカードID</returns>
    public List<int> SelectCardByRarity(List<int> selectRarity)
    {
        List<int> lotteryResult = new List<int>();

        for (int i = 0; i < selectRarity.Count; i++)
        {
            // カードの抽選
            int selectedCard = CardLottery(selectRarity[i]);

            // 指定したレアリティにカードがなかった時の再抽選
            if (selectedCard == -1)
            {
                switch (selectRarity[i])
                {
                    case 1:
                        selectedCard = CardLottery(2);
                        selectedCard = cardRarity2List[selectedCard];   // 抽選した各レアリティのListの要素をCardIDに変換
                        break;
                    case 2:
                        selectedCard = CardLottery(1);
                        selectedCard = cardRarity1List[selectedCard];
                        break;
                    case 3:
                        selectedCard = CardLottery(2);
                        selectedCard = cardRarity2List[selectedCard];
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // 抽選した各レアリティのListの要素をCardIDに変換
                switch (selectRarity[i])
                {
                    case 1:
                        selectedCard = cardRarity1List[selectedCard];
                        break;
                    case 2:
                        selectedCard = cardRarity2List[selectedCard];
                        break;
                    case 3:
                        selectedCard = cardRarity3List[selectedCard];
                        break;
                    default:
                        break;
                }
            }

            lotteryResult.Add(selectedCard);

            // ShopControllerから呼ばれたら
            if (fromShopController)
            {
                shopCards.Add(selectedCard);
            }
        }

        fromShopController = false;

        return lotteryResult;
    }


    /// <summary>
    /// レリックの抽選メソッド
    /// </summary>
    /// <param name="rarity">抽選したいレアリティ</param>
    /// <returns>指定したレアリティのListの要素</returns>
    int RelicLottery(int rarity)
    {
        List<int> SelectedRarityList;

        //引数で指定されたレアリティのListをSelectedRarityListへ代入
        switch (rarity)
        {
            case 1:
                SelectedRarityList = relicRarity1List;
                break;
            case 2:
                SelectedRarityList = relicRarity2List;
                break;
            default:
                Debug.Log("指定されたレアリティのレリックがありません。");
                return -1;
        }

        //所持レリックとショップに出ているカードをmyRelicAndShopRelicsへ追加
        myRelicAndShopRelics = new List<int>(playerData._relicList);
        myRelicAndShopRelics.AddRange(shopRelics);

        int relicLottery = -1;

        //抽選処理
        int maxAttempts = 100;  // 最大試行回数を設定
        int attempts = 0;

        while (relicLottery == -1 || (myRelicAndShopRelics.Contains(SelectedRarityList[relicLottery])) && attempts < maxAttempts)
        {
            relicLottery = Random.Range(0, SelectedRarityList.Count);
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            relicLottery = -1;
            Debug.Log("レリックを抽選できませんでした。");
        }

        myRelicAndShopRelics = null;
        return relicLottery;
    }


    /// <summary>
    /// RelicLotteryメソッドを使って3回抽選を行います
    /// 指定したレアリティがなかった時に代わりのレアリティで再抽選をします
    /// </summary>
    /// <param name="selectRarity">抽選したいレアリティ1~2をListで指定</param>
    /// <returns>抽選したレリックID</returns>
    public List<int> SelectRelicByRarity(List<int> selectRarity)
    {
        List<int> lotteryResult = new List<int>();

        for (int i = 0; i < selectRarity.Count; i++)
        {
            // レリックの抽選
            int selectedRelic = RelicLottery(selectRarity[i]);

            // 指定したレアリティにレリックがなかった時の再抽選
            if (selectedRelic == -1)
            {
                switch (selectRarity[i])
                {
                    case 1:
                        selectedRelic = RelicLottery(2);
                        selectedRelic = relicRarity2List[selectedRelic];   // 抽選した各レアリティのListの要素をRelicIDに変換
                        break;
                    case 2:
                        selectedRelic = RelicLottery(1);
                        selectedRelic = relicRarity1List[selectedRelic];
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // 抽選した各レアリティのListの要素をRelicIDに変換
                switch (selectRarity[i])
                {
                    case 1:
                        selectedRelic = relicRarity1List[selectedRelic];
                        break;
                    case 2:
                        selectedRelic = relicRarity2List[selectedRelic];
                        break;
                    default:
                        break;
                }
            }

            lotteryResult.Add(selectedRelic);

            // ShopControllerから呼ばれたら
            if (fromShopController)
            {
                shopRelics.Add(selectedRelic);
            }
        }

        fromShopController = false;

        return lotteryResult;
    }

}








