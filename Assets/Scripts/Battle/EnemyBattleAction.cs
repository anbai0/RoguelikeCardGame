using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Dictionary<int, int> hasEnemyRelics = new Dictionary< int, int>(); //エネミーの所持しているレリック
    const int maxRelics = 12;
    private EnemyAI enemyAI;
    RelicEffectList relicEffect; //レリックの効果
    bool roundEnabled; //ラウンド中に一度だけ判定を設ける
    public bool GetSetRoundEnabled { get => roundEnabled; set => roundEnabled = value; }
    private string debugMoveName = "無し";
    void Awake()
    {
        InitializeRelics();
    }

    private void InitializeRelics()
    {
        for (int RelicID = 1; RelicID <= maxRelics; RelicID++)
        {
            hasEnemyRelics.Add(RelicID, 0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        roundEnabled = false;
    }

    // Update is called once per frame
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
        var selectMove = enemyAI.SelectMove(GetSetCurrentAP);
        string moveName = selectMove.moveName;
        debugMoveName = moveName;
        int moveCost = selectMove.moveCost;
        GetSetCurrentAP -= moveCost;
        enemyAI.ActionMove(moveName);
        Debug.Log("エネミーの現在のAP:" + GetSetCurrentAP);
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
        GetSetCondition = new ConditionStatus();
        GetSetInflictCondition = GetComponent<InflictCondition>();
        enemyAI = GetComponent<EnemyAI>();
        enemyAI.SetEnemyState(floor, enemyData._enemyName);
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
        var relicEffectID2 = relicEffect.RelicID2(hasEnemyRelics[2], ps.GetSetCondition.upStrength, GetSetCondition.upStrength);
        GetSetCondition.upStrength = relicEffectID2.enemyUpStrength;
        ps.GetSetCondition.upStrength = relicEffectID2.playerUpStrength;
        GetSetConstAP = relicEffect.RelicID3(hasEnemyRelics[3], GetSetConstAP, GetSetChargeAP).constAP;
        ps.GetSetCondition.burn = relicEffect.RelicID6(hasEnemyRelics[6], ps.GetSetCondition.burn);
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
        GetSetCondition = relicEffect.RelicID11(hasEnemyRelics[11], GetSetCondition);
    }
}
