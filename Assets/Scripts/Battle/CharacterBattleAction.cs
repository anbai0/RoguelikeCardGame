using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//これは戦闘時にキャラクターが起こす行動の基本となるスクリプトです。
//今後PlayerとEnemyのBattleActionではこのスクリプトを継承します。
public class CharacterBattleAction : MonoBehaviour
{
    //ステータス
    int HP; //最大HP
    int currentHP; //現在のHP
    int AP; //最大AP
    int constAP; //戦闘開始時の最大AP
    int currentAP; //現在のAP
    int chargeAP; //ラウンドが進むごとに増加するAPの値
    int roundStartAP; //ラウンド開始時の最大AP(IsCurseの判定に用いる)
    int GP; //ダメージを防げるガードポイント
    ConditionStatus condition; //状態異常ステータス
    InflictCondition inflictCondition; //状態異常の効果
    public int GetSetHP { get => HP; set => HP = value; }
    public int GetSetCurrentHP { get => currentHP; set => currentHP = value; }
    public int GetSetAP { get => AP; set => AP = value; }
    public int GetSetConstAP { get => constAP; set => constAP = value; }
    public int GetSetCurrentAP { get => currentAP; set => currentAP = value; }
    public int GetSetChargeAP { get => chargeAP; set => chargeAP = value; }
    public int GetSetGP { get => GP; set => GP = value; }
    public ConditionStatus GetSetCondition { get => condition; set => condition = value; }
    public InflictCondition GetSetInflictCondition { get => inflictCondition; set => inflictCondition = value; }
    // Start is called before the first frame update
    void Start()
    {
        chargeAP = 0;
    }
    /// <summary>
    /// ラウンド開始時のAP計算
    /// </summary>
    public void SetUpAP() 
    {
        var curse = inflictCondition.Curse(constAP, chargeAP, condition.curse, condition.invalidBadStatus);
        AP = curse.nextRoundAP;
        currentAP = AP;
        condition.invalidBadStatus = curse.invalidBadStatus;
    }
    /// <summary>
    /// APを0にして行動不能にする
    /// </summary>
    public void TurnEnd()
    {
        currentAP = 0;
    }
    /// <summary>
    /// テキストの更新
    /// </summary>
    /// <param name="HPText">HPテキスト</param>
    /// <param name="APText">APテキスト</param>
    /// <param name="GPText">GPテキスト</param>
    /// <param name="HPSlider">HPスライダー(エネミーのみ)</param>
    public void UpdateText(Text HPText, Text APText, Text GPText, Slider HPSlider)
    {
        //currentHPの値は0以上HP以下
        currentHP = currentHP > HP ? HP : currentHP;
        currentHP = currentHP < 0 ? 0 : currentHP;
        //HPの表示
        HPText.text = currentHP + "/" + HP;
        //スライダーがあればHPのスライダーを表示
        if (HPSlider != null)
            HPSlider.value = currentHP / (float)HP;
        //currentAPの値は0以上AP以下
        currentAP = currentAP > AP ? AP : currentAP;
        currentAP = currentAP < 0 ? 0 : currentAP;
        //APの表示
        APText.text = currentAP + "/" + AP;
        //GPの値は0以上
        GP = GP < 0 ? 0 : GP;
        //GPの表示
        GPText.text = GP.ToString();
    }
    /// <summary>
    /// ラウンド終了時のAP増加
    /// </summary>
    public void ChargeAP() 
    {
        chargeAP++;
    }
    /// <summary>
    /// 死亡かどうか判定
    /// </summary>
    /// <returns></returns>
    public bool CheckHP() 
    {
        return currentHP <= 0 ? true : false;
    }
    /// <summary>
    /// ラウンド開始時のAPを保存しておく(Curseの計算用)
    /// </summary>
    public void SaveRoundAP() 
    {
        roundStartAP = AP;
    }
    /// <summary>
    /// 呪縛状態になっているか判定
    /// </summary>
    /// <returns>ラウンド開始時より最大APが減っていた場合trueを返す</returns>
    public bool IsCurse() 
    {
        //ターン終了時の最大AP
        int currentMaxAP = inflictCondition.Curse(constAP, chargeAP, condition.curse, condition.invalidBadStatus).nextRoundAP;
        return roundStartAP > currentMaxAP ? true : false;
    }

    //以下、状態異常の呼び出し
    /// <summary>
    /// 筋力増強の呼び出し
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    /// <returns>増加した攻撃力</returns>
    public int UpStrength(int attackPower)
    {
        attackPower = inflictCondition.UpStrength(attackPower, condition.upStrength);
        return attackPower;
    }
    /// <summary>
    /// 自動回復の呼び出し
    /// </summary>
    public void AutoHealing()
    {
        currentHP = inflictCondition.AutoHealing(currentHP, condition.autoHealing);
    }
    /// <summary>
    /// 焦燥の呼び出し
    /// </summary>
    public void Impatience()
    {
        var impatience = inflictCondition.Impatience(currentAP, condition.impatience, condition.invalidBadStatus);
        currentAP = impatience.currentAP;
        condition.invalidBadStatus = impatience.invalidBadStatus;
    }
    /// <summary>
    /// 衰弱の呼び出し
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    /// <returns>減少した攻撃力</returns>
    public int Weakness(int attackPower)
    {
        var weakness = inflictCondition.Weakness(attackPower, condition.weakness, condition.invalidBadStatus);
        attackPower = weakness.attackPower;
        condition.invalidBadStatus = weakness.invalidBadStatus;
        return attackPower;
    }
    /// <summary>
    /// 火傷の呼び出し
    /// </summary>
    public void Burn()
    {
        var burn = inflictCondition.Burn(currentHP, condition.burn, condition.invalidBadStatus);
        currentHP = burn.currentHP;
        condition.invalidBadStatus = burn.invalidBadStatus;
    }
    /// <summary>
    /// 邪毒の呼び出し
    /// </summary>
    /// <param name="moveCount">行動回数</param>
    public void Poison(int moveCount)
    {
        var poison = inflictCondition.Poison(currentHP, condition.poison, condition.invalidBadStatus, moveCount);
        currentHP = poison.currentHP;
        condition.invalidBadStatus = poison.invalidBadStatus;
    }

    //以下、カードや技の効果
    /// <summary>
    /// ダメージを受けたときの処理
    /// </summary>
    /// <param name="damage">受けるダメージ</param>
    public void TakeDamage(int damage) 
    {
        int deductedDamage = 0;
        if (GP > 0) //ガードポイントがあったら
        {
            //ガードポイントの分だけダメージを軽減する
            deductedDamage = damage - GP;
            deductedDamage = deductedDamage < 0 ? 0 : deductedDamage;
            GP -= damage;
        }
        else
        {
            deductedDamage = damage;
        }
        currentHP -= deductedDamage;
    }
    /// <summary>
    /// HPの回復
    /// </summary>
    /// <param name="healingHPPower">HPの回復量</param>
    public void HealingHP(int healingHPPower) 
    {
        currentHP += healingHPPower;
    }
    /// <summary>
    /// APの回復
    /// </summary>
    /// <param name="healingAPPower">APの回復量</param>
    public void HealingAP(int healingAPPower) //APの回復
    {
        currentAP += healingAPPower;
    }
    /// <summary>
    /// GPの追加
    /// </summary>
    /// <param name="addGP"></param>
    public void AddGP(int addGP) 
    {
        GP += addGP;
    }
    /// <summary>
    /// バッドステータスを解除する
    /// </summary>
    public void ReleaseBadStatus() 
    {
        condition.curse = 0;
        condition.impatience = 0;
        condition.weakness = 0;
        condition.burn = 0;
        condition.poison = 0;
    }
    /// <summary>
    /// バフステータスを解除する
    /// </summary>
    public void ReleaseBuffStatus() 
    {
        condition.upStrength = 0;
        condition.autoHealing = 0;
        condition.invalidBadStatus = 0;
    }
    /// <summary>
    /// バッドステータスがあるかチェックする
    /// </summary>
    /// <returns>バッドステータスの数</returns>
    public int CheckBadStatus() 
    {
        var charaC = condition;
        int badStatus = charaC.curse + charaC.impatience + charaC.weakness + charaC.burn + charaC.poison;
        return badStatus;
    }
    /// <summary>
    /// バフステータスがあるかチェックする
    /// </summary>
    /// <returns>バフステータスの数</returns>
    public int CheckBuffStatus() 
    {
        var charaC = condition;
        int buffStatus = charaC.upStrength + charaC.autoHealing + charaC.invalidBadStatus;
        return buffStatus;
    }
    /// <summary>
    /// 状態異常を追加する
    /// </summary>
    /// <param name="status">状態異常の名前</param>
    /// <param name="count">状態異常の個数</param>
    public void AddConditionStatus(string status, int count) 
    {
        if (status == "UpStrength")
        {
            condition.upStrength += count;
        }
        else if (status == "AutoHealing")
        {
            condition.autoHealing += count;
        }
        else if (status == "InvalidBadStatus")
        {
            condition.invalidBadStatus += count;
        }
        else if (status == "Curse")
        {
            condition.curse += count;
        }
        else if (status == "Impatience")
        {
            condition.impatience += count;
        }
        else if (status == "Weakness")
        {
            condition.weakness += count;
        }
        else if (status == "Burn")
        {
            condition.burn += count;
        }
        else if (status == "Poison")
        {
            condition.poison += count;
        }
    }
}
