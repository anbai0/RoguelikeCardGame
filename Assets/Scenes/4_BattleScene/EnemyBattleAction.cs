using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyBattleAction : CharacterBattleAction
{
    [Header("エネミー名テキスト")]
    [SerializeField] Text enemyNameText;
    [Header("エネミー画像")]
    [SerializeField] Image enemyImage;
    [Header("エネミーHPテキスト")]
    [SerializeField] Text enemyHPText;
    [Header("エネミーHPスライダー")]
    [SerializeField] Slider enemyHPSlider;
    [Header("エネミーAPテキスト")]
    [SerializeField] Text enemyAPText;
    [Header("エネミーGPテキスト")]
    [SerializeField] Text enemyGPText;
    [Header("技の名前")]
    [SerializeField] Text moveText;
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
    EnemyConditionDisplay enemyConditionDisplay;
    

    public Dictionary<int, int> hasEnemyRelics = new Dictionary< int, int>(); //エネミーの所持しているレリック
    const int maxRelics = 12;
    RelicEffectList relicEffect; //レリックの効果

    public Dictionary<string, int> enemyCondition = new Dictionary<string, int>(); //エネミーに付与されている状態異常

    int dropMoney = 0;
    public int GetSetDropMoney { get => dropMoney; set => dropMoney = value; } //エネミーが落とすコインの枚数

    EnemyAI enemyAI; //敵の行動スクリプト

    FlashImage flash; //敵行動時の演出

    BattleGameManager bg;

    bool roundEnabled; //ラウンド中に一度だけ判定を設ける
    public bool GetSetRoundEnabled { get => roundEnabled; set => roundEnabled = value; }
    
    string debugMoveName = "無し";

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
        moveText.text = "現在の技:" + debugMoveName;
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
        yield return new WaitForSeconds(0.5f);
        flash.StartFlash(Color.white, 0.1f);
        yield return new WaitForSeconds(0.2f);
        flash.StartFlash(Color.white, 0.1f);
        yield return new WaitForSeconds(0.1f);
        var selectMove = enemyAI.SelectMove(GetSetCurrentAP);
        string moveName = selectMove.moveName;
        debugMoveName = moveName;
        int moveCost = selectMove.moveCost;
        GetSetCurrentAP -= moveCost;
        enemyAI.ActionMove(moveName);
        Debug.Log("エネミーの現在のAP:" + GetSetCurrentAP);
        yield return StartCoroutine(MoveAfterCondition());
        bg.isEnemyMoving = false;
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
        enemyNameText.text = enemyData._enemyName;
        enemyImage.sprite = enemyData._enemyImage;
        enemyHPSlider.value = 1;
        GetSetHP = enemyData._enemyHP;
        GetSetCurrentHP = GetSetHP;
        GetSetConstAP = enemyData._enemyAP;
        GetSetAP = enemyData._enemyAP;
        GetSetCurrentAP = GetSetAP;
        GetSetGP = enemyData._enemyGP;
        dropMoney = enemyData._dropMoney;
        //enemyCondition = GetCondition;
        InitializedCondition();
        GetSetCondition = enemyCondition;
        GetSetInflictCondition = GetComponent<InflictCondition>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAI.SetEnemyState(floor, enemyData._enemyName);
        flash = GetComponent<FlashImage>();
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
        enemyConditionDisplay.ViewIcon(enemyCondition);
    }

    //以下、TakeDamageやHealingHPに関係する状態異常の呼び出し

    /// <summary>
    /// 自動回復の呼び出し
    /// </summary>
    public void AutoHealing()
    {
        HealingHP(GetSetInflictCondition.AutoHealing(enemyCondition["AutoHealing"]));
    }

    /// <summary>
    /// 火傷の呼び出し
    /// </summary>
    public void Burn()
    {
        var burn = GetSetInflictCondition.Burn(enemyCondition["Burn"], enemyCondition["InvalidBadStatus"]);
        TakeDamage(burn.damage);
        enemyCondition["InvalidBadStatus"] = burn.invalidBadStatus;
    }

    /// <summary>
    /// 邪毒の呼び出し
    /// </summary>
    /// <param name="moveCount">行動回数</param>
    public void Poison(int moveCount)
    {
        var poison = GetSetInflictCondition.Poison(enemyCondition["Poison"], enemyCondition["InvalidBadStatus"], moveCount);
        TakeDamage(poison.damage);
        enemyCondition["InvalidBadStatus"] = poison.invalidBadStatus;
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
        var relicEffectID2 = relicEffect.RelicID2(hasEnemyRelics[2], ps.playerCondition["UpStrength"], enemyCondition["UpStrength"]);
        enemyCondition["UpStrength"] = relicEffectID2.enemyUpStrength;
        ps.playerCondition["UpStrength"] = relicEffectID2.playerUpStrength;
        GetSetConstAP = relicEffect.RelicID3(hasEnemyRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        ps.playerCondition["Burn"] = relicEffect.RelicID6(hasEnemyRelics[6], ps.playerCondition["Burn"]);
        GetSetGP = relicEffect.RelicID8(hasEnemyRelics[8], GetSetGP);
        Debug.Log("スタート時のレリックが呼び出されました: " + GetSetConstAP + " to " + GetSetChargeAP);
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
        yield return new WaitForSeconds(1.0f);
        flash.StartFlash(Color.white, 0.5f);
        yield return new WaitForSeconds(1.0f);
        flash.StartFlash(Color.white, 1.0f);
        yield return new WaitForSeconds(1.0f);
        Destroy(enemyImage);
    }
}
