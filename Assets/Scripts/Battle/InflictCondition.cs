using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflictCondition : MonoBehaviour
{
    /// <summary>
    /// バフ:筋力増強
    /// 効果:与えるダメージが(1×このバフの数)上昇する
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    /// <param name="upStrength">筋力増強</param>
    /// <returns>筋力増強が加算された攻撃力</returns>
    public int UpStrength(int attackPower, int upStrength)
    {
        attackPower += upStrength;
        return attackPower;
    }
    /// <summary>
    /// バフ:自動回復
    /// 効果:行動後にHPを(1×このバフの数)回復する。
    /// </summary>
    /// <param name="hp">現在のHP</param>
    /// <param name="autoHealing">自動回復</param>
    /// <returns>回復した現在のHP</returns>
    public int AutoHealing(int hp, int autoHealing)
    {
        hp += autoHealing;
        return hp;
    }
    /// <summary>
    /// デバフ:状態異常無効
    /// 効果:バッドステータスが状態異常無効の数だけ無効化する
    /// </summary>
    /// <param name="badStatus">バッドステータス</param>
    /// <param name="invalidBadStatus">状態異常無効</param>
    /// <returns>残りのバッドステータス,消費後の状態異常無効</returns>
    public (int badStatus, int invalidBadStatus) InvalidBadStatus(int badStatus, int invalidBadStatus)
    {
        if (invalidBadStatus <= 0)
        {
            return (badStatus, invalidBadStatus);
        }
        int currentBadStatus = badStatus - invalidBadStatus;
        if (currentBadStatus < 0)
        {
            currentBadStatus = 0;
        }
        invalidBadStatus -= badStatus;
        if (invalidBadStatus < 0)
        {
            invalidBadStatus = 0;
        }
        return (currentBadStatus, invalidBadStatus);
    }
    /// <summary>
    /// デバフ:呪縛
    /// 効果:最大APが(1×このデバフの数)減少する。
    /// </summary>
    /// <param name="constAP">ゲーム開始時の最大AP</param>
    /// <param name="chargeAP">ラウンド経過で上昇していくAP</param>
    /// <param name="curse">呪縛</param>
    /// <param name="invalidBadStatus">状態異常無効</param>
    /// <returns>次のラウンドの最大AP,消費後の状態異常無効</returns>
    public (int nextRoundAP, int invalidBadStatus) Curse(int constAP, int chargeAP, int curse, int invalidBadStatus)
    {
        int nextRoundAP = constAP + chargeAP;
        //呪縛が0のときは効果を発動しない
        if (curse <= 0)
        {
            return (nextRoundAP, invalidBadStatus);
        }
        var invalidCurse = InvalidBadStatus(curse, invalidBadStatus);
        int currentCurse = invalidCurse.badStatus;
        if (currentCurse < 0)
        {
            currentCurse = 0;
        }
        nextRoundAP = constAP + chargeAP - currentCurse;
        if (nextRoundAP < 0)
        {
            nextRoundAP = 0;
        }
        return (nextRoundAP, invalidCurse.invalidBadStatus);
    }
    /// <summary>
    /// デバフ:焦燥
    /// 効果:行動後にAPを(1×このデバフの数)消費する。
    /// </summary>
    /// <param name="currentAP">現在のAP</param>
    /// <param name="impatience">焦燥</param>
    /// <param name="invalidBadStatus">状態異常無効</param>
    /// <returns>減少した現在のAP,消費後の状態異常無効</returns>
    public (int currentAP, int invalidBadStatus) Impatience(int currentAP, int impatience, int invalidBadStatus)
    {
        //焦燥が0のときは効果を発動しない
        if (impatience <= 0)
        {
            return (currentAP, invalidBadStatus);
        }
        var invalidImpatience = InvalidBadStatus(impatience, invalidBadStatus);
        int currentImpatience = invalidImpatience.badStatus;
        currentAP -= currentImpatience;
        if (currentAP < 0)
        {
            currentAP = 0;
        }
        return (currentAP, invalidImpatience.invalidBadStatus);
    }
    /// <summary>
    /// デバフ:衰弱
    /// 効果:与えるダメージが(1×このバフの数)減少する
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    /// <param name="weakness">衰弱</param>
    /// <param name="invalidBadStatus">状態異常無効</param>
    /// <returns>減少した攻撃力,消費後の状態異常無効</returns>
    public (int attackPower, int invalidBadStatus) Weakness(int attackPower, int weakness, int invalidBadStatus)
    {
        //衰弱が0のときは効果を発動しない
        if (weakness <= 0)
        {
            return (attackPower, invalidBadStatus);
        }
        var invalidWeakness = InvalidBadStatus(weakness, invalidBadStatus);
        int currentWeakness = invalidWeakness.badStatus;
        attackPower = attackPower - currentWeakness;
        if (attackPower < 0)
        {
            attackPower = 0;
        }
        return (attackPower, invalidWeakness.invalidBadStatus);
    }
    /// <summary>
    /// デバフ:火傷
    /// 効果:行動後に(1×このデバフの数)ダメージを受ける
    /// </summary>
    /// <param name="currentHP">現在のHP</param>
    /// <param name="burn">火傷</param>
    /// <param name="invalidBadStatus">状態異常無効</param>
    /// <returns>減少した現在のHP,消費後の状態異常無効</returns>
    public (int currentHP, int invalidBadStatus) Burn(int currentHP, int burn, int invalidBadStatus)
    {
        //火傷が0のときは効果を発動しない
        if (burn <= 0)
        {
            return (currentHP, invalidBadStatus);
        }
        var invalidBurn = InvalidBadStatus(burn, invalidBadStatus);
        int currentBurn = invalidBurn.badStatus;
        currentHP -= currentBurn;
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        return (currentHP, invalidBurn.invalidBadStatus);
    }
    /// <summary>
    /// デバフ:邪毒
    /// 効果:ラウンド終了時、ラウンド中の行動数＊邪毒の数のダメージを受ける
    /// </summary>
    /// <param name="currentHP">現在のHP</param>
    /// <param name="poison">邪毒</param>
    /// <param name="invalidBadStatus">状態異常無効</param>
    /// <param name="moveCount">ラウンド中の行動回数</param>
    /// <returns>減少した現在のHP,消費後の状態異常無効</returns>
    public (int currentHP, int invalidBadStatus) Poison(int currentHP, int poison, int invalidBadStatus, int moveCount)
    {
        //邪毒が0のときは効果を発動しない
        if (poison <= 0)
        {
            return (currentHP, invalidBadStatus);
        }
        var invalidPoison = InvalidBadStatus(poison, invalidBadStatus);
        int currentPoison = invalidPoison.badStatus;
        currentHP -= moveCount * currentPoison;
        if (currentHP < 0)
        {
            currentHP = 0;
        }
        return (currentHP, invalidPoison.invalidBadStatus);
    }
}
