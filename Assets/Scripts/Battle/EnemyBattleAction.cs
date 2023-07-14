using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleAction : MonoBehaviour
{
    [Header("エネミー名テキスト")]
    [SerializeField] Text enemyNameText;
    [Header("エネミー画像")]
    [SerializeField] Image enemyImage;
    [Header("エネミーHPテキスト")]
    [SerializeField] Text enemyHPText;
    [Header("エネミーHPスライダー")]
    [SerializeField] Slider enemyHPSlider;
    [SerializeField] Text moveText;
    private int enemyHP;//最大のHP
    private int enemyCurrentHP;//現在のHP
    private int enemyConstAP;//ゲーム開始時の最大AP
    private int roundStartAP;//ラウンド開始時の最大AP
    private int enemyAP;//最大AP
    private int enemyCurrentAP;//現在のAP
    private int enemyGP;//ガードポイント
    private int enemyChargeAP;//ラウンド経過で上昇していくAPの値
    private ConditionStatus enemyCondition;//エネミーの状態異常ステータス
    private InflictCondition inflictCondition;//状態異常の効果スクリプト
    private EnemyAI enemyAI;
    bool roundEnabled;
    private string debugMoveName = "無し";
    public int GetSetEnemyHP { get => enemyHP; set => enemyHP = value; }
    public int GetSetEnemyCurrentHP { get => enemyCurrentHP; set => enemyCurrentHP = value; }
    public int GetSetEnemyConstAP { get => enemyConstAP; set => enemyConstAP = value; }
    public int GetSetEnemyAP { get => enemyAP; set => enemyAP = value; }
    public int GetSetEnemyCurrentAP { get => enemyCurrentAP; set => enemyCurrentAP = value; }
    public int GetSetEnemyChargeAP { get => enemyChargeAP; set => enemyChargeAP = value; }
    public int GetSetEnemyGP { get => enemyGP; set => enemyGP = value; }
    public bool GetSetRoundEnabled { get => roundEnabled; set => roundEnabled = value; }
    public ConditionStatus GetSetEnemyCondition { get => enemyCondition; set => enemyCondition = value; }
    // Start is called before the first frame update
    void Start()
    {
        enemyChargeAP = 0;
        roundEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }
    public void SetUpAP()//ラウンド開始時のAP計算
    {
        var curse = inflictCondition.Curse(enemyConstAP, enemyChargeAP, enemyCondition.curse, enemyCondition.invalidBadStatus);
        enemyAP = curse.nextRoundAP;
        enemyCurrentAP = enemyAP;
        enemyCondition.invalidBadStatus = curse.invalidBadStatus;
        debugMoveName = "無し";
        roundEnabled = false;
    }
    public void Move()//エネミーの効果処理
    {
        var selectMove = enemyAI.SelectMove(enemyCurrentAP);
        string moveName = selectMove.moveName;
        debugMoveName = moveName;
        int moveCost = selectMove.moveCost;
        enemyCurrentAP -= moveCost;
        enemyAI.ActionMove(moveName);
        Debug.Log("エネミーの現在のAP:" + enemyCurrentAP);
    }
    public void TurnEnd()//エネミーを行動不能にする
    {
        enemyCurrentAP = 0;
    }
    public void SetStatus(EnemyDataManager enemyData)
    {
        enemyNameText.text = enemyData._enemyName;
        enemyImage.sprite = enemyData._enemyImage;
        enemyHPSlider.value = 1;
        enemyHP = enemyData._enemyHP;
        enemyCurrentHP = enemyHP;
        enemyConstAP = enemyData._enemyAP;
        enemyAP = enemyData._enemyAP;
        enemyCurrentAP = enemyAP;
        enemyGP = enemyData._enemyGP;
        enemyCondition = new ConditionStatus();
        inflictCondition = GetComponent<InflictCondition>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAI.SetEnemyState(enemyData._enemyName);
    }
    public void UpdateText() //各テキストの更新
    {
        if (enemyCurrentHP > enemyHP)
        {
            enemyCurrentHP = enemyHP;
        }
        if (enemyCurrentHP < 0)
        {
            enemyCurrentHP = 0;
        }
        enemyHPText.text = enemyCurrentHP + "/" + enemyHP;
        enemyHPSlider.value = enemyCurrentHP / (float)enemyHP;
        if (enemyCurrentAP > enemyAP)
        {
            enemyCurrentAP = enemyAP;
        }
        if (enemyCurrentAP < 0)
        {
            enemyCurrentAP = 0;
        }
        if (enemyGP < 0)
        {
            enemyGP = 0;
        }
        moveText.text = "現在の技:" + debugMoveName;
    }
    public void ChargeAP() //ラウンド終了時のAP増加
    {
        enemyChargeAP++;
    }
    public bool CheckHP() //エネミーが死んでいないかチェックする
    {
        if (enemyCurrentHP <= 0)
        {
            return true;
        }
        return false;
    }
    public void TakeDamage(int damage) //ダメージを受けたときの処理
    {
        int deductedDamage = 0;
        if (enemyGP > 0) //エネミーにガードポイントがあったら
        {
            //ガードポイントの分だけダメージを軽減する
            deductedDamage = damage - enemyGP;
            if (deductedDamage < 0)
            {
                deductedDamage = 0;
            }
            enemyGP -= damage;
        }
        else
        {
            deductedDamage = damage;
        }
        enemyCurrentHP -= deductedDamage;
    }
    public void SaveAP() //ラウンド開始時のAPを保存しておく
    {
        roundStartAP = enemyAP;
    }
    public bool IsCurse() //ラウンド開始時の最大APよりも現在の最大APが少ない場合は呪縛状態
    {
        //ターン終了時の最大AP
        int currentMaxAP = inflictCondition.Curse(enemyConstAP, enemyChargeAP, enemyCondition.curse, enemyCondition.invalidBadStatus).nextRoundAP;
        return roundStartAP > currentMaxAP ? true : false;
    }
    //状態異常
    public int EnemyUpStrengh(int attackPower)
    {
        attackPower = inflictCondition.UpStrength(attackPower, enemyCondition.upStrength);
        return attackPower;
    }
    public void EnemyAutoHealing()
    {
        enemyCurrentHP = inflictCondition.AutoHealing(enemyCurrentHP, enemyCondition.autoHealing);
    }
    public void EnemyImpatience()
    {
        var impatience = inflictCondition.Impatience(enemyCurrentAP, enemyCondition.impatience, enemyCondition.invalidBadStatus);
        enemyCurrentAP = impatience.currentAP;
        enemyCondition.invalidBadStatus = impatience.invalidBadStatus;
    }
    public int EnemyWeakness(int attackPower)
    {
        var weakness = inflictCondition.Weakness(attackPower, enemyCondition.weakness, enemyCondition.invalidBadStatus);
        attackPower = weakness.attackPower;
        enemyCondition.invalidBadStatus = weakness.invalidBadStatus;
        return attackPower;
    }
    public void EnemyBurn()
    {
        var burn = inflictCondition.Burn(enemyCurrentHP, enemyCondition.burn, enemyCondition.invalidBadStatus);
        enemyCurrentHP = burn.currentHP;
        enemyCondition.invalidBadStatus = burn.invalidBadStatus;
    }
    public void EnemyPoison(int moveCount)
    {
        var poison = inflictCondition.Poison(enemyCurrentHP, enemyCondition.poison, enemyCondition.invalidBadStatus, moveCount);
        enemyCurrentHP = poison.currentHP;
        enemyCondition.invalidBadStatus = poison.invalidBadStatus;
    }
    //カード効果
    public void HealingHP(int healingHPPower) //HPの回復
    {
        enemyCurrentHP += healingHPPower;
    }
    public void HealingAP(int healingAPPower) //APの回復
    {
        enemyCurrentAP += healingAPPower;
    }
    public void AddGP(int addGP) //GPの追加
    {
        enemyGP += addGP;
    }
    public void ReleaseBadStatus() //バッドステータスを解除する
    {
        enemyCondition.curse = 0;
        enemyCondition.impatience = 0;
        enemyCondition.weakness = 0;
        enemyCondition.burn = 0;
        enemyCondition.poison = 0;
    }
    public int CheckBadStatus() //バッドステータスがあるかチェックする
    {
        var pc = enemyCondition;
        int badStatus = pc.curse + pc.impatience + pc.weakness + pc.burn + pc.poison;
        return badStatus;
    }
    public void AddConditionStatus(string status, int count) //状態異常を追加する
    {
        if (status == "UpStrength")
        {
            enemyCondition.upStrength += count;
        }
        else if (status == "AutoHealing")
        {
            enemyCondition.autoHealing += count;
        }
        else if (status == "InvalidBadStatus")
        {
            enemyCondition.invalidBadStatus += count;
        }
        else if (status == "Curse")
        {
            enemyCondition.curse += count;
        }
        else if (status == "Impatience")
        {
            enemyCondition.impatience += count;
        }
        else if (status == "Weakness")
        {
            enemyCondition.weakness += count;
        }
        else if (status == "Burn")
        {
            enemyCondition.burn += count;
        }
        else if (status == "Poison")
        {
            enemyCondition.poison += count;
        }
    }
}
