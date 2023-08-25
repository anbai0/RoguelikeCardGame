using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SelfMadeNamespace;

/// <summary>
/// バトルの進行を進めるスクリプト
/// </summary>
public class BattleGameManager : MonoBehaviour
{
    [SerializeField] PlayerBattleAction playerScript;
    [SerializeField] EnemyBattleAction enemyScript;

    //プレイヤー
    PlayerDataManager playerData;
    [SerializeField] Image playerTurnDisplay;

    //エネミー
    EnemyDataManager enemyData;
    [SerializeField] Image enemyTurnDisplay;
    [SerializeField] SelectEnemyName selectEnemyName;
    [SerializeField] SelectEnemyData selectEnemyData;
    [SerializeField] SelectEnemyRelic selectEnemyRelic;
    public string enemyType = "SmallEnemy";
    string enemyName;
    
    //カード
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    [SerializeField] Transform PickCardPlace;
    [SerializeField] CardCostChange cardCostChange;
    List<int> deckNumberList;//プレイヤーのもつデッキナンバーのリスト

    //リザルト
    [SerializeField] BattleRewardManager battleRewardManager;
    [SerializeField] ResultAnimation resultAnimation;
    [SerializeField] GameObject uiManagerBR;
    [SerializeField] GameObject uiManagerBattle;

    //ラウンド
    [SerializeField] RoundTextAnimation roundTextAnimation;
    [SerializeField] GameObject turnEndBlackPanel;

    public bool isPlayerTurn;//プレイヤーのターンか判定//CardEffect()で使用
    private bool isPlayerMove;//プレイヤーか行動中か判定//TurnEnd()で使用
    private bool isTurnEnd;//行動終了ボタンを押したか判定//TurnEnd()で使用
    public bool isAccelerate;//カード＜アクセラレート＞を使用したか判定//TurnCalc()で使用
    public int accelerateValue;//カード＜アクセラレート＞で下げられるコストの値
    private bool isDecelerate;//カード＜アクセラレート＞の効果を無効かしたか判定//TurnCalc()で使用
    private bool isFirstCall;//最初のラウンドのときに呼ぶ判定//EndRound()で使用
    public bool isCoroutine;//コルーチンが動作中か判定//PlayerMove(),EnemyMove()で使用
    public bool isCoroutineEnabled;//動いているコルーチンが存在しているか判定//Update()で使用
    public bool isEnemyMoving;//エネミーのアクションが続いているか判定//PlayerMove(),EnemyMove()で使用
    public float turnTime = 1.0f;//ターンの切り替え時間
    public float roundTime = 2.0f;//ラウンドの切り替え時間
    private int playerMoveCount;//プレイヤーがラウンド中に行動した値//Curseの処理で使用
    private int enemyMoveCount;//エネミーがラウンド中に行動した値//Curseの処理で使用
    public int roundCount;//何ラウンド目かを記録する
    private bool isOnceEndRound; //EndRound()の呼び出しが一回だけか判定
    public int floor = 1; //現在の階層

    GameManager gm;

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
        gm = GameManager.Instance;
        floor = gm.floor;
        PlayerController playerController = "FieldScene".GetComponentInScene<PlayerController>();
        enemyType = playerController.enemyTag;
        StartBGM(enemyType);
        //初期化
        isPlayerTurn = false;
        isTurnEnd = false;
        isAccelerate = false;
        isDecelerate = false;
        isFirstCall = false;
        isCoroutine = false;
        isCoroutineEnabled = false;
        isEnemyMoving = false;
        isOnceEndRound = true;
        playerMoveCount = 0;
        enemyMoveCount = 0;
        accelerateValue = 0;
        roundCount = 0;
        turnEndBlackPanel.SetActive(false);
        enemyName = selectEnemyName.DecideEnemyName(floor, enemyType);
        playerData = gm.playerData; //GameManagerからプレイヤーのデータを受け取る
        ReadEnemy(enemyName); //エネミーの名前から新しくデータを作成
        SetStatus(playerData, enemyData); //それぞれのデータをBattleActionの変数に代入する
        StartRelicEffect();
        InitDeck();
        StartRound();
    }

    /// <summary>
    /// ラウンド開始時の効果処理
    /// </summary>
    private void StartRound() 
    {
        roundCount++;//ラウンド数を加算する
        var roundText = "Round" + roundCount.ToString(); //ラウンド数を表すテキスト
        roundTextAnimation.StartAnimation(roundText); //ラウンド数を表示
        if (isDecelerate) //アクセラレートの効果を初期化
        {
            cardCostChange.UndoCardCost();
            isDecelerate = false;
        }
        //ChageAPとCurseを加味してAPを決める
        playerScript.SetUpAP();
        playerScript.SaveRoundAP();
        enemyScript.SetUpAP();
        enemyScript.GetSetRoundEnabled = false;
        enemyScript.SaveRoundAP();

        isOnceEndRound = true;
        TurnCalc();
    }

    void Update()
    {
        if (isCoroutineEnabled == true && isCoroutine == false) //動いているコルーチンが存在しており、コルーチンが終了していた場合
        {
            isCoroutineEnabled = false; //動いているコルーチンは無し
            //一回だけTurnCalc()を呼ぶ
            TurnCalc();
        }
    }

    /// <summary>
    /// プレイヤーとエネミーの行動順を決める処理
    /// </summary>
    public void TurnCalc() 
    {
        if (isCoroutine) //コルーチンが回っているときはTurnCalc()を回さない
        {
            return;
        }
        if (IsGameEnd()) //戦闘の終了条件を満たしていないか確認する
        {
            //どちらかが先に戦闘不能になった場合、戦闘を止める
            return;
        }
        playerScript.ViewConditionIcon(); //プレイヤーの状態異常アイコンの更新
        enemyScript.ViewConditionIcon(); //エネミーの状態異常アイコンの更新

        if (isAccelerate)
        {
            cardCostChange.CardCostDown(accelerateValue);
            isAccelerate = false;
            isDecelerate = true;
        }

        int playerCurrentAP = playerScript.GetSetCurrentAP;
        int enemyCurrentAP = enemyScript.GetSetCurrentAP;
        if (playerCurrentAP > 0 || enemyCurrentAP > 0) //どちらかのAPが残っている場合
        {
            if (playerCurrentAP >= enemyCurrentAP) //APを比較して多い方が行動する
            {
                if (playerScript.IsCurse()) //呪縛になっていたら 
                {
                    //Curseの処理で減ったAPの更新
                    playerScript.CursedUpdateAP();
                    //変化したAPの値を保存
                    playerScript.SaveRoundAP();
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
                    enemyScript.CursedUpdateAP();
                    //変化したAPの値を保存
                    enemyScript.SaveRoundAP();
                }
                //エネミーの行動
                isPlayerTurn = false;
                playerTurnDisplay.enabled = false;
                enemyTurnDisplay.enabled = true;
                Invoke("EnemyMove", 1.0f);
            }
        }
        else //どちらも行動できない場合
        {
            //ラウンドを終了する
            playerTurnDisplay.enabled = false;
            enemyTurnDisplay.enabled = false;
            if (isOnceEndRound)
            {
                isOnceEndRound = false;
                Invoke("EndRound", roundTime);
            }
        }
    }

    /// <summary>
    /// プレイヤーの効果処理
    /// </summary>
    /// <param name="card">ドロップされたカード</param>
    public void PlayerMove(CardController card) 
    {
        if (playerScript.GetSetCurrentAP < card.cardDataManager._cardCost) //カードのコストがプレイヤーのAPを超えたら何もしない
        {
            return;
        }
        isPlayerMove = true;//プレイヤーは行動中
        playerScript.Move(card);
        playerMoveCount++;
        playerScript.AutoHealing();
        playerScript.Impatience();
        playerScript.Burn();
        TurnCalc();
    }

    /// <summary>
    /// エネミーの効果処理
    /// </summary>
    private void EnemyMove() 
    {
        enemyScript.Move();
        enemyMoveCount++;
        Invoke("TurnCalc", turnTime);
    }

    /// <summary>
    /// ラウンド終了時の効果処理
    /// </summary>
    private void EndRound() 
    {
        playerScript.Poison(playerMoveCount);
        enemyScript.Poison(enemyMoveCount);
        playerScript.ChargeAP();
        enemyScript.ChargeAP();
        if (!isFirstCall) { isFirstCall = true; OnceEndRoundRelicEffect(); }
        EndRoundRelicEffect();
        isTurnEnd = false;//行動終了ボタンの復活
        turnEndBlackPanel.SetActive(false); //TurnEndButtonの暗転を解除
        StartRound();
    }

    /// <summary>
    /// 戦闘終了時の処理
    /// </summary>
    /// <returns>いずれかのHPがなくなったときにtrueを返す</returns>
    private bool IsGameEnd()
    {
        if (playerScript.CheckHP()) //プレイヤーのHPがなくなったら
        {
            //エネミーの勝利演出
            StartCoroutine(LoseAnimation());
            return true;
        }
        if (enemyScript.CheckHP()) //エネミーのHPがなくなったら
        {
            //プレイヤーの勝利演出
            StartCoroutine(WinAnimation());
            return true;
        }
        return false;
    }

    IEnumerator WinAnimation()
    {
        enemyScript.EnemyDefeated(); //エネミーのやられた演出
        yield return new WaitForSeconds(4.0f);
        EndGameRelicEffect();
        yield return new WaitForSeconds(0.5f);
        resultAnimation.StartAnimation("Victory"); //勝利の文字を表示
        ReturnPlayerData();
        yield return new WaitForSeconds(1.0f);
        Destroy(uiManagerBattle);
        uiManagerBR.SetActive(true);　//UIManagerBattleRewardを使用可能に
        if (enemyType == "StrongEnemy" || enemyType == "Boss") //エネミーが強敵以上なら
        {
            uiManagerBR.GetComponent<UIManagerBattleReward>().isDisplayRelics = true; //報酬としてレリックも選択できるようにする
        }
        resultAnimation.DisappearResult(); //勝利の文字を消す
        battleRewardManager.ShowReward(enemyType); //報酬を表示
        uiManagerBR.GetComponent<UIManagerBattleReward>().UIEventsReload();
    }

    IEnumerator LoseAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        resultAnimation.StartAnimation("Defeated");
        yield return new WaitForSeconds(2.0f);
        battleRewardManager.TransitionLoseBattle();
    }
    //ここまでがゲームループ

    /// <summary>
    /// エネミーのデータを読み取る
    /// </summary>
    /// <param name="enemyName">エネミーの名前</param>
    private void ReadEnemy(string enemyName) 
    {
        enemyData = selectEnemyData.SetEnemyDataManager(floor, enemyName);
    }
    private void SetStatus(PlayerDataManager player, EnemyDataManager enemy)
    {
        //プレイヤーのステータスを割り振る
        playerScript.SetStatus(player);
        //エネミーのステータスを割り振る
        enemyScript.SetStatus(floor, enemy);
        enemyScript.hasEnemyRelics = selectEnemyRelic.SetEnemyRelics(enemyScript.hasEnemyRelics, floor, enemyName);
    }

    /// <summary>
    /// プレイヤーのデッキを生成する処理
    /// </summary>
    private void InitDeck() 
    {
        deckNumberList = playerData._deckList;
        for (int init = 0; init < deckNumberList.Count; init++)// デッキの枚数分
        {
            CardController card = Instantiate(cardPrefab, CardPlace);//カードを生成する
            card.name = "Deck" + init.ToString();//生成したカードに名前を付ける
            card.Init(deckNumberList[init]);//デッキデータの表示
            CardController pickCard = Instantiate(cardPrefab, PickCardPlace); //ピックされたとき用のカードも生成しておく
            pickCard.gameObject.GetComponent<PickCard>().enabled = true;
            pickCard.name = "Pick" + init.ToString();
            pickCard.Init(deckNumberList[init]);
            pickCard.transform.localScale *= 1.5f; //大きさを1.5倍にする
            pickCard.transform.Find("CardInfo").gameObject.SetActive(false); //非表示にしておく
            pickCard.GetComponent<CanvasGroup>().blocksRaycasts = false; //レイで選ばれないようにしておく
        }
    }

    /// <summary>
    /// 戦闘開始時に発動するレリック効果の処理
    /// </summary>
    public void StartRelicEffect() 
    {
        enemyScript = playerScript.StartRelicEffect(enemyScript, enemyType);
        playerScript = enemyScript.StartRelicEffect(playerScript);
    }

    /// <summary>
    /// ラウンド終了時に一度だけ発動するレリック効果の処理
    /// </summary>
    public void OnceEndRoundRelicEffect() 
    {
        playerScript.OnceEndRoundRelicEffect();
        enemyScript.OnceEndRoundRelicEffect();
    }

    /// <summary>
    /// ラウンド終了時に発動するレリック効果の処理
    /// </summary>
    public void EndRoundRelicEffect() 
    {
        playerScript.EndRoundRelicEffect();
        enemyScript.EndRoundRelicEffect();
    }

    /// <summary>
    /// 戦闘終了時に発動するレリック効果の処理
    /// </summary>
    public void EndGameRelicEffect() 
    {
        enemyScript.GetSetDropMoney += playerScript.EndGameRelicEffect();
    }

    /// <summary>
    /// 行動終了ボタンを押した処理 
    /// </summary>
    public void TurnEnd() 
    {
        if (!isTurnEnd && !isPlayerMove) //まだボタンが押されていなかったら押すことが出来る
        {
            AudioManager.Instance.PlaySE("選択音1");
            isTurnEnd = true;
            playerScript.TurnEnd();
            turnEndBlackPanel.SetActive(true); //TurnEndButtonの色を暗くする
            TurnCalc();
        }
    }

    /// <summary>
    /// 戦闘終了後に戦闘で変化したプレイヤーデータをGameManagerに返す
    /// </summary>
    void ReturnPlayerData()
    {
        if (playerScript.GetSetCurrentHP < gm.playerData._playerHP) //戦闘終了時の体力が元のプレイヤーの体力を上回っていなければ
        {
            gm.playerData._playerCurrentHP = playerScript.GetSetCurrentHP; //戦闘終了時の体力を返す
        }
        playerData._playerMoney += enemyScript.GetSetDropMoney; //コインを獲得
    }

    /// <summary>
    /// エネミーの種類に応じてBGMを流す
    /// </summary>
    /// <param name="_enemyType">エネミーの種類</param>
    void StartBGM(string _enemyType)
    {
        if (_enemyType == "SmallEnemy")
        {
            // BGMを流します
            if (Random.Range(0, 2) == 0)
            {
                AudioManager.Instance.PlayBGM("it's my turn");
            }
            else
            {
                AudioManager.Instance.PlayBGM("ファニーエイリアン");
            }
        }
        else if (_enemyType == "StrongEnemy")
        {
            AudioManager.Instance.PlayBGM("Social Documentary02");
        }
        else if (_enemyType == "Boss")
        {
            AudioManager.Instance.PlayBGM("深淵を覗く者");
        }
    }
}