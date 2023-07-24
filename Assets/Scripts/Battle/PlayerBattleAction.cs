using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleAction : CharacterBattleAction
{
    [Header("プレイヤー名テキスト")]
    [SerializeField] Text playerNameText;
    [Header("プレイヤーHPテキスト")]
    [SerializeField] Text playerHPText;
    [Header("プレイヤーAPテキスト")]
    [SerializeField] Text playerAPText;
    [Header("プレイヤーGPテキスト")]
    [SerializeField] Text playerGPText;
    int playerMoney;//プレイヤーの所持金
    CardEffectList cardEffectList;//カードの効果スクリプト
    public int GetSetPlayerMoney { get => playerMoney; set => playerMoney = value; }
    private void Update()
    {
        UpdateText(playerHPText, playerAPText, playerGPText, null);
    }
    public void Move(CardController card)//プレイヤーの効果処理
    {
        GetSetCurrentAP -= card.cardDataManager._cardCost;
        Debug.Log("現在のPlayerCurrentAPは" + GetSetCurrentAP);
        cardEffectList.ActiveCardEffect(card);
    }
    public void SetStatus(PlayerDataManager playerData)//各ステータスを割り振る
    {
        playerNameText.text = "現在のキャラ:" + playerData._playerName;
        GetSetHP = playerData._playerHP;
        GetSetCurrentHP = GetSetHP;
        GetSetAP = playerData._playerAP;
        GetSetConstAP = playerData._playerAP;
        GetSetCurrentAP = GetSetAP;
        GetSetGP = playerData._playerGP;
        playerMoney = playerData._playerMoney;
        cardEffectList = GetComponent<CardEffectList>();
        GetSetCondition = new ConditionStatus();
        GetSetInflictCondition = GetComponent<InflictCondition>();
    }
    public void TakeMoney(int getMoney)
    {
    }
}
