using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lottery : MonoBehaviour
{
    PlayerDataManager playerDataManager;
    CardDataManager []cardDataManager;

    ShopController shopController;

    List<CardDataManager> CardRarity1;
    List<CardDataManager> CardRarity2;
    List<CardDataManager> CardRarity3;

    void Start()
    {
        for(int i = 1; i <= 20; i++)
        {
            CardDataManager card = new CardDataManager(i);
            //各カードのレアリティに分ける
            if(card._cardRarity == 1)
            {
                CardRarity1.Add(card);
            }
            if (card._cardRarity == 2)
            {
                CardRarity2.Add(card);
            }
            if (card._cardRarity == 3)
            {
                CardRarity3.Add(card);
            }
        }
        
    }


    void Update()
    {
        
    }

    void zako()     //レア度１が２枚、２が１枚 
    {
        
    }

    void kyoutakara()   //レア度1が１枚、２が２枚
    {

    }

    void boss()     //レア度２が２枚、３が１枚
    {

    }

    void shop()     //レア度2が１枚、レア度１が２枚、回復が１枚（確定）
    {
        int shopLottery2 = Random.Range(0, CardRarity2.Count);
        int rare2 = 0;
        for(int i = 0; i < playerDataManager._deckList.Count; i++)  //持っているカードの枚数分回す
        {
            if (CardRarity2[shopLottery2]._cardID == playerDataManager._deckList[i])　　//抽選されたIDが持っているカードにあるか？
            {
                continue;
            }
            rare2 = CardRarity2[shopLottery2]._cardID;
        }
        shopController.ShowCards(rare2, 1, 1);
    }
}
