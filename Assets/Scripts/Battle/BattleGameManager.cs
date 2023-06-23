using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleGameManager : MonoBehaviour
{
    //プレイヤー
    GameObject player;
    public PlayerDataManager playerData;
    [SerializeField] public Text playerHPText;
    [SerializeField] public Text playerAPText;
    [SerializeField] public Text playerGPText;
    [SerializeField] Text playerNameText;
    [SerializeField] Image playerTurnDisplay;

    //エネミー
    EnemyDataManager enemyData;
    [SerializeField] public Text enemyHPText;
    [SerializeField] Text enemyNameText;
    [SerializeField] public Slider enemyHPSlider;
    [SerializeField] Image enemyImage;
    [SerializeField] Image enemyTurnDisplay;

    //カード
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    CardEffectList cardEffectList;
    List<int> deckNumberList;//プレイヤーのもつデッキナンバーのリスト

    //プレイヤーの変数
    public int playerHP;
    public int playerCurrentHP;
    int playerLastHP;
    public int playerAP;
    public int playerCurrentAP;
    int playerChargeAP;
    public int playerGP;
    int playerLastGP;
    public ConditionStatus playerCondition;
    public BattleRelicStatus playerRelics;
    //エネミーの変数
    public int enemyHP;
    public int enemyCurrentHP;
    public int enemyAP;
    public int enemyCurrentAP;
    int enemyChargeAP;
    public int enemyGP;
    public ConditionStatus enemyCondition;

    //その他の変数
    public int roundCount;
    public float ChangeTurnTime = 2.0f;//ターンが切り替わる速度
    bool isPlayerTurn;//プレイヤーのターンか判定//CardEffect()で使用
    bool isPlayerMove;//プレイヤーが行動したか判定//ChangeTurn()で使用
    bool isChangeTurn;//行動終了ボタンを押したか判定//CardEffect(),ChangeTurn()で使用
    bool isPlayerHealing;//プレイヤーが回復行動をとったか判定//TurnCalc()で使用
    bool isPlayerAddGP;//プレイヤーがGPを増やしたか判定//TurnCalc()で使用
    bool isAutoHealing;//状態異常の自動回復が発動したか判定//CardEffect()で使用
    bool isImpatience;//状態異常の焦燥が発動したか判定//CardEffect()で使用
    bool isBurn;//状態異常の火傷が発動したか判定//CardEffect()で使用
    public bool isAccelerate;//カード＜アクセラレート＞を使用したか判定//TurnCalc()で使用
    bool isDecelerate;//カード＜アクセラレート＞の効果を無効かしたか判定//TurnCalc()で使用
    public int reduceCost;//コストの減少を軽減する値
    int playerMoveCount;//ラウンド中にプレイヤーが何回行動したか記録する
    int AccelerateCount;//ラウンド中にプレイヤーが何回アクセラレートを使用したか記録する

    //Debug用変数
    int p = 1;
    int e = 1;
    public int enemyAttackPower = 1;

    public static BattleGameManager Instance;
    private void Awake()
    {
        if(Instance == null) 
        {
            Instance = this;
        }
    }
    private void Start()
    {
        player = GameObject.Find("TestPlayer");
        ReadPlayer(player);
        playerCondition = new ConditionStatus();
        playerRelics = new BattleRelicStatus();
        enemyData = new EnemyDataManager("Slime");
        enemyCondition = new ConditionStatus();
        isChangeTurn = false;
        isPlayerMove = false;
        cardEffectList = GetComponent<CardEffectList>();
        reduceCost = 0;
        playerMoveCount = 0;
        AccelerateCount = 0;
        isAutoHealing = false;
        isImpatience = false;
        isBurn = false;
        isAccelerate = false;
        isDecelerate = false;
        roundCount = 0;
        StartGame();
    }
    
    private void Update()
    {
    }
    private void StartGame() 
    {
        //プレイヤーのステータスを取得、表示
        playerNameText.text = "現在のキャラ:"+ playerData._playerName;
        playerHP = playerData._playerHP;
        playerCurrentHP = playerHP;
        playerHPText.text = playerCurrentHP + "/" + playerHP;
        playerAP = playerData._playerAP;
        playerCurrentAP = playerAP;
        playerAPText.text = playerCurrentAP + "/" + playerAP;
        playerGP = 0;
        playerGPText.text = playerGP.ToString();
        playerChargeAP = 0;
        //エネミーのステータスを取得、表示
        enemyNameText.text = enemyData._enemyName;
        enemyImage.sprite = enemyData._enemyImage;
        enemyHP = enemyData._enemyHP;
        enemyCurrentHP = enemyHP;
        enemyHPSlider.value = 1;
        enemyHPText.text = enemyCurrentHP + "/" + enemyHP;
        enemyAP = enemyData._enemyAP;
        enemyCurrentAP = enemyAP;
        enemyGP = 0;
        enemyChargeAP = 0;

        //プレイヤーのデッキを作成
        deckNumberList = playerData._deckList;
        for (int init = 0; init < deckNumberList.Count; init++)// デッキの枚数分
        {
            CardController card = Instantiate(cardPrefab, CardPlace);//カードを生成する
            card.name = "Deck" + init.ToString();//生成したカードに名前を付ける
            card.Init(deckNumberList[init]);//デッキデータの表示
        }
        //エネミーのデッキを作成

        PreparateCalc();
    }
    private void SetPlayerRelics()
    {

    }
    private void PreparateCalc() //ラウンド開始時の効果処理
    {
        roundCount++;//ラウンド数を数える
        CardState();//カードの使用状況を更新
        playerAP = playerData._playerAP + playerChargeAP - playerCondition.curse;
        enemyAP = enemyData._enemyAP + enemyChargeAP;
        playerCurrentAP = playerAP;
        enemyCurrentAP = enemyAP;
        playerAPText.text = playerCurrentAP + "/" + playerAP;
        if (isDecelerate) 
        {
            UndoCardCost();
            isDecelerate = false;
        }
        TurnCalc();
    }
    private void CardState() 
    {
        Transform deck = GameObject.Find("CardPlace").transform;
        foreach (Transform child in deck)
        {
            CardController deckCard = child.GetComponent<CardController>();
            if (deckCard.cardDataManager._cardState == 1)
            {
                deckCard.cardDataManager._cardState = 0;
            }
        }
    }
    private void UndoCardCost() 
    {
        Transform deck = GameObject.Find("CardPlace").transform;
        foreach (Transform child in deck)
        {
            CardController deckCard = child.GetComponent<CardController>();
            if (deckCard.cardDataManager._cardType == "Attack")
            {
                deckCard.cardDataManager._cardCost += AccelerateCount;
            }
            Text costText = child.transform.GetChild(3).GetComponentInChildren<Text>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }
    private void TurnCalc() //行動するキャラクターを決める
    {
        if (isAccelerate) 
        {
            CardCostDown();
            AccelerateCount++;
            isAccelerate = false;
            isDecelerate = true;
        }
        Curse();
        if (playerCurrentAP>0 || enemyCurrentAP>0)
        {
            if (playerCurrentAP >= enemyCurrentAP)//プレイヤーのAPがエネミーより多い時
            {
                //プレイヤーのターン
                isPlayerTurn = true;
                isPlayerMove = false;
                isPlayerHealing = false;
                isPlayerAddGP = false;
                playerLastHP = playerCurrentHP;
                playerLastGP = playerGP;
                playerTurnDisplay.enabled = true;
                enemyTurnDisplay.enabled = false;
                Debug.Log("プレイヤーのターン" + p);
            }
            else
            {
                isPlayerTurn = false;
                if (isPlayerAddGP == false && playerLastGP< playerGP || isPlayerHealing == false && playerLastHP < playerCurrentHP)//プレイヤーの体力かＧＰが増えていた場合
                {
                    StartCoroutine(WaitEnemyMove());//エネミーは時間を空けて行動する
                    isPlayerHealing = true;
                    isPlayerAddGP = true;
                }
                else 
                {
                    EnemyTurn();//エネミーのターン
                }
                playerTurnDisplay.enabled = false;
                enemyTurnDisplay.enabled = true;
            }
            //行動できるキャラクターがいなければラウンドを終了する
        }
        else
        {
            playerTurnDisplay.enabled = false;
            enemyTurnDisplay.enabled = false;
            if (playerCondition.poison > 0) 
            {
                StartCoroutine(WaitPlayerPoison());
            }
            Invoke("ChargeAP",ChangeTurnTime);
        }
    }
    private void CardCostDown() 
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
            Text costText = child.transform.GetChild(3).GetComponentInChildren<Text>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }
    private void Curse() 
    {
        playerAP = playerData._playerAP + playerChargeAP - playerCondition.curse;
        if (playerCurrentAP > playerAP) 
        { 
            playerCurrentAP = playerAP;
        }
        playerAPText.text = playerCurrentAP + "/" + playerAP;
    }
    private void PlayerPoison()
    {
        playerCurrentHP -= playerMoveCount * playerCondition.poison;
        playerHPText.text = playerCurrentHP + "/" + playerHP;
        playerMoveCount = 0;
    }
    IEnumerator WaitPlayerPoison() 
    {
        yield return new WaitForSeconds(1.0f);
        PlayerPoison();
    }
    IEnumerator WaitEnemyMove() //エネミーの待機時間
    {
        yield return new WaitForSeconds(1.0f);
        EnemyTurn();
    }
    public void ChangeTurn() //プレイヤーが行動終了ボタンを押した処理
    {
        if (!isChangeTurn && !isPlayerMove)
        {
            //プレイヤーをこのターンの間行動不能にする
            playerCurrentAP = 0;
            playerAPText.text = playerCurrentAP + "/" + playerAP;
            isChangeTurn = true;
            p++;
            TurnCalc();//エネミーにターンを渡す
        }
        
    }
    public void PlayerTurn(CardController card) //プレイヤーの行動処理
    {
        CardEffect(card);
    }
    private void CardEffect(CardController card) //プレイヤーのカード効果を処理
    {
        if (isPlayerTurn && !isChangeTurn)
        {
            if (playerCurrentAP < card.cardDataManager._cardCost)
            { 
                return;
            }
            isPlayerMove = true;
            PlayerMove(card);
            CoditionAfterTurn();
            DisplayCardEffect();
            if (isImpatience||isAutoHealing||isBurn)
            {
                StartCoroutine(WaitTurnCalc());
            }
            else 
            {
                TurnCalc();
            }
            p++;
        }
    }
    private void PlayerMove(CardController card) 
    {
        //出したカードのコストを支払い、効果を発動する
        int currentCardCost = card.cardDataManager._cardCost - reduceCost;
        playerCurrentAP -= currentCardCost;
        Debug.Log("プレイヤーの残りAPは" + playerAP);
        playerAPText.text = playerCurrentAP + "/" + playerAP;
        cardEffectList.ActiveCardEffect(card);
        playerMoveCount++;
    }
    private void CoditionAfterTurn() 
    {
        //行動後の状態異常を反映する
        if (playerCondition.autoHealing >0) 
        {
            isAutoHealing = true;
            StartCoroutine(WaitPlayerAutoHealing());
        }
        if (playerCondition.burn > 0)
        {
            isBurn = true;
            StartCoroutine(WaitPlayerBurn());
        }
        if (playerCondition.impatience > 0)
        {
            isImpatience = true;
            StartCoroutine(WaitPlayerImpatience());
        }
    }
    IEnumerator WaitPlayerAutoHealing()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerAutoHealing();
    }
    IEnumerator WaitPlayerImpatience()
    {
        yield return new WaitForSeconds(1.0f);
        PlayerImpatience();
    }
    IEnumerator WaitPlayerBurn()
    {
        yield return new WaitForSeconds(0.5f);
        PlayerBurn();
    }
    private void PlayerAutoHealing() 
    {
        playerCurrentHP += playerCondition.autoHealing;
        if (playerCurrentHP > playerHP) 
        {
            playerCurrentHP = playerHP;
        }
        playerHPText.text = playerCurrentHP + "/" + playerHP;
    }
    private void PlayerImpatience() 
    {
        playerCurrentAP -= playerCondition.impatience;
        if (playerCurrentAP < 0) { playerCurrentAP = 0; }
        playerAPText.text = playerCurrentAP + "/" + playerAP;
    }
    private void PlayerBurn() 
    {
        playerCurrentHP -= playerCondition.burn;
        playerHPText.text = playerCurrentHP + "/" + playerHP;
    }
    private void DisplayCardEffect() 
    {
        if (playerCurrentHP > playerHP)
        {
            playerCurrentHP = playerHP;
        }
        playerHPText.text = playerCurrentHP + "/" + playerHP;
        playerAPText.text = playerCurrentAP + "/" + playerAP;
        playerGPText.text = playerGP.ToString();
        enemyHPText.text = enemyCurrentHP + "/" + enemyHP;
        enemyHPSlider.value = enemyCurrentHP / (float)enemyHP;
    }
    IEnumerator WaitTurnCalc()
    {
        yield return new WaitForSeconds(1.0f);
        TurnCalc();
        isAutoHealing = false;
        isBurn = false;
        isImpatience = false;
    }

    void EnemyTurn() //エネミーの行動処理
    {
        int cost = 1;
        if (enemyCurrentAP < cost) 
        {
            isPlayerMove = false;
            return;
        }
        Debug.Log("エネミーのターン" + e);
        enemyCurrentAP -= 1;
        Debug.Log("エネミーの残りAPは" + enemyCurrentAP);
        int enemyCurrentAttack = enemyAttackPower - playerGP;
        if (playerGP > 0)
        {
            playerGP -= enemyAttackPower;
            if (playerGP <= 0) 
            {
                playerGP = 0;
            }
        }
        if (enemyCurrentAttack <= 0) 
        {
            enemyCurrentAttack = 0;
        }
        playerCurrentHP -= enemyCurrentAttack;
        playerGPText.text = playerGP.ToString();
        playerHPText.text = playerCurrentHP + "/" + playerHP;
        Invoke("TurnCalc", 1.0f);
        e++;
    }
    private void ReadPlayer(GameObject player)
    {
        if (player.CompareTag("Warrior"))
        {
            playerData = new PlayerDataManager("Warrior");
        }
        else if (player.CompareTag("Wizard"))
        {
            playerData = new PlayerDataManager("Wizard");
        }
    }
    private void ReadEnemy(GameObject enemy)//敵のデータを取得する
    {
        if (enemy.CompareTag("Slime"))
        {
            enemyData = new EnemyDataManager("Slime");
        }
    }

    private void ChargeAP() //最大APを増やして回復する
    {
        //最大APを1増やす
        playerChargeAP += 1;
        enemyChargeAP += 1;

        isChangeTurn = false;
        reduceCost = 0;
        //ターンを進める
        PreparateCalc();
    }
}