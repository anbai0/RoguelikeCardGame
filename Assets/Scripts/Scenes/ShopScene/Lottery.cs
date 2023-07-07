using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery : MonoBehaviour
{

    PlayerDataManager playerDataManager;
    CardDataManager cardData;       // カードのデータを処理

    [SerializeField]
    List<int> CardRarity1List;
    [SerializeField]
    List<int> CardRarity2List;
    [SerializeField]
    List<int> CardRarity3List;

    public List<int> ShopCards = new List<int>();
    List<int> DeckAndShopCards; //現在の所持カード＋ショップに追加されたカードを格納する

    public static bool isInitialize = false;

    int MaxNumCards = 20;

    int foreachCount = 0;

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



    public (int,int,int) ShopLottery(int selectRare1, int selectRare2, int selectRare3)     // レア度2が１枚、レア度1が２枚、回復が１枚（確定）
    {

        // レアリティ2の抽選
        int cardLottery1;
        int Lottery1 = CardLottery(2);
        if (Lottery1 != -1)     // レアリティ2のカードがあったら
        {
            cardLottery1 = CardRarity2List[Lottery1];           // 各レアリティの抽選されたListの要素をCardIDに変換
            ShopCards.Add(cardLottery1);                        // ショップに追加するカードはListに格納する
        }
        else     // なかったらレアリティ1のカードを抽選
        {
            cardLottery1 = CardRarity1List[CardLottery(1)];     //レアリティ1のカードを抽選
            ShopCards.Add(cardLottery1);
        }

        // レアリティ1の抽選
        int cardLottery2;
        int Lottery2 = CardLottery(1);
        if (Lottery2 != -1)     // レアリティ1のカードがあったら
        {
            cardLottery2 = CardRarity1List[Lottery2];           // 各レアリティの抽選されたListの要素をCardIDに変換
            ShopCards.Add(cardLottery2);                        // ショップに追加するカードはListに格納する
        }
        else     // なかったらレアリティ2のカードを抽選
        {
            cardLottery2 = CardRarity2List[CardLottery(2)];
            ShopCards.Add(cardLottery2);
        }

        // レアリティ1の抽選
        int cardLottery3;
        int Lottery3 = CardLottery(1);
        if (Lottery3 != -1)     // レアリティ1のカードがあったら
        {
            cardLottery3 = CardRarity1List[Lottery3];           // 各レアリティの抽選されたListの要素をCardIDに変換
            ShopCards.Add(cardLottery3);                        // ショップに追加するカードはListに格納する
        }
        else     // なかったらレアリティ2のカードを抽選
        {
            cardLottery3 = CardRarity2List[CardLottery(2)];
            ShopCards.Add(cardLottery3);
        }

        return (cardLottery1, cardLottery2, cardLottery3);

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
