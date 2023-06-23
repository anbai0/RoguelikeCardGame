using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleGameManager : MonoBehaviour
{
    //�v���C���[
    GameObject player;
    public PlayerDataManager playerData;
    [SerializeField] public Text playerHPText;
    [SerializeField] public Text playerAPText;
    [SerializeField] public Text playerGPText;
    [SerializeField] Text playerNameText;
    [SerializeField] Image playerTurnDisplay;

    //�G�l�~�[
    EnemyDataManager enemyData;
    [SerializeField] public Text enemyHPText;
    [SerializeField] Text enemyNameText;
    [SerializeField] public Slider enemyHPSlider;
    [SerializeField] Image enemyImage;
    [SerializeField] Image enemyTurnDisplay;

    //�J�[�h
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    CardEffectList cardEffectList;
    List<int> deckNumberList;//�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    //�v���C���[�̕ϐ�
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
    //�G�l�~�[�̕ϐ�
    public int enemyHP;
    public int enemyCurrentHP;
    public int enemyAP;
    public int enemyCurrentAP;
    int enemyChargeAP;
    public int enemyGP;
    public ConditionStatus enemyCondition;

    //���̑��̕ϐ�
    public int roundCount;
    public float ChangeTurnTime = 2.0f;//�^�[�����؂�ւ�鑬�x
    bool isPlayerTurn;//�v���C���[�̃^�[��������//CardEffect()�Ŏg�p
    bool isPlayerMove;//�v���C���[���s������������//ChangeTurn()�Ŏg�p
    bool isChangeTurn;//�s���I���{�^����������������//CardEffect(),ChangeTurn()�Ŏg�p
    bool isPlayerHealing;//�v���C���[���񕜍s�����Ƃ���������//TurnCalc()�Ŏg�p
    bool isPlayerAddGP;//�v���C���[��GP�𑝂₵��������//TurnCalc()�Ŏg�p
    bool isAutoHealing;//��Ԉُ�̎����񕜂���������������//CardEffect()�Ŏg�p
    bool isImpatience;//��Ԉُ�̏ő�����������������//CardEffect()�Ŏg�p
    bool isBurn;//��Ԉُ�̉Ώ�����������������//CardEffect()�Ŏg�p
    public bool isAccelerate;//�J�[�h���A�N�Z�����[�g�����g�p����������//TurnCalc()�Ŏg�p
    bool isDecelerate;//�J�[�h���A�N�Z�����[�g���̌��ʂ𖳌�������������//TurnCalc()�Ŏg�p
    public int reduceCost;//�R�X�g�̌������y������l
    int playerMoveCount;//���E���h���Ƀv���C���[������s���������L�^����
    int AccelerateCount;//���E���h���Ƀv���C���[������A�N�Z�����[�g���g�p�������L�^����

    //Debug�p�ϐ�
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
        //�v���C���[�̃X�e�[�^�X���擾�A�\��
        playerNameText.text = "���݂̃L����:"+ playerData._playerName;
        playerHP = playerData._playerHP;
        playerCurrentHP = playerHP;
        playerHPText.text = playerCurrentHP + "/" + playerHP;
        playerAP = playerData._playerAP;
        playerCurrentAP = playerAP;
        playerAPText.text = playerCurrentAP + "/" + playerAP;
        playerGP = 0;
        playerGPText.text = playerGP.ToString();
        playerChargeAP = 0;
        //�G�l�~�[�̃X�e�[�^�X���擾�A�\��
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

        //�v���C���[�̃f�b�L���쐬
        deckNumberList = playerData._deckList;
        for (int init = 0; init < deckNumberList.Count; init++)// �f�b�L�̖�����
        {
            CardController card = Instantiate(cardPrefab, CardPlace);//�J�[�h�𐶐�����
            card.name = "Deck" + init.ToString();//���������J�[�h�ɖ��O��t����
            card.Init(deckNumberList[init]);//�f�b�L�f�[�^�̕\��
        }
        //�G�l�~�[�̃f�b�L���쐬

        PreparateCalc();
    }
    private void SetPlayerRelics()
    {

    }
    private void PreparateCalc() //���E���h�J�n���̌��ʏ���
    {
        roundCount++;//���E���h���𐔂���
        CardState();//�J�[�h�̎g�p�󋵂��X�V
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
    private void TurnCalc() //�s������L�����N�^�[�����߂�
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
            if (playerCurrentAP >= enemyCurrentAP)//�v���C���[��AP���G�l�~�[��葽����
            {
                //�v���C���[�̃^�[��
                isPlayerTurn = true;
                isPlayerMove = false;
                isPlayerHealing = false;
                isPlayerAddGP = false;
                playerLastHP = playerCurrentHP;
                playerLastGP = playerGP;
                playerTurnDisplay.enabled = true;
                enemyTurnDisplay.enabled = false;
                Debug.Log("�v���C���[�̃^�[��" + p);
            }
            else
            {
                isPlayerTurn = false;
                if (isPlayerAddGP == false && playerLastGP< playerGP || isPlayerHealing == false && playerLastHP < playerCurrentHP)//�v���C���[�̗̑͂��f�o�������Ă����ꍇ
                {
                    StartCoroutine(WaitEnemyMove());//�G�l�~�[�͎��Ԃ��󂯂čs������
                    isPlayerHealing = true;
                    isPlayerAddGP = true;
                }
                else 
                {
                    EnemyTurn();//�G�l�~�[�̃^�[��
                }
                playerTurnDisplay.enabled = false;
                enemyTurnDisplay.enabled = true;
            }
            //�s���ł���L�����N�^�[�����Ȃ���΃��E���h���I������
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
    IEnumerator WaitEnemyMove() //�G�l�~�[�̑ҋ@����
    {
        yield return new WaitForSeconds(1.0f);
        EnemyTurn();
    }
    public void ChangeTurn() //�v���C���[���s���I���{�^��������������
    {
        if (!isChangeTurn && !isPlayerMove)
        {
            //�v���C���[�����̃^�[���̊ԍs���s�\�ɂ���
            playerCurrentAP = 0;
            playerAPText.text = playerCurrentAP + "/" + playerAP;
            isChangeTurn = true;
            p++;
            TurnCalc();//�G�l�~�[�Ƀ^�[����n��
        }
        
    }
    public void PlayerTurn(CardController card) //�v���C���[�̍s������
    {
        CardEffect(card);
    }
    private void CardEffect(CardController card) //�v���C���[�̃J�[�h���ʂ�����
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
        //�o�����J�[�h�̃R�X�g���x�����A���ʂ𔭓�����
        int currentCardCost = card.cardDataManager._cardCost - reduceCost;
        playerCurrentAP -= currentCardCost;
        Debug.Log("�v���C���[�̎c��AP��" + playerAP);
        playerAPText.text = playerCurrentAP + "/" + playerAP;
        cardEffectList.ActiveCardEffect(card);
        playerMoveCount++;
    }
    private void CoditionAfterTurn() 
    {
        //�s����̏�Ԉُ�𔽉f����
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

    void EnemyTurn() //�G�l�~�[�̍s������
    {
        int cost = 1;
        if (enemyCurrentAP < cost) 
        {
            isPlayerMove = false;
            return;
        }
        Debug.Log("�G�l�~�[�̃^�[��" + e);
        enemyCurrentAP -= 1;
        Debug.Log("�G�l�~�[�̎c��AP��" + enemyCurrentAP);
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
    private void ReadEnemy(GameObject enemy)//�G�̃f�[�^���擾����
    {
        if (enemy.CompareTag("Slime"))
        {
            enemyData = new EnemyDataManager("Slime");
        }
    }

    private void ChargeAP() //�ő�AP�𑝂₵�ĉ񕜂���
    {
        //�ő�AP��1���₷
        playerChargeAP += 1;
        enemyChargeAP += 1;

        isChangeTurn = false;
        reduceCost = 0;
        //�^�[����i�߂�
        PreparateCalc();
    }
}