using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDataManager
{
    public int _cardID;
    public string _cardType;
    public string _cardName;
    public string _cardEffect;
    public int _cardRarity;
    public int _cardCost;
    public int _cardAttackPower;
    public int _cardHealingPower;
    public int _cardGuardPoint;
    public int _cardState;
    public Sprite _cardImage;
    

    public CardDataManager(int cardID) 
    {
        CardData cardData = Resources.Load<CardData>("CardDataList/Card" + cardID);
        _cardID = cardData.cardID;
        _cardType = cardData.cardType;
        _cardName = cardData.cardName;
        _cardEffect = cardData.cardEffect;
        _cardRarity = cardData.cardRarity;
        _cardCost = cardData.cardCost;
        _cardAttackPower = cardData.cardAttackPower;
        _cardHealingPower = cardData.cardHealingPower;
        _cardGuardPoint = cardData.cardGuardPoint;
        _cardState = cardData.cardState;
        _cardImage = cardData.cardImage;
    }
}
