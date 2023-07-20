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
    PlayerConditionDisplay conditionDisplay;

    //�G�l�~�[
    EnemyDataManager enemyData;
    [SerializeField] Image enemyTurnDisplay;

    //�J�[�h
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    List<int> deckNumberList;//�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    //�����b�N
    RelicStatus playerRelics;
    RelicEffectList relicEffect;

    public bool isPlayerTurn;//�v���C���[�̃^�[��������//CardEffect()�Ŏg�p
    private bool isPlayerMove;//�v���C���[���s����������//TurnEnd()�Ŏg�p
    private bool isTurnEnd;//�s���I���{�^����������������//TurnEnd()�Ŏg�p
    public bool isAccelerate;//�J�[�h���A�N�Z�����[�g�����g�p����������//TurnCalc()�Ŏg�p
    private bool isDecelerate;//�J�[�h���A�N�Z�����[�g���̌��ʂ𖳌�������������//TurnCalc()�Ŏg�p
    private bool isFirstCall;//�ŏ��̃��E���h�̂Ƃ��ɌĂԔ���//EndRound()�Ŏg�p
    public bool isCoroutine;//�R���[�`�������쒆������//PlayerMove(),EnemyMove()�Ŏg�p
    public float turnTime = 1.0f;//�^�[���̐؂�ւ�����
    public float roundTime = 2.0f;//���E���h�̐؂�ւ�����
    private int playerMoveCount;//�v���C���[�����E���h���ɍs�������l
    private int enemyMoveCount;//�G�l�~�[�����E���h���ɍs�������l
    private int AccelerateCount;//���E���h���Ƀv���C���[������A�N�Z�����[�g���g�p�������L�^����
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
        playerScript = GetComponent<PlayerBattleAction>();
        enemyScript = GetComponent<EnemyBattleAction>();
        relicEffect = GetComponent<RelicEffectList>();
        conditionDisplay = GameObject.Find("ConditionPlace").GetComponent<PlayerConditionDisplay>();
        //������
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
    private void StartRound() //���E���h�J�n���̌��ʏ���
    {
        roundCount++;//���E���h�������Z����
        if (isDecelerate) //�A�N�Z�����[�g�̌��ʂ�������
        {
            UndoCardCost();
            isDecelerate = false;
            AccelerateCount = 0;
        }
        //ChageAP��Curse����������AP�����߂�
        playerScript.SetUpAP();
        playerScript.SaveAP();
        enemyScript.SetUpAP();
        enemyScript.SaveAP();
        TurnCalc();
    }
    public void TurnCalc() //�s���������߂�
    {
        conditionDisplay.ViewIcon(playerScript.GetSetPlayerCondition); //��Ԉُ�̃A�C�R���̍X�V
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
                EnemyMove();
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
        if (playerScript.GetSetPlayerCurrentAP < card.cardDataManager._cardCost) //�J�[�h�̃R�X�g���v���C���[��AP�𒴂����牽�����Ȃ�
        {
            return;
        }
        isPlayerMove = true;//�v���C���[�͍s����
        playerScript.Move(card);
        playerMoveCount++;
        playerScript.PlayerAutoHealing();
        playerScript.PlayerImpatience();
        playerScript.PlayerBurn();
        TurnCalc();
    }
    private void EnemyMove() //�G�l�~�[�̌��ʏ���
    {
        if (isCoroutine) //�R���[�`���������Ă���Ƃ��ɉ���Ă�����EnemyMove�͓������Ȃ�
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
    private void EndRound() //���E���h�I�����̌��ʏ���
    {
        playerScript.PlayerPoison(playerMoveCount);
        enemyScript.EnemyPoison(enemyMoveCount);
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
        else if (player.CompareTag("Sorcerer"))
        {
            playerData = new PlayerDataManager("Sorcerer");
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
        ps.GetSetPlayerCondition.upStrength = relicEffect.RelicID2(pr.hasRelicID2, ps.GetSetPlayerCondition.upStrength, es.GetSetEnemyCondition.upStrength).playerUpStrength;
        es.GetSetEnemyCondition.upStrength = relicEffect.RelicID2(pr.hasRelicID2, ps.GetSetPlayerCondition.upStrength, es.GetSetEnemyCondition.upStrength).enemyUpStrength;
        ps.GetSetPlayerConstAP = relicEffect.RelicID3(pr.hasRelicID3, ps.GetSetPlayerConstAP, ps.GetSetPlayerChargeAP).playerConstAP;
        ps.GetSetPlayerConstAP = relicEffect.RelicID4(pr.hasRelicID4, ps.GetSetPlayerConstAP);
        ps.GetSetPlayerConstAP = relicEffect.RelicID5(pr.hasRelicID5, ps.GetSetPlayerConstAP, ps.GetSetPlayerChargeAP).playerConstAP;
        ps.GetSetPlayerCondition.burn = relicEffect.RelicID6(pr.hasRelicID6, ps.GetSetPlayerCondition.burn);
        ps.GetSetPlayerHP = relicEffect.RelicID7(pr.hasRelicID7, ps.GetSetPlayerHP);
        ps.GetSetPlayerGP = relicEffect.RelicID8(pr.hasRelicID8, ps.GetSetPlayerGP);
        ps.GetSetPlayerCondition.upStrength = relicEffect.RelicID12(pr.hasRelicID12, "Slime", ps.GetSetPlayerCondition.upStrength);
        Debug.Log("�X�^�[�g���̃����b�N���Ăяo����܂���: " + ps.GetSetPlayerConstAP + " to " + ps.GetSetPlayerChargeAP);
    }
    public void OnceEndRoundRelicEffect() //���E���h�I�����Ɉ�x�����������郌���b�N����
    {
        var ps = playerScript;
        var pr = playerRelics;
        ps.GetSetPlayerChargeAP = relicEffect.RelicID3(pr.hasRelicID3, ps.GetSetPlayerConstAP, ps.GetSetPlayerChargeAP).playerChargeAP;
        ps.GetSetPlayerChargeAP = relicEffect.RelicID5(pr.hasRelicID5, ps.GetSetPlayerAP, ps.GetSetPlayerChargeAP).playerChargeAP;
    }
    public void EndRoundRelicEffect() //���E���h�I�����ɔ������郌���b�N����
    {
        var ps = playerScript;
        var pr = playerRelics;
        var pc = ps.GetSetPlayerCondition;
        (pc.curse, pc.impatience, pc.weakness, pc.burn, pc.poison) = relicEffect.RelicID11(pr.hasRelicID11, pc.curse, pc.impatience, pc.weakness, pc.burn, pc.poison);
    }
    public void EndGameRelicEffect() //�퓬�I�����ɔ������郌���b�N����
    {
        var ps = playerScript;
        var pr = playerRelics;
        int money = 10;
        money = relicEffect.RelicID9(playerRelics.hasRelicID9, money);
        ps.GetSetPlayerCurrentHP = relicEffect.RelicID10(pr.hasRelicID10, ps.GetSetPlayerCurrentHP);
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
    private void UndoCardCost() //�A�N�Z�����[�g�̌��ʂ𖳌�
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