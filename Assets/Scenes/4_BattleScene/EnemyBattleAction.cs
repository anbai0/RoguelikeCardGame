using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// エネミーの行動をまとめたスクリプト
/// </summary>
public class EnemyBattleAction : CharacterBattleAction
{
    [SerializeField, Header("エネミー画像")] Image enemyImage;
    [SerializeField, Header("エネミーHPテキスト")] Text enemyHPText;
    [SerializeField, Header("エネミーHPスライダー")] Slider enemyHPSlider;
    [SerializeField, Header("エネミーAPテキスト")] Text enemyAPText;
    [SerializeField, Header("エネミーGPテキスト")] Text enemyGPText;
    [SerializeField, Header("ダメージ表示オブジェクト")] GameObject damageUI;
    [SerializeField, Header("回復表示オブジェクト")] GameObject healingUI;
    [SerializeField, Header("ガード表示オブジェクト")] GameObject gardUI;
    [SerializeField, Header("ダメージと回復表示の出現場所")] GameObject damageOrHealingPos;
    [SerializeField, Header("状態異常のアイコン表示スクリプト")] EnemyConditionDisplay enemyConditionDisplay;
    [SerializeField, Header("３層目の敵が持つレリック")] RelicController enemyRelic;
    [SerializeField, Header("レリックの表示位置")] Transform enemyRelicPlace;

    [SerializeField] EnemyAI enemyAI;
    [SerializeField] FlashImage flash;

    public Dictionary<int, int> hasEnemyRelics = new Dictionary< int, int>(); //エネミーの所持しているレリック
    const int maxRelics = 12;
    RelicEffectList relicEffect; //レリックの効果

    public Dictionary<string, int> enemyCondition = new Dictionary<string, int>(); //エネミーに付与されている状態異常

    int dropMoney = 0;
    public int GetSetDropMoney { get => dropMoney; set => dropMoney = value; } //エネミーが落とすコインの枚数

    bool roundEnabled; //ラウンド中に一度だけ判定を設ける
    public bool GetSetRoundEnabled { get => roundEnabled; set => roundEnabled = value; }
    
    BattleGameManager bg;

    void Awake()
    {
        InitializeRelics();
    }

    /// <summary>
    /// エネミーのレリックのIDと所持数を初期化する
    /// </summary>
    private void InitializeRelics()
    {
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            hasEnemyRelics.Add(RelicID, 0);
        }
    }

    void Start()
    {
        bg = BattleGameManager.Instance;
        roundEnabled = false;
    }

    void Update()
    {
        UpdateText(enemyHPText, enemyAPText, enemyGPText, enemyHPSlider);
    }

    /// <summary>
    /// エネミーの行動処理
    /// </summary>
    public void Move()//エネミーの効果処理
    {
        StartCoroutine(MoveFlash());
    }
    IEnumerator MoveFlash()
    {
        bg.isEnemyMoving = true;
        var selectMove = enemyAI.SelectMove(GetSetCurrentAP);
        string moveName = selectMove.moveName;
        if (moveName == "RoundEnd") //EnemyAIで選択された行動が行動終了の場合
        {
            TurnEnd();
            bg.TurnCalc();
            yield break;
        }
        yield return new WaitForSeconds(1.0f);
        flash.StartFlash(Color.white, 0.1f);
        yield return new WaitForSeconds(0.2f);
        flash.StartFlash(Color.white, 0.1f);
        yield return new WaitForSeconds(0.2f);
        int moveCost = selectMove.moveCost;
        GetSetCurrentAP -= moveCost;
        enemyAI.ActionMove(moveName);
        yield return StartCoroutine(MoveAfterCondition());
        bg.isEnemyMoving = false;
        bg.TurnCalc();
        yield break;
    }

    IEnumerator MoveAfterCondition()
    {
        AutoHealing();
        Impatience();
        Burn();
        yield return null;
    }

    /// <summary>
    /// 各ステータスをセットする処理
    /// </summary>
    /// <param name="floor">現在の階層</param>
    /// <param name="enemyData">選択されたエネミーデータ</param>
    public void SetStatus(int floor, EnemyDataManager enemyData)
    {
        enemyImage.sprite = enemyData._enemyImage;
        enemyHPSlider.value = 1;
        GetSetHP = enemyData._enemyHP;
        GetSetCurrentHP = GetSetHP;
        GetSetConstAP = enemyData._enemyAP;
        GetSetAP = enemyData._enemyAP;
        GetSetCurrentAP = GetSetAP;
        GetSetGP = enemyData._enemyGP;
        dropMoney = enemyData._dropMoney;
        InitializedCondition();
        GetSetCondition = enemyCondition;
        GetSetInflictCondition = GetComponent<InflictCondition>();
        enemyAI.SetEnemyState(floor, enemyData._enemyName);
    }

    /// <summary>
    /// 状態異常の名前と所持数を初期化しておく処理
    /// </summary>
    void InitializedCondition()
    {
        enemyCondition.Add("UpStrength", 0);
        enemyCondition.Add("AutoHealing", 0);
        enemyCondition.Add("InvalidBadStatus", 0);
        enemyCondition.Add("Curse", 0);
        enemyCondition.Add("Impatience", 0);
        enemyCondition.Add("Weakness", 0);
        enemyCondition.Add("Burn", 0);
        enemyCondition.Add("Poison", 0);
    }

    // 所持しているレリックの表示
    public void ViewEnemyRelic(GameManager gm)
    {
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            //辞書内に指定したRelicIDのキーが存在するかどうかとレリックを１つ以上所持しているか
            if (hasEnemyRelics.ContainsKey(RelicID) && hasEnemyRelics[RelicID] >= 1)
            {
                RelicController relic = Instantiate(enemyRelic, enemyRelicPlace);
                relic.Init(RelicID);                                               // 取得したRelicControllerのInitメソッドを使いレリックの生成と表示をする

                relic.transform.GetChild(4).gameObject.SetActive(true);
                relic.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = hasEnemyRelics[RelicID].ToString();      // Prefabの子オブジェクトである所持数を表示するテキストを変更

                relic.transform.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[RelicID]._relicName.ToString();        // レリックの名前を変更
                relic.transform.GetChild(5).GetChild(1).GetComponent<TextMeshProUGUI>().text = gm.relicDataList[RelicID]._relicEffect.ToString();      // レリック説明変更

            }
        }
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

        if (deductedDamage >= 0)
        {
            AudioManager.Instance.PlaySE("攻撃1");
            StartCoroutine(DamageFlash(deductedDamage));
        }
    }
    IEnumerator DamageFlash(int deductedDamage)
    {
        flash.StartFlash(Color.white, 0.1f);
        yield return new WaitForSeconds(0.1f);
        ViewDamage(deductedDamage);
        yield break;
    }

    /// <summary>
    /// ダメージ演出を表示する処理
    /// </summary>
    /// <param name="damage">受けたダメージ</param>
    private void ViewDamage(int damage)
    {
        GameObject damageObj = Instantiate(damageUI, damageOrHealingPos.transform);
        //float rndX = Random.Range(-50, 50);
        //float rndY = Random.Range(-50, 50);
        //damageObj.transform.position += new Vector3(rndX, rndY, 0);
        damageObj.GetComponent<TextMeshProUGUI>().text = damage.ToString();
    }

    /// <summary>
    /// ガードしたことを伝えるテキストを表示する処理
    /// </summary>
    void ViewGard()
    {
        // SE
        int rand = Random.Range(0,3);
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
    public void HealingHP(int healingHPPower) //HPの回復
    {
        GetSetCurrentHP += healingHPPower;
        if (healingHPPower > 0)
        {
            AudioManager.Instance.PlaySE("回復");
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
        enemyConditionDisplay.UpdateConditionIcon(enemyCondition);
    }

    //以下、TakeDamageやHealingHPに関係する状態異常の呼び出し

    /// <summary>
    /// 自動回復の呼び出し
    /// </summary>
    public void AutoHealing()
    {
        if (enemyCondition["AutoHealing"] > 0)
        {
            HealingHP(GetSetInflictCondition.AutoHealing(enemyCondition["AutoHealing"]));
        }
    }

    /// <summary>
    /// 火傷の呼び出し
    /// </summary>
    public void Burn()
    {
        if (enemyCondition["Burn"] > 0) 
        {
            TakeDamage(enemyCondition["Burn"]);
        }
    }

    /// <summary>
    /// 邪毒の呼び出し
    /// </summary>
    /// <param name="moveCount">行動回数</param>
    public void Poison(int moveCount)
    {
        if (enemyCondition["Poison"] > 0)
        {
            var poison = GetSetInflictCondition.Poison(enemyCondition["Poison"], moveCount);
            TakeDamage(poison);
        }
    }

    /// <summary>
    /// 戦闘開始時に発動するレリック効果
    /// </summary>
    /// <param name="playerBattleAction">プレイヤーのステータス</param>
    /// <returns>変更を加えたプレイヤーのステータス</returns>
    public PlayerBattleAction StartRelicEffect(PlayerBattleAction playerBattleAction)
    {
        relicEffect = GetComponent<RelicEffectList>();
        var ps = playerBattleAction;
        relicEffect.RelicID2(hasEnemyRelics[2]);
        GetSetConstAP = relicEffect.RelicID3(hasEnemyRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        ps.AddConditionStatus("Burn", relicEffect.RelicID6(hasEnemyRelics[6]));
        GetSetGP = relicEffect.RelicID8(hasEnemyRelics[8], GetSetGP);
        return ps;
    }

    /// <summary>
    /// ラウンド終了時に一度だけ発動するレリック効果
    /// </summary>
    public void OnceEndRoundRelicEffect()
    {
        GetSetChargeAP = relicEffect.RelicID3(hasEnemyRelics[3], GetSetConstAP, GetSetChargeAP).chargeAP;
    }

    /// <summary>
    /// ラウンド終了時に発動するレリック効果
    /// </summary>
    public void EndRoundRelicEffect()
    {
        enemyCondition = relicEffect.RelicID11(hasEnemyRelics[11], enemyCondition);
    }

    /// <summary>
    /// 負けたときのアニメーション
    /// </summary>
    public void EnemyDefeated()
    {
        StartCoroutine(EnemyDefeatedAnimation());
    }
    IEnumerator EnemyDefeatedAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        flash.StartFlash(Color.white, 0.5f);
        yield return new WaitForSeconds(1.2f);
        flash.StartFlash(Color.white, 0.5f);
        yield return new WaitForSeconds(1.2f);
        flash.StartFlash(Color.white, 1.0f);
        yield return new WaitForSeconds(1.0f);
        Destroy(enemyImage);
    }
}
