using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SelfMadeNamespace;
using DG.Tweening;

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
    float enemyMoveTime = 0.5f;
    
    //カード
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    [SerializeField] GameObject DropPlace;
    Vector3 originCardPlace;
    [SerializeField] Transform PickCardPlace;
    [SerializeField] CardCostChange cardCostChange;
    public List<int> deckNumberList;//プレイヤーのもつデッキナンバーのリスト

    //レリック
    public int relicID2Player;
    public int relicID2Enemy;

    //リザルト
    [SerializeField] BattleRewardManager battleRewardManager;
    [SerializeField] ResultAnimation resultAnimation;
    [SerializeField] GameObject uiManagerBR;
    [SerializeField] GameObject uiManagerBattle;

    //ラウンド
    [SerializeField] RoundTextAnimation roundTextAnimation;
    [SerializeField] GameObject turnEndBlackPanel;

    //エフェクト
    [SerializeField] BattleEffect battleEffect;
    [SerializeField] CanvasGroup turnEndButtonGroup;
    Tween buttonTween;

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
    public float turnTime = 0.5f;//ターンの切り替え時間
    public float roundTime = 1.0f;//ラウンドの切り替え時間
    private int playerMoveCount;//プレイヤーがラウンド中に行動した値//Curseの処理で使用
    private int enemyMoveCount;//エネミーがラウンド中に行動した値//Curseの処理で使用
    public int roundCount;//何ラウンド目かを記録する
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
        //floor = 1;
        enemyType = PlayerController.Instance.enemyTag;
        StartBGM(enemyType);
        //初期化
        originCardPlace = CardPlace.transform.position;
        isPlayerTurn = false;
        isTurnEnd = false;
        isAccelerate = false;
        isDecelerate = false;
        isFirstCall = false;
        isCoroutine = false;
        isCoroutineEnabled = false;
        isEnemyMoving = false;
        relicID2Player = 0;
        relicID2Enemy = 0;
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
        
        // バトル終了時、カードに触れなくする
        if ((playerScript.CheckHP() || enemyScript.CheckHP() || !isPlayerTurn))
        {
            CanvasGroup[] cards = CardPlace.GetComponentsInChildren<CanvasGroup>();
            foreach (CanvasGroup card in cards)
            {
                card.blocksRaycasts = false;
            }
            CardPlace.transform.position = originCardPlace + new Vector3(0, -150, 0);
        }
        else if(isPlayerTurn)
        {
            CanvasGroup[] cards = CardPlace.GetComponentsInChildren<CanvasGroup>();
            foreach (CanvasGroup card in cards)
            {
                card.blocksRaycasts = true;
            }
            CardPlace.transform.position = originCardPlace;
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
            isPlayerTurn = false;
            return;
        }
        playerScript.ViewConditionIcon(); //プレイヤーの状態異常アイコンの更新
        enemyScript.ViewConditionIcon(); //エネミーの状態異常アイコンの更新

        if (isAccelerate)
        {
            cardCostChange.CardCostDown(accelerateValue);
            isAccelerate = false;
            isDecelerate = true;
            Debug.Log("アクセラレーション処理");
        }

        int playerCurrentAP = playerScript.GetSetCurrentAP;
        int enemyCurrentAP = enemyScript.GetSetCurrentAP;
        
        if (playerCurrentAP >= 0 || enemyCurrentAP > 0) //どちらかのAPが残っている場合
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
                turnEndBlackPanel.SetActive(false); //TurnEndButtonの暗転を解除
                isPlayerMove = false;
                playerTurnDisplay.enabled = true;
                enemyTurnDisplay.enabled = false;
                Debug.Log("プレイヤーの行動可否 = " + CheckPlayerCanMove());
                if (!CheckPlayerCanMove())
                {
                    WaitTurnEndCompletion();
                }
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
                turnEndBlackPanel.SetActive(true); //TurnEndButtonの色を暗くする
                playerTurnDisplay.enabled = false;
                enemyTurnDisplay.enabled = true;
                if(roundCount == 1)
                {
                    Invoke("EnemyMove", 0.1f);
                }
                else
                {
                    EnemyMove();
                }
                
            }
        }
        else //どちらも行動できない場合
        {
            WaitTurnEndCompletion();
        }
    }

    /// <summary>
    /// プレイヤーにターンが回って来た時に行動できるかチェックする
    /// </summary>
    /// <returns>行動できるならtrueを行動できないのであればfalseを返す</returns>
    bool CheckPlayerCanMove()
    {
        // 前のターンでターン終了をしていた場合、このターンは行動出来ない
        if(isTurnEnd) 
            return false;

        //本来ならばCardPlaceからデッキ情報を取得したいが、カードのParentを外す関係上取得できないときがあるのでParentの動くことのないPickCardPlaceから取得する
        CardController[] cards = PickCardPlace.GetComponentsInChildren<CardController>();
        foreach (var card in cards)
        {
            int cardCost = card.cardDataManager._cardCost;
            int cardState = card.cardDataManager._cardState;
            if (cardCost <= playerScript.GetSetCurrentAP && cardState == 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// ターンエンドの実行及び実行の待機を行う
    /// </summary>
    void WaitTurnEndCompletion()
    {
        //ターンディスプレイはどちらもオフに
        playerTurnDisplay.enabled = false;
        enemyTurnDisplay.enabled = false;
        isPlayerTurn = false;
        turnEndBlackPanel.SetActive(true); //TurnEndButtonの色を暗くする

        if (isTurnEnd) //プレイヤーが行動終了ボタンを押していたら
        {
            StartCoroutine(WaitEndRound()); //ターンを終了する
        }
        else
        {
            turnEndBlackPanel.SetActive(false); //TurnEndButtonの暗転を解除
            buttonTween = turnEndButtonGroup.DOFade(0.0f, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            isPlayerMove = false; //プレイヤーに行動終了のタイミングを委ねる
        }
    }

    IEnumerator WaitEndRound()
    {
        yield return new WaitForSeconds(roundTime);
        EndRound();
        yield break;
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
        isPlayerTurn = false;
        //攻撃エフェクトを発動
        var cardType = card.cardDataManager._cardType;
        if(cardType == "Attack")
        {
            battleEffect.Attack(DropPlace);
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
            if(buttonTween != null)
            {
                buttonTween.Kill();
                turnEndButtonGroup.alpha = 1;
            }
            turnEndBlackPanel.SetActive(true); //TurnEndButtonの色を暗くする

            TurnCalc(); //ターン処理に移る
        }
    }

    /// <summary>
    /// ラウンド終了時の効果処理
    /// </summary>
    private void EndRound() 
    {
        playerScript.Poison(playerMoveCount);
        enemyScript.Poison(enemyMoveCount);
        playerMoveCount = 0; //プレイヤーの行動回数をリセットする
        enemyMoveCount = 0; //エネミーの行動回数をリセットする
        playerScript.ChargeAP();
        enemyScript.ChargeAP();
        EndRoundRelicEffect();
        if (!isFirstCall) { isFirstCall = true; OnceEndRoundRelicEffect(); }
        isTurnEnd = false;//行動終了ボタンの復活
        turnEndBlackPanel.SetActive(false); //TurnEndButtonの暗転を解除
        StateReset();
        StartRound();
    }

    void StateReset()
    {
        foreach(Transform child in CardPlace)
        {
            if (child.GetComponent<CardController>().cardDataManager._cardState == 1)
            {
                child.GetComponent<CardController>().cardDataManager._cardState = 0;
            }
            
        }
        foreach (Transform child in PickCardPlace)
        {
            if (child.GetComponent<CardController>().cardDataManager._cardState == 1)
            {
                child.GetComponent<CardController>().cardDataManager._cardState = 0;
            }

        }
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
        uiManagerBR.GetComponent<UIManagerBattleReward>().isDisplayRelics = true; //報酬としてレリックを選択できるようにする
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
        enemyScript.ViewEnemyRelic(gm);
        uiManagerBattle.GetComponent<UIManagerBattle>().UIEventsReload();
    }

    /// <summary>
    /// プレイヤーのデッキを生成する処理
    /// </summary>
    private void InitDeck() 
    {
        deckNumberList = playerData._deckList;
        int deckCount = deckNumberList.Count;
        ChangeSpace(deckCount);
        for (int init = 0; init < deckCount; init++)// デッキの枚数分
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

    void ChangeSpace(int deckCount)
    {
        if(deckCount>7 && deckCount == 9)
        {
            SetDeckSpace(60, 0);
        }
        else if(deckCount > 7 && deckCount == 10)
        {
            SetDeckSpace(40, 0);
        }
        else if (deckCount > 7 && deckCount == 11)
        {
            SetDeckSpace(20, 0);
        }
        else
        {
            SetDeckSpace(80, 0);
        }
    }

    /// <summary>
    /// デッキの枚数に応じて配置するスペースを設定する
    /// </summary>
    /// <param name="horizontalSpace"></param>
    /// <param name="verticalSpace"></param>
    void SetDeckSpace(int horizontalSpace,int verticalSpace)
    {
        var space = new Vector2(horizontalSpace, verticalSpace);
        var cardPlaceGrid = CardPlace.GetComponent<GridLayoutGroup>();
        var pickCardPlaceGrid = PickCardPlace.GetComponent<GridLayoutGroup>();
        cardPlaceGrid.spacing = space;
        pickCardPlaceGrid.spacing = space;
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
    /// 戦闘終了後に戦闘で変化したプレイヤーデータをGameManagerに返す
    /// </summary>
    void ReturnPlayerData()
    {
        if(playerScript.GetSetCurrentHP > gm.playerData._playerHP) //戦闘終了時の体力が参照元の最大体力を上回っていた場合
        {
            playerScript.GetSetCurrentHP = gm.playerData._playerHP; //最大体力を超えないようにする
        }
        gm.playerData._playerCurrentHP = playerScript.GetSetCurrentHP; //戦闘終了時の体力を返す
        gm.playerData._playerMoney += enemyScript.GetSetDropMoney; //コインを獲得
        gm.playerData._deckList = deckNumberList; //魔女の霊薬を所持していれば使用後に削除して返す
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