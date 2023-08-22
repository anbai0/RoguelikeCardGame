using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SelfMadeNamespace;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class BattleRewardManager : MonoBehaviour
{
    [Header("��V��ʗpUI")]
    [SerializeField]
    GameObject battleRewardUI;

    [Header("�Q�Ƃ���R���|�[�l���g")]
    [SerializeField]
    Lottery lottery;
    [SerializeField] 
    SceneFader sceneFader;
    
    [Header("��������J�[�h�I�u�W�F�N�g")]
    [SerializeField]
    GameObject cardPrefab;
    [Header("�������郌���b�N�I�u�W�F�N�g")]
    [SerializeField]
    GameObject relicPrefab;
    [Header("�J�[�h�̐����ꏊ")]
    [SerializeField]
    Transform cardPlace;
    [Header("�����b�N�̐����ꏊ")]
    [SerializeField]
    Transform relicPlace;
    
    CardController cardController;
    RelicController relicController;
    
    List<int> rewardCardID = null; //��V�Ƃ��đI�΂ꂽ�J�[�h��ID
    List<int> rewardRelicID = null; //��V�Ƃ��đI�΂ꂽ�����b�N��ID
    const int RelicID1 = 1; //�A���A�h�l�̎�(���A���e�B�R)

    GameManager gm;
    BattleGameManager bg;

    void Start()
    {
        gm = GameManager.Instance;
        bg = BattleGameManager.Instance;
        battleRewardUI.SetActive(false);
    }

    /// <summary>
    /// ��V��ʂ�\�����鏈��
    /// </summary>
    /// <param name="type">�G�l�~�[�̎��</param>
    public void ShowReward(string type)
    {
        battleRewardUI.SetActive(true); 
        SelectRewardByCards(type);
        SelectRewardByRelics(type);
        battleRewardUI.GetComponent<DisplayAnimation>().StartPopUPAnimation();
    }

    /// <summary>
    /// �G�l�~�[�̎�ނɉ����ĕ�V�̃J�[�h�𒊑I�A�\�������鏈��
    /// </summary>
    /// <param name="type">�G�l�~�[�̎��</param>
    void SelectRewardByCards(string type)
    {
        if (type == "SmallEnemy")
        {
            rewardCardID = lottery.SelectCardByRarity(new List<int> { 2, 1, 1 });
        }
        else if (type == "StrongEnemy")
        {
            rewardCardID = lottery.SelectCardByRarity(new List<int> { 2, 2, 1 });
        }
        else if (type == "Boss")
        {
            rewardCardID = lottery.SelectCardByRarity(new List<int> { 3, 2, 2 });
        }
        ShowCards();
    }

    /// <summary>
    /// �G�l�~�[�̎�ނɉ����ă����b�N�𒊑I�A�\�������鏈��
    /// </summary>
    /// <param name="type">�G�l�~�[�̎��</param>
    public void SelectRewardByRelics(string type)
    {
        if (type == "StrongEnemy")
        {
            rewardRelicID = lottery.SelectRelicByRarity(new List<int> { 2, 1, 1 });
        }
        else if (type == "Boss")
        {
            rewardRelicID = lottery.SelectRelicByRarity(new List<int> { 2, 2 });
            rewardRelicID.Insert(0, RelicID1);
        }
        else
        {
            rewardRelicID = null;
        }
        ShowRelics();
    }

    /// <summary>
    /// ���I���ꂽ�J�[�h�̐����ƕ\�������鏈��
    /// </summary>
    void ShowCards()
    {
        for (int cardCount = 0; cardCount < rewardCardID.Count; cardCount++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardPlace);
            cardObj.transform.SetParent(cardPlace);
            cardController = cardObj.GetComponent<CardController>();
            cardController.Init(rewardCardID[cardCount]);
            cardController.cardDataManager._cardState = -1; //�o�g����ʂɂ���J�[�h�Ƌ�ʂ���ׂ�State��-1�ɂ���
        }
    }

    /// <summary>
    /// ���I���ꂽ�����b�N�̐����ƕ\�����鏈��
    /// </summary>
    void ShowRelics()
    {
        if (rewardRelicID != null) //���X�g��ID�������Ă����
        {
            for (int relicCount = 0; relicCount < rewardRelicID.Count; relicCount++)
            {
                GameObject relicObj = Instantiate(relicPrefab, relicPlace);
                relicObj.transform.SetParent(relicPlace);
                relicController = relicObj.GetComponent<RelicController>();
                relicController.Init(rewardRelicID[relicCount]);
                Transform relicBG = relicObj.transform.GetChild(8); //�\������BackGround���擾
                TextMeshProUGUI relicName = relicBG.GetChild(0).GetComponent<TextMeshProUGUI>(); //�����b�N�̖��O
                relicName.text = relicController.relicDataManager._relicName;
                TextMeshProUGUI relicEffect = relicBG.GetChild(1).GetComponent<TextMeshProUGUI>(); //�����b�N�̌���
                relicEffect.text = relicController.relicDataManager._relicEffect;
            }
        }
    }

    private string unloadSceneName = null; //���[�h����V�[���̖��O

    public void UnLoadBattleScene()
    {
        if (bg.enemyType == "StrongEnemy")
        {
            if (gm.floor < 3) //�K�w��3�K�܂œ��B���Ă��Ȃ��ꍇ
            {
                gm.floor++; //�K�w��1�グ��
                unloadSceneName = "FieldScene"; //���[�h����V�[�����t�B�[���h�V�[���ɐݒ�
                TransitionAfterBattle();
            }
            else
            {
                unloadSceneName = "ResultScene"; //���[�h����V�[�������U���g�V�[���ɐݒ�
                TransitionAfterBattle();
            }
        }
        else
        {
            // �o�g���V�[�����A�����[�h
            sceneFader.SceneChange(unLoadSceneName: "BattleScene");
            PlayerController playerController = "FieldScene".GetComponentInScene<PlayerController>();
            PlayerController.isPlayerActive = true;       // �v���C���[�𓮂���悤�ɂ���
            playerController.enemy.SetActive(false);      // �G�l�~�[������
            playerController = null;
        }
    }

    /// <summary>
    /// �퓬�I����ɑJ�ڂ���ۂ̏���
    /// TransitionAfterBattleScenes���\�b�h��FadeOutInWrapper���\�b�h�ɓn���Ď��s
    /// </summary>
    public void TransitionAfterBattle()
    {
        sceneFader.FadeOutInWrapper(TransitionAfterBattleScenes);
    }

    /// <summary>
    /// �o�g���V�[���ƃt�B�[���h�ɂ���V�[�������ׂăA�����[�h���A�I�����ꂽ�V�[�������[�h����
    /// </summary>
    public async Task TransitionAfterBattleScenes()
    {
        AsyncOperation asyncOperation;

        //�o�g���V�[�����A�����[�h
        Scene battleScene = SceneManager.GetSceneByName("BattleScene");
        asyncOperation = SceneManager.UnloadSceneAsync(battleScene);
        while (!asyncOperation.isDone) await Task.Yield();

        //�V���b�v�V�[�������[�h����Ă���΃A�����[�h����
        Scene shopScene = SceneManager.GetSceneByName("ShopScene");
        if (shopScene.isLoaded)
        {
            asyncOperation = SceneManager.UnloadSceneAsync(shopScene);
            while (!asyncOperation.isDone) await Task.Yield();
        }

        // �Q�Ɖ����̊֌W�Ńt�B�[���h�V�[�����Ō�ɃA�����[�h
        Scene fieldScene = SceneManager.GetSceneByName("FieldScene");
        asyncOperation = SceneManager.UnloadSceneAsync(fieldScene);
        while (!asyncOperation.isDone) await Task.Yield();

        // �t�B�[���h�V�[�������U���g�V�[�������[�h
        asyncOperation = SceneManager.LoadSceneAsync(unloadSceneName, LoadSceneMode.Additive);
        while (!asyncOperation.isDone) await Task.Yield();
    }

    public void TransitionLoseBattle()
    {
        sceneFader.FadeOutInWrapper(TransitionLoseBattleScenes);
    }

    public async Task TransitionLoseBattleScenes()
    {
        AsyncOperation asyncOperation;

        //�o�g���V�[�����A�����[�h
        Scene battleScene = SceneManager.GetSceneByName("BattleScene");
        asyncOperation = SceneManager.UnloadSceneAsync(battleScene);
        while (!asyncOperation.isDone) await Task.Yield();

        //�V���b�v�V�[�������[�h����Ă���΃A�����[�h����
        Scene shopScene = SceneManager.GetSceneByName("ShopScene");
        if (shopScene.isLoaded)
        {
            asyncOperation = SceneManager.UnloadSceneAsync(shopScene);
            while (!asyncOperation.isDone) await Task.Yield();
        }

        // �Q�Ɖ����̊֌W�Ńt�B�[���h�V�[�����Ō�ɃA�����[�h
        Scene fieldScene = SceneManager.GetSceneByName("FieldScene");
        asyncOperation = SceneManager.UnloadSceneAsync(fieldScene);
        while (!asyncOperation.isDone) await Task.Yield();

        // ���U���g�V�[�������[�h
        asyncOperation = SceneManager.LoadSceneAsync("ResultScene", LoadSceneMode.Additive);
        while (!asyncOperation.isDone) await Task.Yield();
    }
}