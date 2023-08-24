using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 戦闘時にキャラクターが起こす行動の基本となるスクリプト
/// (PlayerとEnemyのBattleActionではこのスクリプトを継承)
/// </summary>
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
    public int GetSetHP { get => HP; set => HP = value; }
    public int GetSetCurrentHP { get => currentHP; set => currentHP = value; }
    public int GetSetAP { get => AP; set => AP = value; }
    public int GetSetConstAP { get => constAP; set => constAP = value; }
    public int GetSetCurrentAP { get => currentAP; set => currentAP = value; }
    public int GetSetChargeAP { get => chargeAP; set => chargeAP = value; }
    public int GetSetGP { get => GP; set => GP = value; }

    Dictionary<string, int> condition = new Dictionary<string, int>(); //付与されている状態異常
    InflictCondition inflictCondition; //状態異常の効果
    public Dictionary<string, int> GetSetCondition { get => condition; set => condition = value; }
    public InflictCondition GetSetInflictCondition { get => inflictCondition; set => inflictCondition = value; }

    bool isGardAppeared; //ガードのアイコンが表示されているか判定

    void Start()
    {
        chargeAP = 0;
        isGardAppeared = false;
    }
    
    /// <summary>
    /// ラウンド開始時のAP計算
    /// </summary>
    public void SetUpAP() 
    {
        var curse = inflictCondition.Curse(constAP, chargeAP, condition["Curse"], condition["InvalidBadStatus"]);
        AP = curse.nextRoundAP;
        currentAP = AP;
        condition["InvalidBadStatus"] = curse.invalidBadStatus;
    }

    /// <summary>
    /// 呪縛状態の時のAPを更新する処理
    /// </summary>
    public void CursedUpdateAP()
    {
        var curse = inflictCondition.Curse(constAP, chargeAP, condition["Curse"], condition["InvalidBadStatus"]);
        AP = curse.nextRoundAP;
        condition["InvalidBadStatus"] = curse.invalidBadStatus;
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
        //GPが無ければガードアイコンは表示しない
        var GPIcon = GPText.transform.parent.gameObject;
        if (GP == 0)
        {
            GPIcon.SetActive(false);
            isGardAppeared = false;
        }
        else
        {
            GPIcon.SetActive(true);
            if (!isGardAppeared)
            {
                isGardAppeared = true;
                GPIconAppearance(GPIcon);
            }
        }
        //GPの表示
        GPText.text = GP.ToString();
    }
    
    /// <summary>
    /// ガードアイコンを出現させるときに動かすアニメーション
    /// </summary>
    /// <param name="_GPIcon">各キャラクターのGPアイコン</param>
    private void GPIconAppearance(GameObject _GPIcon)
    {
        _GPIcon.GetComponent<FlashImage>().StartFlash(Color.white, 0.1f);
        _GPIcon.GetComponent<IconAnimation>().StartAnimation();
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
        int currentMaxAP = inflictCondition.Curse(constAP, chargeAP, condition["Curse"], condition["InvalidBadStatus"]).nextRoundAP;
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
        attackPower = inflictCondition.UpStrength(attackPower, condition["UpStrength"]);
        return attackPower;
    }
    
    /// <summary>
    /// 焦燥の呼び出し
    /// </summary>
    public void Impatience()
    {
        var impatience = inflictCondition.Impatience(condition["Impatience"], condition["InvalidBadStatus"]);
        currentAP -= impatience.impatience;
        condition["InvalidBadStatus"] = impatience.invalidBadStatus;
    }
    
    /// <summary>
    /// 衰弱の呼び出し
    /// </summary>
    /// <param name="attackPower">攻撃力</param>
    /// <returns>減少した攻撃力</returns>
    public int Weakness(int attackPower)
    {
        var weakness = inflictCondition.Weakness(attackPower, condition["Weakness"], condition["InvalidBadStatus"]);
        attackPower = weakness.attackPower;
        condition["InvalidBadStatus"] = weakness.invalidBadStatus;
        return attackPower;
    }

    //以下、カードや技の効果
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
        condition["Curse"] = 0;
        condition["Impatience"] = 0;
        condition["Weakness"] = 0;
        condition["Burn"] = 0;
        condition["Poison"] = 0;
    }
    
    /// <summary>
    /// バフステータスを解除する
    /// </summary>
    public void ReleaseBuffStatus() 
    {
        condition["UpStrength"] = 0;
        condition["AutoHealing"] = 0;
        condition["InvalidBadStatus"] = 0;
    }
    
    /// <summary>
    /// バッドステータスがあるかチェックする
    /// </summary>
    /// <returns>バッドステータスの数</returns>
    public int CheckBadStatus() 
    {
        var charaC = condition;
        int badStatus = charaC["Curse"] + charaC["Impatience"] + charaC["Weakness"] + charaC["Burn"] + charaC["Poison"];
        return badStatus;
    }
    
    /// <summary>
    /// バフステータスがあるかチェックする
    /// </summary>
    /// <returns>バフステータスの数</returns>
    public int CheckBuffStatus() 
    {
        var charaC = condition;
        int buffStatus = charaC["UpStrength"] + charaC["AutoHealing"] + charaC["InvalidBadStatus"];
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
            condition["UpStrength"] += count;
        }
        else if (status == "AutoHealing")
        {
            condition["AutoHealing"] += count;
        }
        else if (status == "InvalidBadStatus")
        {
            condition["InvalidBadStatus"] += count;
        }
        else if (status == "Curse")
        {
            condition["Curse"] += count;
        }
        else if (status == "Impatience")
        {
            condition["Impatience"] += count;
        }
        else if (status == "Weakness")
        {
            condition["Weakness"] += count;
        }
        else if (status == "Burn")
        {
            condition["Burn"] += count;
        }
        else if (status == "Poison")
        {
            condition["Poison"] += count;
        }
    }
}
