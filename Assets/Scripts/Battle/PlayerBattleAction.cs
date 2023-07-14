using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBattleAction : MonoBehaviour
{
    [Header("プレイヤー名テキスト")]
    [SerializeField] Text playerNameText;
    [Header("プレイヤーHPテキスト")]
    [SerializeField] Text playerHPText;
    [Header("プレイヤーAPテキスト")]
    [SerializeField] Text playerAPText;
    [Header("プレイヤーGPテキスト")]
    [SerializeField] Text playerGPText;
    private int playerHP;//最大のHP
    private int playerCurrentHP;//現在のHP
    private int playerConstAP;//ゲーム開始時の最大AP
    private int roundStartAP;//ラウンド開始時の最大AP
    private int playerAP;//最大AP
    private int playerCurrentAP;//現在のAP
    private int playerChargeAP;//ラウンド経過で上昇していくAPの値
    private int playerGP;//ガードポイント
    private int playerMoney;//プレイヤーの所持金
    private ConditionStatus playerCondition;//プレイヤーの状態異常ステータス
    private CardEffectList cardEffectList;//カードの効果スクリプト
    private InflictCondition inflictCondition;//状態異常の効果スクリプト
    public int GetSetPlayerHP { get => playerHP; set => playerHP = value; }
    public int GetSetPlayerCurrentHP { get => playerCurrentHP; set => playerCurrentHP = value; }
    public int GetSetPlayerConstAP { get => playerConstAP; set => playerConstAP = value; }
    public int GetSetPlayerAP { get => playerAP; set => playerAP = value; }
    public int GetSetPlayerCurrentAP { get => playerCurrentAP; set => playerCurrentAP = value; }
    public int GetSetPlayerChargeAP { get => playerChargeAP; set => playerChargeAP = value; }
    public int GetSetPlayerGP { get => playerGP; set => playerGP = value; }
    public int GetSetPlayerMoney { get => playerMoney; set => playerMoney = value; }
    public ConditionStatus GetSetPlayerCondition { get => playerCondition; set => playerCondition = value; }
    private void Start()
    {
        playerChargeAP = 0;
    }
    private void Update()
    {
        UpdateText();
    }
    public void SetUpAP()//ラウンド開始時のAP計算
    {
        var curse = inflictCondition.Curse(playerConstAP, playerChargeAP, playerCondition.curse, playerCondition.invalidBadStatus);
        playerAP = curse.nextRoundAP;
        playerCurrentAP = playerAP;
        playerCondition.invalidBadStatus = curse.invalidBadStatus;
    }
    public void Move(CardController card)//プレイヤーの効果処理
    {
        playerCurrentAP -= card.cardDataManager._cardCost;
        Debug.Log("現在のPlayerCurrentAPは" + playerCurrentAP);
        cardEffectList.ActiveCardEffect(card);
    }
    public void TurnEnd()//プレイヤーを行動不能にする
    {
        playerCurrentAP = 0;
    }
    public void SetStatus(PlayerDataManager playerData)//各ステータスを割り振る
    {
        playerNameText.text = "現在のキャラ:" + playerData._playerName;
        playerHP = playerData._playerHP;
        playerCurrentHP = playerHP;
        playerConstAP = playerData._playerAP;
        playerAP = playerData._playerAP;
        playerCurrentAP = playerAP;
        playerGP = playerData._playerGP;
        playerMoney = playerData._money;
        cardEffectList = GetComponent<CardEffectList>();
        playerCondition = new ConditionStatus();
        inflictCondition = GetComponent<InflictCondition>();
    }

    public void UpdateText() //各テキストの更新
    {
        if (playerCurrentHP > playerHP)
        {
            playerCurrentHP = playerHP;
        }
        if (playerCurrentHP < 0)
        {
            playerCurrentHP = 0;
        }
        playerHPText.text = playerCurrentHP + "/" + playerHP;
        if (playerCurrentAP > playerAP)
        {
            playerCurrentAP = playerAP;
        }
        if (playerCurrentAP < 0)
        {
            playerCurrentAP = 0;
        }
        playerAPText.text = playerCurrentAP + "/" + playerAP;
        if (playerGP < 0)
        {
            playerGP = 0;
        }
        playerGPText.text = playerGP.ToString();
    }
    public void ChargeAP() //ラウンド終了時のAP増加
    {
        playerChargeAP++;
    }
    public bool CheckHP() //プレイヤーが死んでいないかチェックする
    {
        if (playerCurrentHP <= 0)
        {
            return true;
        }
        return false;
    }
    public void TakeDamage(int damage) //ダメージを受けたときの処理
    {
        int deductedDamage = 0;
        if (playerGP > 0) //プレイヤーにガードポイントがあったら
        {
            //ガードポイントの分だけダメージを軽減する
            deductedDamage = damage - playerGP;
            if (deductedDamage < 0)
            {
                deductedDamage = 0;
            }
            playerGP -= damage;
        }
        else
        {
            deductedDamage = damage;
        }
        playerCurrentHP -= deductedDamage;
    }
    public void SaveAP() //ラウンド開始時のAPを保存しておく
    {
        roundStartAP = playerAP;
    }
    public bool IsCurse() //呪縛状態になっているか確認する
    {
        //ターン終了時の最大AP
        int currentMaxAP = inflictCondition.Curse(playerConstAP, playerChargeAP, playerCondition.curse, playerCondition.invalidBadStatus).nextRoundAP;
        return roundStartAP > currentMaxAP ? true : false;
    }
    //以下、状態異常
    public int PlayerUpStrength(int attackPower)
    {
        attackPower = inflictCondition.UpStrength(attackPower, playerCondition.upStrength);
        return attackPower;
    }
    public void PlayerAutoHealing()
    {
        playerCurrentHP = inflictCondition.AutoHealing(playerCurrentHP, playerCondition.autoHealing);
    }
    public void PlayerImpatience()
    {
        var impatience = inflictCondition.Impatience(playerCurrentAP, playerCondition.impatience, playerCondition.invalidBadStatus);
        playerCurrentAP = impatience.currentAP;
        playerCondition.invalidBadStatus = impatience.invalidBadStatus;
    }
    public int PlayerWeakness(int attackPower)
    {
        var weakness = inflictCondition.Weakness(attackPower, playerCondition.weakness, playerCondition.invalidBadStatus);
        attackPower = weakness.attackPower;
        playerCondition.invalidBadStatus = weakness.invalidBadStatus;
        return attackPower;
    }
    public void PlayerBurn()
    {
        var burn = inflictCondition.Burn(playerCurrentHP, playerCondition.burn, playerCondition.invalidBadStatus);
        playerCurrentHP = burn.currentHP;
        playerCondition.invalidBadStatus = burn.invalidBadStatus;
    }
    public void PlayerPoison(int moveCount)
    {
        var poison = inflictCondition.Poison(playerCurrentHP, playerCondition.poison, playerCondition.invalidBadStatus, moveCount);
        playerCurrentHP = poison.currentHP;
        playerCondition.invalidBadStatus = poison.invalidBadStatus;
    }
    //以下、カード効果
    public void HealingHP(int healingHPPower) //HPの回復
    {
        playerCurrentHP += healingHPPower;
    }
    public void HealingAP(int healingAPPower) //APの回復
    {
        playerCurrentAP += healingAPPower;
    }
    public void AddGP(int addGP) //GPの追加
    {
        playerGP += addGP;
    }
    public void ReleaseBadStatus() //バッドステータスを解除する
    {
        playerCondition.curse = 0;
        playerCondition.impatience = 0;
        playerCondition.weakness = 0;
        playerCondition.burn = 0;
        playerCondition.poison = 0;
    }
    public int CheckBadStatus() //バッドステータスがあるかチェックする
    {
        var pc = playerCondition;
        int badStatus = pc.curse + pc.impatience + pc.weakness + pc.burn + pc.poison;
        return badStatus;
    }
    public void AddConditionStatus(string status, int count) //状態異常を追加する
    {
        if (status == "UpStrength")
        {
            playerCondition.upStrength += count;
        }
        else if (status == "AutoHealing")
        {
            playerCondition.autoHealing += count;
        }
        else if (status == "InvalidBadStatus")
        {
            playerCondition.invalidBadStatus += count;
        }
        else if (status == "Curse")
        {
            playerCondition.curse += count;
        }
        else if (status == "Impatience")
        {
            playerCondition.impatience += count;
        }
        else if (status == "Weakness")
        {
            playerCondition.weakness += count;
        }
        else if (status == "Burn")
        {
            playerCondition.burn += count;
        }
        else if (status == "Poison")
        {
            playerCondition.poison += count;
        }
    }
    public void TakeMoney(int getMoney)
    {
    }
}
