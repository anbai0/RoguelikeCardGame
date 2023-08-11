using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    [Header("ダメージ表示オブジェクト")]
    [SerializeField]
    GameObject damageUI;
    [Header("回復表示オブジェクト")]
    [SerializeField]
    GameObject healingUI;
    [Header("ダメージと回復表示の出現場所")]
    [SerializeField]
    GameObject damageOrHealingPos;
    [Header("状態異常のアイコン表示スクリプト")]
    [SerializeField]
    PlayerConditionDisplay playerConditionDisplay;
    [Header("画面揺れのスクリプト")]
    [SerializeField]
    ShakeBattleField shakeBattleField;

    public Dictionary<int, int> hasPlayerRelics = new Dictionary<int, int>(); //プレイヤーの所持しているレリック
    int maxRelics = 12;
    RelicEffectList relicEffect; //レリックの効果

    public Dictionary<string, int> playerCondition = new Dictionary<string, int>(); //プレイヤーに付与されている状態異常
    
    CardEffectList cardEffectList;//カードの効果スクリプト

    int playerMoney;//プレイヤーの所持金
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
        InitializedCondition();
        GetSetCondition = playerCondition;
        cardEffectList = GetComponent<CardEffectList>();
        GetSetInflictCondition = GetComponent<InflictCondition>();
        //hasPlayerRelics = GameManager.Instance.hasRelics;
        //Debug用
        for(int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            hasPlayerRelics[RelicID] = 0;
        }
    }

    /// <summary>
    /// 状態異常の名前と所持数を初期化しておく処理
    /// </summary>
    void InitializedCondition()
    {
        playerCondition.Add("UpStrength", 0);
        playerCondition.Add("AutoHealing", 0);
        playerCondition.Add("InvalidBadStatus", 0);
        playerCondition.Add("Curse", 0);
        playerCondition.Add("Impatience", 0);
        playerCondition.Add("Weakness", 0);
        playerCondition.Add("Burn", 0);
        playerCondition.Add("Poison", 0);
    }

    /// <summary>
    /// ダメージを受けたときの処理
    /// </summary>
    /// <param name="damage">受けたダメージ</param>
    public void TakeDamage(int damage)
    {
        int deductedDamage = 0;
        if (GetSetGP > 0) //ガードポイントがあったら
        {
            //ガードポイントの分だけダメージを軽減する
            deductedDamage = damage - GetSetGP;
            deductedDamage = deductedDamage < 0 ? 0 : deductedDamage;
            GetSetGP -= damage;
        }
        else
        {
            deductedDamage = damage;
        }
        GetSetCurrentHP -= deductedDamage;

        if (deductedDamage > 0)
        {
            ViewDamage(deductedDamage);
            shakeBattleField.Shake(0.25f, 10f);
        }
    }

    /// <summary>
    /// ダメージ演出を表示する処理
    /// </summary>
    /// <param name="damage">受けたダメージ</param>
    void ViewDamage(int _damage)
    {
        GameObject damageObj = Instantiate(damageUI, damageOrHealingPos.transform);
        damageObj.GetComponent<TextMeshProUGUI>().text = _damage.ToString();
    }

    /// <summary>
    /// HP回復の処理
    /// </summary>
    /// <param name="healingHPPower">HPの回復量</param>
    public void HealingHP(int healingHPPower)
    {
        GetSetCurrentHP += healingHPPower;
        if (healingHPPower > 0)
        {
            ViewHealing(healingHPPower);
        }
    }

    /// <summary>
    /// 回復演出を表示する処理
    /// </summary>
    /// <param name="_healingHPPower">HPの回復量</param>
    void ViewHealing(int _healingHPPower)
    {
        GameObject healingObj = Instantiate(healingUI, damageOrHealingPos.transform);
        healingObj.GetComponent<TextMeshProUGUI>().text = _healingHPPower.ToString();
    }

    /// <summary>
    /// 状態異常のアイコンを表示する処理
    /// </summary>
    public void ViewConditionIcon()
    {
        playerConditionDisplay.ViewIcon(playerCondition);
    }

    //以下、TakeDamageやHealingHPに関係する状態異常の呼び出し
    /// <summary>
    /// 自動回復の呼び出し
    /// </summary>
    public void AutoHealing()
    {
        HealingHP(GetSetInflictCondition.AutoHealing(playerCondition["AutoHealing"]));
    }

    /// <summary>
    /// 火傷の呼び出し
    /// </summary>
    public void Burn()
    {
        var burn = GetSetInflictCondition.Burn(playerCondition["Burn"], playerCondition["InvalidBadStatus"]);
        TakeDamage(burn.damage);
        playerCondition["InvalidBadStatus"] = burn.invalidBadStatus;
    }

    /// <summary>
    /// 邪毒の呼び出し
    /// </summary>
    /// <param name="moveCount">行動回数</param>
    public void Poison(int moveCount)
    {
        var poison = GetSetInflictCondition.Poison(playerCondition["Poison"], playerCondition["InvalidBadStatus"], moveCount);
        TakeDamage(poison.damage);
        playerCondition["InvalidBadStatus"] = poison.invalidBadStatus;
    }

    /// <summary>
    /// 戦闘開始時に発動するレリック効果
    /// </summary>
    /// <param name="enemyBattleAction">エネミーのステータス</param>
    /// <param name="enemyType">エネミーの種類</param>
    /// <returns>変更を加えたエネミーのステータス</returns>
    public EnemyBattleAction StartRelicEffect(EnemyBattleAction enemyBattleAction, string enemyType)
    {
        relicEffect = GetComponent<RelicEffectList>();
        var es = enemyBattleAction;
        Debug.Log(playerCondition.ContainsKey("UpStrength"));
        var relicEffectID2 = relicEffect.RelicID2(hasPlayerRelics[2], playerCondition["UpStrength"], es.enemyCondition["UpStrength"]);
        playerCondition["UpStrength"] = relicEffectID2.playerUpStrength;
        es.enemyCondition["UpStrength"] = relicEffectID2.enemyUpStrength;
        GetSetConstAP = relicEffect.RelicID3(hasPlayerRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        GetSetConstAP = relicEffect.RelicID4(hasPlayerRelics[4], GetSetConstAP);
        GetSetConstAP = relicEffect.RelicID5(hasPlayerRelics[5], GetSetConstAP, GetSetChargeAP).constAP;
        es.enemyCondition["Burn"] = relicEffect.RelicID6(hasPlayerRelics[6], es.enemyCondition["Burn"]);
        GetSetHP = relicEffect.RelicID7(hasPlayerRelics[7], GetSetHP);
        GetSetGP = relicEffect.RelicID8(hasPlayerRelics[8], GetSetGP);
        playerCondition["UpStrength"] = relicEffect.RelicID12(hasPlayerRelics[12], enemyType, playerCondition["UpStrength"]);
        Debug.Log("スタート時のレリックが呼び出されました: " + GetSetConstAP + " to " + GetSetChargeAP);
        return es;
    }

    /// <summary>
    /// ラウンド終了時に一度だけ発動するレリック効果
    /// </summary>
    public void OnceEndRoundRelicEffect()
    {
        GetSetChargeAP = relicEffect.RelicID3(hasPlayerRelics[3], GetSetConstAP, GetSetChargeAP).chargeAP;
        GetSetChargeAP = relicEffect.RelicID5(hasPlayerRelics[5], GetSetAP, GetSetChargeAP).chargeAP;
    }

    /// <summary>
    /// ラウンド終了時に発動するレリック効果
    /// </summary>
    public void EndRoundRelicEffect()
    {
        playerCondition = relicEffect.RelicID11(hasPlayerRelics[11], playerCondition);
    }

    /// <summary>
    /// 戦闘終了時に発動するレリック効果
    /// </summary>
    /// <returns>戦闘終了後のゴールド獲得時に多く貰える数</returns>
    public int EndGameRelicEffect()
    {
        int money = 10;
        money = relicEffect.RelicID9(hasPlayerRelics[9], money);
        GetSetCurrentHP = relicEffect.RelicID10(hasPlayerRelics[10], GetSetCurrentHP);
        return money;
    }
}
