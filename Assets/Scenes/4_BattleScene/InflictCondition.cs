using UnityEngine;

/// <summary>
/// 状態異常の効果をまとめたスクリプト
/// </summary>
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
    /// <param name="autoHealing">自動回復</param>
    /// <returns>自動回復の値</returns>
    public int AutoHealing(int autoHealing)
    {
        //効果減衰などの処理があればここに書く
        return autoHealing;
    }

    /// <summary>
    /// デバフ:状態異常無効
    /// 効果:バッドステータスが状態異常無効の数だけ無効化する
    /// </summary>
    /// <param name="badStatus">付与されるバッドステータスの数</param>
    /// <param name="invalidBadStatus">状態異常無効の数</param>
    /// <returns>残りのバッドステータスの数,使用後の状態異常無効の数</returns>
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
    /// <returns>次のラウンドの最大AP</returns>
    public int Curse(int constAP, int chargeAP, int curse)
    {
        int nextRoundAP = constAP + chargeAP;
        //呪縛が0のときは効果を発動しない
        if (curse <= 0)
        {
            return nextRoundAP;
        }
        nextRoundAP = constAP + chargeAP - curse;
        if (nextRoundAP < 0)
        {
            nextRoundAP = 0;
        }
        return nextRoundAP;
    }

    /// <summary>
    /// デバフ:焦燥
    /// 効果:行動後にAPを(1×このデバフの数)消費する。
    /// </summary>
    public void Impatience()
    {
        //今のところ処理の必要は無し
    }

    /// <summary>
    /// デバフ:衰弱
    /// 効果:与えるダメージが(1×このバフの数)減少する
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    /// <param name="weakness">衰弱</param>
    /// <returns>減少した攻撃力</returns>
    public int Weakness(int attackPower, int weakness)
    {
        //衰弱が0のときは効果を発動しない
        if (weakness <= 0)
        {
            return attackPower;
        }
        attackPower = attackPower - weakness;
        if (attackPower < 0)
        {
            attackPower = 0;
        }
        return attackPower;
    }

    /// <summary>
    /// デバフ:火傷
    /// 効果:行動後に(1×このデバフの数)ダメージを受ける
    /// </summary>
    public void Burn()
    {
        //今のところ処理は無し
    }

    /// <summary>
    /// デバフ:邪毒
    /// 効果:ラウンド終了時、ラウンド中の行動数＊邪毒の数のダメージを受ける
    /// </summary>
    /// <param name="poison">邪毒</param>
    /// <param name="moveCount">ラウンド中の行動回数</param>
    /// <returns>受けるダメージ</returns>
    public int Poison(int poison, int moveCount)
    {
        int damage = 0;
        //邪毒が0のときは効果を発動しない
        if (poison <= 0)
        {
            return damage;
        }
        damage = moveCount * poison;
        return damage;
    }
}
