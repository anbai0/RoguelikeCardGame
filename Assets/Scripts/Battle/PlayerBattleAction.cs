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
    RelicEffectList relicEffect; //レリックの効果
    public int GetSetPlayerMoney { get => playerMoney; set => playerMoney = value; }
    private void Update()
    {
        UpdateText(playerHPText, playerAPText, playerGPText, null);
    }
    /// <summary>
    /// プレイヤーが使用したカード効果の処理
    /// </summary>
    /// <param name="card">使用したカード</param>
    public void Move(CardController card)
    {
        GetSetCurrentAP -= card.cardDataManager._cardCost;
        Debug.Log("現在のPlayerCurrentAPは" + GetSetCurrentAP);
        cardEffectList.ActiveCardEffect(card);
    }
    /// <summary>
    /// 各ステータスをセットする処理
    /// </summary>
    /// <param name="playerData">職種に応じたプレイヤーデータ</param>
    public void SetStatus(PlayerDataManager playerData)
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
        GetSetRelicStatus = new RelicStatus();
    }
    /// <summary>
    /// 戦闘開始時に発動するレリック効果
    /// </summary>
    /// <param name="enemyBattleAction">エネミーのステータス</param>
    /// <param name="enemyName">エネミーの名前</param>
    /// <returns>変更を加えたエネミーのステータス</returns>
    public EnemyBattleAction StartRelicEffect(EnemyBattleAction enemyBattleAction, string enemyName)
    {
        relicEffect = GetComponent<RelicEffectList>();
        var es = enemyBattleAction;
        var pr = GetSetRelicStatus;
        var relicEffectID2 = relicEffect.RelicID2(pr.hasRelicID2, GetSetCondition.upStrength, es.GetSetCondition.upStrength);
        GetSetCondition.upStrength = relicEffectID2.playerUpStrength;
        es.GetSetCondition.upStrength = relicEffectID2.enemyUpStrength;
        GetSetConstAP = relicEffect.RelicID3(pr.hasRelicID3, GetSetConstAP, GetSetChargeAP).constAP;
        GetSetConstAP = relicEffect.RelicID4(pr.hasRelicID4, GetSetConstAP);
        GetSetConstAP = relicEffect.RelicID5(pr.hasRelicID5, GetSetConstAP, GetSetChargeAP).constAP;
        es.GetSetCondition.burn = relicEffect.RelicID6(pr.hasRelicID6, es.GetSetCondition.burn);
        GetSetHP = relicEffect.RelicID7(pr.hasRelicID7, GetSetHP);
        GetSetGP = relicEffect.RelicID8(pr.hasRelicID8, GetSetGP);
        GetSetCondition.upStrength = relicEffect.RelicID12(pr.hasRelicID12, enemyName, GetSetCondition.upStrength);
        Debug.Log("スタート時のレリックが呼び出されました: " + GetSetConstAP + " to " + GetSetChargeAP);
        return es;
    }
    /// <summary>
    /// ラウンド終了時に一度だけ発動するレリック効果
    /// </summary>
    public void OnceEndRoundRelicEffect()
    {
        var pr = GetSetRelicStatus;
        GetSetChargeAP = relicEffect.RelicID3(pr.hasRelicID3, GetSetConstAP, GetSetChargeAP).chargeAP;
        GetSetChargeAP = relicEffect.RelicID5(pr.hasRelicID5, GetSetAP, GetSetChargeAP).chargeAP;
    }
    /// <summary>
    /// ラウンド終了時に発動するレリック効果
    /// </summary>
    public void EndRoundRelicEffect()
    {
        var pr = GetSetRelicStatus;
        GetSetCondition = relicEffect.RelicID11(pr.hasRelicID11, GetSetCondition);
    }
    /// <summary>
    /// 戦闘終了時に発動するレリック効果
    /// </summary>
    /// <returns>戦闘終了後のゴールド獲得時に多く貰える数</returns>
    public int EndGameRelicEffect()
    {
        var pr = GetSetRelicStatus;
        int money = 10;
        money = relicEffect.RelicID9(pr.hasRelicID9, money);
        GetSetCurrentHP = relicEffect.RelicID10(pr.hasRelicID10, GetSetCurrentHP);
        return money;
    }
    public void TakeMoney(int getMoney)
    {
    }
}
