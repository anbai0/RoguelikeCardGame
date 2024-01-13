using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SelfMadeNamespace;
using DG.Tweening;

/// <summary>
/// �o�g���̐i�s��i�߂�X�N���v�g
/// </summary>
public class BattleGameManager : MonoBehaviour
{
    [SerializeField] PlayerBattleAction playerScript;
    [SerializeField] EnemyBattleAction enemyScript;

    //�v���C���[
    PlayerDataManager playerData;
    [SerializeField] Image playerTurnDisplay;

    //�G�l�~�[
    EnemyDataManager enemyData;
    [SerializeField] Image enemyTurnDisplay;
    [SerializeField] SelectEnemyName selectEnemyName;
    [SerializeField] SelectEnemyData selectEnemyData;
    [SerializeField] SelectEnemyRelic selectEnemyRelic;
    public string enemyType = "SmallEnemy";
    string enemyName;
    float enemyMoveTime = 0.5f;
    
    //�J�[�h
    [SerializeField] CardController cardPrefab;
    [SerializeField] Transform CardPlace;
    [SerializeField] GameObject DropPlace;
    Vector3 originCardPlace;
    [SerializeField] Transform PickCardPlace;
    [SerializeField] CardCostChange cardCostChange;
    public List<int> deckNumberList;//�v���C���[�̂��f�b�L�i���o�[�̃��X�g

    //�����b�N
    public int relicID2Player;
    public int relicID2Enemy;

    //���U���g
    [SerializeField] BattleRewardManager battleRewardManager;
    [SerializeField] ResultAnimation resultAnimation;
    [SerializeField] GameObject uiManagerBR;
    [SerializeField] GameObject uiManagerBattle;

    //���E���h
    [SerializeField] RoundTextAnimation roundTextAnimation;
    [SerializeField] GameObject turnEndBlackPanel;

    //�G�t�F�N�g
    [SerializeField] BattleEffect battleEffect;
    [SerializeField] CanvasGroup turnEndButtonGroup;
    Tween buttonTween;

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
    public float turnTime = 0.5f;//�^�[���̐؂�ւ�����
    public float roundTime = 1.0f;//���E���h�̐؂�ւ�����
    private int playerMoveCount;//�v���C���[�����E���h���ɍs�������l//Curse�̏����Ŏg�p
    private int enemyMoveCount;//�G�l�~�[�����E���h���ɍs�������l//Curse�̏����Ŏg�p
    public int roundCount;//�����E���h�ڂ����L�^����
    public int floor = 1; //���݂̊K�w

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
        //������
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
        playerData = gm.playerData; //GameManager����v���C���[�̃f�[�^���󂯎��
        ReadEnemy(enemyName); //�G�l�~�[�̖��O����V�����f�[�^���쐬
        SetStatus(playerData, enemyData); //���ꂼ��̃f�[�^��BattleAction�̕ϐ��ɑ������
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
        var roundText = "Round" + roundCount.ToString(); //���E���h����\���e�L�X�g
        roundTextAnimation.StartAnimation(roundText); //���E���h����\��
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
        
        // �o�g���I�����A�J�[�h�ɐG��Ȃ�����
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
            isPlayerTurn = false;
            return;
        }
        playerScript.ViewConditionIcon(); //�v���C���[�̏�Ԉُ�A�C�R���̍X�V
        enemyScript.ViewConditionIcon(); //�G�l�~�[�̏�Ԉُ�A�C�R���̍X�V

        if (isAccelerate)
        {
            cardCostChange.CardCostDown(accelerateValue);
            isAccelerate = false;
            isDecelerate = true;
            Debug.Log("�A�N�Z�����[�V��������");
        }

        int playerCurrentAP = playerScript.GetSetCurrentAP;
        int enemyCurrentAP = enemyScript.GetSetCurrentAP;
        
        if (playerCurrentAP >= 0 || enemyCurrentAP > 0) //�ǂ��炩��AP���c���Ă���ꍇ
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
                turnEndBlackPanel.SetActive(false); //TurnEndButton�̈Ó]������
                isPlayerMove = false;
                playerTurnDisplay.enabled = true;
                enemyTurnDisplay.enabled = false;
                Debug.Log("�v���C���[�̍s���� = " + CheckPlayerCanMove());
                if (!CheckPlayerCanMove())
                {
                    WaitTurnEndCompletion();
                }
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
                turnEndBlackPanel.SetActive(true); //TurnEndButton�̐F���Â�����
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
        else //�ǂ�����s���ł��Ȃ��ꍇ
        {
            WaitTurnEndCompletion();
        }
    }

    /// <summary>
    /// �v���C���[�Ƀ^�[��������ė������ɍs���ł��邩�`�F�b�N����
    /// </summary>
    /// <returns>�s���ł���Ȃ�true���s���ł��Ȃ��̂ł����false��Ԃ�</returns>
    bool CheckPlayerCanMove()
    {
        // �O�̃^�[���Ń^�[���I�������Ă����ꍇ�A���̃^�[���͍s���o���Ȃ�
        if(isTurnEnd) 
            return false;

        //�{���Ȃ��CardPlace����f�b�L�����擾���������A�J�[�h��Parent���O���֌W��擾�ł��Ȃ��Ƃ�������̂�Parent�̓������Ƃ̂Ȃ�PickCardPlace����擾����
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
    /// �^�[���G���h�̎��s�y�ю��s�̑ҋ@���s��
    /// </summary>
    void WaitTurnEndCompletion()
    {
        //�^�[���f�B�X�v���C�͂ǂ�����I�t��
        playerTurnDisplay.enabled = false;
        enemyTurnDisplay.enabled = false;
        isPlayerTurn = false;
        turnEndBlackPanel.SetActive(true); //TurnEndButton�̐F���Â�����

        if (isTurnEnd) //�v���C���[���s���I���{�^���������Ă�����
        {
            StartCoroutine(WaitEndRound()); //�^�[�����I������
        }
        else
        {
            turnEndBlackPanel.SetActive(false); //TurnEndButton�̈Ó]������
            buttonTween = turnEndButtonGroup.DOFade(0.0f, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            isPlayerMove = false; //�v���C���[�ɍs���I���̃^�C�~���O���ς˂�
        }
    }

    IEnumerator WaitEndRound()
    {
        yield return new WaitForSeconds(roundTime);
        EndRound();
        yield break;
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
        isPlayerTurn = false;
        //�U���G�t�F�N�g�𔭓�
        var cardType = card.cardDataManager._cardType;
        if(cardType == "Attack")
        {
            battleEffect.Attack(DropPlace);
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
    }

    /// <summary>
    /// �s���I���{�^�������������� 
    /// </summary>
    public void TurnEnd()
    {
        if (!isTurnEnd && !isPlayerMove) //�܂��{�^����������Ă��Ȃ������牟�����Ƃ��o����
        {
            AudioManager.Instance.PlaySE("�I����1");
            isTurnEnd = true;
            playerScript.TurnEnd();
            if(buttonTween != null)
            {
                buttonTween.Kill();
                turnEndButtonGroup.alpha = 1;
            }
            turnEndBlackPanel.SetActive(true); //TurnEndButton�̐F���Â�����

            TurnCalc(); //�^�[�������Ɉڂ�
        }
    }

    /// <summary>
    /// ���E���h�I�����̌��ʏ���
    /// </summary>
    private void EndRound() 
    {
        playerScript.Poison(playerMoveCount);
        enemyScript.Poison(enemyMoveCount);
        playerMoveCount = 0; //�v���C���[�̍s���񐔂����Z�b�g����
        enemyMoveCount = 0; //�G�l�~�[�̍s���񐔂����Z�b�g����
        playerScript.ChargeAP();
        enemyScript.ChargeAP();
        EndRoundRelicEffect();
        if (!isFirstCall) { isFirstCall = true; OnceEndRoundRelicEffect(); }
        isTurnEnd = false;//�s���I���{�^���̕���
        turnEndBlackPanel.SetActive(false); //TurnEndButton�̈Ó]������
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
        enemyScript.EnemyDefeated(); //�G�l�~�[�̂��ꂽ���o
        yield return new WaitForSeconds(4.0f);
        EndGameRelicEffect();
        yield return new WaitForSeconds(0.5f);
        resultAnimation.StartAnimation("Victory"); //�����̕�����\��
        ReturnPlayerData();
        yield return new WaitForSeconds(1.0f);
        Destroy(uiManagerBattle);
        uiManagerBR.SetActive(true);�@//UIManagerBattleReward���g�p�\��
        uiManagerBR.GetComponent<UIManagerBattleReward>().isDisplayRelics = true; //��V�Ƃ��ă����b�N��I���ł���悤�ɂ���
        resultAnimation.DisappearResult(); //�����̕���������
        battleRewardManager.ShowReward(enemyType); //��V��\��
        uiManagerBR.GetComponent<UIManagerBattleReward>().UIEventsReload();
    }

    IEnumerator LoseAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        resultAnimation.StartAnimation("Defeated");
        yield return new WaitForSeconds(2.0f);
        battleRewardManager.TransitionLoseBattle();
    }
    //�����܂ł��Q�[�����[�v

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
        //�G�l�~�[�̃X�e�[�^�X������U��
        enemyScript.SetStatus(floor, enemy);
        enemyScript.hasEnemyRelics = selectEnemyRelic.SetEnemyRelics(enemyScript.hasEnemyRelics, floor, enemyName);
        enemyScript.ViewEnemyRelic(gm);
        uiManagerBattle.GetComponent<UIManagerBattle>().UIEventsReload();
    }

    /// <summary>
    /// �v���C���[�̃f�b�L�𐶐����鏈��
    /// </summary>
    private void InitDeck() 
    {
        deckNumberList = playerData._deckList;
        int deckCount = deckNumberList.Count;
        ChangeSpace(deckCount);
        for (int init = 0; init < deckCount; init++)// �f�b�L�̖�����
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
    /// �f�b�L�̖����ɉ����Ĕz�u����X�y�[�X��ݒ肷��
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
    /// �퓬�I����ɐ퓬�ŕω������v���C���[�f�[�^��GameManager�ɕԂ�
    /// </summary>
    void ReturnPlayerData()
    {
        if(playerScript.GetSetCurrentHP > gm.playerData._playerHP) //�퓬�I�����̗̑͂��Q�ƌ��̍ő�̗͂������Ă����ꍇ
        {
            playerScript.GetSetCurrentHP = gm.playerData._playerHP; //�ő�̗͂𒴂��Ȃ��悤�ɂ���
        }
        gm.playerData._playerCurrentHP = playerScript.GetSetCurrentHP; //�퓬�I�����̗̑͂�Ԃ�
        gm.playerData._playerMoney += enemyScript.GetSetDropMoney; //�R�C�����l��
        gm.playerData._deckList = deckNumberList; //�����̗����������Ă���Ύg�p��ɍ폜���ĕԂ�
    }

    /// <summary>
    /// �G�l�~�[�̎�ނɉ�����BGM�𗬂�
    /// </summary>
    /// <param name="_enemyType">�G�l�~�[�̎��</param>
    void StartBGM(string _enemyType)
    {
        if (_enemyType == "SmallEnemy")
        {          
            // BGM�𗬂��܂�
            if (Random.Range(0, 2) == 0)
            {
                AudioManager.Instance.PlayBGM("it's my turn");
            }
            else
            {
                AudioManager.Instance.PlayBGM("�t�@�j�[�G�C���A��");
            }
        }
        else if (_enemyType == "StrongEnemy")
        {
            AudioManager.Instance.PlayBGM("Social Documentary02");
        }
        else if (_enemyType == "Boss")
        {
            AudioManager.Instance.PlayBGM("�[����`����");
        }
    }
}