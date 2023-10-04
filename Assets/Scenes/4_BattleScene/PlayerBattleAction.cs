using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// プレイヤーの行動をまとめたスクリプト
/// </summary>
public class PlayerBattleAction : CharacterBattleAction
{
    [SerializeField, Header("プレイヤーHPテキスト")] Text playerHPText;
    [SerializeField, Header("プレイヤーAPテキスト")] Text playerAPText;
    [SerializeField, Header("プレイヤーGPテキスト")] Text playerGPText;
    [SerializeField, Header("ダメージ表示オブジェクト")] GameObject damageUI;
    [SerializeField, Header("回復表示オブジェクト")] GameObject healingUI;
    [SerializeField, Header("ガード表示オブジェクト")] GameObject gardUI;
    [SerializeField, Header("ダメージと回復表示の出現場所")] GameObject damageOrHealingPos;
    [SerializeField, Header("状態異常のアイコン表示スクリプト")] PlayerConditionDisplay playerConditionDisplay;
    [SerializeField, Header("画面揺れのスクリプト")] ShakeBattleField shakeBattleField;
    [SerializeField, Header("戦闘エフェクト")] BattleEffect battleEffect;
    [SerializeField, Header("回復エフェクト表示位置")] GameObject healEffectPos;
    
    [SerializeField] RelicEffectList relicEffect;
    [SerializeField] CardEffectList cardEffectList;
    
    public Dictionary<int, int> hasPlayerRelics = new Dictionary<int, int>(); //プレイヤーの所持しているレリック
    public Dictionary<string, int> playerCondition = new Dictionary<string, int>(); //プレイヤーに付与されている状態異常
    
    int playerMoney; //プレイヤーの所持金
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
        cardEffectList.ActiveCardEffect(card);
    }

    /// <summary>
    /// 各ステータスをセットする処理
    /// </summary>
    /// <param name="playerData">職種に応じたプレイヤーデータ</param>
    public void SetStatus(PlayerDataManager playerData)
    {
        GetSetHP = playerData._playerHP;
        GetSetCurrentHP = playerData._playerCurrentHP;
        GetSetAP = playerData._playerAP;
        GetSetConstAP = playerData._playerAP;
        GetSetCurrentAP = GetSetAP;
        GetSetGP = playerData._playerGP;
        playerMoney = playerData._playerMoney;
        InitializedCondition();
        GetSetCondition = playerCondition;
        GetSetInflictCondition = GetComponent<InflictCondition>();
        hasPlayerRelics = GameManager.Instance.hasRelics;
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
        //if (damage <= 0) return; //ダメージが0以下だった場合この処理を回さない

        int deductedDamage = 0;
        if (GetSetGP > 0) //ガードポイントがあったら
        {
            //ガードポイントの分だけダメージを軽減する
            deductedDamage = damage - GetSetGP;
            deductedDamage = deductedDamage < 0 ? 0 : deductedDamage;
            GetSetGP -= damage;
            ViewGard();
        }
        else
        {
            deductedDamage = damage;
        }
        GetSetCurrentHP -= deductedDamage;

        if (deductedDamage > 0)
        {
            AudioManager.Instance.PlaySE("攻撃1");
            ViewDamage(deductedDamage);
            shakeBattleField.Shake(0.25f, 10f);
        }
        else if (deductedDamage == 0)
        {
            AudioManager.Instance.PlaySE("攻撃1");
            ViewDamage(deductedDamage);
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
    /// ガードしたことを伝えるテキストを表示する処理
    /// </summary>
    void ViewGard()
    {
        // SE
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                AudioManager.Instance.PlaySE("guard1");
                break;
            case 1:
                AudioManager.Instance.PlaySE("guard2");
                break;
            case 2:
                AudioManager.Instance.PlaySE("guard3");
                break;
            default:
                break;
        }

        var gardPos = damageOrHealingPos.transform.position + new Vector3(0f, 120f, 0f);
        GameObject gardObj = Instantiate(gardUI, gardPos, Quaternion.identity, damageOrHealingPos.transform);
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
            AudioManager.Instance.PlaySE("回復");
            ViewHealing(healingHPPower);
            battleEffect.Heal(healEffectPos);
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
        playerConditionDisplay.UpdateConditionIcon(playerCondition);
    }

    //以下、TakeDamageやHealingHPに関係する状態異常の呼び出し
    /// <summary>
    /// 自動回復の呼び出し
    /// </summary>
    public void AutoHealing()
    {
        if (playerCondition["AutoHealing"] > 0)
        {
            HealingHP(GetSetInflictCondition.AutoHealing(playerCondition["AutoHealing"]));
        }
    }

    /// <summary>
    /// 火傷の呼び出し
    /// </summary>
    public void Burn()
    {
        if(playerCondition["Burn"] > 0)
        {
            TakeDamage(playerCondition["Burn"]);
        }
    }

    /// <summary>
    /// 邪毒の呼び出し
    /// </summary>
    /// <param name="moveCount">行動回数</param>
    public void Poison(int moveCount)
    {
        if (playerCondition["Poison"] > 0) 
        {
            var poison = GetSetInflictCondition.Poison(playerCondition["Poison"], moveCount);
            TakeDamage(poison);
        }
    }

    /// <summary>
    /// 戦闘開始時に発動するレリック効果
    /// </summary>
    /// <param name="enemyBattleAction">エネミーのステータス</param>
    /// <param name="enemyType">エネミーの種類</param>
    /// <returns>変更を加えたエネミーのステータス</returns>
    public EnemyBattleAction StartRelicEffect(EnemyBattleAction enemyBattleAction, string enemyType)
    {
        var es = enemyBattleAction;
        relicEffect.RelicID2(hasPlayerRelics[2]);
        GetSetConstAP = relicEffect.RelicID3(hasPlayerRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        GetSetConstAP = relicEffect.RelicID4(hasPlayerRelics[4], GetSetConstAP);
        GetSetConstAP = relicEffect.RelicID5(hasPlayerRelics[5], GetSetConstAP, GetSetChargeAP).constAP;
        es.AddConditionStatus("Burn", relicEffect.RelicID6(hasPlayerRelics[6]));
        GetSetGP = relicEffect.RelicID8(hasPlayerRelics[8], GetSetGP);
        relicEffect.RelicID12(hasPlayerRelics[12], enemyType);
        return es;
    }

    /// <summary>
    /// ラウンド終了時に一度だけ発動するレリック効果
    /// </summary>
    public void OnceEndRoundRelicEffect()
    {
        GetSetChargeAP = relicEffect.RelicID3(hasPlayerRelics[3], GetSetConstAP, GetSetChargeAP).chargeAP;
    }

    /// <summary>
    /// ラウンド終了時に発動するレリック効果
    /// </summary>
    public void EndRoundRelicEffect()
    {
        GetSetChargeAP = relicEffect.RelicID5(hasPlayerRelics[5], GetSetAP, GetSetChargeAP).chargeAP;
        playerCondition = relicEffect.RelicID11(hasPlayerRelics[11], playerCondition);
    }

    /// <summary>
    /// 戦闘終了時に発動するレリック効果
    /// </summary>
    /// <returns>戦闘終了後のゴールド獲得時に多く貰える数</returns>
    public int EndGameRelicEffect()
    {
        HealingHP(relicEffect.RelicID10(hasPlayerRelics[10]));
        return relicEffect.RelicID9(hasPlayerRelics[9]);
    }
}
