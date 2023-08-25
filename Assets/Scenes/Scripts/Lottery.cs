using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カードやレリックの抽選を行うスクリプトです。
/// </summary>
public class Lottery : MonoBehaviour
{
    private GameManager gm;

    private const int MaxNumCards = 20;       // 全カードの枚数
    private const int MaxNumRelics = 11;      // 全レリックの数

    [Header("ここから下はデバッグ用に表示させてます")]
    //各レアリティのリスト
    [SerializeField] List<int> cardRarity1List;
    [SerializeField] List<int> cardRarity2List;
    [SerializeField] List<int> cardRarity3List;
    [SerializeField] List<int> relicRarity1List;
    [SerializeField] List<int> relicRarity2List;
    [SerializeField] public List<int> shopCards = new List<int>();       // ショップに追加されたカード。データをリセットするときにクリアします
    [SerializeField] List<int> currentCardsLotteryID = new List<int>(); // 現在抽選しているカードのID
    [SerializeField] List<int> currentRelicsLotteryID = new List<int>(); // 現在抽選しているレリックのID

    public static Lottery Instance;
    void Start()
    {
        // シングルトンインスタンスをセットアップ
        if (Instance == null)
        {
            Instance = this;
        }

        // GameManager取得(変数名省略)
        gm = GameManager.Instance;

        for (int i = 1; i <= MaxNumCards; i++)
        {
            CardDataManager cardData = gm.cardDataList[i];

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
            RelicDataManager relicData = gm.relicDataList[i];

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
    }

    /// <summary>
    /// カードの抽選メソッド
    /// </summary>
    /// <param name="rarity">抽選したいレアリティ</param>
    /// <returns>指定したレアリティのListの要素</returns>
    int CardLottery(int rarity)
    {
        List<int> SelectedRarityList;

        // 引数で指定されたレアリティのListをSelectedRarityListへ代入
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

        // 所持カードとショップに出ているカードと今回抽選されたカードを格納
        List<int> currentLotteryCards = new List<int>(gm.playerData._deckList);
        currentLotteryCards.AddRange(shopCards);
        currentLotteryCards.AddRange(currentCardsLotteryID);

        // 強化済みカードがあれば対応する未強化のカードを除外
        for (int num = 0; num < currentLotteryCards.Count; num++)
        {
            if (currentLotteryCards[num] >= 101)
            {
                currentLotteryCards[num] -= 100;
            }
        }

        int cardLottery = -1;

        //抽選処理
        int maxAttempts = 100;  // 最大試行回数を設定
        int attempts = 0;

        while (cardLottery == -1 || (currentLotteryCards.Contains(SelectedRarityList[cardLottery])) && attempts < maxAttempts)
        {
            cardLottery = Random.Range(0, SelectedRarityList.Count);
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            cardLottery = -1;
            Debug.Log("カードを抽選できませんでした。");
        }

        currentLotteryCards.Clear();    // 次の抽選時に所持カードが異なっていることを考慮し、Listをクリア
        return cardLottery;
    }


    /// <summary>
    /// CardLotteryメソッドを使って(selectRarityの要素)回分抽選を行います。
    /// 指定したレアリティがなかった時に代わりのレアリティで再抽選をします。
    /// shopで呼ぶ場合は第二引数にtrueを指定してください。
    /// </summary>
    /// <param name="selectRarity">抽選したいレアリティ1~3をListで指定</param>
    /// <returns>抽選したカードID</returns>
    public List<int> SelectCardByRarity(List<int> selectRarity, bool fromShopController = false)
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

            currentCardsLotteryID.Add(selectedCard);    // 今回の抽選で出たカードIDを格納
            lotteryResult.Add(selectedCard);            // 戻り値である抽選の結果を格納

            // ShopControllerから呼ばれたら
            if (fromShopController)
            {
                shopCards.Add(selectedCard);
            }
        }

        currentCardsLotteryID.Clear();      // 抽選が終わったのでListをクリア
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

        // 引数で指定されたレアリティのListをSelectedRarityListへ代入
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

        int relicLottery = -1;

        // 抽選処理
        int maxAttempts = 100;  // 最大試行回数を設定
        int attempts = 0;

        while (relicLottery == -1 || (currentRelicsLotteryID.Contains(SelectedRarityList[relicLottery])) && attempts < maxAttempts)
        {
            relicLottery = Random.Range(0, SelectedRarityList.Count);
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            relicLottery = -1;
            Debug.Log("レリックを抽選できませんでした。");
        }

        return relicLottery;
    }


    /// <summary>
    /// RelicLotteryメソッドを使って(selectRarityの要素)回分抽選を行います
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

            currentRelicsLotteryID.Add(selectedRelic);
            lotteryResult.Add(selectedRelic);
        }

        currentRelicsLotteryID.Clear();      // 抽選が終わったのでListをクリア
        return lotteryResult;
    }

}
