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
    private EnemyAI enemyAI;
    bool roundEnabled; //ラウンド中に一度だけ判定を設ける
    public bool GetSetRoundEnabled { get => roundEnabled; set => roundEnabled = value; }
    private string debugMoveName = "無し";
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
    public void SetStatus(EnemyDataManager enemyData)
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
        enemyAI.SetEnemyState(enemyData._enemyName);
    }
}
