using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SelfMadeNamespace;

public class BattleGameManager : MonoBehaviour
{
    PlayerBattleAction playerScript;
    EnemyBattleAction enemyScript;

    //�v���C���[
    GameObject player;
    PlayerDataManager playerData;
    [SerializeField] 
    Image playerTurnDisplay;

    //�G�l�~�[
    EnemyDataManager enemyData;
    [SerializeField] 
    Image enemyTurnDisplay;
    SelectEnemyName selectEnemyName;
    SelectEnemyData selectEnemyData;
    SelectEnemyRelic selectEnemyRelic;
    public string enemyType = "SmallEnemy";
    string enemyName;
    
    //�J�[�h
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    [SerializeField] Transform PickCardPlace;
    [SerializeField] CardCostChange cardCostChange;
    List<int> deckNumberList;//�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    //���U���g
    [SerializeField]
    BattleRewardManager battleRewardManager;
    ResultAnimation resultAnimation;
    [SerializeField]
    GameObject uiManagerBR;
    [SerializeField]
    GameObject uiManagerBattle;

    public bool isPlayerTurn;//�v���C���[�̃^�[��������//CardEffect()�Ŏg�p
    private bool isPlayerMove;//�v���C���[���s����������//TurnEnd()�Ŏg�p
    private bool isTurnEnd;//�s���I���{�^����������������//TurnEnd()�Ŏg�p
    public bool isAccelerate;//�J�[�h���A�N�Z�����[�g�����g�p����������//TurnCalc()�Ŏg�p
    public int accelerateValue;//�J�[�h���A�N�Z�����[�g���ŉ�������R�X�g�̒l
    private bool isDecelerate;//�J�[�h���A�N�Z�����[�g���̌��ʂ𖳌�������������//TurnCalc()�Ŏg�p
    private bool isFirstCall;//�ŏ��̃��E���h�̂Ƃ��ɌĂԔ���//EndRound()�Ŏg�p
    public bool isCoroutine;//�R���[�`�������쒆������//PlayerMove(),EnemyMove()�Ŏg�p
    public bool isCoroutineEnabled;//�����Ă���R���[�`�������݂��Ă��邩����//Update()�Ŏg�p
    public bool isEnemyMoving;//�G�l�~�[�̃A�N�V�����������Ă��邩����//PlayerMove(),EnemyMove()�Ŏg�p
    public float turnTime = 1.0f;//�^�[���̐؂�ւ�����
    public float roundTime = 2.0f;//���E���h�̐؂�ւ�����
    private int playerMoveCount;//�v���C���[�����E���h���ɍs�������l//Curse�̏����Ŏg�p
    private int enemyMoveCount;//�G�l�~�[�����E���h���ɍs�������l//Curse�̏����Ŏg�p
    public int roundCount;//�����E���h�ڂ����L�^����
    private bool isOnceEndRound; //EndRound()�̌Ăяo������񂾂�������
    public int floor = 1; //���݂̊K�w

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
        GameManager gm = GameManager.Instance;
        floor = gm.floor;
        PlayerController playerController = "FieldScene".GetComponentInScene<PlayerController>();
        enemyType = playerController.enemyTag;
        playerScript = GetComponent<PlayerBattleAction>();
        enemyScript = GetComponent<EnemyBattleAction>();
        selectEnemyName = GetComponent<SelectEnemyName>();
        selectEnemyData = GetComponent<SelectEnemyData>();
        selectEnemyRelic = GetComponent<SelectEnemyRelic>();
        resultAnimation = GetComponent<ResultAnimation>();
        //������
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
        player = GameObject.Find("TestPlayer");
        enemyName = selectEnemyName.DecideEnemyName(floor, enemyType);
        ReadPlayer(player);
        ReadEnemy(enemyName);
        SetStatus(playerData, enemyData);
        StartRelicEffect();
        InitDeck();
        StartRound();
    }

    /// <summary>
    /// ���E���h�J�n���̌��ʏ���
    /// </summary>
    private void StartRound() 
    {
        roundCount++;//���E���h�������Z����
        if (isDecelerate) //�A�N�Z�����[�g�̌��ʂ�������
        {
            cardCostChange.UndoCardCost();
            isDecelerate = false;
        }
        //ChageAP��Curse����������AP�����߂�
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
        if (isCoroutineEnabled == true && isCoroutine == false) //�����Ă���R���[�`�������݂��Ă���A�R���[�`�����I�����Ă����ꍇ
        {
            isCoroutineEnabled = false; //�����Ă���R���[�`���͖���
            //��񂾂�TurnCalc()���Ă�
            TurnCalc();
        }
    }

    /// <summary>
    /// �v���C���[�ƃG�l�~�[�̍s���������߂鏈��
    /// </summary>
    public void TurnCalc() 
    {
        if (isCoroutine) //�R���[�`��������Ă���Ƃ���TurnCalc()���񂳂Ȃ�
        {
            return;
        }
        if (IsGameEnd()) //�퓬�̏I�������𖞂����Ă��Ȃ����m�F����
        {
            //�ǂ��炩����ɐ퓬�s�\�ɂȂ����ꍇ�A�퓬���~�߂�
            return;
        }
        playerScript.ViewConditionIcon(); //�v���C���[�̏�Ԉُ�A�C�R���̍X�V
        enemyScript.ViewConditionIcon(); //�G�l�~�[�̏�Ԉُ�A�C�R���̍X�V

        if (isAccelerate)
        {
            cardCostChange.CardCostDown(accelerateValue);
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
                    playerScript.CursedUpdateAP();
                    //�ω�����AP�̒l��ۑ�
                    playerScript.SaveRoundAP();
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
                    enemyScript.CursedUpdateAP();
                    //�ω�����AP�̒l��ۑ�
                    enemyScript.SaveRoundAP();
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
            if (isOnceEndRound)
            {
                isOnceEndRound = false;
                Invoke("EndRound", roundTime);
            }
        }
    }

    /// <summary>
    /// �v���C���[�̌��ʏ���
    /// </summary>
    /// <param name="card">�h���b�v���ꂽ�J�[�h</param>
    public void PlayerMove(CardController card) 
    {
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

    /// <summary>
    /// �G�l�~�[�̌��ʏ���
    /// </summary>
    private void EnemyMove() 
    {
        enemyScript.Move();
        enemyMoveCount++;
        Invoke("TurnCalc", turnTime);
    }

    /// <summary>
    /// ���E���h�I�����̌��ʏ���
    /// </summary>
    private void EndRound() 
    {
        Debug.Log("EndRound���Ăяo���ꂽ");
        playerScript.Poison(playerMoveCount);
        enemyScript.Poison(enemyMoveCount);
        playerScript.ChargeAP();
        enemyScript.ChargeAP();
        if (!isFirstCall) { isFirstCall = true; OnceEndRoundRelicEffect(); }
        EndRoundRelicEffect();
        isTurnEnd = false;//�s���I���{�^���̕���
        StartRound();
    }

    /// <summary>
    /// �퓬�I�����̏���
    /// </summary>
    /// <returns>�����ꂩ��HP���Ȃ��Ȃ����Ƃ���true��Ԃ�</returns>
    private bool IsGameEnd()
    {
        if (playerScript.CheckHP()) //�v���C���[��HP���Ȃ��Ȃ�����
        {
            //�G�l�~�[�̏������o
            StartCoroutine(LoseAnimation());
            return true;
        }
        if (enemyScript.CheckHP()) //�G�l�~�[��HP���Ȃ��Ȃ�����
        {
            //�v���C���[�̏������o
            StartCoroutine(WinAnimation());
            return true;
        }
        return false;
    }

    IEnumerator WinAnimation()
    {
        Debug.Log("Player�̏���");
        enemyScript.EnemyDefeated(); //�G�l�~�[�̂��ꂽ���o
        yield return new WaitForSeconds(4.0f);
        EndGameRelicEffect();
        yield return new WaitForSeconds(0.5f);
        resultAnimation.StartAnimation("Victory"); //�����̕�����\��
        yield return new WaitForSeconds(1.0f);
        Destroy(uiManagerBattle);
        //uiManagerBattle.SetActive(false); //UIManagerBattle���g�p�s��
        uiManagerBR.SetActive(true);�@//UIManagerBattleReward���g�p�\��
        if (enemyType == "StrongEnemy" || enemyType == "Boss") //�G�l�~�[�����G�ȏ�Ȃ�
        {
            uiManagerBR.GetComponent<UIManagerBattleReward>().isDisplayRelics = true; //��V�Ƃ��ă����b�N���I���ł���悤�ɂ���
        }
        resultAnimation.DisappearResult(); //�����̕���������
        battleRewardManager.ShowReward(enemyType); //��V��\��
        uiManagerBR.GetComponent<UIManagerBattleReward>().UIEventsReload();
    }

    IEnumerator LoseAnimation()
    {
        Debug.Log("Enemy�̏���");
        yield return new WaitForSeconds(1.0f);
        resultAnimation.StartAnimation("Defeated");
    }
    //�����܂ł��Q�[�����[�v

    /// <summary>
    /// �v���C���[�̃f�[�^��ǂݎ�鏈��
    /// </summary>
    /// <param name="player">�v���C���[�̃I�u�W�F�N�g</param>
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

    /// <summary>
    /// �G�l�~�[�̃f�[�^��ǂݎ��
    /// </summary>
    /// <param name="enemyName">�G�l�~�[�̖��O</param>
    private void ReadEnemy(string enemyName) 
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

    /// <summary>
    /// �v���C���[�̃f�b�L�𐶐����鏈��
    /// </summary>
    private void InitDeck() 
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

    /// <summary>
    /// �퓬�J�n���ɔ������郌���b�N���ʂ̏���
    /// </summary>
    public void StartRelicEffect() 
    {
        enemyScript = playerScript.StartRelicEffect(enemyScript, enemyType);
        playerScript = enemyScript.StartRelicEffect(playerScript);
    }

    /// <summary>
    /// ���E���h�I�����Ɉ�x�����������郌���b�N���ʂ̏���
    /// </summary>
    public void OnceEndRoundRelicEffect() 
    {
        playerScript.OnceEndRoundRelicEffect();
        enemyScript.OnceEndRoundRelicEffect();
    }

    /// <summary>
    /// ���E���h�I�����ɔ������郌���b�N���ʂ̏���
    /// </summary>
    public void EndRoundRelicEffect() 
    {
        playerScript.EndRoundRelicEffect();
        enemyScript.EndRoundRelicEffect();
    }

    /// <summary>
    /// �퓬�I�����ɔ������郌���b�N���ʂ̏���
    /// </summary>
    public void EndGameRelicEffect() 
    {
        enemyScript.GetSetDropMoney += playerScript.EndGameRelicEffect();
    }

    /// <summary>
    /// �s���I���{�^�������������� 
    /// </summary>
    public void TurnEnd() 
    {
        if (!isTurnEnd && !isPlayerMove) //�܂��{�^����������Ă��Ȃ������牟�����Ƃ��o����
        {
            isTurnEnd = true;
            playerScript.TurnEnd();
            TurnCalc();
        }
    }
}