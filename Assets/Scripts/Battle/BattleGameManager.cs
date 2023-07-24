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
    [SerializeField] Image playerTurnDisplay;
    ConditionDisplay playerConditionDisplay;

    //�G�l�~�[
    EnemyDataManager enemyData;
    [SerializeField] Image enemyTurnDisplay;
    ConditionDisplay enemyConditionDisplay;

    //�J�[�h
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    [SerializeField] Transform PickCardPlace;
    List<int> deckNumberList;//�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    //�����b�N
    RelicStatus playerRelics;
    RelicEffectList relicEffect;

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
    private int playerMoveCount;//�v���C���[�����E���h���ɍs�������l
    private int enemyMoveCount;//�G�l�~�[�����E���h���ɍs�������l
    private int accelerateCount;//���E���h���Ƀv���C���[������A�N�Z�����[�g���g�p�������L�^����
    public int roundCount;//�����E���h�ڂ����L�^����

    //Debug�p
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
        //enemyName = GameManager.instance.EnemyName;
        playerScript = GetComponent<PlayerBattleAction>();
        enemyScript = GetComponent<EnemyBattleAction>();
        relicEffect = GetComponent<RelicEffectList>();
        playerConditionDisplay = GameObject.Find("PlayerConditionPlace").GetComponent<ConditionDisplay>();
        enemyConditionDisplay = GameObject.Find("EnemyConditionPlace").GetComponent<ConditionDisplay>();
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
        IsGameEnd();//�퓬�̏I�������𖞂����Ă��Ȃ����m�F����
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
    private void IsGameEnd()
    {
        if (playerScript.CheckHP()) //�v���C���[��HP���Ȃ��Ȃ�����
        {
            //�G�l�~�[�̏������o
            Debug.Log("Enemy�̏���");
        }
        if (enemyScript.CheckHP()) //�G�l�~�[��HP���Ȃ��Ȃ�����
        {
            EndGameRelicEffect();
            //�v���C���[�̏������o
            Debug.Log("Player�̏���");
        }
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
        //�v���C���[�̃X�e�[�^�X������U��
        playerScript.SetStatus(player);
        deckNumberList = player._deckList;
        playerRelics = new RelicStatus();
        SetRelics(playerRelics);
        //�G�l�~�[�̃X�e�[�^�X������U��
        enemyScript.SetStatus(enemy);
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
    private void SetRelics(RelicStatus playerRelics)
    {
        //Debug�p�ɐݒ肵�����́AGameManager���烌���b�N�̃��X�g���󂯎���悤�ɂȂ����璼�ڌĂяo����悤�ɂ���
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
    public void StartRelicEffect() //�퓬�J�n���ɔ������郌���b�N����
    {
        var ps = playerScript;
        var es = enemyScript;
        var pr = playerRelics;
        ps.GetSetCondition.upStrength = relicEffect.RelicID2(pr.hasRelicID2, ps.GetSetCondition.upStrength, es.GetSetCondition.upStrength).playerUpStrength;
        es.GetSetCondition.upStrength = relicEffect.RelicID2(pr.hasRelicID2, ps.GetSetCondition.upStrength, es.GetSetCondition.upStrength).enemyUpStrength;
        ps.GetSetConstAP = relicEffect.RelicID3(pr.hasRelicID3, ps.GetSetConstAP, ps.GetSetChargeAP).playerConstAP;
        ps.GetSetConstAP = relicEffect.RelicID4(pr.hasRelicID4, ps.GetSetConstAP);
        ps.GetSetConstAP = relicEffect.RelicID5(pr.hasRelicID5, ps.GetSetConstAP, ps.GetSetChargeAP).playerConstAP;
        ps.GetSetCondition.burn = relicEffect.RelicID6(pr.hasRelicID6, ps.GetSetCondition.burn);
        ps.GetSetHP = relicEffect.RelicID7(pr.hasRelicID7, ps.GetSetHP);
        ps.GetSetGP = relicEffect.RelicID8(pr.hasRelicID8, ps.GetSetGP);
        ps.GetSetCondition.upStrength = relicEffect.RelicID12(pr.hasRelicID12, "Slime", ps.GetSetCondition.upStrength);
        Debug.Log("�X�^�[�g���̃����b�N���Ăяo����܂���: " + ps.GetSetConstAP + " to " + ps.GetSetChargeAP);
    }
    public void OnceEndRoundRelicEffect() //���E���h�I�����Ɉ�x�����������郌���b�N����
    {
        var ps = playerScript;
        var pr = playerRelics;
        ps.GetSetChargeAP = relicEffect.RelicID3(pr.hasRelicID3, ps.GetSetConstAP, ps.GetSetChargeAP).playerChargeAP;
        ps.GetSetChargeAP = relicEffect.RelicID5(pr.hasRelicID5, ps.GetSetAP, ps.GetSetChargeAP).playerChargeAP;
    }
    public void EndRoundRelicEffect() //���E���h�I�����ɔ������郌���b�N����
    {
        var ps = playerScript;
        var pr = playerRelics;
        var pc = ps.GetSetCondition;
        (pc.curse, pc.impatience, pc.weakness, pc.burn, pc.poison) = relicEffect.RelicID11(pr.hasRelicID11, pc.curse, pc.impatience, pc.weakness, pc.burn, pc.poison);
    }
    public void EndGameRelicEffect() //�퓬�I�����ɔ������郌���b�N����
    {
        var ps = playerScript;
        var pr = playerRelics;
        int money = 10;
        money = relicEffect.RelicID9(playerRelics.hasRelicID9, money);
        ps.GetSetCurrentHP = relicEffect.RelicID10(pr.hasRelicID10, ps.GetSetCurrentHP);
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
            Text costText = child.transform.Find("CardInfo/Cost").GetComponentInChildren<Text>();
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
            Text costText = child.transform.Find("CardInfo/Cost").GetComponentInChildren<Text>();
            costText.text = deckCard.cardDataManager._cardCost.ToString();
        }
    }
}