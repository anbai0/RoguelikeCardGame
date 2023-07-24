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
    public void SetUpAP() //ラウンド開始時のAP計算
    {
        var curse = inflictCondition.Curse(constAP, chargeAP, condition.curse, condition.invalidBadStatus);
        AP = curse.nextRoundAP;
        currentAP = AP;
        condition.invalidBadStatus = curse.invalidBadStatus;
    }
    public void TurnEnd() //行動不能にする
    {
        currentAP = 0;
    }
    public void UpdateText(Text HPText, Text APText, Text GPText, Slider HPSlider) //テキストの更新
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
    public void ChargeAP() //ラウンド終了時のAP増加
    {
        chargeAP++;
    }
    public bool CheckHP() //死亡かどうか判定する
    {
        return currentHP <= 0 ? true : false;
    }
    public void SaveRoundAP() //ラウンド開始時のAPを保存しておく
    {
        roundStartAP = AP;
    }
    public bool IsCurse() //呪縛状態になっているか確認する
    {
        //ターン終了時の最大AP
        int currentMaxAP = inflictCondition.Curse(constAP, chargeAP, condition.curse, condition.invalidBadStatus).nextRoundAP;
        return roundStartAP > currentMaxAP ? true : false; //ラウンド開始時より最大APが減っていた場合trueを返す
    }

    //以下、状態異常の効果処理
    public int UpStrength(int attackPower)
    {
        attackPower = inflictCondition.UpStrength(attackPower, condition.upStrength);
        return attackPower;
    }
    public void AutoHealing()
    {
        currentHP = inflictCondition.AutoHealing(currentHP, condition.autoHealing);
    }
    public void Impatience()
    {
        var impatience = inflictCondition.Impatience(currentAP, condition.impatience, condition.invalidBadStatus);
        currentAP = impatience.currentAP;
        condition.invalidBadStatus = impatience.invalidBadStatus;
    }
    public int Weakness(int attackPower)
    {
        var weakness = inflictCondition.Weakness(attackPower, condition.weakness, condition.invalidBadStatus);
        attackPower = weakness.attackPower;
        condition.invalidBadStatus = weakness.invalidBadStatus;
        return attackPower;
    }
    public void Burn()
    {
        var burn = inflictCondition.Burn(currentHP, condition.burn, condition.invalidBadStatus);
        currentHP = burn.currentHP;
        condition.invalidBadStatus = burn.invalidBadStatus;
    }
    public void Poison(int moveCount)
    {
        var poison = inflictCondition.Poison(currentHP, condition.poison, condition.invalidBadStatus, moveCount);
        currentHP = poison.currentHP;
        condition.invalidBadStatus = poison.invalidBadStatus;
    }

    //以下、カードや技の効果
    public void TakeDamage(int damage) //ダメージを受けたときの処理
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
    public void HealingHP(int healingHPPower) //HPの回復
    {
        currentHP += healingHPPower;
    }
    public void HealingAP(int healingAPPower) //APの回復
    {
        currentAP += healingAPPower;
    }
    public void AddGP(int addGP) //GPの追加
    {
        GP += addGP;
    }
    public void ReleaseBadStatus() //バッドステータスを解除する
    {
        condition.curse = 0;
        condition.impatience = 0;
        condition.weakness = 0;
        condition.burn = 0;
        condition.poison = 0;
    }
    public int CheckBadStatus() //バッドステータスがあるかチェックする
    {
        var charaC = condition;
        int badStatus = charaC.curse + charaC.impatience + charaC.weakness + charaC.burn + charaC.poison;
        return badStatus;
    }
    public void AddConditionStatus(string status, int count) //状態異常を追加する
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
