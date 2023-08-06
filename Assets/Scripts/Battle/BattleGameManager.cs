using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleGameManager : MonoBehaviour
{
    PlayerBattleAction playerScript;
    EnemyBattleAction enemyScript;
    //�v���C���[
    GameObject player;
    PlayerDataManager playerData;
    [SerializeField] 
    Image playerTurnDisplay;
    [SerializeField]
    ConditionDisplay playerConditionDisplay;

    //�G�l�~�[
    EnemyDataManager enemyData;
    [SerializeField] 
    Image enemyTurnDisplay;
    [SerializeField]
    ConditionDisplay enemyConditionDisplay;
    SelectEnemyData selectEnemyData;
    SelectEnemyRelic selectEnemyRelic;

    //�J�[�h
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    [SerializeField] Transform PickCardPlace;
    List<int> deckNumberList;//�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    public bool isPlayerTurn;//�v���C���[�̃^�[��������//CardEffect()�Ŏg�p
    private bool isPlayerMove;//�v���C���[���s����������//TurnEnd()�Ŏg�p
    private bool isTurnEnd;//�s���I���{�^����������������//TurnEnd()�Ŏg�p
    public bool isAccelerate;//�J�[�h���A�N�Z�����[�g�����g�p����������//TurnCalc()�Ŏg�p
    public int accelerateValue;//�J�[�h���A�N�Z�����[�g���ŉ�������R�X�g�̒l
    private bool isDecelerate;//�J�[�h���A�N�Z�����[�g���̌��ʂ𖳌�������������//TurnCalc()�Ŏg�p
    private bool isFirstCall;//�ŏ��̃��E���h�̂Ƃ��ɌĂԔ���//EndRound()�Ŏg�p
    public bool isCoroutine;//�R���[�`�������쒆������//PlayerMove(),EnemyMove()�Ŏg�p
    public float turnTime = 1.0f;//�^�[���̐؂�ւ�����
    public float roundTime = 2.0f;//���E���h�̐؂�ւ�����
    private int playerMoveCount;//�v���C���[�����E���h���ɍs�������l//Curse�̏����Ŏg�p
    private int enemyMoveCount;//�G�l�~�[�����E���h���ɍs�������l//Curse�̏����Ŏg�p
    private int accelerateCount;//���E���h���Ƀv���C���[������A�N�Z�����[�g���g�p�������L�^����
    public int roundCount;//�����E���h�ڂ����L�^����

    public string enemyName = "Slime";
    [SerializeField]
    int floor = 1;

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
        //enemyName = GameManager.instance.EnemyName;
        playerScript = GetComponent<PlayerBattleAction>();
        enemyScript = GetComponent<EnemyBattleAction>();
        selectEnemyData = GetComponent<SelectEnemyData>();
        selectEnemyRelic = GetComponent<SelectEnemyRelic>();
        //������
        isPlayerTurn = false;
        isTurnEnd = false;
        isAccelerate = false;
        isDecelerate = false;
        isFirstCall = false;
        isCoroutine = false;
        playerMoveCount = 0;
        enemyMoveCount = 0;
        accelerateCount = 0;
        accelerateValue = 0;
        roundCount = 0;
        player = GameObject.Find("TestPlayer");
        ReadPlayer(player);
        ReadEnemy(enemyName);
        SetStatus(playerData, enemyData);
        StartRelicEffect();
        InitDeck();
        StartRound();
    }
    private void StartRound() //���E���h�J�n���̌��ʏ���
    {
        roundCount++;//���E���h�������Z����
        if (isDecelerate) //�A�N�Z�����[�g�̌��ʂ�������
        {
            UndoCardCost();
            isDecelerate = false;
            accelerateCount = 0;
        }
        //ChageAP��Curse����������AP�����߂�
        playerScript.SetUpAP();
        playerScript.SaveRoundAP();
        enemyScript.SetUpAP();
        enemyScript.GetSetRoundEnabled = false;
        enemyScript.SaveRoundAP();
        TurnCalc();
    }
    public void TurnCalc() //�s���������߂�
    {
        if (IsGameEnd()) //�퓬�̏I�������𖞂����Ă��Ȃ����m�F����
        {
            //�ǂ��炩����ɐ퓬�s�\�ɂȂ����ꍇ�A�퓬���~�߂�
            return;
        }

        playerConditionDisplay.ViewIcon(playerScript.GetSetCondition); //�v���C���[�̏�Ԉُ�A�C�R���̍X�V
        enemyConditionDisplay.ViewIcon(enemyScript.GetSetCondition); //�G�l�~�[�̏�Ԉُ�A�C�R���̍X�V

        if (isAccelerate)
        {
            CardCostDown();
            accelerateCount++;
            isAccelerate = false;
            isDecelerate = true;
        }
        int playerCurrentAP = playerScript.GetSetCurrentAP;
        int enemyCurrentAP = enemyScript.GetSetCurrentAP;
        if (playerCurrentAP > 0 || enemyCurrentAP > 0) //�ǂ��炩��AP���c���Ă���ꍇ
        {
            if (playerCurrentAP >= enemyCurrentAP) //AP���r���đ��������s������
            {
                if (playerScript.IsCurse()) //�����ɂȂ��Ă����� 
                {
                    //Curse�̏����Ō�����AP�̍X�V
                    playerScript.SetUpAP();
                }
                //�v���C���[�̍s��
                isPlayerTurn = true;
                isPlayerMove = false;
                playerTurnDisplay.enabled = true;
                enemyTurnDisplay.enabled = false;
            }
            else
            {
                if (enemyScript.IsCurse()) //�����ɂȂ��Ă����� 
                {
                    //Curse�̏����Ō�����AP�̍X�V
                    enemyScript.SetUpAP();
                }
                //�G�l�~�[�̍s��
                isPlayerTurn = false;
                playerTurnDisplay.enabled = false;
                enemyTurnDisplay.enabled = true;
                Invoke("EnemyMove", 1.0f);
            }
        }
        else //�ǂ�����s���ł��Ȃ��ꍇ
        {
            //���E���h���I������
            playerTurnDisplay.enabled = false;
            enemyTurnDisplay.enabled = false;
            Invoke("EndRound", roundTime);
        }
    }
    public void PlayerMove(CardController card) //�v���C���[�̌��ʏ���
    {
        if (isCoroutine) //�R���[�`���������Ă���Ƃ��ɉ���Ă�����PlayerMove�͓������Ȃ�
            return;
        if (playerScript.GetSetCurrentAP < card.cardDataManager._cardCost) //�J�[�h�̃R�X�g���v���C���[��AP�𒴂����牽�����Ȃ�
        {
            return;
        }
        isPlayerMove = true;//�v���C���[�͍s����
        playerScript.Move(card);
        playerMoveCount++;
        playerScript.AutoHealing();
        playerScript.Impatience();
        playerScript.Burn();
        TurnCalc();
    }
    private void EnemyMove() //�G�l�~�[�̌��ʏ���
    {
        if (isCoroutine) //�R���[�`���������Ă���Ƃ��ɉ���Ă�����EnemyMove�͓������Ȃ�
            return;
        enemyScript.Move();
        enemyMoveCount++;
        enemyScript.AutoHealing();
        enemyScript.Impatience();
        enemyScript.Burn();
        //playerScript.AddConditionStatus("Burn", 1);
        playerScript.AddConditionStatus("InvalidBadStatus", 1);
        Invoke("TurnCalc", turnTime);
    }
    private void EndRound() //���E���h�I�����̌��ʏ���
    {
        playerScript.Poison(playerMoveCount);
        enemyScript.Poison(enemyMoveCount);
        playerScript.ChargeAP();
        enemyScript.ChargeAP();
        if (!isFirstCall) { isFirstCall = true; OnceEndRoundRelicEffect(); }
        EndRoundRelicEffect();
        isTurnEnd = false;//�s���I���{�^���̕���
        StartRound();
    }
    private bool IsGameEnd()
    {
        if (playerScript.CheckHP()) //�v���C���[��HP���Ȃ��Ȃ�����
        {
            //�G�l�~�[�̏������o
            Debug.Log("Enemy�̏���");
            return true;
        }
        if (enemyScript.CheckHP()) //�G�l�~�[��HP���Ȃ��Ȃ�����
        {
            EndGameRelicEffect();
            //�v���C���[�̏������o
            Debug.Log("Player�̏���");
            return true;
        }
        return false;
    }
    //�����܂ł��Q�[�����[�v
    //�ȉ��͊֐�(���\�b�h)
    private void ReadPlayer(GameObject player) //�v���C���[�̃f�[�^��ǂݎ��
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
    private void ReadEnemy(string enemyName) //�G�l�~�[�̃f�[�^��ǂݎ��
    {
        enemyData = selectEnemyData.SetEnemyDataManager(floor, enemyName);
    }
    private void SetStatus(PlayerDataManager player, EnemyDataManager enemy)
    {
        //�v���C���[�̃X�e�[�^�X������U��
        playerScript.SetStatus(player);
        deckNumberList = player._deckList;
        //�G�l�~�[�̃X�e�[�^�X������U��
        enemyScript.SetStatus(floor, enemy);
        enemyScript.hasEnemyRelics = selectEnemyRelic.SetEnemyRelics(enemyScript.hasEnemyRelics, floor, enemyName);
    }
    private void InitDeck() //�f�b�L����
    {
        deckNumberList = playerData._deckList;
        for (int init = 0; init < deckNumberList.Count; init++)// �f�b�L�̖�����
        {
            CardController card = Instantiate(cardPrefab, CardPlace);//�J�[�h�𐶐�����
            card.name = "Deck" + init.ToString();//���������J�[�h�ɖ��O��t����
            card.Init(deckNumberList[init]);//�f�b�L�f�[�^�̕\��
            CardController pickCard = Instantiate(cardPrefab, PickCardPlace); //�s�b�N���ꂽ�Ƃ��p�̃J�[�h���������Ă���
            pickCard.gameObject.GetComponent<PickCard>().enabled = true;
            pickCard.name = "Pick" + init.ToString();
            pickCard.Init(deckNumberList[init]);
            pickCard.transform.localScale *= 1.5f; //�傫����1.5�{�ɂ���
            pickCard.transform.Find("CardInfo").gameObject.SetActive(false); //��\���ɂ��Ă���
            pickCard.GetComponent<CanvasGroup>().blocksRaycasts = false; //���C�őI�΂�Ȃ��悤�ɂ��Ă���
        }
    }
    public void StartRelicEffect() //�퓬�J�n���ɔ������郌���b�N����
    {
        enemyScript = playerScript.StartRelicEffect(enemyScript, enemyName);
        playerScript = enemyScript.StartRelicEffect(playerScript);
    }
    public void OnceEndRoundRelicEffect() //���E���h�I�����Ɉ�x�����������郌���b�N����
    {
        playerScript.OnceEndRoundRelicEffect();
        enemyScript.OnceEndRoundRelicEffect();
    }
    public void EndRoundRelicEffect() //���E���h�I�����ɔ������郌���b�N����
    {
        playerScript.EndRoundRelicEffect();
        enemyScript.EndRoundRelicEffect();
    }
    public void EndGameRelicEffect() //�퓬�I�����ɔ������郌���b�N����
    {
        int money = 10;
        money = playerScript.EndGameRelicEffect();
    }
    public void TurnEnd() //�s���I���{�^�������������� 
    {
        if (!isTurnEnd && !isPlayerMove) //�܂��{�^����������Ă��Ȃ������牟�����Ƃ��o����
        {
            isTurnEnd = true;
            playerScript.TurnEnd();
            TurnCalc();
        }
    }
    private void CardCostDown() //CardEffectList�̃A�N�Z�����[�g���������̏���
    {
        Transform deck = GameObject.Find("CardPlace").transform; //�S�Ẵf�b�L��T��
        foreach (Transform child in deck)
        {
            CardController deckCard = child.GetComponent<CardController>();
            if (deckCard.cardDataManager._cardType == "Attack") //�J�[�h�̃^�C�v��Attack�Ȃ�
            {
                //�J�[�h�̃R�X�g��������
                deckCard.cardDataManager._cardCost -= accelerateValue;
                if (deckCard.cardDataManager._cardCost < 1)
                {
                    deckCard.cardDataManager._cardCost = 1;
                }
            }
            TextMeshProUGUI costText = child.transform.Find("CardInfo/CardCost").GetComponentInChildren<TextMeshProUGUI>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }
    private void UndoCardCost() //�A�N�Z�����[�g�̌��ʂ𖳌�
    {
        Transform deck = GameObject.Find("CardPlace").transform;
        foreach (Transform child in deck)
        {
            CardController deckCard = child.GetComponent<CardController>();
            if (deckCard.cardDataManager._cardType == "Attack")
            {
                deckCard.cardDataManager._cardCost += accelerateValue * accelerateCount;
            }
            TextMeshProUGUI costText = child.transform.Find("CardInfo/CardCost").GetComponentInChildren<TextMeshProUGUI>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }
}