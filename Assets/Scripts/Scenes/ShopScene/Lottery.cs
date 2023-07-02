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

    List<int> ShopCards;
    List<int> DeckAndShopCards; //現在の所持カード＋ショップに追加されたカードを格納する




    int MaxNumCards = 20;
    
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

        CardLottery(1);
    }

    /// <summary>
    /// カードの抽選メソッド
    /// </summary>
    /// <param name="rarity">抽選したいレアリティ</param>
    /// <returns>指定したレアリティのカードID (int)</returns>
    int CardLottery(int rarity)
    {
        List<int> SelectedRarityList = null;

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
        DeckAndShopCards.AddRange(playerDataManager._deckList);
        DeckAndShopCards.AddRange(ShopCards);

        int cardLottery;

        //抽選処理
        while (true)
        {
            cardLottery = Random.Range(0, SelectedRarityList.Count);   // 指定されたレアリティのListのからランダムに要素を選択
            bool hasCards = false;

            for (int i = 0; i < DeckAndShopCards.Count; i++)  // 所持カードとショップに出ているカードの枚数分回す
            {
                if (SelectedRarityList[cardLottery] == DeckAndShopCards[i])  // 抽選されたカードを所持していたら再抽選
                {
                    hasCards = true;
                    break;
                }
            }
            
            if (!hasCards) // 抽選されたカードを所持していなかったら抽選終了
                break;
        }

        DeckAndShopCards = null;
        return cardLottery;
    }


    public (int,int,int) ShopLottery()     // レア度2が１枚、レア度1が２枚、回復が１枚（確定）
    {
        
        int cardLottery1 = CardLottery(2);
        ShopCards.Add(cardLottery1);     // ショップに追加するカードはListに格納する

        int cardLottery2 = CardLottery(1);
        ShopCards.Add(cardLottery2);

        int cardLottery3 = CardLottery(1);
        ShopCards.Add(cardLottery3);
        
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
