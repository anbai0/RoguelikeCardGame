using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery : MonoBehaviour
{
    CardDataManager cardData;       // カードのデータを処理

    //各レアリティのリスト
    [SerializeField]
    List<int> CardRarity1List;
    [SerializeField]
    List<int> CardRarity2List;
    [SerializeField]
    List<int> CardRarity3List;

    public List<int> ShopCards = new List<int>();   //ショップに追加されたカード
    List<int> DeckAndShopCards;                     //現在の所持カード＋ショップに追加されたカードを格納する

    public static bool isInitialize = false;

    int MaxNumCards = 20;       //カード枚数

    public bool fromShopController = false;


    void Start()
    {
        for (int i = 1; i <= MaxNumCards; i++)
        {
            cardData = new CardDataManager(i);

            //各カードのレアリティに分ける
            if (cardData._cardRarity == 1)
            {
                CardRarity1List.Add(cardData._cardID);
            }
            if (cardData._cardRarity == 2)
            {
                CardRarity2List.Add(cardData._cardID);
            }
            if (cardData._cardRarity == 3)
            {
                CardRarity3List.Add(cardData._cardID);
            }
        }

        isInitialize = true;
    }

    /// <summary>
    /// カードの抽選メソッド
    /// </summary>
    /// <param name="rarity">抽選したいレアリティ</param>
    /// <returns>指定したレアリティのListの要素 (int)</returns>
    int CardLottery(int rarity)
    {
        List<int> SelectedRarityList;

        //引数で指定されたレアリティのListをSelectedRarityListへ代入
        switch (rarity)
        {
            case 1:
                SelectedRarityList = CardRarity1List;
                break;
            case 2:
                SelectedRarityList = CardRarity2List;
                break;
            case 3:
                SelectedRarityList = CardRarity3List;
                break;
            default:
                Debug.Log("指定されたレアリティがありません。");
                return -1;
        }

        //所持カードとショップに出ているカードをDeckAndShopCardsへ追加
        var playerData = new PlayerDataManager("Warrior");
        DeckAndShopCards = new List<int>(playerData._deckList);
        DeckAndShopCards.AddRange(ShopCards);

        int cardLottery = -1;

        //抽選処理
        int maxAttempts = 100;  // 最大試行回数を設定
        int attempts = 0;

        while (cardLottery == -1 || (DeckAndShopCards.Contains(SelectedRarityList[cardLottery])) && attempts < maxAttempts)
        {
            cardLottery = Random.Range(0, SelectedRarityList.Count);
            attempts++;
        }


        if (attempts >= maxAttempts)
        {
            cardLottery = -1;
            Debug.Log("カードを抽選できませんでした。");
        }


        DeckAndShopCards = null;
        return cardLottery;
    }



    public int[] SelectCardByRarity(int[] selectRarity)
    {

        int[] lotteryResult = new int[selectRarity.Length];
        //int result1, result2, result3;

        
        for (int i = 0; i < selectRarity.Length; i++)
        {
            //カードの抽選
            lotteryResult[i] = CardLottery(selectRarity[i]);

            //指定したレアリティにカードがなかった時の再抽選
            if (lotteryResult[i] == -1)
            {
                Debug.Log("a");
                switch (selectRarity[i])
                {
                    case 1:
                        lotteryResult[i] = CardLottery(2);
                        lotteryResult[i] = CardRarity2List[lotteryResult[i]];   //抽選した各レアリティのListの要素をCardIDに変換
                        break;
                    case 2:
                        lotteryResult[i] = CardLottery(1);
                        lotteryResult[i] = CardRarity1List[lotteryResult[i]];
                        break;
                    case 3:
                        lotteryResult[i] = CardLottery(2);
                        lotteryResult[i] = CardRarity2List[lotteryResult[i]];
                        break;
                    default:
                        break;
                }
            }else
            {
                //抽選した各レアリティのListの要素をCardIDに変換
                switch (selectRarity[i])
                {
                    case 1:
                        lotteryResult[i] = CardRarity1List[lotteryResult[i]];
                        break;
                    case 2:
                        lotteryResult[i] = CardRarity2List[lotteryResult[i]];
                        break;
                    case 3:
                        lotteryResult[i] = CardRarity3List[lotteryResult[i]];
                        break;
                    default:
                        break;
                }
            }

            // ShopControllerから呼ばれたら
            if (fromShopController)
            {
                ShopCards.Add(lotteryResult[i]);
            }
        }

        fromShopController = false;

        //result1 = lotteryResult[0];
        //result2 = lotteryResult[1];
        //result3 = lotteryResult[2];


        return (lotteryResult);
    }



    void zako()     // レア度1が２枚、レア度2が１枚 
    {
        
    }

    void kyoutakara()   // レア度1が１枚、レア度2が２枚
    {

    }

    void boss()     // レア度2が２枚、レア度3が１枚
    {

    }


}
