using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleGameManager : MonoBehaviour
{
    PlayerBattleAction playerScript;
    EnemyBattleAction enemyScript;
    //プレイヤー
    GameObject player;
    PlayerDataManager playerData;
    [SerializeField] Image playerTurnDisplay;
    PlayerConditionDisplay conditionDisplay;

    //エネミー
    EnemyDataManager enemyData;
    [SerializeField] Image enemyTurnDisplay;

    //カード
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    List<int> deckNumberList;//プレイヤーのもつデッキナンバーのリスト

    //レリック
    RelicStatus playerRelics;
    RelicEffectList relicEffect;

    public bool isPlayerTurn;//プレイヤーのターンか判定//CardEffect()で使用
    private bool isPlayerMove;//プレイヤーか行動中か判定//TurnEnd()で使用
    private bool isTurnEnd;//行動終了ボタンを押したか判定//TurnEnd()で使用
    public bool isAccelerate;//カード＜アクセラレート＞を使用したか判定//TurnCalc()で使用
    private bool isDecelerate;//カード＜アクセラレート＞の効果を無効かしたか判定//TurnCalc()で使用
    private bool isFirstCall;//最初のラウンドのときに呼ぶ判定//EndRound()で使用
    public bool isCoroutine;//コルーチンが動作中か判定//PlayerMove(),EnemyMove()で使用
    public float turnTime = 1.0f;//ターンの切り替え時間
    public float roundTime = 2.0f;//ラウンドの切り替え時間
    private int playerMoveCount;//プレイヤーがラウンド中に行動した値
    private int enemyMoveCount;//エネミーがラウンド中に行動した値
    private int AccelerateCount;//ラウンド中にプレイヤーが何回アクセラレートを使用したか記録する
    public int roundCount;//何ラウンド目かを記録する

    //Debug用
    int RelicID2 = 0;
    int RelicID3 = 0;
    int RelicID4 = 0;
    int RelicID5 = 0;
    int RelicID6 = 0;
    int RelicID7 = 0;
    int RelicID8 = 0;
    int RelicID9 = 0;
    int RelicID10 = 0;
    int RelicID11 = 0;
    int RelicID12 = 0;
    public string enemyName = "Slime";

    public static BattleGameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        playerScript = GetComponent<PlayerBattleAction>();
        enemyScript = GetComponent<EnemyBattleAction>();
        relicEffect = GetComponent<RelicEffectList>();
        conditionDisplay = GameObject.Find("ConditionPlace").GetComponent<PlayerConditionDisplay>();
        //初期化
        isPlayerTurn = false;
        isTurnEnd = false;
        isAccelerate = false;
        isDecelerate = false;
        isFirstCall = false;
        isCoroutine = false;
        playerMoveCount = 0;
        enemyMoveCount = 0;
        AccelerateCount = 0;
        roundCount = 0;
        player = GameObject.Find("TestPlayer");
        ReadPlayer(player);
        ReadEnemy(enemyName);
        SetStatus(playerData, enemyData);
        StartRelicEffect();
        InitDeck();
        StartRound();
    }
    private void StartRound() //ラウンド開始時の効果処理
    {
        roundCount++;//ラウンド数を加算する
        if (isDecelerate) //アクセラレートの効果を初期化
        {
            UndoCardCost();
            isDecelerate = false;
            AccelerateCount = 0;
        }
        //ChageAPとCurseを加味してAPを決める
        playerScript.SetUpAP();
        playerScript.SaveAP();
        enemyScript.SetUpAP();
        enemyScript.SaveAP();
        TurnCalc();
    }
    public void TurnCalc() //行動順を決める
    {
        conditionDisplay.ViewIcon(playerScript.GetSetPlayerCondition); //状態異常のアイコンの更新
        Debug.Log("now weakness amount is: " + playerScript.GetSetPlayerCondition.weakness);
        Debug.Log("now invalidBadStatuses amount is: " + playerScript.GetSetPlayerCondition.invalidBadStatus);
        if (isAccelerate)
        {
            CardCostDown();
            AccelerateCount++;
            isAccelerate = false;
            isDecelerate = true;
        }
        int playerCurrentAP = playerScript.GetSetPlayerCurrentAP;
        int enemyCurrentAP = enemyScript.GetSetEnemyCurrentAP;
        IsGameEnd();//戦闘の終了条件を満たしていないか確認する
        if (playerCurrentAP > 0 || enemyCurrentAP > 0) //どちらかのAPが残っている場合
        {
            if (playerCurrentAP >= enemyCurrentAP) //APを比較して多い方が行動する
            {
                if (playerScript.IsCurse()) //呪縛になっていたら 
                {
                    //Curseの処理で減ったAPの更新
                    playerScript.SetUpAP();
                }
                //プレイヤーの行動
                isPlayerTurn = true;
                isPlayerMove = false;
                playerTurnDisplay.enabled = true;
                enemyTurnDisplay.enabled = false;
            }
            else
            {
                if (enemyScript.IsCurse()) //呪縛になっていたら 
                {
                    //Curseの処理で減ったAPの更新
                    enemyScript.SetUpAP();
                }
                //エネミーの行動
                isPlayerTurn = false;
                playerTurnDisplay.enabled = false;
                enemyTurnDisplay.enabled = true;
                EnemyMove();
            }
        }
        else //どちらも行動できない場合
        {
            //ラウンドを終了する
            playerTurnDisplay.enabled = false;
            enemyTurnDisplay.enabled = false;
            Invoke("EndRound", roundTime);
        }
    }
    public void PlayerMove(CardController card) //プレイヤーの効果処理
    {
        if (isCoroutine) //コルーチンが動いているときに回ってきたらPlayerMoveは動かさない
            return;
        if (playerScript.GetSetPlayerCurrentAP < card.cardDataManager._cardCost) //カードのコストがプレイヤーのAPを超えたら何もしない
        {
            return;
        }
        isPlayerMove = true;//プレイヤーは行動中
        playerScript.Move(card);
        playerMoveCount++;
        playerScript.PlayerAutoHealing();
        playerScript.PlayerImpatience();
        playerScript.PlayerBurn();
        TurnCalc();
    }
    private void EnemyMove() //エネミーの効果処理
    {
        if (isCoroutine) //コルーチンが動いているときに回ってきたらEnemyMoveは動かさない
            return;
        enemyScript.Move();
        //playerScript.TakeDamage(enemyScript.Move());
        enemyMoveCount++;
        enemyScript.EnemyAutoHealing();
        enemyScript.EnemyImpatience();
        enemyScript.EnemyBurn();
        //playerScript.AddConditionStatus("Burn", 1);
        playerScript.AddConditionStatus("InvalidBadStatus", 1);
        Invoke("TurnCalc", turnTime);
    }
    private void EndRound() //ラウンド終了時の効果処理
    {
        playerScript.PlayerPoison(playerMoveCount);
        enemyScript.EnemyPoison(enemyMoveCount);
        playerScript.ChargeAP();
        enemyScript.ChargeAP();
        if (!isFirstCall) { isFirstCall = true; OnceEndRoundRelicEffect(); }
        EndRoundRelicEffect();
        isTurnEnd = false;//行動終了ボタンの復活
        StartRound();
    }
    private void IsGameEnd()
    {
        if (playerScript.CheckHP()) //プレイヤーのHPがなくなったら
        {
            //エネミーの勝利演出
            Debug.Log("Enemyの勝利");
        }
        if (enemyScript.CheckHP()) //エネミーのHPがなくなったら
        {
            EndGameRelicEffect();
            //プレイヤーの勝利演出
            Debug.Log("Playerの勝利");
        }
    }
    //ここまでがゲームループ
    //以下は関数(メソッド)
    private void ReadPlayer(GameObject player) //プレイヤーのデータを読み取る
    {
        if (player.CompareTag("Warrior"))
        {
            playerData = new PlayerDataManager("Warrior");
        }
        else if (player.CompareTag("Sorcerer"))
        {
            playerData = new PlayerDataManager("Sorcerer");
        }
    }
    private void ReadEnemy(string enemyName) //エネミーのデータを読み取る
    {
        if (enemyName == "Slime")
        {
            enemyData = new EnemyDataManager("Slime");
        }
        else if (enemyName == "SkeletonSwordsman")
        {
            enemyData = new EnemyDataManager("SkeletonSwordsman");
        }
        else if (enemyName == "Naga")
        {
            enemyData = new EnemyDataManager("Naga");
        }
        else if (enemyName == "Chimera")
        {
            enemyData = new EnemyDataManager("Chimera");
        }
        else if (enemyName == "DarkKnight")
        {
            enemyData = new EnemyDataManager("DarkKnight");
        }
    }
    private void SetStatus(PlayerDataManager player, EnemyDataManager enemy)
    {
        //プレイヤーのステータスを割り振る
        playerScript.SetStatus(player);
        deckNumberList = player._deckList;
        playerRelics = new RelicStatus();
        SetRelics(playerRelics);
        //エネミーのステータスを割り振る
        enemyScript.SetStatus(enemy);
    }
    private void InitDeck() //デッキ生成
    {
        deckNumberList = playerData._deckList;
        for (int init = 0; init < deckNumberList.Count; init++)// デッキの枚数分
        {
            CardController card = Instantiate(cardPrefab, CardPlace);//カードを生成する
            card.name = "Deck" + init.ToString();//生成したカードに名前を付ける
            card.Init(deckNumberList[init]);//デッキデータの表示
        }
    }
    private void SetRelics(RelicStatus playerRelics)
    {
        //Debug用に設定したもの、GameManagerからレリックのリストを受け取れるようになったら直接呼び出せるようにする
        playerRelics.hasRelicID2 = RelicID2;
        playerRelics.hasRelicID3 = RelicID3;
        playerRelics.hasRelicID4 = RelicID4;
        playerRelics.hasRelicID5 = RelicID5;
        playerRelics.hasRelicID6 = RelicID6;
        playerRelics.hasRelicID7 = RelicID7;
        playerRelics.hasRelicID8 = RelicID8;
        playerRelics.hasRelicID9 = RelicID9;
        playerRelics.hasRelicID10 = RelicID10;
        playerRelics.hasRelicID11 = RelicID11;
        playerRelics.hasRelicID12 = RelicID12;
    }
    public void StartRelicEffect() //戦闘開始時に発動するレリック効果
    {
        var ps = playerScript;
        var es = enemyScript;
        var pr = playerRelics;
        ps.GetSetPlayerCondition.upStrength = relicEffect.RelicID2(pr.hasRelicID2, ps.GetSetPlayerCondition.upStrength, es.GetSetEnemyCondition.upStrength).playerUpStrength;
        es.GetSetEnemyCondition.upStrength = relicEffect.RelicID2(pr.hasRelicID2, ps.GetSetPlayerCondition.upStrength, es.GetSetEnemyCondition.upStrength).enemyUpStrength;
        ps.GetSetPlayerConstAP = relicEffect.RelicID3(pr.hasRelicID3, ps.GetSetPlayerConstAP, ps.GetSetPlayerChargeAP).playerConstAP;
        ps.GetSetPlayerConstAP = relicEffect.RelicID4(pr.hasRelicID4, ps.GetSetPlayerConstAP);
        ps.GetSetPlayerConstAP = relicEffect.RelicID5(pr.hasRelicID5, ps.GetSetPlayerConstAP, ps.GetSetPlayerChargeAP).playerConstAP;
        ps.GetSetPlayerCondition.burn = relicEffect.RelicID6(pr.hasRelicID6, ps.GetSetPlayerCondition.burn);
        ps.GetSetPlayerHP = relicEffect.RelicID7(pr.hasRelicID7, ps.GetSetPlayerHP);
        ps.GetSetPlayerGP = relicEffect.RelicID8(pr.hasRelicID8, ps.GetSetPlayerGP);
        ps.GetSetPlayerCondition.upStrength = relicEffect.RelicID12(pr.hasRelicID12, "Slime", ps.GetSetPlayerCondition.upStrength);
        Debug.Log("スタート時のレリックが呼び出されました: " + ps.GetSetPlayerConstAP + " to " + ps.GetSetPlayerChargeAP);
    }
    public void OnceEndRoundRelicEffect() //ラウンド終了時に一度だけ発動するレリック効果
    {
        var ps = playerScript;
        var pr = playerRelics;
        ps.GetSetPlayerChargeAP = relicEffect.RelicID3(pr.hasRelicID3, ps.GetSetPlayerConstAP, ps.GetSetPlayerChargeAP).playerChargeAP;
        ps.GetSetPlayerChargeAP = relicEffect.RelicID5(pr.hasRelicID5, ps.GetSetPlayerAP, ps.GetSetPlayerChargeAP).playerChargeAP;
    }
    public void EndRoundRelicEffect() //ラウンド終了時に発動するレリック効果
    {
        var ps = playerScript;
        var pr = playerRelics;
        var pc = ps.GetSetPlayerCondition;
        (pc.curse, pc.impatience, pc.weakness, pc.burn, pc.poison) = relicEffect.RelicID11(pr.hasRelicID11, pc.curse, pc.impatience, pc.weakness, pc.burn, pc.poison);
    }
    public void EndGameRelicEffect() //戦闘終了時に発動するレリック効果
    {
        var ps = playerScript;
        var pr = playerRelics;
        int money = 10;
        money = relicEffect.RelicID9(playerRelics.hasRelicID9, money);
        ps.GetSetPlayerCurrentHP = relicEffect.RelicID10(pr.hasRelicID10, ps.GetSetPlayerCurrentHP);
    }
    public void TurnEnd() //行動終了ボタンを押した処理 
    {
        if (!isTurnEnd && !isPlayerMove) //まだボタンが押されていなかったら押すことが出来る
        {
            isTurnEnd = true;
            playerScript.TurnEnd();
            TurnCalc();
        }
    }
    private void CardCostDown() //CardEffectListのアクセラレートが発動時の処理
    {
        Transform deck = GameObject.Find("CardPlace").transform;
        foreach (Transform child in deck)
        {
            CardController deckCard = child.GetComponent<CardController>();
            if (deckCard.cardDataManager._cardType == "Attack")
            {
                deckCard.cardDataManager._cardCost -= 1;
                if (deckCard.cardDataManager._cardCost < 1)
                {
                    deckCard.cardDataManager._cardCost = 1;
                }
            }
            TextMeshProUGUI costText = child.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }
    private void UndoCardCost() //アクセラレートの効果を無効
    {
        Transform deck = GameObject.Find("CardPlace").transform;
        foreach (Transform child in deck)
        {
            CardController deckCard = child.GetComponent<CardController>();
            if (deckCard.cardDataManager._cardType == "Attack")
            {
                deckCard.cardDataManager._cardCost += AccelerateCount;
            }
            TextMeshProUGUI costText = child.transform.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }
}